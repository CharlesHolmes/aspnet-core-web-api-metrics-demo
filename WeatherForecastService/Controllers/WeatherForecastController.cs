using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
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
            MetricTags tags = GetTags(GetUser(), city, includeRadar, includeSatellite);
            try
            {
                await _metrics.IncrementRequestCount(tags);
                return await _service.GetWeatherForecasts(includeRadar, includeSatellite);
            }
            finally
            {
                stopwatch.Stop();
                await _metrics.RecordRequestLatency((int)stopwatch.ElapsedMilliseconds, tags);
            }
        }

        private string GetUser()
        {
            if (Request.Headers.TryGetValue("weather-user", out StringValues userHeader)
                && userHeader.Count == 1)
            {
                return userHeader.Single();
            }
            else
            {
                return "n/a";
            }
        }

        private static MetricTags GetTags(string userName, string city, bool includeRadar, bool includeSatellite)
        {
            return new MetricTags
            {
                UserName = userName,
                City = city,
                IncludeRadar = includeRadar,
                IncludeSatellite = includeSatellite
            };
        }
    }
}
