﻿using Autometrics.Instrumentation.Attributes;

namespace Autometrics.Samples.InstrumentedExamples
{
    public class BusinessLayer
    {
        public DataAccessLayer DataAccessLayer { get; set; }

        [AutometricsMethod]
        public void ProcessRequest()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Processing request in Business Layer.");
            Console.ResetColor();
            Thread.Sleep(new Random().Next(100, 500));

            if (new Random().NextDouble() < 0.1)
            {
                throw new InvalidOperationException("An error occurred in the Business Layer.");
            }

            CalculateShippingCost();
            CheckInventory();
            EvaluateRisk();

            DataAccessLayer.FetchData();
        }

        [AutometricsMethod]
        public void CalculateShippingCost()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Calculating shipping cost in Business Layer.");
            Console.ResetColor();
            Thread.Sleep(new Random().Next(100, 300));
        }

        [AutometricsMethod]
        public void CheckInventory()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Checking inventory in Business Layer.");
            Console.ResetColor();
            Thread.Sleep(new Random().Next(200, 600));
        }

        [AutometricsMethod]
        public void EvaluateRisk()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Evaluating risk in Business Layer.");
            Console.ResetColor();
            Thread.Sleep(new Random().Next(300, 800));
        }
    }
}