using System.Diagnostics;

namespace Autometrics.Samples.ConsoleApp.LoadTesting
{
    internal class OverheadTesting
    {
        public static void PerformSimpleTest(int numberOfIterations = 1000)
        {
            SimpleTestExamples simpleExampleTesting = new SimpleTestExamples();

            Stopwatch stopwatch = new Stopwatch();

            // Test the method with Autometrics
            stopwatch.Start();
            for (int i = 0; i < numberOfIterations; i++)
            {
                int cursorLeft = Console.CursorLeft;
                int cursorTop = Console.CursorTop;
                Console.Write($"Testing with Autometrics - Iteration: {i + 1}");
                simpleExampleTesting.MethodWithAutometrics();
                Console.SetCursorPosition(cursorLeft, cursorTop);
            }
            stopwatch.Stop();
            long avgTimeWithAutometrics = stopwatch.ElapsedMilliseconds / numberOfIterations;

            // Test the method without Autometrics
            stopwatch.Restart();
            for (int i = 0; i < numberOfIterations; i++)
            {
                int cursorLeft = Console.CursorLeft;
                int cursorTop = Console.CursorTop;
                Console.Write($"Testing without Autometrics - Iteration: {i + 1}");
                simpleExampleTesting.MethodWithoutAutometrics();
                Console.SetCursorPosition(cursorLeft, cursorTop);
            }
            stopwatch.Stop();
            long avgTimeWithoutAutometrics = stopwatch.ElapsedMilliseconds / numberOfIterations;
            Console.WriteLine("\n\n\n");
            Console.WriteLine($"Average time with Autometrics: {avgTimeWithAutometrics} ms");
            Console.WriteLine($"Average time without Autometrics: {avgTimeWithoutAutometrics} ms");
            Console.WriteLine($"Overhead: {avgTimeWithAutometrics - avgTimeWithoutAutometrics} ms");
        }

        public static void PerformRecursiveTest(int numberOfIterations = 100, int maxDepth = 5)
        {
            RecursiveTestExamples recursiveExampleTesting = new RecursiveTestExamples();

            Stopwatch stopwatch = new Stopwatch();

            // Test the recursive method with Autometrics
            stopwatch.Start();
            for (int i = 0; i < numberOfIterations; i++)
            {
                int cursorTop = Console.CursorTop;
                Console.Write($"Testing with Autometrics - Iteration: {i + 1}");
                recursiveExampleTesting.RecursiveMethodWithAutometrics(maxDepth, 1);
                Console.SetCursorPosition(0, cursorTop);
            }
            stopwatch.Stop();
            long avgTimeWithAutometrics = stopwatch.ElapsedMilliseconds / numberOfIterations;

            Console.WriteLine(); // New line added

            // Test the recursive method without Autometrics
            stopwatch.Restart();
            for (int i = 0; i < numberOfIterations; i++)
            {
                int cursorTop = Console.CursorTop;
                Console.Write($"Testing without Autometrics - Iteration: {i + 1}");
                recursiveExampleTesting.RecursiveMethodWithoutAutometrics(maxDepth, 1);
                Console.SetCursorPosition(0, cursorTop);
            }
            stopwatch.Stop();
            long avgTimeWithoutAutometrics = stopwatch.ElapsedMilliseconds / numberOfIterations;

            Console.Clear();
            Console.WriteLine();

            Console.WriteLine($"Average time with Autometrics: {avgTimeWithAutometrics} ms");
            Console.WriteLine($"Average time without Autometrics: {avgTimeWithoutAutometrics} ms");
            Console.WriteLine($"Overhead: {avgTimeWithAutometrics - avgTimeWithoutAutometrics} ms");
        }
    }
}