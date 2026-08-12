using OpenFeature;

namespace SnapshotScan.Flags;

/// <summary>
/// Base-v1 direct evaluations. These use the real OpenFeature 2.14.0 contract.
/// </summary>
public sealed class DirectEvaluations(IFeatureClient client)
{
    private const string BooleanKey = "acceptance.direct.boolean";

    public async Task<(bool Enabled, string Variant)> EvaluateAsync()
    {
        var enabled = await client.GetBooleanValueAsync(BooleanKey, false);
        var variant = await client.GetStringValueAsync("acceptance.direct.string", "control");
        return (enabled, variant);
    }

    public Task<bool> EvaluateDynamicAsync(string tenant)
    {
        var key = $"acceptance.dynamic.{tenant}";
        return client.GetBooleanValueAsync(key, true);
    }
}

