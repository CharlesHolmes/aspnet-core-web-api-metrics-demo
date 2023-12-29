namespace WeatherForecastService.Errors
{
    public static class WeatherExceptionExtensions
    {
        public static IApplicationBuilder UseWeatherExceptionHandler(this IApplicationBuilder application)
        {
            return application.UseMiddleware<WeatherExceptionMiddleware>();
        }
    }
}
