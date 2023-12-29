namespace WeatherForecastService.Errors.Exceptions
{
    public abstract class WeatherExceptionBase : ApplicationException
    {
        public int HttpStatusCode { get; init; }

        protected WeatherExceptionBase(int httpStatusCode, string message) : base(message) 
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
