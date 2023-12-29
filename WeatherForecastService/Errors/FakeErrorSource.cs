using WeatherForecastService.Errors.Exceptions;

namespace WeatherForecastService.Errors
{
    public class FakeErrorSource : IFakeErrorSource
    {
        private readonly Random _random = new Random();

        public void CauseExceptionMaybe()
        {
            var randomInt = _random.Next(0, 1000);
            if (randomInt == 999)
            {
                throw new WeatherForecastException();
            }
            else if (randomInt >= 995)
            {
                throw new ForecastUnauthorizedException();
            }
            else if (randomInt >= 990)
            {
                throw new InvalidWeatherCredentialsException();
            }
        }
    }
}
