using SnapshotScan.Flags;

namespace SnapshotScan.Dispatch;

/// <summary>
/// Base-v3 interface dispatch contract. Scanner must follow implementations by Symbol,
/// never because a type or method happens to contain words such as "flag" or "strategy".
/// </summary>
public interface IDispatchFlagStrategy
{
    Task<bool> EvaluateAsync(string key, bool fallback);
}

/// <summary>
/// Base-v3 abstract/virtual dispatch contract.
/// </summary>
public abstract class DispatchFlagStrategy
{
    protected DispatchFlagStrategy(OpenFeatureBooleanGateway gateway) => Gateway = gateway;

    protected OpenFeatureBooleanGateway Gateway { get; }

    public abstract Task<bool> EvaluateAsync(string key, bool fallback);
}

/// <summary>
/// One sealed implementation shared by interface, abstract and Delegate scenarios.
/// </summary>
public sealed class AlphaDispatchFlagStrategy : DispatchFlagStrategy, IDispatchFlagStrategy
{
    public AlphaDispatchFlagStrategy(OpenFeatureBooleanGateway gateway)
        : base(gateway)
    {
    }

    public override Task<bool> EvaluateAsync(string key, bool fallback) =>
        Gateway.EvaluateAsync(key, fallback);
}

/// <summary>
/// A second sealed implementation lets Factory/open-interface scenarios have two real targets.
/// </summary>
public sealed class BetaDispatchFlagStrategy : DispatchFlagStrategy, IDispatchFlagStrategy
{
    public BetaDispatchFlagStrategy(OpenFeatureBooleanGateway gateway)
        : base(gateway)
    {
    }

    public override Task<bool> EvaluateAsync(string key, bool fallback) =>
        Gateway.EvaluateAsync(key, fallback);
}
