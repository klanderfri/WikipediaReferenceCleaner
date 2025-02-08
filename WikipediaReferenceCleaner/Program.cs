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
            processor.ProcessReferences(inputFilePath, outputFilePath);
        }

        private static void Messenger_MessageRaised(object? sender, EventArgs e)
        {
            Console.WriteLine(((MessageArgs)e).Message);
        }

        private static string GetFilepath(string type)
        {
            var filename = $"wp-ref-cleaner-{type}.txt";
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return Path.Combine(desktop, filename);
        }
    }
}
