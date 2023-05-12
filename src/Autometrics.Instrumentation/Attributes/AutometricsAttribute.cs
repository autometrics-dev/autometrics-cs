using AspectInjector.Broker;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Autometrics.Instrumentation.Attributes
{
    /// <summary>
    /// Autometrics attribute provides method duration tracking and exception tagging.
    /// For more information, see the project on GitHub:
    /// <see href="https://github.com/autometrics-dev">Autometrics on GitHub</see>
    /// </summary>
    [Aspect(Scope.Global)]
    [Injection(typeof(AutometricsAttribute))]
    [AttributeUsage(AttributeTargets.Method)]
    public class AutometricsAttribute : Attribute
    {

        // Aspect Injection renames methods, if we get a renamed one it will look like this: __a$_around_SaferMethod_100663303_o
        // We need a regex to identify these methods
        private readonly static string renamedMethodRegex = @"^__a\$_around_(?<originalMethodName>.*)_\d{5,10}_o$";

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
            [Argument(Source.Target)] Func<object[], object> method)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool success = false;
            object result = null;

            try
            {
                result = method(arguments);
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
                string callingMethodName = GetCallingMethodName(metadata);
                MetricCounters.RecordFunctionCall(stopwatch.ElapsedMilliseconds, methodName, success, metadata.DeclaringType.FullName, callingMethodName);
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

            for (int i = 0; i < stackFrames.Length; i++)
            {
                if (stackFrames[i].GetMethod() == method)
                {
                    // The calling method is the one before the current method in the stack
                    callingMethod = i + 1 < stackFrames.Length ? stackFrames[i + 1].GetMethod() : null;
                }
            }

            // If the calling method isn't null, match it to our regex to see if it's a renamed method, then return the original name
            if (callingMethod != null)
            {
                var match = Regex.Match(callingMethod.Name, renamedMethodRegex, RegexOptions.Compiled);
                if (match.Success)
                {
                    return match.Groups["originalMethodName"].Value;
                }
                else
                {
                    return callingMethod.Name;
                }
            }

            return null;
        }
    }
}
