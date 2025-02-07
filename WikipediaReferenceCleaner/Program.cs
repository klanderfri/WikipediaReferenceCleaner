using ReferenceProcessing;

namespace WikipediaReferenceCleaner
{
    internal class Program
    {
        const string Indention = "  ";

        static void Main(string[] args)
        {
            var rawReferences = ReadFromInputFile();

            Console.WriteLine("Parsing references...");
            var readReferences = RefListReader.ReadReferences(rawReferences);

            Console.WriteLine("Cleaning up references...");
            var formatedReferences = RefListWriter.ConvertReferencesToString(readReferences);

            WriteToOutputFile(formatedReferences);

            Console.WriteLine("The references was successfully processed.");
        }

        private static string ReadFromInputFile()
        {
            var filepath = GetFilepath("input");

            Console.WriteLine("Reading references from file at");
            Console.WriteLine(Indention + filepath);
            Console.WriteLine(Indention + "...");

            return File.ReadAllText(filepath);
        }

        private static void WriteToOutputFile(string processedReferences)
        {
            var filepath = GetFilepath("output");

            Console.WriteLine("Writing references to file at");
            Console.WriteLine(Indention + filepath);
            Console.WriteLine(Indention + "...");

            File.WriteAllText(filepath, processedReferences);
        }

        private static string GetFilepath(string type)
        {
            var filename = $"wp-ref-cleaner-{type}.txt";
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return Path.Combine(desktop, filename);
        }
    }
}
