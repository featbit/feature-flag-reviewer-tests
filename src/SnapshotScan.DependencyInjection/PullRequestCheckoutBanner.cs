using OpenFeature;

namespace SnapshotScan.DependencyInjection;

/// <summary>
/// Head-only PR scenario: a new direct OpenFeature evaluation with an explicit
/// business result, so the Differ should report one added Key and Reference.
/// </summary>
public sealed class PullRequestCheckoutBanner(IFeatureClient client)
{
    public async Task<string> GetBannerAsync()
    {
        var enabled = await client.GetBooleanValueAsync(
            "acceptance.pr.checkout-banner",
            false);

        return enabled
            ? "pr-checkout-banner"
            : "checkout-banner-hidden";
    }
}
