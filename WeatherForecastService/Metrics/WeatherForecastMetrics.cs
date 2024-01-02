namespace WeatherForecastService.Metrics
{
    public class WeatherForecastMetricsLoggerSinkForDebugging : IWeatherForecastMetrics
    {
        private readonly ILogger<WeatherForecastMetricsLoggerSinkForDebugging> _logger;

        public WeatherForecastMetricsLoggerSinkForDebugging(
            ILogger<WeatherForecastMetricsLoggerSinkForDebugging> logger)
        {
            _logger = logger;
        }

        public Task IncrementRequestCount(MetricTags tags)
        {
            _logger.LogInformation("Weather forecast request count was incremented.  Tags: {tags}", tags);
            return Task.CompletedTask;
        }

        public Task RecordRequestLatency(int milliseconds, MetricTags tags)
        {
            _logger.LogInformation("Weather forecast request latency (ms) was {latency}.  Tags: {tags}", milliseconds, tags);
            return Task.CompletedTask;
        }
    }
}
