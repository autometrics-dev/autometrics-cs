namespace Autometrics.Instrumentation.SLO
{
    public class Objective
    {
        public Objective(string objectiveName, ObjectivePercentile objectivePercentile)
        {
            ObjectiveName = objectiveName;
            ObjectivePercentile = objectivePercentile;
            ObjectiveType = ObjectiveType.SuccessRate;
        }

        public Objective(string objectiveName, ObjectivePercentile objectivePercentile, ObjectiveLatency objectiveLatencyThreshold, ObjectiveType objectiveType)
        {
            ObjectiveName = objectiveName;
            ObjectivePercentile = objectivePercentile;
            ObjectiveLatencyThreshold = objectiveLatencyThreshold;
            ObjectiveType = objectiveType;
        }

        /// <summary>
        /// Optional ObjectiveLatencyThreshold to be used for this method
        /// <see href="https://github.com/autometrics-dev/autometrics-shared/blob/main/SPEC.md#objectivelatency_threshold">Objective Latency Threshold Spec</see>
        /// </summary>
        public ObjectiveLatency ObjectiveLatencyThreshold { get; private set; }

        /// <summary>
        /// Optional ObjectiveName to be used for this method
        /// <see href="https://github.com/autometrics-dev/autometrics-shared/blob/main/SPEC.md#objectivename">Objective Name Spec</see>
        /// </summary>
        public string ObjectiveName { get; private set; }

        /// <summary>
        /// Optional ObjectivePercentile to be used for this method
        /// <see href="https://github.com/autometrics-dev/autometrics-shared/blob/main/SPEC.md#objectivepercentile">Objective Percentile Spec</see>
        /// </summary>
        public ObjectivePercentile ObjectivePercentile { get; private set; }

        /// <summary>
        /// Specifies which type of Objective this is and allows the tags to be set accordingly
        /// </summary>
        public ObjectiveType ObjectiveType { get; private set; }

        /// <summary>
        /// Adds the tags for the call count metric, if the ObjectiveType matches
        /// </summary>
        /// <param name="callTags">Current tags for the method</param>
        /// <returns></returns>
        public KeyValuePair<string, object?>[] GetCallCountTags(List<KeyValuePair<string, object?>> callTags)
        {
            List<KeyValuePair<string,object?>> countTags = new List<KeyValuePair<string, object?>>();

            if (ObjectiveType == ObjectiveType.SuccessRate || ObjectiveType == ObjectiveType.SuccessAndLatency)
            {
                countTags.Add(new KeyValuePair<string, object?>("objective.name", ObjectiveName));
                countTags.Add(new KeyValuePair<string, object?>("objective.percentile", ((double)ObjectivePercentile / 10)));
            }

            countTags.AddRange(callTags);
            return countTags.ToArray();
        }

        /// <summary>
        /// Adds the tags for the call duration metric, if the ObjectiveType matches
        /// </summary>
        /// <param name="callTags">Current tags for the method</param>
        /// <returns></returns>
        public KeyValuePair<string, object?>[] GetCallDurationTags(List<KeyValuePair<string, object?>> callTags)
        {
            List<KeyValuePair<string,object?>> durationTags = new List<KeyValuePair<string, object?>>();

            if (ObjectiveType == ObjectiveType.LatencyThreshold || ObjectiveType == ObjectiveType.SuccessAndLatency)
            {
                durationTags.Add(new KeyValuePair<string, object?>("objective.name", ObjectiveName));
                durationTags.Add(new KeyValuePair<string, object?>("objective.percentile", (double)ObjectivePercentile / 10));
                durationTags.Add(new KeyValuePair<string, object?>("objective.latency_threshold", (double)ObjectiveLatencyThreshold / 1000));
            }

            durationTags.AddRange(callTags);
            return durationTags.ToArray();
        }
    }
}