namespace ReferenceProcessing
{
    public class ReferenceProcessor(Messenger messenger)
    {
        const string Indention = "  ";

        public bool ProcessReferences(string inputFilePath, string outputFilePath)
        {
            var rawReferences = ReadFromInputFile(inputFilePath);

            messenger.SendMessage("Parsing references...");
            var refReader = new RefListReader(messenger);
            var readSuccessfully = refReader.ReadReferences(rawReferences, out List<Reference> readReferences);
            if (!readSuccessfully) { return false; }

            messenger.SendMessage("Cleaning up references...");
            var refWriter = new RefListWriter(messenger);
            var wroteSuccessfully = refWriter.ConvertReferencesToString(readReferences, out string formatedReferences);
            if (!wroteSuccessfully) { return false; }

            WriteToOutputFile(outputFilePath, formatedReferences);

            return true;
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
