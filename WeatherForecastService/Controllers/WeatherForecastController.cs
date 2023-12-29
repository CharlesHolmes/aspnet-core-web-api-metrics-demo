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
        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                await _metrics.IncrementRequestCount();
                return await _service.GetWeatherForecasts();
            }
            finally
            {
                stopwatch.Stop();
                await _metrics.RecordRequestLatency((int)stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
