using Autometrics.Instrumentation.Attributes;
using Autometrics.Instrumentation.SLO;

namespace Autometrics.Samples.ConsoleApp.InstrumentedExamples
{
    public class PresentationLayer
    {
        public BusinessLayer? BusinessLayer { get; set; }

        [Autometrics]
        public void HandleRequest()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Handling request in Presentation Layer.");
            Console.ResetColor();
            Thread.Sleep(new Random().Next(100, 500));

            UserAuthentication();
            ImageFetching();
            ProfileUpdates();
            OrderGeneration();

            BusinessLayer?.ProcessRequest();
        }

        [Autometrics("UserAuth", ObjectivePercentile.P99)]
        public void UserAuthentication()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("User authentication in Presentation Layer.");
            Console.ResetColor();
            Thread.Sleep(new Random().Next(100, 300));

            // Simulate a 1.5% failure rate to trigger our SLO.
            if (new Random().NextDouble() < 0.015)
            {
                throw new InvalidOperationException("An error occurred in the Presentation Layer.");
            }
        }

        [Autometrics]
        public void ImageFetching()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Image fetching in Presentation Layer.");
            Console.ResetColor();
            Thread.Sleep(new Random().Next(200, 600));
        }

        [Autometrics]
        public void ProfileUpdates()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Profile updates in Presentation Layer.");
            Console.ResetColor();
            Thread.Sleep(new Random().Next(300, 800));
        }

        [Autometrics("OrderCreation", ObjectivePercentile.P99, ObjectiveLatency.Ms500, ObjectiveType.SuccessAndLatency)]
        public void OrderGeneration()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Order generation in Presentation Layer.");
            Console.ResetColor();
            Thread.Sleep(new Random().Next(400, 1000));
        }
    }
}