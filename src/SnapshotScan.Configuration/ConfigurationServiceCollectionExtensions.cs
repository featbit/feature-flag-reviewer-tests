using Microsoft.Extensions.DependencyInjection;

namespace SnapshotScan.Configuration;

/// <summary>
/// A real OptionsBuilder binding. SnapshotScanner must connect the static section to the
/// nested POCO properties consumed from another project.
/// </summary>
public static class ConfigurationServiceCollectionExtensions
{
    public static IServiceCollection AddSnapshotConfiguration(this IServiceCollection services)
    {
        services.AddOptions<FeatureReviewOptions>()
            .BindConfiguration("FeatureReview:Options");
        return services;
    }
}
