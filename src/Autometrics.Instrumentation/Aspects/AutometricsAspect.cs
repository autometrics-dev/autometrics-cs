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
        // Aspect Injection renames methods, if we get a renamed one it will look like this: __a$_around_SaferMethod_100663303_o
        // We need a regex to identify these methods
        private static readonly Regex _methodRegex = new Regex(@"^__a\$_around_(?<originalMethodName>.*)_\d{5,10}_o$", RegexOptions.Compiled);

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
            if (triggers.Length > 0 && triggers[0] is AutometricsAttribute attribute)
            {
                slo = attribute.SLO;
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
                MetricCounters.RecordFunctionCall(stopwatch.Elapsed.TotalSeconds, methodName, success, metadata.DeclaringType.FullName, GetCallingMethodName(metadata), slo);
            }
        }

        /// <summary>
        /// This resolves the name of the calling method, and if the calling method was renamed by AspectInjector, it will attempt to return the original name via regex
        /// </summary>
        /// <param name="method">The metadata from AspectInjector</param>
        /// <returns></returns>
        private static string? GetCallingMethodName(MethodBase method)
        {
            var stackTrace = new StackTrace();
            var stackFrames = stackTrace.GetFrames();
            MethodBase callingMethod = null;

            if (stackFrames == null)
            {
                return null;
            }

            int length = stackFrames.Length;
            for (int i = 0; i < length; i++)
            {
                if (stackFrames[i].GetMethod() == method)
                {
                    // The calling method is the one before the current method in the stack
                    callingMethod = i + 1 < length ? stackFrames[i + 1].GetMethod() : null;
                }
            }

            // If the calling method isn't null, match it to our regex to see if it's a renamed method, then return the original name
            if (callingMethod != null)
            {
                var match = _methodRegex.Match(callingMethod.Name);
                if (match.Success)
                {
                    return match.Groups["originalMethodName"].Value;
                }
                return callingMethod.Name;
            }

            return null;
        }
    }
}