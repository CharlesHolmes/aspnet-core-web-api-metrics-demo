namespace WeatherForecastService.Metrics
{
    public interface IWeatherApiMetrics
    {
        Task IncrementRequestCount(string requestName, Dictionary<string, string> tags);
        Task RecordRequestLatency(string requestName, int milliseconds, Dictionary<string, string> tags);
    }
}
