namespace WeatherForecastService.Errors
{
    public interface IFakeErrorSource
    {
        void CauseExceptionMaybe();
    }
}
