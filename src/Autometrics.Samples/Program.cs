namespace MetricsSample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Generate Activity Metrics to Console");
            Console.WriteLine("2. Generate Activity Metrics to Prometheus");
            Console.WriteLine("3. Generate Activity Extended Metrics to Prometheus");
            Console.WriteLine("4. Exit");
            Console.Write("Enter the option number: ");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                switch (option)
                {
                    case 1:
                        ConsoleMetrics.GenerateActivity();
                        break;

                    case 2:
                        PrometheusMetrics.GenerateActivity();
                        break;

                    case 3:
                        Console.Write("Enter the number of minutes for metric generation: ");
                        if (int.TryParse(Console.ReadLine(), out int minutes))
                        {
                            ExtendedPrometheusMetrics.GenerateActivity(minutes);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number.");
                        }
                        break;

                    case 4:
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

            Console.WriteLine("Completed generating metrics.");
            Console.ReadLine();
        }
    }
}