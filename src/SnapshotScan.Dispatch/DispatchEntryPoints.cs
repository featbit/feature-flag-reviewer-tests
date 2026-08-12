using SnapshotScan.Flags;

namespace SnapshotScan.Dispatch;

/// <summary>
/// Human-reviewable Base-v3 entry points for interface, virtual and Delegate dispatch.
/// No method is executed by SnapshotScanner; all evidence must come from static analysis.
/// </summary>
public sealed class DispatchEntryPoints(OpenFeatureBooleanGateway gateway)
{
    public Task<bool> KnownInterfaceAsync()
    {
        IDispatchFlagStrategy strategy = new AlphaDispatchFlagStrategy(gateway);
        return strategy.EvaluateAsync("acceptance.dispatch.interface", false);
    }

    // Base v2 + v3 composition: a value returned through interface dispatch controls business code.
    public async Task<string> UseKnownInterfaceResultAsync()
    {
        var interfaceEnabled = await KnownInterfaceAsync();

        if (!interfaceEnabled)
        {
            return "legacy-interface";
        }

        return "dispatched-interface";
    }

    public Task<bool> KnownVirtualAsync()
    {
        DispatchFlagStrategy strategy = new BetaDispatchFlagStrategy(gateway);
        return strategy.EvaluateAsync("acceptance.dispatch.virtual", true);
    }

    public Task<bool> InterfaceFactoryAsync(bool useAlpha)
    {
        var strategy = CreateStrategy(useAlpha);
        return strategy.EvaluateAsync("acceptance.dispatch.factory", false);
    }

    public Task<bool> MethodGroupAsync()
    {
        Func<string, bool, Task<bool>> callback =
            new AlphaDispatchFlagStrategy(gateway).EvaluateAsync;
        return callback("acceptance.dispatch.method-group", false);
    }

    public Task<bool> LambdaAsync()
    {
        Func<string, bool, Task<bool>> callback =
            (key, fallback) => gateway.EvaluateAsync(key, fallback);
        return callback("acceptance.dispatch.lambda", true);
    }

    public Task<bool> DelegateParameterAsync() =>
        InvokeAsync(
            new BetaDispatchFlagStrategy(gateway).EvaluateAsync,
            "acceptance.dispatch.parameter",
            false);

    public Task<bool> DelegateFactoryAsync(bool useMethodGroup) =>
        CreateDelegate(useMethodGroup)("acceptance.dispatch.delegate-factory", true);

    // The source contains two known implementations, but an interface parameter can still
    // receive an external implementation at runtime. Known candidates must remain incomplete.
    public Task<bool> OpenInterfaceAsync(IDispatchFlagStrategy strategy) =>
        strategy.EvaluateAsync("acceptance.dispatch.open-interface", false);

    // Open Delegate boundary: one branch is a verified source target while the other
    // comes from an unknown caller. The known reference is Possible, never Definite.
    public Task<bool> OpenDelegateAsync(
        bool useKnown,
        Func<string, bool, Task<bool>> externalCallback) =>
        CreateOpenDelegate(useKnown, externalCallback)(
            "acceptance.dispatch.open-delegate",
            false);

    // Negative boundary: the factory body cannot be reduced to source object creations.
    public Task<bool> UnknownFactoryAsync() =>
        LoadExternalStrategy().EvaluateAsync("acceptance.dispatch.unknown-factory", false);

    private IDispatchFlagStrategy CreateStrategy(bool useAlpha) =>
        useAlpha
            ? new AlphaDispatchFlagStrategy(gateway)
            : new BetaDispatchFlagStrategy(gateway);

    private static Task<bool> InvokeAsync(
        Func<string, bool, Task<bool>> callback,
        string key,
        bool fallback) =>
        callback(key, fallback);

    private Func<string, bool, Task<bool>> CreateDelegate(bool useMethodGroup)
    {
        if (useMethodGroup)
        {
            return new AlphaDispatchFlagStrategy(gateway).EvaluateAsync;
        }

        return (key, fallback) => gateway.EvaluateAsync(key, fallback);
    }

    private Func<string, bool, Task<bool>> CreateOpenDelegate(
        bool useKnown,
        Func<string, bool, Task<bool>> externalCallback)
    {
        if (useKnown)
        {
            return new AlphaDispatchFlagStrategy(gateway).EvaluateAsync;
        }

        return externalCallback;
    }

    private static IDispatchFlagStrategy LoadExternalStrategy() =>
        throw new NotSupportedException("Runtime plugin loading is intentionally outside Base v3.");
}
