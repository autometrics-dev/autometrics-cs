using Autometrics.Instrumentation.Attributes;
using Autometrics.Samples.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Autometrics.Samples.WebApp.Controllers
{
    [Route("test")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [Autometrics]
        [HttpGet("InstrumentedFunction")]
        public IActionResult InstrumentedFunction()
        {
            int waitMS = (new Random().Next(200,4000));
            ExternalExample externalExample = new ExternalExample();
            externalExample.InstrumentedMethod();

            Thread.Sleep(waitMS);
            return Ok($"This is the response from the Instrumented Function, it waited {waitMS}ms");
        }

        [HttpGet("NonInstrumentedFunction")]
        public IActionResult NonInstrumentedFunction()
        {
            int waitMS = (new Random().Next(200, 4000));
            Thread.Sleep(waitMS);
            return Ok($"This is the response from the Non-Instrumented Function, it waited {waitMS}ms");
        }
    }
}
