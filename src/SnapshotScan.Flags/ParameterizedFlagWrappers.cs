using OpenFeature;

namespace SnapshotScan.Flags;

/// <summary>
/// The sink is deliberately parameterized so callers must supply both key and default value.
/// </summary>
public sealed class OpenFeatureBooleanGateway(IFeatureClient client)
{
    public Task<bool> EvaluateAsync(string key, bool fallback)
    {
        var sinkKey = (string)key;
        var sinkFallback = fallback;
        return client.GetBooleanValueAsync(sinkKey, sinkFallback);
    }
}

/// <summary>
/// A second wrapper layer builds keys through interpolation and forwards defaults.
/// </summary>
public sealed class ParameterizedFlagWrapper(OpenFeatureBooleanGateway gateway)
{
    public Task<bool> EvaluateVersionAsync(string area, int version, bool fallback)
    {
        var key = $"{area}.v{version}";
        return gateway.EvaluateAsync(key, fallback);
    }

    public Task<bool> EvaluateFixedAsync() =>
        gateway.EvaluateAsync("acceptance.wrapper.fixed", true);
}

