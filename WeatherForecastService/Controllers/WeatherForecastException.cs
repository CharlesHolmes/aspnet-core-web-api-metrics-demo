namespace WeatherForecastService.Controllers
{
    public class WeatherForecastException : ApplicationException
    {
        public WeatherForecastException() : base("The weather is anybody's guess right now.") { }
    }
}
