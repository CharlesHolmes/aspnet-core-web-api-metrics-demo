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

        public async Task IncrementRequestCount()
        {
            var cloudWatchRequest = IncrementCloudWatchCounter("Weather Forecast Request Count");
            IncrementDatadogCounter("weather_api.forecast_requests.count");
            await cloudWatchRequest;
        }

        public async Task RecordRequestLatency(int milliseconds)
        {
            var cloudWatchRequest = SetCloudWatchHistogram("Weather Forecast Request Latency", milliseconds);
            SetDatadogHistogram("weather_api.forecast_requests.latency", milliseconds);
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
