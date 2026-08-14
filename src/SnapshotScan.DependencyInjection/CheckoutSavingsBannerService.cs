using OpenFeature;

namespace SnapshotScan.DependencyInjection;

/// <summary>
/// The single-feature PR sample. The savings banner business implementation is
/// reachable only when the String flag selects variation "v2".
/// </summary>
public sealed class CheckoutSavingsBannerService(IFeatureClient client)
{
    public async Task<CheckoutSavingsBanner?> BuildAsync(
        decimal subtotal,
        decimal discountAmount)
    {
        var variation = await client.GetStringValueAsync(
            "acceptance.pr.checkout-savings-banner",
            "v1");

        if (variation == "v2")
        {
            var normalizedSubtotal = Math.Max(0m, subtotal);
            var savings = Math.Clamp(discountAmount, 0m, normalizedSubtotal);
            var payableTotal = normalizedSubtotal - savings;

            return new CheckoutSavingsBanner(
                "Your checkout savings",
                $"You saved {savings:0.00} on this order.",
                payableTotal);
        }

        return null;
    }
}

public sealed record CheckoutSavingsBanner(
    string Title,
    string Message,
    decimal PayableTotal);
