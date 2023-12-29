namespace WeatherForecastService.Errors.Exceptions
{
    public class WeatherForecastException : WeatherExceptionBase
    {
        public WeatherForecastException() : base(500, "The weather is anybody's guess right now.") { }
    }
}
