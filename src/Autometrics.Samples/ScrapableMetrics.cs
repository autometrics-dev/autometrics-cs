using Autometrics.Samples.InstrumentedExamples;

namespace MetricsSample
{
    internal class ScrapableMetrics
    {
        public static void GenerateActivity()
        {
            Console.WriteLine($"Generating metrics to present to be scraped.");
            
            Console.WriteLine("Listener ready, starting to generate metrics.");
            MySampleApplication.DoApplicationStuff();
        }
    }
}