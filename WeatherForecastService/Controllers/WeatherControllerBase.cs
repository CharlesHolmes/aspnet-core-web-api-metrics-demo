using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace WeatherForecastService.Controllers
{
    public class WeatherControllerBase : ControllerBase
    {
        protected string GetUser()
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
    }
}
