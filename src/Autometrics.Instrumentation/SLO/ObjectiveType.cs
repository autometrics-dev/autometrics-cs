using System;
using System.Collections.Generic;
using System.Text;

namespace Autometrics.Instrumentation.SLO
{
    public enum ObjectiveType
    {
        /// <summary>
        /// Only create the SLO on the success rate for this Method, only requires ObjectivePercentile
        /// </summary>
        SuccessRate,

        /// <summary>
        /// Only create the SLO on the latency for this Method, requires ObjectiveLatencyThreshold and ObjectivePercentile
        /// </summary>
        LatencyThreshold,

        /// <summary>
        /// Create the SLO on both the success rate and latency for this Method, requires ObjectiveLatencyThreshold and ObjectivePercentile
        /// </summary>
        SuccessAndLatency
    }
}
