using Autometrics.Instrumentation.Attributes;

namespace Autometrics.Samples.Library
{
    /// <summary>
    /// This class and project are to show how an intermediate library can be instrumented, and how it will show up in the metrics.
    /// Only one addition of the OltpExporter is needed in the application, and all instrumented libraries will be included.
    /// </summary>
    public class ExternalExample
    {
        [Autometrics]
        public string InstrumentedMethod()
        {
            return "Hello World?";
        }
    }
}