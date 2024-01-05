namespace WeatherForecastService.Metrics.Datadog
{
    public interface IDatadogMetrics
    {
        public void IncrementDatadogCounter(string name, Dictionary<string, string> tags);

        public void SetDatadogHistogram(string name, double value, Dictionary<string, string> tags);
    }
}
