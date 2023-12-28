namespace WeatherForecastService.Latency
{
    public interface IFakeLatency
    {
        int GetGaussianLatencyMs();
        int GetUniformLatencyMs();
    }
}
