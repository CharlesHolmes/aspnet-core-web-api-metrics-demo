using WeatherForecastService.Errors;
using WeatherForecastService.Latency;
using WeatherForecastService.Models;

namespace WeatherForecastService.Services
{
    public class SunMoonTimesService : ISunMoonTimesService
    {
        private readonly IFakeErrorSource _errorSource;
        private readonly IFakeLatencySource _latencySource;
        private readonly Random _random;

        public SunMoonTimesService(
            IFakeErrorSource errorSource,
            IFakeLatencySource latencySource)
        {
            _errorSource = errorSource;
            _latencySource = latencySource;
            _random = new Random();
        }

        public async Task<SunMoonTimes> GetSunMoonData()
        {

            _errorSource.CauseExceptionMaybe();
            await _latencySource.DoFastOperation();
            return new SunMoonTimes
            {
                Sunrise = DateTime.Now.AddSeconds(_random.NextDouble() * 86400),
                Sunset = DateTime.Now.AddSeconds(_random.NextDouble() * 86400),
                Moonrise = DateTime.Now.AddSeconds(_random.NextDouble() * 86400),
                Moonset = DateTime.Now.AddSeconds(_random.NextDouble() * 86400)
            };
        }
    }
}
