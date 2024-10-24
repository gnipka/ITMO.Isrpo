using Microsoft.Extensions.DependencyInjection;
using Prometheus.SystemMetrics;
using Prometheus.SystemMetrics.Collectors;
using Workout.Metrics;

namespace Workout.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddMetrics(this IServiceCollection services)
    {
        services.AddSystemMetrics(registerDefaultCollectors: false);

        services.AddSystemMetricCollector<DiskCollector>();
        services.AddSystemMetricCollector<LinuxCpuUsageCollector>();
        services.AddSystemMetricCollector<LinuxMemoryCollector>();
        services.AddSystemMetricCollector<NetworkCollector>();
        services.AddSystemMetricCollector<LoadAverageCollector>();
        
        services.AddSingleton<ServerResponseReporter>();
        services.AddSingleton<ServerResponseTimeReporter>();
        services.AddSingleton<ServerActiveRequestsReporter>();
    }
}