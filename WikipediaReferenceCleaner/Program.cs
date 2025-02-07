using ReferenceProcessing;

namespace WikipediaReferenceCleaner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rawReferences = ReadFromInputFile();

            var readReferences = RefListReader.ReadReferences(rawReferences);
            var formatedReferences = RefListWriter.ConvertReferencesToString(readReferences);

            var outputFile = WriteToOuputFile(formatedReferences);

            Console.WriteLine("The references are processed. The result is found at");
            Console.WriteLine(outputFile);
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

        private static string GetFilepath(string type)
        {
            var filename = $"wp-ref-cleaner-{type}.txt";
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return Path.Combine(desktop, filename);
        }
    }
}
