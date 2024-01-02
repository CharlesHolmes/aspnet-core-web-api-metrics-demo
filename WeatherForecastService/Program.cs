using Amazon;
using Amazon.CloudWatch;
using Amazon.Extensions.NETCore.Setup;
using WeatherForecastService.Errors;
using WeatherForecastService.Latency;
using WeatherForecastService.Metrics;
using WeatherForecastService.Services;

namespace WeatherForecastService
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AddDefaultAWSOptions(new AWSOptions { Region = RegionEndpoint.USEast1 })
                .AddAWSService<IAmazonCloudWatch>()
                .AddSingleton<IFakeLatencySource, FakeLatencySource>()
                .AddSingleton<IFakeErrorSource, FakeErrorSource>()
                .AddSingleton<IWeatherService, WeatherService>();
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddSingleton<IWeatherForecastMetrics, WeatherForecastMetricsLoggerSinkForDebugging>();
            }
            else
            {
                builder.Services.AddSingleton<IWeatherForecastMetrics, WeatherForecastMetrics>()
                    .AddSingleton<ICloudwatchMetrics, CloudwatchMetrics>()
                    .AddSingleton<IDatadogMetrics, DatadogMetrics>();
            }

            var app = builder.Build();
            app.UseWeatherExceptionHandler();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthorization();
            app.MapControllers();
            await app.RunAsync();
        }
    }
}
