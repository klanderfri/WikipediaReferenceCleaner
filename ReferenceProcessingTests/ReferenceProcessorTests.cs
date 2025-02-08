using ReferenceProcessing;

namespace ReferenceProcessingTests
{
    //Test-branch.
    //DO NOT MERGE

    public class ReferenceProcessorTests
    {
        [Fact]
        public void ProcessShortReferences()
        {
            RunTest("small-reference-list.input.txt");
        }

        [Fact]
        public void ProcessorHandlesAlreadyProcessedReferences()
        {
            RunTest("small-reference-list.expected-output.txt");
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Fail();
        }

        private static void RunTest(string inputDataFileName)
        {
            var tempFolder = TestUtils.CreateTempFolder();
            try
            {
                var messenger = new Messenger();
                var processor = new ReferenceProcessor(messenger);

                var inputFilePath = TestUtils.GetFullPathToTestFile(inputDataFileName);
                var outputFilePath = TestUtils.CreateTempOutputFilepath(tempFolder);

                var success = processor.ProcessReferences(inputFilePath, outputFilePath);
                Assert.True(success);

                var expected = TestUtils.GetDataFromTestFile("small-reference-list.expected-output.txt");
                var resultText = File.ReadAllText(outputFilePath);
                Assert.Equal(expected, resultText);
            }
            finally
            {
                Directory.Delete(tempFolder, true);
            }
        }
    }
}
