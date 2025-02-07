using WikipediaReferenceCleaner;

namespace WikipediaReferenceCleanerTests
{
    public class ReferencesProcessorTests
    {
        [Fact]
        public void ProcessTwoReferencesTest()
        {
            const string input = "{{reflist|refs=\r\n# Governments and Agencies  \r\n<ref name=\"polisen\">{{cite web |title=04 februari 12.33, Skottlossning, Örebro |url=https://polisen.se/aktuellt/handelser/2025/februari/4/04-februari-12.33-skottlossning-orebro/ |date=4 February 2025 |website=polisen.se |publisher=[[Swedish Police Authority]] |access-date=4 February 2025 |language=sv |archive-date=5 February 2025 |archive-url=https://web.archive.org/web/20250205220201/https://polisen.se/aktuellt/handelser/2025/februari/4/04-februari-12.33-skottlossning-orebro/ |url-status=live }}</ref>\r\n\r\n# Swedish Radio (SR)\r\n<ref name=\"sr-csk-i-stabslage\">{{Cite news|url=https://www.sverigesradio.se/artikel/just-nu-csk-upp-i-stabslage-efter-skjutningen-i-orebro|title=Tiotal döda i skolskjutning – så stöttar Värmland Örebro|date=4 February 2025|lang=sv|publisher=[[Sveriges Radio]]|access-date=4 February 2025|archive-date=5 February 2025|archive-url=https://web.archive.org/web/20250205061239/https://www.sverigesradio.se/artikel/just-nu-csk-upp-i-stabslage-efter-skjutningen-i-orebro|url-status=live}}</ref>\r\n}}\r\n";
            const string expected = "{{reflist|refs=\r\n\r\n# Governments and Agencies\r\n<ref name=\"polisen\">\r\n  {{cite web\r\n    |title=04 februari 12.33, Skottlossning, Örebro\r\n    |url=https://polisen.se/aktuellt/handelser/2025/februari/4/04-februari-12.33-skottlossning-orebro/\r\n    |website=polisen.se\r\n    |publisher=[[Swedish Police Authority]]\r\n    |date=4 February 2025\r\n    |access-date=4 February 2025\r\n    |language=sv\r\n    |url-status=live\r\n    |archive-url=https://web.archive.org/web/20250205220201/https://polisen.se/aktuellt/handelser/2025/februari/4/04-februari-12.33-skottlossning-orebro/\r\n    |archive-date=5 February 2025 }}</ref>\r\n\r\n# Swedish Radio (SR)\r\n<ref name=\"sr-csk-i-stabslage\">\r\n  {{cite news\r\n    |title=Tiotal döda i skolskjutning – så stöttar Värmland Örebro\r\n    |url=https://www.sverigesradio.se/artikel/just-nu-csk-upp-i-stabslage-efter-skjutningen-i-orebro\r\n    |publisher=[[Sveriges Radio]]\r\n    |date=4 February 2025\r\n    |access-date=4 February 2025\r\n    |lang=sv\r\n    |url-status=live\r\n    |archive-url=https://web.archive.org/web/20250205061239/https://www.sverigesradio.se/artikel/just-nu-csk-upp-i-stabslage-efter-skjutningen-i-orebro\r\n    |archive-date=5 February 2025 }}</ref>\r\n\r\n}}\r\n";

            var actual = RunTest(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ProcessorHandlesAlreadyProcessedReferencesTest()
        {
            const string expected = "{{reflist|refs=\r\n\r\n# Governments and Agencies\r\n<ref name=\"polisen\">\r\n  {{cite web\r\n    |title=04 februari 12.33, Skottlossning, Örebro\r\n    |url=https://polisen.se/aktuellt/handelser/2025/februari/4/04-februari-12.33-skottlossning-orebro/\r\n    |website=polisen.se\r\n    |publisher=[[Swedish Police Authority]]\r\n    |date=4 February 2025\r\n    |access-date=4 February 2025\r\n    |language=sv\r\n    |url-status=live\r\n    |archive-url=https://web.archive.org/web/20250205220201/https://polisen.se/aktuellt/handelser/2025/februari/4/04-februari-12.33-skottlossning-orebro/\r\n    |archive-date=5 February 2025 }}</ref>\r\n\r\n# Swedish Radio (SR)\r\n<ref name=\"sr-csk-i-stabslage\">\r\n  {{cite news\r\n    |title=Tiotal döda i skolskjutning – så stöttar Värmland Örebro\r\n    |url=https://www.sverigesradio.se/artikel/just-nu-csk-upp-i-stabslage-efter-skjutningen-i-orebro\r\n    |publisher=[[Sveriges Radio]]\r\n    |date=4 February 2025\r\n    |access-date=4 February 2025\r\n    |lang=sv\r\n    |url-status=live\r\n    |archive-url=https://web.archive.org/web/20250205061239/https://www.sverigesradio.se/artikel/just-nu-csk-upp-i-stabslage-efter-skjutningen-i-orebro\r\n    |archive-date=5 February 2025 }}</ref>\r\n\r\n}}\r\n";

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
