namespace WeatherForecastService.Metrics
{
    public record MetricTags
    {
        public string City { get; init; }
        public bool IncludeRadar { get; init; }
        public bool IncludeSatellite { get; init; }
    }
}
