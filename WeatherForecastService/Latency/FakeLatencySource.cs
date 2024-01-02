namespace WeatherForecastService.Latency
{
    public class FakeLatencySource : IFakeLatencySource
    {
        private const int _gaussianMeanMs = 3000;
        private const int _gaussianStdevMs = 2000;
        private readonly Random _random = new Random();

        public Task DoSlowOperation()
        {
            // thanks, internet!
            // https://stackoverflow.com/a/218600
            double u1 = 1.0 - _random.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - _random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            var latencyMs = Math.Max(0, (int)Math.Floor(_gaussianMeanMs + _gaussianStdevMs * randStdNormal)); //random normal(mean,stdDev^2)
            return Task.Delay(latencyMs);
        }
    }
}
