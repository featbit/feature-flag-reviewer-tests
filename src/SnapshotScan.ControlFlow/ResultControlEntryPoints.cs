using SnapshotScan.App;

namespace SnapshotScan.ControlFlow;

/// <summary>
/// Base-v2 scenarios where an evaluation result is stored before it controls business code.
/// </summary>
public sealed class ResultControlEntryPoints(
    BaseEntryPoints baseEntryPoints,
    ConditionalEntryPoints conditionalEntryPoints)
{
    // The result crosses the SearchAsync return boundary, is assigned to a local,
    // and is used later after an unrelated statement.
    public async Task<string> UseSearchResultAsync()
    {
        var searchV3Enabled = await baseEntryPoints.SearchAsync();
        var selectedPipeline = "legacy-search";

        if (searchV3Enabled)
        {
            selectedPipeline = "search-v3";
        }

        return selectedPipeline;
    }

    // The same runtime branch that selects the key/default pair is passed into the
    // evaluation. Its returned value is then stored and used by a negated guard.
    public async Task<string> UseSelectedResultAsync(bool useBeta)
    {
        var selectedFlagEnabled = await conditionalEntryPoints.SelectKeyAndDefaultAsync(useBeta);

        if (!selectedFlagEnabled)
        {
            return "selection-disabled";
        }

        return "selection-enabled";
    }

    // The fixed wrapper result is stored once and controls explicit if/else blocks.
    public async Task<string> UseFixedResultAsync()
    {
        var fixedFlagEnabled = await baseEntryPoints.FixedAsync();

        if (fixedFlagEnabled)
        {
            return "fixed-enabled";
        }
        else
        {
            return "fixed-disabled";
        }
    }
}

/// <summary>
/// Adds another ordinary static call boundary before the result reaches its consumer.
/// </summary>
public sealed class CheckoutDecisionService(BaseEntryPoints baseEntryPoints)
{
    public Task<bool> IsCheckoutV2EnabledAsync() =>
        baseEntryPoints.CheckoutAsync();
}

/// <summary>
/// Base-v2 multi-hop result propagation: OpenFeature -> wrappers -> App entry point
/// -> decision service -> local variable -> early-return guard -> business code.
/// </summary>
public sealed class CheckoutEndpoint(CheckoutDecisionService decisions)
{
    public async Task<string> ExecuteAsync()
    {
        var checkoutV2Enabled = await decisions.IsCheckoutV2EnabledAsync();

        if (!checkoutV2Enabled)
        {
            return "legacy-checkout";
        }

        return "checkout-v2";
    }
}
