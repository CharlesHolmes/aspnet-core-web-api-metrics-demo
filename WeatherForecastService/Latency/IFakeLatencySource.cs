namespace WeatherForecastService.Latency
{
    public interface IFakeLatencySource
    {
        Task DoSlowOperation();
    }
}
