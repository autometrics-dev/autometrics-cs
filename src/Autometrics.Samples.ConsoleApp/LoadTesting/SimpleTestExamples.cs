using Autometrics.Instrumentation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
