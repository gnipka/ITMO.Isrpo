namespace Workout.Metrics;

public static class MetricsConstants
{
    public static class Responses
    {

        /// <summary>
        /// Всего ответов.
        /// </summary>
        public const string TOTAL_RESPONSES = "total_responses";

        /// <summary>
        /// Intenral server error.
        /// </summary>
        public const string SERVER_500_RESPONSE_COUNTER = "server_500_response_counter";

        /// <summary>
        /// Not found.
        /// </summary>
        public const string SERVER_404_RESPONSE_COUNTER = "server_404_response_counter";

        /// <summary>
        /// Forbidden.
        /// </summary>
        public const string SERVER_403_RESPONSE_COUNTER = "server_403_response_counter";

        /// <summary>
        /// Unauthorized.
        /// </summary>
        public const string SERVER_401_RESPONSE_COUNTER = "server_401_response_counter";

        /// <summary>
        /// Bad request.
        /// </summary>
        public const string SERVER_400_RESPONSE_COUNTER = "server_400_response_counter";

        /// <summary>
        /// Ответы сервера 300+;
        /// </summary>
        public const string SERVER_300_RESPONSE_COUNTER = "server_300_response_counter";

        /// <summary>
        /// Ответы сервера 200+;
        /// </summary>
        public const string SERVER_200_RESPONSE_COUNTER = "server_200_response_counter";
    }
    
    public static class Requests
    {

        /// <summary>
        /// Количество соединений с сервером.
        /// </summary>
        public const string ACTIVE_REQUSET_COUNTER = "active_requset_counter";

        /// <summary>
        /// Продолжительность исполнения запроса API.
        /// </summary>
        public const string REQUEST_DURATION_SECONDS = "request_duration_seconds";
    }
}