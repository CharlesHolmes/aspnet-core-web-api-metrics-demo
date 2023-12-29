namespace WeatherForecastService.Metrics
{
    public interface ICloudwatchMetrics
    {
        Task IncrementCloudWatchCounter(string name, MetricTags tags);

        Task SetCloudWatchHistogram(string name, double value, MetricTags tags);
    }
}
