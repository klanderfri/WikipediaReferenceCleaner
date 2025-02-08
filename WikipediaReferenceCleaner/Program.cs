using ReferenceProcessing;

namespace WikipediaReferenceCleaner
{
    internal class Program
    {
        const string Indention = "  ";

        static void Main(string[] args)
        {
            var messanger = new Messenger();
            messanger.MessageRaised += Messanger_MessageRaised;

            var rawReferences = ReadFromInputFile();

            Console.WriteLine("Parsing references...");
            var refReader = new RefListReader(messanger);
            var readSuccessfully = refReader.ReadReferences(rawReferences, out List<Reference> readReferences);

            if (readSuccessfully)
            {
                Console.WriteLine("Cleaning up references...");
                var refWriter = new RefListWriter(messanger);
                var formatedReferences = refWriter.ConvertReferencesToString(readReferences);

                WriteToOutputFile(formatedReferences);

                Console.WriteLine("The references was successfully processed.");
            }
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

        private static void Messanger_MessageRaised(object? sender, EventArgs e)
        {
            Console.WriteLine(((MessageArgs)e).Message);
        }
    }
}
