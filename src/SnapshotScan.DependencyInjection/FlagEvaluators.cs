using Microsoft.Extensions.Options;
using SnapshotScan.Configuration;
using SnapshotScan.Flags;

namespace SnapshotScan.DependencyInjection;

public interface IConstructorFlagEvaluator
{
    Task<bool> EvaluateAsync(string key, bool fallback);
}

public interface IKeyedFlagEvaluator
{
    Task<bool> EvaluateAsync(string key, bool fallback);
}

public interface IFactoryFlagEvaluator
{
    Task<bool> EvaluateAsync(string key, bool fallback);
}

public interface IEnumerableFlagEvaluator
{
    Task<bool> EvaluateAsync(string key, bool fallback);
}

public interface ITryAddFlagEvaluator
{
    Task<bool> EvaluateAsync(string key, bool fallback);
}

public interface IExplicitFlagEvaluator
{
    Task<bool> EvaluateAsync(string key, bool fallback);
}

/// <summary>
/// The two finite implementations intentionally share the same parameterized wrapper.
/// Keys/defaults belong to consumers, so DI candidate expansion cannot contaminate them.
/// </summary>
public sealed class AlphaFlagEvaluator :
    IConstructorFlagEvaluator,
    IKeyedFlagEvaluator,
    IFactoryFlagEvaluator,
    IEnumerableFlagEvaluator,
    ITryAddFlagEvaluator,
    IExplicitFlagEvaluator
{
    private readonly OpenFeatureBooleanGateway _gateway;

    public AlphaFlagEvaluator(OpenFeatureBooleanGateway gateway) => _gateway = gateway;

    public Task<bool> EvaluateAsync(string key, bool fallback) =>
        _gateway.EvaluateAsync(key, fallback);
}

public sealed class BetaFlagEvaluator :
    IKeyedFlagEvaluator,
    IFactoryFlagEvaluator,
    IEnumerableFlagEvaluator,
    ITryAddFlagEvaluator,
    IExplicitFlagEvaluator
{
    private readonly OpenFeatureBooleanGateway _gateway;

    public BetaFlagEvaluator(OpenFeatureBooleanGateway gateway) => _gateway = gateway;

    public Task<bool> EvaluateAsync(string key, bool fallback) =>
        _gateway.EvaluateAsync(key, fallback);
}

public interface IConfiguredCheckoutService
{
    Task<bool> IsEnabledAsync();
}

/// <summary>
/// Options provide both arguments, while DI supplies this service to the business endpoint.
/// </summary>
public sealed class ConfiguredCheckoutService : IConfiguredCheckoutService
{
    private readonly IOptions<FeatureReviewOptions> _options;
    private readonly OpenFeatureBooleanGateway _gateway;

    public ConfiguredCheckoutService(
        IOptions<FeatureReviewOptions> options,
        OpenFeatureBooleanGateway gateway)
    {
        _options = options;
        _gateway = gateway;
    }

    public Task<bool> IsEnabledAsync() => _gateway.EvaluateAsync(
        _options.Value.Checkout.Key,
        _options.Value.Checkout.DefaultValue);
}
