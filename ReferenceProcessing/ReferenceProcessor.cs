namespace ReferenceProcessing
{
    public class ReferenceProcessor(Messenger messenger)
    {
        const string Indention = "  ";

        public void ProcessReferences(string inputFilePath, string outputFilePath)
        {
            var rawReferences = ReadFromInputFile(inputFilePath);

            messenger.SendMessage("Parsing references...");
            var refReader = new RefListReader(messenger);
            var readSuccessfully = refReader.ReadReferences(rawReferences, out List<Reference> readReferences);

            if (readSuccessfully)
            {
                messenger.SendMessage("Cleaning up references...");
                var refWriter = new RefListWriter(messenger);
                var formatedReferences = refWriter.ConvertReferencesToString(readReferences);

                WriteToOutputFile(outputFilePath, formatedReferences);

                messenger.SendMessage("The references was successfully processed.");
            }
        }

        private string ReadFromInputFile(string inputFilepath)
        {
            messenger.SendMessage("Reading references from file at");
            messenger.SendMessage(Indention + inputFilepath);
            messenger.SendMessage(Indention + "...");

            return File.ReadAllText(inputFilepath);
        }

        private void WriteToOutputFile(string outputFilepath, string processedReferences)
        {
            messenger.SendMessage("Writing references to file at");
            messenger.SendMessage(Indention + outputFilepath);
            messenger.SendMessage(Indention + "...");

            File.WriteAllText(outputFilepath, processedReferences);
        }
    }
}
