using ReferenceProcessing;

namespace WikipediaReferenceCleaner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var messanger = new Messenger();
            var processor = new ReferenceProcessor(messanger);
            messanger.MessageRaised += Messanger_MessageRaised;

            var inputFilePath = GetFilepath("input");
            var outputFilePath = GetFilepath("output");
            processor.ProcessReferences(inputFilePath, outputFilePath);
        }

        private static void Messanger_MessageRaised(object? sender, EventArgs e)
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
