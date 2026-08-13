using Microsoft.Extensions.Configuration;
using OpenFeature;
using SnapshotScan.Flags;

namespace SnapshotScan.Configuration;

/// <summary>
/// Base-v4 direct IConfiguration scenarios. Every resolved value still flows through the
/// parameterized gateway before reaching the real OpenFeature evaluation.
/// </summary>
public sealed class ConfigurationEntryPoints(
    IConfiguration configuration,
    OpenFeatureBooleanGateway gateway,
    IFeatureClient client)
{
    // PR removal: the endpoint remains callable, but it no longer evaluates a flag.
    // Keeping the business entry makes the removed Reference easy to review in isolation.
    public Task<bool> EvaluateIndexerAsync() =>
        Task.FromResult(false);

    // Both key and default are read through GetValue<T>.
    public Task<bool> EvaluateGetValueAsync() => gateway.EvaluateAsync(
        configuration.GetValue<string>("FeatureReview:DirectGetValue:Key")!,
        configuration.GetValue<bool>("FeatureReview:DirectGetValue:DefaultValue"));

    // Base, Development and Production deliberately provide three correlated pairs.
    public Task<bool> EvaluateEnvironmentAsync() => gateway.EvaluateAsync(
        configuration["FeatureReview:Environment:Key"]!,
        configuration.GetValue<bool>("FeatureReview:Environment:DefaultValue"));

    // Negative boundary: keep the dynamic access at the real Sink so the report can
    // preserve CONFIGURATION_DYNAMIC_PATH instead of guessing a runtime value.
    public Task<bool> EvaluateDynamicPathAsync(string configurationPath) =>
        client.GetBooleanValueAsync(configuration[configurationPath]!, false);
}
