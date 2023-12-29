namespace WeatherForecastService.Errors.Exceptions
{
    public class ForecastUnauthorizedException : WeatherExceptionBase
    {
        public ForecastUnauthorizedException() : base(403, "Client is not authorized to retrieve a weather forecast.") { }
    }
}
