namespace WeatherForecastService.Models
{
    public class SunMoonTimes
    {
        public DateTimeOffset Sunrise { get; set; }
        public DateTimeOffset Sunset { get; set; }
        public DateTimeOffset Moonrise { get; set; }
        public DateTimeOffset Moonset { get; set; }
    }
}
