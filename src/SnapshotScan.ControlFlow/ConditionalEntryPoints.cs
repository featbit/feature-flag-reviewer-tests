using SnapshotScan.Flags;

namespace SnapshotScan.ControlFlow;

/// <summary>
/// Base-v2 input-side control flow: runtime branches select correlated key/default values,
/// and an inline evaluation result directly controls a business block.
/// </summary>
public sealed class ConditionalEntryPoints(
    OpenFeatureBooleanGateway gateway,
    ParameterizedFlagWrapper wrapper)
{
    // One branch chooses both values. The only valid pairs are beta/false and stable/true.
    public Task<bool> SelectKeyAndDefaultAsync(bool useBeta)
    {
        string key;
        bool fallback;
        if (useBeta)
        {
            key = "acceptance.control.beta";
            fallback = false;
        }
        else
        {
            key = "acceptance.control.stable";
            fallback = true;
        }

        return gateway.EvaluateAsync(key, fallback);
    }

    // The evaluation result directly controls this complete if block.
    public async Task<string> UseFixedFlagInControlFlowAsync()
    {
        if (await wrapper.EvaluateFixedAsync())
        {
            return "new-checkout";
        }

        return "legacy-checkout";
    }
}

