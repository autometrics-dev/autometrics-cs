using Autometrics.Samples.ConsoleApp.LoadTesting;

namespace Autometrics.Samples.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Generate Activity Metrics to Console");
            Console.WriteLine("2. Generate Activity Metrics for the Otel Collector");
            Console.WriteLine("3. Generate Activity Metrics to be Scraped by Prometheus");
            Console.WriteLine("4. Test AutometricsMethod Overhead (Simple)");
            Console.WriteLine("5. Test AutometricsMethod Overhead (Recursive / Complex)");
            Console.WriteLine("6. Exit");
            Console.Write("Enter the option number: ");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                switch (option)
                {
                    case 1:
                        ConsoleMetrics.GenerateActivity();
                        break;

                    case 2:
                        OtelCollector.GenerateActivity();
                        break;

                    case 3:
                        ScrapableMetrics.GenerateActivity();
                        break;

                    case 4:
                        Console.Write("Enter the number of iterations to test for overhead: ");
                        if (int.TryParse(Console.ReadLine(), out int simpleIterations))
                        {
                            OverheadTesting.PerformSimpleTest(simpleIterations);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number.");
                        }
                        break;
                    case 5:
                        Console.Write("Enter the number of iterations to test for overhead: ");
                        if (int.TryParse(Console.ReadLine(), out int complexIterations))
                        {
                            Console.Write("Enter the depth of the recursion to test for overhead: ");
                            if (int.TryParse(Console.ReadLine(), out int recursionDepth))
                            {
                                OverheadTesting.PerformRecursiveTest(complexIterations, recursionDepth);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number.");
                        }
                        break;
                    case 6:
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid option. Please select a valid option.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid option number.");
            }

            Console.WriteLine("\n\n\n");
            Console.WriteLine("Completed generating metrics.");
            Console.ReadLine();
        }
    }
}