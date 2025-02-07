using ReferenceProcessing;

namespace WikipediaReferenceCleaner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rawReferences = ReadFromInputFile();

            var processor = new ReferencesProcessor();
            var processedReferences = processor.ProcessLines(rawReferences);

            var outputFile = WriteToOuputFile(processedReferences);

            Console.WriteLine("The references are processed. The result is found at");
            Console.WriteLine(outputFile);
        }

        private static string GetFilepath(string type)
        {
            var filename = $"wp-ref-cleaner-{type}.txt";
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return Path.Combine(desktop, filename);
        }

        private static string ReadFromInputFile()
        {
            var filepath = GetFilepath("input");
            return File.ReadAllText(filepath);
        }

        private static string WriteToOuputFile(string processedReferences)
        {
            var filepath = GetFilepath("output");
            File.WriteAllText(filepath, processedReferences);
            return filepath;
        }
    }
}
