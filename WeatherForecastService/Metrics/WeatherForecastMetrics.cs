using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using System.Diagnostics.Metrics;

namespace WeatherForecastService.Metrics
{
    public class WeatherForecastMetrics : IWeatherForecastMetrics
    {
        private readonly IAmazonCloudWatch _cloudWatch;

        public WeatherForecastMetrics(IAmazonCloudWatch cloudWatch)
        {
            _cloudWatch = cloudWatch;
            //TODO (cholmes) send to datadog as well
        }

        public async Task IncrementGaussianRequestCount()
        {
            await _cloudWatch.PutMetricDataAsync(new PutMetricDataRequest
            {
                Namespace = "WeatherForecastMetricsDemo",
                MetricData = new List<MetricDatum>
                {
                    new MetricDatum
                    {
                        MetricName = "Request Count (Gaussian Latency)",
                        Unit = StandardUnit.Count,
                        Value = 1,
                        StorageResolution = 1
                    }
                }
            });
        }

        public async Task IncrementUniformRequestCount()
        {
            await _cloudWatch.PutMetricDataAsync(new PutMetricDataRequest
            {
                Namespace = "WeatherForecastMetricsDemo",
                MetricData = new List<MetricDatum>
                {
                    new MetricDatum
                    {
                        MetricName = "Request Count (Uniform Latency)",
                        Unit = StandardUnit.Count,
                        Value = 1,
                        StorageResolution = 1
                    }
                }
            });
        }

        public async Task RecordGaussianRequestLatency(int milliseconds)
        {
            await _cloudWatch.PutMetricDataAsync(new PutMetricDataRequest
            {
                Namespace = "WeatherForecastMetricsDemo",
                MetricData = new List<MetricDatum>
                {
                    new MetricDatum
                    {
                        MetricName = "Request Latency (Gaussian Distribution)",
                        Unit = StandardUnit.Milliseconds,
                        Value = milliseconds,
                        StorageResolution = 1
                    }
                }
            });
        }

        public async Task RecordUniformRequestLatency(int milliseconds)
        {
            await _cloudWatch.PutMetricDataAsync(new PutMetricDataRequest
            {
                Namespace = "WeatherForecastMetricsDemo",
                MetricData = new List<MetricDatum>
                {
                    new MetricDatum
                    {
                        MetricName = "Request Latency (Uniform Distribution)",
                        Unit = StandardUnit.Milliseconds,
                        Value = milliseconds,
                        StorageResolution = 1
                    }
                }
            });
        }
    }
}
