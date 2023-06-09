﻿using Autometrics.Instrumentation.Attributes;

namespace Autometrics.Samples.ConsoleApp.LoadTesting
{
    internal class RecursiveTestExamples
    {
        [Autometrics]
        public void RecursiveMethodWithAutometrics(int depth, int currentDepth)
        {
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;
            Console.SetCursorPosition(0, cursorTop + 1); // Move cursor one line down
            Console.Write($" - Current depth with Autometrics: {currentDepth}");
            Thread.Sleep(50);

            if (currentDepth < depth)
            {
                RecursiveMethodWithAutometrics(depth, currentDepth + 1);
            }
            Console.SetCursorPosition(cursorLeft, cursorTop); // Reset cursor position
        }

        public void RecursiveMethodWithoutAutometrics(int depth, int currentDepth)
        {
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;
            Console.SetCursorPosition(0, cursorTop + 1); // Move cursor one line down
            Console.Write($" - Current depth without Autometrics: {currentDepth}");
            Thread.Sleep(50);

            if (currentDepth < depth)
            {
                RecursiveMethodWithoutAutometrics(depth, currentDepth + 1);
            }
            Console.SetCursorPosition(cursorLeft, cursorTop); // Reset cursor position
        }
    }
}