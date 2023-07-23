using AspectInjector.Broker;
using Autometrics.Instrumentation.Aspects;
using Autometrics.Instrumentation.SLO;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Autometrics.Instrumentation.Attributes
{

    [Injection(typeof(AutometricsAspect))]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AutometricsAttribute : Attribute
    {
        /// <summary>
        /// An Optional SLO to be used for this method,
        /// <see href="https://github.com/autometrics-dev/autometrics-shared/blob/main/SPEC.md#service-level-objectives-slos">Service-Level Objectives (SLOs) Spec</see>
        /// </summary>
        public Objective? SLO { get; }
        public string EntryAssemblyName { get; private set; }
        public string? ServiceName { get; set; }

        public AutometricsAttribute(string? serviceName = null)
        {
            SLO = null;
            EntryAssemblyName = GetAssemblyName();
            ServiceName = serviceName;
        }

        public AutometricsAttribute(string objectiveName, ObjectivePercentile objectivePercentile, ObjectiveLatency objectiveLatencyThreshold, ObjectiveType objectiveType = ObjectiveType.SuccessAndLatency, string? serviceName = null)
        {
            SLO = new Objective(objectiveName, objectivePercentile, objectiveLatencyThreshold, objectiveType);
            EntryAssemblyName = GetAssemblyName();
            ServiceName = serviceName;
        }

        public AutometricsAttribute(string objectiveName, ObjectivePercentile objectivePercentile, string? serviceName = null)
        {
            SLO = new Objective(objectiveName, objectivePercentile);
            EntryAssemblyName = GetAssemblyName();
            ServiceName = serviceName;
        }

        private string GetAssemblyName()
        {
            return Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";
        }

        /// <summary>
        /// Gets the ServiceName, starting at the Attribule level, then the Environment Variable, then the EntryAssemblyName
        /// </summary>
        /// <returns></returns>
        public string GetServiceName()
        {
            return ServiceName ?? Environment.GetEnvironmentVariable("AUTOMETRICS_SERVICE_NAME") ?? Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME") ?? EntryAssemblyName;
        }

    }
}