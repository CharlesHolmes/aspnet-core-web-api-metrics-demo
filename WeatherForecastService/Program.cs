using WeatherForecastService.Metrics;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.CloudWatch;
using WeatherForecastService.Latency;
using WeatherForecastService.Errors;

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
                .AddSingleton<IWeatherForecastMetrics, WeatherForecastMetrics>()
                .AddSingleton<IFakeLatencySource, FakeLatencySource>();
            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthorization();
            app.MapControllers();
            app.UseWeatherExceptionHandler();
            await app.RunAsync();
        }
    }
}
