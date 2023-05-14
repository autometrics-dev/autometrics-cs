using Autometrics.Instrumentation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autometrics.Instrumentation.Tests
{
    internal class InstrumentedMethods
    {
        [Autometrics]
        public void SimpleMethod(bool exceptionBeforeSleep, int sleepTimeMs, bool exceptionAfterSleep)
        {
            if (exceptionBeforeSleep)
            {
                throw new Exception("Exception thrown");
            }
            Thread.Sleep(sleepTimeMs);
            if (exceptionAfterSleep)
            {
                throw new Exception("Exception thrown");
            }
        }

        [Autometrics]
        public void NestedMethod(bool exceptionBeforeSleep, int sleepTimeMs, bool exceptionAfterSleep)
        {
            SimpleMethod(exceptionBeforeSleep, sleepTimeMs, exceptionAfterSleep);
        }
        
    }
}
