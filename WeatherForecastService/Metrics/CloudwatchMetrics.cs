using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;

namespace WeatherForecastService.Metrics
{
    public class CloudwatchMetrics : ICloudwatchMetrics
    {
        private readonly IAmazonCloudWatch _cloudWatch;

        public CloudwatchMetrics(IAmazonCloudWatch cloudWatch)
        {
            _cloudWatch = cloudWatch;
        }

        public Task IncrementCloudWatchCounter(string name, MetricTags tags)
        {
            List<Dimension> dimensions = GetCloudwatchDimensions(tags);
            return EmitCloudWatchMetric(name, StandardUnit.Count, 1, dimensions);
        }

        public Task SetCloudWatchHistogram(string name, double value, MetricTags tags)
        {
            List<Dimension> dimensions = GetCloudwatchDimensions(tags);
            return EmitCloudWatchMetric(name, StandardUnit.Milliseconds, value, dimensions);
        }

        private List<Dimension> GetCloudwatchDimensions(MetricTags tags)
        {
            return new List<Dimension>
            {
                new Dimension
                {
                    Name = "City",
                    Value = tags.City
                },
                new Dimension
                {
                    Name = "IncludeRadar",
                    Value = tags.IncludeRadar.ToString()
                },
                new Dimension
                {
                    Name = "IncludeSatellite",
                    Value = tags.IncludeSatellite.ToString()
                }
            };
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
