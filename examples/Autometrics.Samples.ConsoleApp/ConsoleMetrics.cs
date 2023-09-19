using Autometrics.Samples.ConsoleApp.InstrumentedExamples;
using OpenTelemetry;
using OpenTelemetry.Metrics;

namespace Autometrics.Samples.ConsoleApp
{
    internal class ConsoleMetrics
    {
        public static void GenerateActivity()
        {
            // Create a meter provider with the console exporter connected to the Autometrics.Instrumentation meter
            using var meterProvider = Sdk.CreateMeterProviderBuilder()
                .AddMeter("Autometrics.Instrumentation")
                .AddView(
                    instrumentName: "function.calls.duration",
                    new ExplicitBucketHistogramConfiguration { Boundaries = new double[] { 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10 } })
                .AddConsoleExporter()
                .Build();

            Console.WriteLine("Listener ready, starting to generate metrics.");

            // Run our sample application
            MySampleApplication.DoApplicationStuff();
        }
    }
}