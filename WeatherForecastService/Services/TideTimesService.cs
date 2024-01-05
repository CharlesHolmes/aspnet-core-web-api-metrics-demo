using WeatherForecastService.Errors;
using WeatherForecastService.Latency;
using WeatherForecastService.Models;

namespace WeatherForecastService.Services
{
    public class TideTimesService : ITideTimesService
    {
        private readonly IFakeErrorSource _errorSource;
        private readonly IFakeLatencySource _latencySource;
        private readonly Random _random;

        public TideTimesService(
            IFakeErrorSource errorSource,
            IFakeLatencySource latencySource)
        {
            _errorSource = errorSource;
            _latencySource = latencySource;
            _random = new Random();
        }

        public async Task<IEnumerable<TideTime>> GetTideTimes()
        {
            _errorSource.CauseExceptionMaybe();
            await _latencySource.DoFastOperation();
            return Enumerable.Range(1, 4).Select(_ => new TideTime
            {
                Time = DateTimeOffset.Now.AddSeconds(_random.NextDouble() * 86400),
                Type = _random.Next(0, 2) == 0 ? "Low" : "High"
            });
        }
    }
}
