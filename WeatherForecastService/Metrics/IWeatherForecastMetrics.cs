namespace WeatherForecastService.Metrics
{
    public interface IWeatherForecastMetrics
    {
        Task IncrementRequestCount(MetricTags tags);
        Task RecordRequestLatency(int milliseconds, MetricTags tags);
    }
}