using Amazon;
using Amazon.CloudWatch;
using Amazon.Extensions.NETCore.Setup;
using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using WeatherForecastService.Errors;
using WeatherForecastService.Latency;
using WeatherForecastService.Metrics;
using WeatherForecastService.Metrics.Cloudwatch;
using WeatherForecastService.Metrics.Datadog;
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
                .AddSingleton<IWeatherService, WeatherService>()
                .AddSingleton<ISunMoonTimesService, SunMoonTimesService>();
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddSingleton<IWeatherApiMetrics, WeatherApiMetricsLoggerSinkForDebugging>();
            }
            else
            {
                builder.Services.AddSingleton<IWeatherApiMetrics, WeatherApiMetrics>()
                    .AddSingleton<ICloudwatchMetrics, CloudwatchMetrics>()
                    .AddSingleton<IDatadogMetrics, DatadogMetrics>();
                AWSSDKHandler.RegisterXRayForAllServices();
                AWSXRayRecorder.InitializeInstance(recorder: new AWSXRayRecorderBuilder().Build());
            }

            var app = builder.Build();
            app.UseXRay("WeatherForecastService");
            app.UseWeatherExceptionHandler();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthorization();
            app.MapControllers();
            await app.RunAsync();
        }
    }
}
