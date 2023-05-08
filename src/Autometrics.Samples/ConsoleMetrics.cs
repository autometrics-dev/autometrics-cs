﻿using Autometrics.Samples.InstrumentedExamples;
using OpenTelemetry;
using OpenTelemetry.Extensions.Hosting;
using OpenTelemetry.Metrics;

namespace MetricsSample
{
    internal class ConsoleMetrics
    {
        public static void GenerateActivity()
        {
            // Create a meter provider with the console exporter connected to the Autometrics.Instrumentation meter
            using var meterProvider = Sdk.CreateMeterProviderBuilder()
                .AddMeter("Autometrics.Instrumentation")
                .AddConsoleExporter()
                .Build();

            Console.WriteLine("Listener ready, starting to generate metrics.");

            // Run our sample application
            MySampleApplication.DoApplicationStuff();
        }
    }
}