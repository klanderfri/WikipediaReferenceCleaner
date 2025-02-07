using ReferenceProcessing;

namespace ReferenceProcessingTests
{
    public class ReferencesProcessorTests
    {
        [Fact]
        public void ProcessShortReferencesTest()
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

        private static string RunTest(string rawReferences)
        {
            var readReferences = RefListReader.ReadReferences(rawReferences);
            var formatedReferences = RefListWriter.ConvertReferencesToString(readReferences);

            return formatedReferences;
        }
    }
}
