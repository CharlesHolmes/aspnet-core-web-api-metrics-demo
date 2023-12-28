namespace WeatherForecastService.Metrics
{
    public interface IWeatherForecastMetrics
    {
        Task IncrementGaussianRequestCount();
        Task IncrementUniformRequestCount();
        Task RecordGaussianRequestLatency(int milliseconds);
        Task RecordUniformRequestLatency(int milliseconds);
    }
}