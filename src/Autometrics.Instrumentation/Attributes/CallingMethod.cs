using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Autometrics.Instrumentation.Attributes
{
    internal class CallingMethod
    {
        // Aspect Injection renames methods, if we get a renamed one it will look like this: __a$_around_SaferMethod_100663303_o
        // We need a regex to identify these methods
        private static readonly Regex _methodRegex = new Regex(@"^__a\$_around_(?<originalMethodName>.*)_\d{5,10}_o$", RegexOptions.Compiled);

        public CallingMethod(MethodBase method)
        {
            try
            {
                SetCallingMethod(method);
            }
            catch 
            {
                HasMethodData = false;
            }
        }

        public string? MethodModule { get; set; }
        public string? MethodName { get; set; }
        public bool HasMethodData { get; internal set; } = false;

        /// <summary>
        /// This resolves the name of the calling method, and if the calling method was renamed by AspectInjector, it will attempt to return the original name via regex
        /// </summary>
        /// <param name="method">The metadata from AspectInjector</param>
        /// <returns></returns>
        private void SetCallingMethod(MethodBase method)
        {
            var stackTrace = new StackTrace();
            var stackFrames = stackTrace.GetFrames();
            MethodBase callingMethod = null;

            if (stackFrames == null)
            {
                MethodModule = null;
                MethodName = null;
                return;
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
                HasMethodData = true;
                var match = _methodRegex.Match(callingMethod.Name);
                if (match.Success)
                {
                    MethodName = match.Groups["originalMethodName"].Value;
                }
                else
                {
                    MethodName = callingMethod.Name;
                }

                MethodModule = callingMethod.Module.Name;
            }
        }
    }
}
