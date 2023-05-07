using Autometrics.Instrumentation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autometrics.Samples.LoadTesting
{
    internal class SimpleTestExamples
    {
        [AutometricsMethod]
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
