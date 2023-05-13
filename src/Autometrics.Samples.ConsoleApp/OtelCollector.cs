using Autometrics.Samples.ConsoleApp.InstrumentedExamples;
using OpenTelemetry;
using OpenTelemetry.Metrics;

namespace Autometrics.Samples.ConsoleApp
{
    internal class OtelCollector
    {
        public static void GenerateActivity()
        {
            // Create a meter provider with the Otlp exporter connected to the Autometrics.Instrumentation
            using var meterProvider = Sdk.CreateMeterProviderBuilder()
                .AddMeter("Autometrics.Instrumentation")
                .AddOtlpExporter()
                .Build();

            Console.WriteLine("Listener ready, starting to generate metrics.");

            // Run our sample application
            MySampleApplication.DoApplicationStuff();
        }
    }
}