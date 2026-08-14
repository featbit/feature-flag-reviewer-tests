using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SnapshotScan.Configuration;
using SnapshotScan.Flags;

namespace SnapshotScan.DependencyInjection;

/// <summary>
/// The real Microsoft DI composition root for all Base-v4 scenarios.
/// </summary>
public static class DependencyInjectionCompositionExtensions
{
    public static IServiceCollection AddSnapshotBaseV4(
        this IServiceCollection services,
        bool useAlphaFactory)
    {
        services.AddSnapshotConfiguration();
        services.AddTransient<OpenFeatureBooleanGateway>();

        services.AddSingleton<IConstructorFlagEvaluator, AlphaFlagEvaluator>();

        services.AddKeyedSingleton<IKeyedFlagEvaluator, AlphaFlagEvaluator>("alpha");
        services.AddKeyedSingleton<IKeyedFlagEvaluator, BetaFlagEvaluator>("beta");

        services.AddTransient<IFactoryFlagEvaluator>(provider =>
            useAlphaFactory
                ? new AlphaFlagEvaluator(provider.GetRequiredService<OpenFeatureBooleanGateway>())
                : new BetaFlagEvaluator(provider.GetRequiredService<OpenFeatureBooleanGateway>()));

        services.AddTransient<IEnumerableFlagEvaluator, AlphaFlagEvaluator>();
        services.AddTransient<IEnumerableFlagEvaluator, BetaFlagEvaluator>();

        services.AddTransient<ITryAddFlagEvaluator, AlphaFlagEvaluator>();
        services.TryAddTransient<ITryAddFlagEvaluator, BetaFlagEvaluator>();

        services.AddTransient<IExplicitFlagEvaluator, AlphaFlagEvaluator>();
        services.AddTransient<IExplicitFlagEvaluator, BetaFlagEvaluator>();

        services.AddTransient<IConfiguredCheckoutService, ConfiguredCheckoutService>();
        services.AddTransient<ConstructorInjectionEntry>();
        services.AddTransient<KeyedConstructorEntry>();
        services.AddTransient<FactoryEntry>();
        services.AddTransient<EnumerableEntry>();
        services.AddTransient<TryAddEntry>();
        services.AddTransient<ConfiguredCheckoutEndpoint>();
        services.AddTransient<CheckoutSavingsBannerService>();
        return services;
    }
}

public sealed class BaseV4CompositionRoot
{
    public void Configure(IServiceCollection services, bool useAlphaFactory) =>
        services.AddSnapshotBaseV4(useAlphaFactory);
}
