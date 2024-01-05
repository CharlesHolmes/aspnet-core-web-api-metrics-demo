namespace WeatherForecastService.Metrics.Cloudwatch
{
    public interface ICloudwatchMetrics
    {
        Task IncrementCloudWatchCounter(string name, Dictionary<string, string> tags);

        Task SetCloudWatchHistogram(string name, double value, Dictionary<string, string> tags);
    }
}
