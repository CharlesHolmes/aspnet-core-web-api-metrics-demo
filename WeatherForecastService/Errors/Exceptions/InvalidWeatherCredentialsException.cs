namespace WeatherForecastService.Errors.Exceptions
{
    public class InvalidWeatherCredentialsException : WeatherExceptionBase
    {
        public InvalidWeatherCredentialsException() : base(401, "Weather forecast credentials were invalid!") { }
    }
}
