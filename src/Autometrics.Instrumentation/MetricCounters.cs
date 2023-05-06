using System.Diagnostics.Metrics;
using System.Reflection;

namespace Autometrics.Instrumentation
{
    /// <summary>
    /// A static class responsible for collecting and reporting various metrics related to function calls, such as call count, duration, and build information for the instrumented assembly.
    /// </summary>
    internal static class MetricCounters
    {
        /// <summary>
        /// The meter used to track all metrics for the library, the collector will need to be configured to collect this meter
        /// </summary>
        private static readonly Meter autometricsMeter = new Meter("Autometrics.Instrumentation");

        /// <summary>
        /// The counter used to track the number of function calls
        /// </summary>
        private static readonly Counter<long> functionCallCount = autometricsMeter.CreateCounter<long>("function_calls_count");

        /// <summary>
        /// The histogram used to track the duration of function calls
        /// </summary>
        private static readonly Histogram<long> functionCallDuration = autometricsMeter.CreateHistogram<long>("function_calls_duration");

        /// <summary>
        /// The non-changing gauge used to track the build information
        /// </summary>
        private static readonly ObservableGauge<int> buildInformation = autometricsMeter.CreateObservableGauge<int>("build_info", observeBuildInfo, null, "A never changing gauge to allow tracking of build and commit information");

        /// <summary>
        /// Tags attached to the readings of our gaguge indicating our build information
        /// </summary>
        private static readonly KeyValuePair<string, object?>[]? buildTags;

        /// <summary>
        /// The name of the assembly we are instrumenting, to be used as a tag
        /// </summary>
        private static readonly string? assemblyName;

        /// <summary>
        /// Generates a dummy gague measurement with tags of the current build information
        /// </summary>
        /// <returns></returns>
        private static Measurement<int> observeBuildInfo()
        {
            return new Measurement<int>(0, buildTags);
        }

        /// <summary>
        /// Instantiates the static class and its members to insure we only have one instance of each
        /// </summary>
        static MetricCounters()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            if (assembly != null)
            {
                assemblyName = assembly.GetName()?.Name;
                buildTags = SetBuildTags(assembly);
            }
            else
            {
                assemblyName = null;
                buildTags = null;
            }
        }

        /// <summary>
        /// Records the duration, result, and caller of a function call
        /// </summary>
        /// <param name="duration">Duration in milliseconds</param>
        /// <param name="functionName">The name of the function tracked</param>
        /// <param name="success">A string value of "ok" or "error" based on the outcome</param>
        /// <param name="caller">optional caller data</param>
        internal static void RecordFunctionCall(long duration, string functionName, bool success, string? caller)
        {
            List<KeyValuePair<string, object?>> callTags = new List<KeyValuePair<string, object?>>
            {
                new KeyValuePair<string, object?>("function", functionName),
                new KeyValuePair<string, object?>("module", assemblyName),
                new KeyValuePair<string, object?>("result", success ? "ok" : "error")
            };

            if (caller != null)
            {
                callTags.Add(new KeyValuePair<string, object?>("caller", caller));
            }

            functionCallCount.Add(1, callTags.ToArray());
            functionCallDuration.Record(duration, callTags.ToArray());
        }

        /// <summary>
        /// Generates the build tags which are used to identify the build and commit information
        /// This is pulled from the version information of the assembly with additional git commit information when available
        /// </summary>
        /// <returns>A KeyValuePair of the current version and the commit if available</returns>
        private static KeyValuePair<string, object?>[] SetBuildTags(Assembly assembly)
        {
            Dictionary<string, object?> buildTags = new Dictionary<string, object?>();

            AssemblyName? assemblyName = assembly?.GetName();
            AssemblyInformationalVersionAttribute assemblyInformationalVersionAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (assemblyName != null)
            {
                Version version = assemblyName.Version;
                if (version != null)
                {
                    buildTags.Add("version", $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}");

                    // Only add the InformationalVersion if it is different from the first three parts of the version
                    if (assemblyInformationalVersionAttribute?.InformationalVersion != $"{version.Major}.{version.Minor}.{version.Build}")
                    {
                        buildTags.Add("commit", assemblyInformationalVersionAttribute?.InformationalVersion);
                    }
                }
            }

            return buildTags.ToArray();
        }
    }
}