using Autometrics.Instrumentation.Attributes;

namespace Autometrics.Samples.InstrumentedExamples
{
    public class PresentationLayer
    {
        public BusinessLayer BusinessLayer { get; set; }

        [AutometricsMethod]
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

            BusinessLayer.ProcessRequest();
        }

        [AutometricsMethod]
        public void UserAuthentication()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("User authentication in Presentation Layer.");
            Console.ResetColor();
            Thread.Sleep(new Random().Next(100, 300));
        }

        [AutometricsMethod]
        public void ImageFetching()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Image fetching in Presentation Layer.");
            Console.ResetColor();
            Thread.Sleep(new Random().Next(200, 600));
        }

        [AutometricsMethod]
        public void ProfileUpdates()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Profile updates in Presentation Layer.");
            Console.ResetColor();
            Thread.Sleep(new Random().Next(300, 800));
        }

        [AutometricsMethod]
        public void OrderGeneration()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Order generation in Presentation Layer.");
            Console.ResetColor();
            Thread.Sleep(new Random().Next(400, 1000));
        }
    }
}