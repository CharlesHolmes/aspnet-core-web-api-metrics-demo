using Amazon.XRay.Recorder.Core;

namespace WeatherForecastService.Latency
{
    public class FakeLatencySource : IFakeLatencySource
    {
        private const int _longMeanMs = 3000;
        private const int _longStdevMs = 2000;
        private const int _shortMeanMs = 1000;
        private const int _shortStdevMs = 300;
        private readonly Random _random = new Random();

        public async Task DoSlowOperation()
        {
            await AWSXRayRecorder.Instance.TraceMethodAsync(
                $"{nameof(FakeLatencySource)}.{nameof(DoSlowOperation)}", 
                () => WaitRandomDelay(_longMeanMs, _longStdevMs));
        }

        public async Task DoFastOperation()
        {
            await AWSXRayRecorder.Instance.TraceMethodAsync(
                $"{nameof(FakeLatencySource)}.{nameof(DoFastOperation)}",
                () => WaitRandomDelay(_shortMeanMs, _shortStdevMs));
        }

        private Task WaitRandomDelay(int mean, int stdev)
        {
            // thanks, internet!
            // https://stackoverflow.com/a/218600
            double u1 = 1.0 - _random.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - _random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            var latencyMs = Math.Max(0, (int)Math.Floor(mean + stdev * randStdNormal)); //random normal(mean,stdDev^2)
            return Task.Delay(latencyMs);
        }
    }
}
