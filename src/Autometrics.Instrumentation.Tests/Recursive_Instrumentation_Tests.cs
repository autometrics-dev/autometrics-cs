using Autometrics.Instrumentation.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autometrics.Instrumentation.Tests
{
    internal class Recursive_Instrumentation_Tests
    {
        private MetricListener metricListner;
        private InstrumentedMethods instrumentedMethods;

        [SetUp]
        public void Setup()
        {
            metricListner = new MetricListener();
            instrumentedMethods = new InstrumentedMethods();
        }

        [Test]
        public void Record_Late_Failing_Methods()
        {
            double sleepDuration = .8;
            try
            {
                // Call our sample method and tell it what to do
                instrumentedMethods.NestedMethod(exceptionBeforeSleep: false, sleepTimeMs: (int)(sleepDuration * 1000), exceptionAfterSleep: true);
            }
            catch
            {
                // This was expected to throw an exception, but we should still get instrumentation.
            }

            // Wait for the metrics to be published
            Thread.Sleep(250);

            MetricBatch batch = metricListner.GetMetricBatch();
            metricListner.Reset();

            Assert.IsTrue(batch.IntReadings == 0, "An unexpected INT reading was returned.");
            Assert.IsTrue(batch.LongReadings == 2, "No function call count was seen");
            Assert.IsTrue(batch.DoubleReadings == 2, "No duration reading was seen");

            Assert.IsTrue(batch.ValidateDuration(sleepDuration, .5), "The recorded duration was out of acceptable tollerance");

            Assert.IsTrue(batch.ValidateFunctionNames(new[] { "SimpleMethod", "NestedMethod" }), "One or more of the function name tags was incorrect");
            Assert.IsTrue(batch.ValidateModuleName("Autometrics.Instrumentation.Tests.InstrumentedMethods"), "One or more of the module name tags was incorrect");
            Assert.IsTrue(batch.ValidateResult("error"), "One or more of the result tags was incorrect");
            Assert.IsTrue(batch.ValidateCallers(new[] { "Record_Late_Failing_Methods", "NestedMethod" }), "One or more of the caller tags was incorrect");
        }

        [Test]
        public void Record_Early_Failing_Methods()
        {
            double sleepDuration = .8;
            try
            {
                // Call our sample method and tell it what to do
                instrumentedMethods.NestedMethod(exceptionBeforeSleep: true, sleepTimeMs: (int)(sleepDuration * 1000), exceptionAfterSleep: true);
            }
            catch
            {
                // This was expected to throw an exception, but we should still get instrumentation.
            }

            // Wait for the metrics to be published
            Thread.Sleep(250);

            MetricBatch batch = metricListner.GetMetricBatch();
            metricListner.Reset();

            Assert.IsTrue(batch.IntReadings == 0, "An unexpected INT reading was returned.");
            Assert.IsTrue(batch.LongReadings == 2, "No function call count was seen");
            Assert.IsTrue(batch.DoubleReadings == 2, "No duration reading was seen");

            // Duration for this test should be near zero as the exception was thrown before the sleep
            Assert.IsTrue(batch.ValidateDuration(0, .5), "The recorded duration was out of acceptable tollerance");

            Assert.IsTrue(batch.ValidateFunctionNames(new[] { "SimpleMethod", "NestedMethod" }), "One or more of the function name tags was incorrect");
            Assert.IsTrue(batch.ValidateModuleName("Autometrics.Instrumentation.Tests.InstrumentedMethods"), "One or more of the module name tags was incorrect");
            Assert.IsTrue(batch.ValidateResult("error"), "One or more of the result tags was incorrect");
            Assert.IsTrue(batch.ValidateCallers(new[] { "Record_Early_Failing_Methods", "NestedMethod" }), "One or more of the caller tags was incorrect");
        }

        [Test]
        public void Record_Fast_Sucessful_Methods()
        {
            double sleepDuration = .2;
            try
            {
                // Call our sample method and tell it what to do
                instrumentedMethods.NestedMethod(exceptionBeforeSleep: false, sleepTimeMs: (int)(sleepDuration * 1000), exceptionAfterSleep: true);
            }
            catch
            {
                // This was expected to throw an exception, but we should still get instrumentation.
            }

            // Wait for the metrics to be published
            Thread.Sleep(250);

            MetricBatch batch = metricListner.GetMetricBatch();
            metricListner.Reset();

            Assert.IsTrue(batch.IntReadings == 0, "An unexpected INT reading was returned.");
            Assert.IsTrue(batch.LongReadings == 2, "No function call count was seen");
            Assert.IsTrue(batch.DoubleReadings == 2, "No duration reading was seen");

            Assert.IsTrue(batch.ValidateDuration(sleepDuration, .4), "The recorded duration was out of acceptable tollerance");

            Assert.IsTrue(batch.ValidateFunctionNames(new[] { "SimpleMethod", "NestedMethod" }), "One or more of the function name tags was incorrect");
            Assert.IsTrue(batch.ValidateModuleName("Autometrics.Instrumentation.Tests.InstrumentedMethods"), "One or more of the module name tags was incorrect");
            Assert.IsTrue(batch.ValidateResult("error"), "One or more of the result tags was incorrect");
            Assert.IsTrue(batch.ValidateCallers(new[] { "Record_Fast_Sucessful_Methods", "NestedMethod" }), "One or more of the caller tags was incorrect");
        }

        [Test]
        public void Record_Slow_Sucessful_Methods()
        {
            double sleepDuration = 5;
            try
            {
                // Call our sample method and tell it what to do
                instrumentedMethods.NestedMethod(exceptionBeforeSleep: false, sleepTimeMs: (int)(sleepDuration * 1000), exceptionAfterSleep: true);
            }
            catch
            {
                // This was expected to throw an exception, but we should still get instrumentation.
            }

            // Wait for the metrics to be published
            Thread.Sleep(250);

            MetricBatch batch = metricListner.GetMetricBatch();
            metricListner.Reset();

            Assert.IsTrue(batch.IntReadings == 0, "An unexpected INT reading was returned.");
            Assert.IsTrue(batch.LongReadings == 2, "No function call count was seen");
            Assert.IsTrue(batch.DoubleReadings == 2, "No duration reading was seen");

            Assert.IsTrue(batch.ValidateDuration(sleepDuration, .4), "The recorded duration was out of acceptable tollerance");

            Assert.IsTrue(batch.ValidateFunctionNames(new[] { "SimpleMethod", "NestedMethod" }), "One or more of the function name tags was incorrect");
            Assert.IsTrue(batch.ValidateModuleName("Autometrics.Instrumentation.Tests.InstrumentedMethods"), "One or more of the module name tags was incorrect");
            Assert.IsTrue(batch.ValidateResult("error"), "One or more of the result tags was incorrect");
            Assert.IsTrue(batch.ValidateCallers(new[] { "Record_Slow_Sucessful_Methods", "NestedMethod" }), "One or more of the caller tags was incorrect");
        }
    }
}
