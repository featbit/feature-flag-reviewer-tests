using Microsoft.Extensions.DependencyInjection;

namespace SnapshotScan.DependencyInjection;

public sealed class ConstructorInjectionEntry
{
    private readonly IConstructorFlagEvaluator _evaluator;

    public ConstructorInjectionEntry(IConstructorFlagEvaluator evaluator) =>
        _evaluator = evaluator;

    public Task<bool> EvaluateAsync() =>
        _evaluator.EvaluateAsync("acceptance.di.constructor", true);
}

public sealed class KeyedConstructorEntry
{
    private readonly IKeyedFlagEvaluator _evaluator;

    public KeyedConstructorEntry(
        [FromKeyedServices("beta")] IKeyedFlagEvaluator evaluator) =>
        _evaluator = evaluator;

    public Task<bool> EvaluateAsync() =>
        _evaluator.EvaluateAsync("acceptance.di.keyed", true);
}

public sealed class FactoryEntry
{
    private readonly IFactoryFlagEvaluator _evaluator;

    public FactoryEntry(IFactoryFlagEvaluator evaluator) => _evaluator = evaluator;

    public Task<bool> EvaluateAsync() =>
        _evaluator.EvaluateAsync("acceptance.di.factory", false);
}

public sealed class EnumerableEntry
{
    private readonly IEnumerable<IEnumerableFlagEvaluator> _evaluators;

    public EnumerableEntry(IEnumerable<IEnumerableFlagEvaluator> evaluators) =>
        _evaluators = evaluators;

    public async Task EvaluateAllAsync()
    {
        foreach (var evaluator in _evaluators)
        {
            _ = await evaluator.EvaluateAsync("acceptance.di.enumerable", false);
        }
    }
}

public sealed class TryAddEntry
{
    private readonly ITryAddFlagEvaluator _evaluator;

    public TryAddEntry(ITryAddFlagEvaluator evaluator) => _evaluator = evaluator;

    public Task<bool> EvaluateAsync() =>
        _evaluator.EvaluateAsync("acceptance.di.try-add", true);
}

public sealed class ExplicitResolutionEntry
{
    public Task<bool> EvaluateAsync(IServiceProvider provider) =>
        provider.GetRequiredService<IExplicitFlagEvaluator>()
            .EvaluateAsync("acceptance.di.explicit", false);

    public Task<bool> EvaluateDynamicKeyAsync(
        IServiceProvider provider,
        string serviceKey) =>
        provider.GetRequiredKeyedService<IKeyedFlagEvaluator>(serviceKey)
            .EvaluateAsync("acceptance.di.dynamic-key", false);
}

/// <summary>
/// Complete Base-v4 chain: Options -> DI-selected service -> wrapper -> OpenFeature ->
/// returned local -> complete if/else business decision.
/// </summary>
public sealed class ConfiguredCheckoutEndpoint
{
    private readonly IConfiguredCheckoutService _checkoutService;

    public ConfiguredCheckoutEndpoint(IConfiguredCheckoutService checkoutService) =>
        _checkoutService = checkoutService;

    public async Task<string> ExecuteAsync()
    {
        var enabled = await _checkoutService.IsEnabledAsync();

        if (!enabled)
        {
            return "legacy-checkout";
        }
        else
        {
            return "configured-checkout";
        }
    }
}
