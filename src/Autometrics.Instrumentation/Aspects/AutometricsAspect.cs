using AspectInjector.Broker;
using Autometrics.Instrumentation.Attributes;
using Autometrics.Instrumentation.SLO;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Autometrics.Instrumentation.Aspects
{
    /// <summary>
    /// Autometrics attribute provides method duration tracking and exception tagging.
    /// For more information, see the project on GitHub:
    /// <see href="https://github.com/autometrics-dev">Autometrics on GitHub</see>
    /// </summary>
    [Aspect(Scope.Global)]
    public class AutometricsAspect
    {
        /// <summary>
        /// This is the method that will be injected into the target method, it will wrap the target method in a try/catch block and record the duration of the method call
        /// If any exceptions are thrown, they will be rethrown after the duration is recorded and a result of error tagged on the metric
        /// </summary>
        /// <param name="arguments">Argument from the wrapped method</param>
        /// <param name="metadata">Metadata on the wrapped method</param>
        /// <param name="methodName">The name of the wrapped method</param>
        /// <param name="method">The original unwrapped method</param>
        /// <returns></returns>
        [Advice(Kind.Around, Targets = Target.Method)]
        public object HandleMethod(
            [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.Metadata)] MethodBase metadata,
            [Argument(Source.Name)] string methodName,
            [Argument(Source.Target)] Func<object[], object> method,
            [Argument(Source.Triggers)] Attribute[] triggers)
        {
            Stopwatch stopwatch = new Stopwatch();
            bool success = false;

            // We'll start with our objective as null, but if the trigger has one we'll use it
            Objective? slo = null;
            string? serviceName = null;
            if (triggers.Length > 0 && triggers[0] is AutometricsAttribute attribute)
            {
                slo = attribute.SLO;
                serviceName = attribute.GetServiceName();
            }

            try
            {
                // start the stopwatch, call the method, stop the stopwatch to record the duration
                stopwatch.Start();
                object result = method(arguments);
                success = true;
                return result;
            }
            catch (Exception)
            {
                success = false;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                CallingMethod caller = new CallingMethod(metadata);
                MetricCounters.RecordFunctionCall(stopwatch.Elapsed.TotalSeconds, methodName, success, metadata.DeclaringType.FullName, caller, serviceName, slo);
            }
        }
    }
}