using Microsoft.Extensions.Configuration;

namespace SnapshotScan.Configuration;

/// <summary>
/// Base-v4 Options shape. The nested property and ConfigurationKeyName attribute make
/// the JSON-to-POCO mapping visible without relying on naming conventions in the scanner.
/// </summary>
public sealed class FeatureReviewOptions
{
    public CheckoutFlagOptions Checkout { get; set; } = new();
}

public sealed class CheckoutFlagOptions
{
    [ConfigurationKeyName("flag-key")]
    public string Key { get; set; } = string.Empty;

    public bool DefaultValue { get; set; }
}
