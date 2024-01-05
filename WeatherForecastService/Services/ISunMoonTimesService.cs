using WeatherForecastService.Models;

namespace WeatherForecastService.Services
{
    public interface ISunMoonTimesService
    {
        Task<SunMoonTimes> GetSunMoonData();
    }
}
