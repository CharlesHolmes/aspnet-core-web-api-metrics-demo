using System.Text.Json;

namespace WeatherForecastService.Metrics
{
    public class WeatherApiMetricsLoggerSinkForDebugging : IWeatherApiMetrics
    {
        private readonly ILogger<WeatherApiMetricsLoggerSinkForDebugging> _logger;

        public WeatherApiMetricsLoggerSinkForDebugging(ILogger<WeatherApiMetricsLoggerSinkForDebugging> logger)
        {
            _logger = logger;
        }

        public Task IncrementRequestCount(string requestName, Dictionary<string, string> tags)
        {
            _logger.LogInformation($"{requestName} count was incremented.  Tags: {{tags}}", SerializeTagDictionary(tags));
            return Task.CompletedTask;
        }

        public Task RecordRequestLatency(string requestName, int milliseconds, Dictionary<string, string> tags)
        {
            _logger.LogInformation($"{requestName} latency (ms) was {{latency}}.  Tags: {{tags}}", milliseconds, SerializeTagDictionary(tags));
            return Task.CompletedTask;
        }

        private static string SerializeTagDictionary(Dictionary<string, string> tags)
        {
            return JsonSerializer.Serialize(tags, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
    }
}
