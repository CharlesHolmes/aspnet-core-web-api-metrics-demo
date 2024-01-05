using WeatherForecastService.Metrics.Cloudwatch;
using WeatherForecastService.Metrics.Datadog;

namespace WeatherForecastService.Metrics
{
    public class WeatherApiMetrics : IWeatherApiMetrics
    {
        private readonly ICloudwatchMetrics _cloudwatchMetrics;
        private readonly IDatadogMetrics _datadogMetrics;

        public WeatherApiMetrics(
            ICloudwatchMetrics cloudwatchMetrics,
            IDatadogMetrics datadogMetrics)
        {
            _cloudwatchMetrics = cloudwatchMetrics;
            _datadogMetrics = datadogMetrics;
        }

        public async Task IncrementRequestCount(string requestName, Dictionary<string, string> tags)
        {
            await _cloudwatchMetrics.IncrementCloudWatchCounter($"{requestName} Count", tags);
            _datadogMetrics.IncrementDatadogCounter($"weather_api.{requestName}.count", tags);
        }

        public async Task RecordRequestLatency(string requestName, int milliseconds, Dictionary<string, string> tags)
        {
            await _cloudwatchMetrics.SetCloudWatchHistogram($"{requestName} Latency", milliseconds, tags);
            _datadogMetrics.SetDatadogHistogram($"weather_api.{requestName}.latency", milliseconds, tags);
        }
    }
}
