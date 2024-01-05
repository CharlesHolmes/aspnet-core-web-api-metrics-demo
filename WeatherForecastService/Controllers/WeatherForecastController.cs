using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherForecastService.Metrics;
using WeatherForecastService.Models;
using WeatherForecastService.Services;

namespace WeatherForecastService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : WeatherControllerBase
    {
        private const string NAME_FOR_METRICS = "Weather Forecast Request";
        private readonly IWeatherApiMetrics _metrics;
        private readonly IWeatherService _service;

        public WeatherForecastController(
            IWeatherApiMetrics metrics,
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
            Dictionary<string, string> tags = GetTags(GetUser(), city, includeRadar, includeSatellite);
            try
            {
                await _metrics.IncrementRequestCount(NAME_FOR_METRICS, tags);
                return await _service.GetWeatherForecasts(includeRadar, includeSatellite);
            }
            finally
            {
                stopwatch.Stop();
                await _metrics.RecordRequestLatency(NAME_FOR_METRICS, (int)stopwatch.ElapsedMilliseconds, tags);
            }
        }

        private static Dictionary<string, string> GetTags(string userName, string city, bool includeRadar, bool includeSatellite)
        {
            return new Dictionary<string, string>
            {
                { "UserName", userName },
                { "City", city },
                { "Include Radar", includeRadar.ToString() },
                { "Include Satellite", includeSatellite.ToString() }
            };
        }
    }
}
