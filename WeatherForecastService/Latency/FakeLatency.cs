namespace WeatherForecastService.Latency
{
    public class FakeLatency : IFakeLatency
    {
        private const int _secondsBetweenUpdates = 60;
        private readonly Random _random = new Random();
        private int _gaussianMeanMs;
        private int _gaussianStdevMs;
        private int _uniformMaxMs;
        private DateTime? _lastUpdated;

        public FakeLatency()
        {
            UpdateLatencies();
        }

        private void UpdateLatencies()
        {
            if (!_lastUpdated.HasValue || _lastUpdated.Value.AddSeconds(_secondsBetweenUpdates) < DateTime.Now)
            {
                _gaussianMeanMs = _random.Next(500, 10000);
                _gaussianStdevMs = _random.Next(0, _gaussianMeanMs / 2);
                _uniformMaxMs = _random.Next(100, 10000);
                _lastUpdated = DateTime.Now;
            }
        }

        public int GetGaussianLatencyMs()
        {
            UpdateLatencies();
            // thanks, internet!
            // https://stackoverflow.com/a/218600
            double u1 = 1.0 - _random.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - _random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            return Math.Max(0, (int)Math.Floor(_gaussianMeanMs + _gaussianStdevMs * randStdNormal)); //random normal(mean,stdDev^2)
        }

        public int GetUniformLatencyMs()
        {
            UpdateLatencies();
            return _uniformMaxMs;
        }
    }
}
