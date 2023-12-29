namespace WeatherForecastService.Metrics
{
    public interface IWeatherForecastMetrics
    {
        Task IncrementRequestCount();
        Task RecordRequestLatency(int milliseconds);
    }
}