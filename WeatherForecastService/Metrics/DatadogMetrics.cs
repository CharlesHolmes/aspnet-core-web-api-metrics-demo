using StatsdClient;

namespace WeatherForecastService.Metrics
{
    public sealed class DatadogMetrics : IDatadogMetrics, IDisposable
    {
        private readonly ILogger<DatadogMetrics> _logger;
        private readonly DogStatsdService _client;

        public DatadogMetrics(ILogger<DatadogMetrics> logger)
        {
            _logger = logger;
            _client = GetDatadogClient();
        }

        public void IncrementDatadogCounter(string name, MetricTags tags)
        {
            string[] datadogTags = GetDatadogTags(tags);
            _client.Increment(name, tags: datadogTags);

        }

        public void SetDatadogHistogram(string name, double value, MetricTags tags)
        {
            string[] datadogTags = GetDatadogTags(tags);
            _client.Histogram(name, value, tags: datadogTags);
        }

        public void Dispose()
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }

        private string[] GetDatadogTags(MetricTags tags)
        {
            return new string[]
            {
                $"username:{tags.UserName}",
                $"city:{tags.City}",
                $"include_radar:{tags.IncludeRadar}",
                $"include_satellite:{tags.IncludeSatellite}"
            };
        }

        private DogStatsdService GetDatadogClient()
        {
            var client = new DogStatsdService();
            var config = new StatsdConfig
            {
                StatsdServerName = "127.0.0.1",
                StatsdPort = 8125
            };

            bool configured = client.Configure(config, (e) =>
            {
                _logger.LogCritical(e, "Cannot initialize dogstatsd.");
                throw new InvalidOperationException("Cannot initialize dogstatsd.", e);
            });

            if (configured)
            {
                return client;
            }
            else
            {
                throw new InvalidOperationException("Cannot initialize dogstatsd.");
            }
        }
    }
}
