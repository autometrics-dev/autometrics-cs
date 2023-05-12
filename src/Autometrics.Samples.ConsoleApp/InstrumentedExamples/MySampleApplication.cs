using Autometrics.Samples.Library;
using System;
using System.Threading;

namespace Autometrics.Samples.ConsoleApp.InstrumentedExamples
{
    internal class MySampleApplication
    {
        public static void DoApplicationStuff()
        {
            ExternalExample externalExample = new ExternalExample();
            var presentationLayer = new PresentationLayer();
            var businessLayer = new BusinessLayer();
            var dataAccessLayer = new DataAccessLayer();

            presentationLayer.BusinessLayer = businessLayer;
            businessLayer.DataAccessLayer = dataAccessLayer;

            while (true)
            {
                try
                {
                    presentationLayer.HandleRequest();
                    externalExample.InstrumentedMethod();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unhappy customers, our application has failed but should restart in 1 second.\nThis should be properly tagged with an 'error' call on some or multiple functions.");
                    Console.WriteLine($"Error: {ex.Message}");
                }

                int countdown = 10;
                Console.WriteLine($"Generation complete. Press any key to exit or wait for the {countdown} seconds countdown to restart.");

                while (countdown > 0)
                {
                    if (Console.KeyAvailable)
                    {
                        Console.ReadKey(true); // Read the key to clear the input buffer
                        return; // Return to the main menu
                    }

                    Thread.Sleep(1000); // Sleep for 1 second
                    countdown--;

                    // Update the countdown message
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.WriteLine($"Generation complete. Press any key to exit or wait for the {countdown} seconds countdown to restart.      ");
                }
            }
        }
    }
}
