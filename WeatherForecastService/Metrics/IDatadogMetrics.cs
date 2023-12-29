namespace WeatherForecastService.Metrics
{
    public interface IDatadogMetrics
    {
        public void IncrementDatadogCounter(string name, MetricTags tags);

        public void SetDatadogHistogram(string name, double value, MetricTags tags);
    }
}
