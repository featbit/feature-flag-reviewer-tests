using SnapshotScan.Flags;

namespace SnapshotScan.App;

/// <summary>
/// Base-v1 cross-project entry points. Each method is a distinct caller instance.
/// </summary>
public sealed class BaseEntryPoints(ParameterizedFlagWrapper wrapper)
{
    public Task<bool> CheckoutAsync() =>
        wrapper.EvaluateVersionAsync("acceptance.checkout", 2, false);

    public Task<bool> CheckoutAgainAsync() =>
        wrapper.EvaluateVersionAsync("acceptance.checkout", 2, false);

    public Task<bool> SearchAsync() =>
        wrapper.EvaluateVersionAsync("acceptance.search", 3, true);

    public Task<bool> FixedAsync() =>
        wrapper.EvaluateFixedAsync();
}

