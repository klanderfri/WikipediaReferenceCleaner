using ReferenceProcessing;

namespace ReferenceProcessingTests
{
    public class ReferencesProcessorTests
    {
        [Fact]
        public void ProcessTwoReferencesTest()
        {
            var input = TestUtils.GetDataFromTestFile("small-reference-list.input.txt");
            var expected = TestUtils.GetDataFromTestFile("small-reference-list.expected-output.txt");

            var actual = RunTest(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ProcessorHandlesAlreadyProcessedReferencesTest()
        {
            var expected = TestUtils.GetDataFromTestFile("small-reference-list.expected-output.txt");

            var actual = RunTest(expected);

            Assert.Equal(expected, actual);
        }

        private static string RunTest(string input)
        {
            var inputLines = input.Split("\r\n", StringSplitOptions.None);

            var processor = new ReferencesProcessor();
            return processor.ProcessLines(inputLines);
        }
    }
}
