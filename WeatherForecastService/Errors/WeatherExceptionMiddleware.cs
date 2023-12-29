using WeatherForecastService.Errors.Exceptions;

namespace WeatherForecastService.Errors
{
    internal class WeatherExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public WeatherExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (WeatherExceptionBase e)
            {
                context.Response.StatusCode = e.HttpStatusCode;
            }
        }
    }
}
