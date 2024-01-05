using WeatherForecastService.Models;

namespace WeatherForecastService.Services
{
    public interface ITideTimesService
    {
        Task<IEnumerable<TideTime>> GetTideTimes();
    }
}
