using ReferenceProcessing;

namespace ReferenceProcessingTests
{
    public class ReferencesReadAndWriteTests
    {
        [Fact]
        public void ProcessShortReferencesTest()
        {
            var input = TestUtils.GetDataFromTestFile("small-reference-list.input.txt");
            var expected = TestUtils.GetDataFromTestFile("small-reference-list.expected-output.txt");

            var (readSuccess, formatedRefs) = RunTest(input);

            Assert.True(readSuccess);
            Assert.Equal(expected, formatedRefs);
        }

        [Fact]
        public void ProcessorHandlesAlreadyProcessedReferencesTest()
        {
            var expected = TestUtils.GetDataFromTestFile("small-reference-list.expected-output.txt");

            var (success, formatedRefs) = RunTest(expected);

            Assert.True(success);
            Assert.Equal(expected, formatedRefs);
        }

        private static (bool success, string formatedRefs) RunTest(string rawReferences)
        {
            var messsenger = new Messenger();
            var reader = new RefListReader(messsenger);
            var writer = new RefListWriter(messsenger);

            var readSuccessfully = reader.ReadReferences(rawReferences, out List<Reference> readReferences);
            var wroteSuccessfully = writer.ConvertReferencesToString(readReferences, out string formatedReferences);

            return (readSuccessfully && wroteSuccessfully, formatedReferences);
        }
    }
}
