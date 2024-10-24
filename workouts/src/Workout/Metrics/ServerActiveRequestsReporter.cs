using Prometheus;

namespace Workout.Metrics;

/// <summary>
/// Количество активных соединений с сервером
/// </summary>
public class ServerActiveRequestsReporter
{
    private readonly Gauge _activeRequestCounter;

    public ServerActiveRequestsReporter()
    {
        _activeRequestCounter = Prometheus.Metrics.CreateGauge(MetricsConstants.Requests.ACTIVE_REQUSET_COUNTER, "Количество активных запросов.");
    }

    public void HoldRequest()
    {
        _activeRequestCounter.Inc();
    }
    public void ReleseRequest()
    {
        _activeRequestCounter.Dec();
    }
}