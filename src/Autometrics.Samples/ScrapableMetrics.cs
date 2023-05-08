using Autometrics.Samples.InstrumentedExamples;
using OpenTelemetry;
using OpenTelemetry.Metrics;

namespace MetricsSample
{
    internal class ScrapableMetrics
    {
        public static void GenerateActivity()
        {
            // Create a meter provider with the console exporter connected to the Autometrics.Instrumentation meter
            using var meterProvider = Sdk.CreateMeterProviderBuilder()
                .AddMeter("Autometrics.Instrumentation")
                .AddPrometheusHttpListener(
                options =>
                {
                    options.UriPrefixes = new string[] { "http://localhost:9090/" };
                })
                .Build();

            Console.WriteLine("Listener ready at http://localhost:9091/, starting to generate metrics.");

            // Run our sample application
            MySampleApplication.DoApplicationStuff();
        }
    }
}