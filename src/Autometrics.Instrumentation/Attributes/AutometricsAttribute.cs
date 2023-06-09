﻿using AspectInjector.Broker;
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

        public AutometricsAttribute()
        {
            SLO = null;
        }

        public AutometricsAttribute(string objectiveName, ObjectivePercentile objectivePercentile, ObjectiveLatency objectiveLatencyThreshold, ObjectiveType objectiveType = ObjectiveType.SuccessAndLatency)
        {
            SLO = new Objective(objectiveName, objectivePercentile, objectiveLatencyThreshold, objectiveType);
        }

        public AutometricsAttribute(string objectiveName, ObjectivePercentile objectivePercentile)
        {
            SLO = new Objective(objectiveName, objectivePercentile);
        }

    }
}