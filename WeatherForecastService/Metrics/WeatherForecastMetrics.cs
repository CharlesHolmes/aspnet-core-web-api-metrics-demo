namespace WeatherForecastService.Metrics
{
    public class WeatherForecastMetrics : IWeatherForecastMetrics
    {
        private readonly ICloudwatchMetrics _cloudwatchMetrics;
        private readonly IDatadogMetrics _datadogMetrics;

        public WeatherForecastMetrics(
            ICloudwatchMetrics cloudwatchMetrics,
            IDatadogMetrics datadogMetrics)
        {
            _cloudwatchMetrics = cloudwatchMetrics;
            _datadogMetrics = datadogMetrics;
        }

        public async Task IncrementRequestCount(MetricTags tags)
        {
            await _cloudwatchMetrics.IncrementCloudWatchCounter("Weather Forecast Request Count", tags);
            _datadogMetrics.IncrementDatadogCounter("weather_api.forecast_requests.count", tags);
        }

        public async Task RecordRequestLatency(int milliseconds, MetricTags tags)
        {
            await _cloudwatchMetrics.SetCloudWatchHistogram("Weather Forecast Request Latency", milliseconds, tags);
            _datadogMetrics.SetDatadogHistogram("weather_api.forecast_requests.latency", milliseconds, tags);
        }
    }
}
