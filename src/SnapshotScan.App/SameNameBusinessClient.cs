namespace SnapshotScan.App.BusinessApi;

/// <summary>
/// This deliberately resembles an OpenFeature API by method name only.
/// SnapshotScanner must not report it as a feature flag evaluation.
/// </summary>
public sealed class BusinessFeatureClient
{
    public Task<bool> GetBooleanValueAsync(string key, bool fallback) =>
        Task.FromResult(fallback);
}

public static class SameNameBusinessCallSite
{
    public static Task<bool> EvaluateAsync(BusinessFeatureClient client) =>
        client.GetBooleanValueAsync("acceptance.false-positive", false);
}

