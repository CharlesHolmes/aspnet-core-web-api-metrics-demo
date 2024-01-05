using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherForecastService.Metrics;
using WeatherForecastService.Models;
using WeatherForecastService.Services;

namespace WeatherForecastService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TideTimesController : WeatherControllerBase
    {
        private const string NAME_FOR_METRICS = "Tide Times Request";
        private readonly ITideTimesService _service;
        private readonly IWeatherApiMetrics _metrics;

        public TideTimesController(
            ITideTimesService service,
            IWeatherApiMetrics metrics)
        {
            _service = service;
            _metrics = metrics;
        }

        [HttpGet]
        public async Task<IEnumerable<TideTime>> GetTideTimes(string city)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Dictionary<string, string> tags = GetTags(GetUser(), city);
            try
            {
                await _metrics.IncrementRequestCount(NAME_FOR_METRICS, tags);
                return await _service.GetTideTimes();
            }
            finally
            {
                stopwatch.Stop();
                await _metrics.RecordRequestLatency(NAME_FOR_METRICS, (int)stopwatch.ElapsedMilliseconds, tags);
            }
        }

        private static Dictionary<string, string> GetTags(string userName, string city)
        {
            return new Dictionary<string, string>
            {
                { "UserName", userName },
                { "City", city }
            };
        }
    }
}
