using WeatherForecastService.Errors;
using WeatherForecastService.Latency;

namespace WeatherForecastService.Services
{
    public class WeatherService : IWeatherService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IFakeErrorSource _errorSource;
        private readonly IFakeLatencySource _latencySource;

        public WeatherService(
            IFakeErrorSource errorSource,
            IFakeLatencySource latencySource)
        {
            _errorSource = errorSource;
            _latencySource = latencySource;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts()
        {
            _errorSource.CauseExceptionMaybe();
            await _latencySource.DoSlowOperation();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTimeOffset.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();
        }
    }
}
