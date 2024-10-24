using System;
using Prometheus;

namespace Workout.Metrics;

/// <summary>
/// Время ответа сервера
/// </summary>
public class ServerResponseTimeReporter
{
    private readonly Histogram _responseTimeHistogram = Prometheus.Metrics.CreateHistogram(MetricsConstants.Requests.REQUEST_DURATION_SECONDS,"Время, затраченное на обработку запроса.", 
        new HistogramConfiguration
        {
            Buckets = Histogram.LinearBuckets(0, 1, 10),
            LabelNames = new[] { "controller", "method" }
        });

    public void RegisterResponseTime(string controller, string method, TimeSpan elapsed)
    {
        _responseTimeHistogram.Labels(controller, method).Observe(elapsed.TotalSeconds);
    }
}