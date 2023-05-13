using Autometrics.Instrumentation.Attributes;

namespace Autometrics.Samples.ConsoleApp.LoadTesting
{
    internal class SimpleTestExamples
    {
        [Autometrics]
        public void MethodWithAutometrics()
        {
            Thread.Sleep(200);
        }

        public void MethodWithoutAutometrics()
        {
            Thread.Sleep(200);
        }
    }
}