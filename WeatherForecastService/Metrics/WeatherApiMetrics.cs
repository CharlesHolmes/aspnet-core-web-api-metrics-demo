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
            string processed = requestName.Replace(" ", "_").Replace("/", "_").ToLower();
            _datadogMetrics.IncrementDatadogCounter($"weather_api.{processed}.count", tags);
        }

        public async Task RecordRequestLatency(string requestName, int milliseconds, Dictionary<string, string> tags)
        {
            await _cloudwatchMetrics.SetCloudWatchHistogram($"{requestName} Latency", milliseconds, tags);
            string processed = requestName.Replace(" ", "_").Replace("/", "_").ToLower();
            _datadogMetrics.SetDatadogHistogram($"weather_api.{processed}.latency", milliseconds, tags);
        }
    }
}
