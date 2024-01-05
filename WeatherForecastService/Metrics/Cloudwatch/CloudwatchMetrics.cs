using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;

namespace WeatherForecastService.Metrics.Cloudwatch
{
    public class CloudwatchMetrics : ICloudwatchMetrics
    {
        private readonly IAmazonCloudWatch _cloudWatch;

        public CloudwatchMetrics(IAmazonCloudWatch cloudWatch)
        {
            _cloudWatch = cloudWatch;
        }

        public Task IncrementCloudWatchCounter(string name, Dictionary<string, string> tags)
        {
            List<Dimension> dimensions = GetCloudwatchDimensions(tags);
            return EmitCloudWatchMetric(name, StandardUnit.Count, 1, dimensions);
        }

        public Task SetCloudWatchHistogram(string name, double value, Dictionary<string, string> tags)
        {
            List<Dimension> dimensions = GetCloudwatchDimensions(tags);
            return EmitCloudWatchMetric(name, StandardUnit.Milliseconds, value, dimensions);
        }

        private List<Dimension> GetCloudwatchDimensions(Dictionary<string, string> tags)
        {
            return tags.Select(kvp => new Dimension { Name = kvp.Key, Value = kvp.Value }).ToList();
        }

        private Task EmitCloudWatchMetric(string name, StandardUnit unit, double value, List<Dimension> dimensions)
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
                        StorageResolution = 1,
                        Dimensions = dimensions
                    }
                }
            });
        }
    }
}
