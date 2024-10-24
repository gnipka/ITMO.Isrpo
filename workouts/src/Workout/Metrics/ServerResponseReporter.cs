using Prometheus;

namespace Workout.Metrics
{
    /// <summary>
    /// Ответы сервера по HTTP кодам
    /// </summary>
    public class ServerResponseReporter
    {
        private readonly Counter _totalRequestCounter = Prometheus.Metrics.CreateCounter(MetricsConstants.Responses.TOTAL_RESPONSES, "Общее количество обработанных запросов.");

        private readonly Counter _500ResponseCounter = Prometheus.Metrics.CreateCounter(MetricsConstants.Responses.SERVER_500_RESPONSE_COUNTER,"Количество ответов с кодом 500+.");

        private readonly Counter _404ResponseCounter = Prometheus.Metrics.CreateCounter(MetricsConstants.Responses.SERVER_404_RESPONSE_COUNTER,"Количество ответов с кодом 404.");

        private readonly Counter _403ResponseCounter = Prometheus.Metrics.CreateCounter(MetricsConstants.Responses.SERVER_403_RESPONSE_COUNTER,"Количество ответов с кодом 403.");

        private readonly Counter _401ResponseCounter = Prometheus.Metrics.CreateCounter(MetricsConstants.Responses.SERVER_401_RESPONSE_COUNTER,"Количество ответов с кодом 401.");

        private readonly Counter _400ResponseCounter = Prometheus.Metrics.CreateCounter(MetricsConstants.Responses.SERVER_400_RESPONSE_COUNTER, "Количество ответов с кодом 400.");

        private readonly Counter _300ResponseCounter = Prometheus.Metrics.CreateCounter(MetricsConstants.Responses.SERVER_300_RESPONSE_COUNTER, "Количество ответов с кодом 300+.");

        private readonly Counter _200ResponseCounter = Prometheus.Metrics.CreateCounter(MetricsConstants.Responses.SERVER_200_RESPONSE_COUNTER, "Количество ответов с кодом 200+.");

        public void RegisterRequest(int statusCode)
        {
            _totalRequestCounter.Inc();

            var counter = statusCode switch
            {
                >= 200 and < 300 => _200ResponseCounter,
                >= 300 and < 400 => _300ResponseCounter,
                401 => _401ResponseCounter,
                403 => _403ResponseCounter,
                404 => _404ResponseCounter,
                <= 400 and < 500 => _400ResponseCounter,
                <= 500 => _500ResponseCounter,
                _ => _500ResponseCounter
            };

            counter.Inc();
        }
    }
}