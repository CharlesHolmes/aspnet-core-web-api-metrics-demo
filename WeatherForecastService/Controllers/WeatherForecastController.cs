using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherForecastService.Metrics;
using WeatherForecastService.Models;
using WeatherForecastService.Services;

namespace WeatherForecastService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastMetrics _metrics;
        private readonly IWeatherService _service;

        public WeatherForecastController(
            IWeatherForecastMetrics metrics,
            IWeatherService service)
        {
            _metrics = metrics;
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(string city, bool includeRadar, bool includeSatellite)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            MetricTags tags = GetTags(city, includeRadar, includeSatellite);
            try
            {
                await _metrics.IncrementRequestCount(tags);
                return await _service.GetWeatherForecasts();
            }
            finally
            {
                stopwatch.Stop();
                await _metrics.RecordRequestLatency((int)stopwatch.ElapsedMilliseconds, tags);
            }
        }

        private static MetricTags GetTags(string city, bool includeRadar, bool includeSatellite)
        {
            return new MetricTags
            {
                City = city,
                IncludeRadar = includeRadar,
                IncludeSatellite = includeSatellite
            };
        }
    }
}
