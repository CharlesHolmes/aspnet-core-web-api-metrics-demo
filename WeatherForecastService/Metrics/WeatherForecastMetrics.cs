using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using StatsdClient;

namespace WeatherForecastService.Metrics
{
    public class WeatherForecastMetrics : IWeatherForecastMetrics
    {
        private readonly ILogger<WeatherForecastMetrics> _logger;
        private readonly IAmazonCloudWatch _cloudWatch;

        public WeatherForecastMetrics(
            IAmazonCloudWatch cloudWatch,
            ILogger<WeatherForecastMetrics> logger)
        {
            _cloudWatch = cloudWatch;
            _logger = logger;
        }

        public async Task IncrementGaussianRequestCount()
        {
            var cloudWatchRequest = IncrementCloudWatchCounter("Request Count (Gaussian Latency)");
            IncrementDatadogCounter("weather_api.gaussian_requests.count");
            await cloudWatchRequest;
        }

        public async Task IncrementUniformRequestCount()
        {
            var cloudWatchRequest = IncrementCloudWatchCounter("Request Count (Uniform Latency)");
            IncrementDatadogCounter("weather_api.uniform_requests.count");
            await cloudWatchRequest;
        }

        public async Task RecordGaussianRequestLatency(int milliseconds)
        {
            var cloudWatchRequest = SetCloudWatchHistogram("Request Latency (Gaussian Distribution)", milliseconds);
            SetDatadogHistogram("weather_api.gaussian_requests.latency", milliseconds);
            await cloudWatchRequest;
        }

        public async Task RecordUniformRequestLatency(int milliseconds)
        {
            var cloudWatchRequest = SetCloudWatchHistogram("Request Latency (Uniform Distribution)", milliseconds);
            SetDatadogHistogram("weather_api.uniform_requests.latency", milliseconds);
            await cloudWatchRequest;
        }

        private Task IncrementCloudWatchCounter(string name)
        {
            return EmitCloudWatchMetric(name, StandardUnit.Count, 1);
        }

        private Task SetCloudWatchHistogram(string name, double value)
        {
            return EmitCloudWatchMetric(name, StandardUnit.Milliseconds, value);
        }

        private Task EmitCloudWatchMetric(string name, StandardUnit unit, double value)
        {
            return _cloudWatch.PutMetricDataAsync(new PutMetricDataRequest
            {
                Namespace = "WeatherForecastMetricsDemo",
                MetricData = new List<MetricDatum>
                {
                    new MetricDatum
                    {
                        MetricName = name,
                        Unit = unit,
                        Value = value,
                        StorageResolution = 1
                    }
                }
            });
        }

        private void IncrementDatadogCounter(string name)
        {
            EmitToDatadog(client => client.Increment(name));
        }

        private void SetDatadogHistogram(string name, double value)
        {
            EmitToDatadog(client => client.Histogram(name, value));
        }

        private void EmitToDatadog(Action<DogStatsdService> action)
        {
            using (var dogStats = new DogStatsdService())
            {
                var config = new StatsdConfig
                {
                    StatsdServerName = "127.0.0.1",
                    StatsdPort = 8125
                };

                bool configured = dogStats.Configure(config, (e) =>
                {
                    _logger.LogCritical(e, "Cannot initialize dogstatsd.");
                    throw new InvalidOperationException("Cannot initialize dogstatsd.", e);
                });

                if (configured)
                {
                    action(dogStats);
                }
            }
        }
    }
}
