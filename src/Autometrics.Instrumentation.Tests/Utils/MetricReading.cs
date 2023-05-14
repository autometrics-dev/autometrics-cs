using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Autometrics.Instrumentation.Tests.Utils
{
    /// <summary>
    /// Represents a single reading that the MetricListener has received
    /// </summary>
    internal class MetricReading<T>
    {
        public MetricReading(string metricName, T reading, KeyValuePair<string, object?>[] tags)
        {
            MetricName = metricName;
            Reading = reading;
            Tags = tags;

            foreach (KeyValuePair<string, object?> tag in Tags)
            {
                switch (tag.Key)
                {
                    case "function":
                        Function = tag.Value?.ToString();
                        break;
                    case "module":
                        Module = tag.Value?.ToString();
                        break;
                    case "result":
                        Result = tag.Value?.ToString();
                        break;
                    case "caller":
                        Caller = tag.Value?.ToString();
                        break;
                    case "objectiveName":
                        ObjectiveName = tag.Value?.ToString();
                        break;
                    case "objectivePercentile":
                        ObjectivePercentile = tag.Value?.ToString();
                        break;
                    case "objectiveLatencyThreshold":
                        ObjectiveLatencyThreshold = tag.Value?.ToString();
                        break;
                    case "version":
                        Version = tag.Value?.ToString();
                        break;
                }
            }
        }

        public string MetricName { get; set; }
        public T Reading { get; set; }
        public KeyValuePair<string, object?>[] Tags { get; set; }

        public string? Function { get; private set; }
        public string? Module { get; private set; }
        public string? Result { get; private set; }
        public string? Caller { get; private set; }
        public string? ObjectiveName { get; private set; }
        public string? ObjectivePercentile { get; private set; }
        public string? ObjectiveLatencyThreshold { get; private set; }
        public string? Version { get; private set; }
    }
}
