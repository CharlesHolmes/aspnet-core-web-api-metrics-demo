using WeatherForecastService.Metrics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherForecastService.Latency;

namespace WeatherForecastService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastMetrics _metrics;
        private readonly IFakeLatency _fakeLatency;
        private readonly Random _random;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IWeatherForecastMetrics metrics,
            IFakeLatency fakeLatency)
        {
            _logger = logger;
            _metrics = metrics;
            _fakeLatency = fakeLatency;
            _random = new Random();
        }

        private IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            if (_random.Next(0, 1000) == 999)
            {
                throw new WeatherForecastException();
            }

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTimeOffset.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetWithGaussianLatency")]
        public async Task<IEnumerable<WeatherForecast>> GetWithGaussianLatency()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var simulatedDelay = _fakeLatency.GetGaussianLatencyMs();
            try
            {
                await _metrics.IncrementGaussianRequestCount();
                await Task.Delay(simulatedDelay);
                return GetWeatherForecast();
            }
            finally
            {
                stopwatch.Stop();
                await _metrics.RecordGaussianRequestLatency((int)stopwatch.ElapsedMilliseconds);
                _logger.LogDebug(
                    "Served a weather forecast request with {simulatedDelayMs} milliseconds of Gaussian-distributed latency.  Total request time: {overallLatencyMs}.",
                    simulatedDelay,
                    stopwatch.ElapsedMilliseconds);
            }
        }

        [HttpGet("GetWithUniformLatency")]
        public async Task<IEnumerable<WeatherForecast>> GetWithUniformLatency()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var simulatedDelay = _fakeLatency.GetUniformLatencyMs();
            try
            {
                await _metrics.IncrementUniformRequestCount();
                await Task.Delay(simulatedDelay);
                return GetWeatherForecast();
            }
            finally
            {
                stopwatch.Stop();
                await _metrics.RecordUniformRequestLatency((int)stopwatch.ElapsedMilliseconds);
                _logger.LogDebug(
                    "Served a weather forecast request with {simulatedDelayMs} milliseconds of uniformly distributed latency.  Total request time: {overallLatencyMs}.",
                    simulatedDelay,
                    stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
