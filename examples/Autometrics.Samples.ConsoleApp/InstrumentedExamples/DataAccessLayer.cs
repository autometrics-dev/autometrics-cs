using Autometrics.Instrumentation.Attributes;
using Autometrics.Instrumentation.SLO;

namespace Autometrics.Samples.ConsoleApp.InstrumentedExamples
{
    public class DataAccessLayer
    {
        [Autometrics(objectiveName: "Data Access Layer",
            objectivePercentile: ObjectivePercentile.P99,
            objectiveLatencyThreshold: ObjectiveLatency.Ms100)]
        public void FetchData()
        {
            Console.WriteLine("Fetching data in Data Access Layer.");

            if (CheckRedisCache())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Data found in Redis cache.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Data not found in Redis cache. Fetching from SQL server.");
                Console.ResetColor();
                FetchDataFromSqlServer();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Updating Redis cache with new data.");
                Console.ResetColor();
                UpdateRedisCache();
            }

            Thread.Sleep(new Random().Next(100, 500));

            if (new Random().NextDouble() < 0.05)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occurred in the Data Access Layer.");
                Console.ResetColor();
                throw new Exception("An error occurred in the Data Access Layer.");
            }
        }

        [Autometrics("FetchData", ObjectivePercentile.P99_9, ObjectiveLatency.Ms7500)]
        private bool CheckRedisCache()
        {
            Thread.Sleep(new Random().Next(10, 50));
            return new Random().NextDouble() >= 0.1;
        }

        private void FetchDataFromSqlServer()
        {
            Thread.Sleep(new Random().Next(200, 1000));
        }

        private void UpdateRedisCache()
        {
            Thread.Sleep(new Random().Next(10, 50));
        }
    }
}