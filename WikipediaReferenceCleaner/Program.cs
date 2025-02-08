using ReferenceProcessing;

namespace WikipediaReferenceCleaner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var messenger = new Messenger();
            var processor = new ReferenceProcessor(messenger);
            messenger.MessageRaised += Messenger_MessageRaised;

            var inputFilePath = GetFilepath("input");
            var outputFilePath = GetFilepath("output");
            var wasSuccessful = processor.ProcessReferences(inputFilePath, outputFilePath);

            WriteResultMessage(wasSuccessful)
        }

        private static void Messenger_MessageRaised(object? sender, EventArgs e)
        {
            var args = (MessageArgs)e;
            if (args.IsErrorMessage)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine(((MessageArgs)e).Message);

            Console.ResetColor();
        }

        private static string GetFilepath(string type)
        {
            var filename = $"wp-ref-cleaner-{type}.txt";
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return Path.Combine(desktop, filename);
        }

        private static void WriteResultMessage(bool wasSuccessful)
        {
            Console.WriteLine();

            if (wasSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("The references was successfully processed.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed processing references.");
            }

            Console.ResetColor();
        }
    }
}
