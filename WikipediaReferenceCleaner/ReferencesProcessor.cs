using System.Text;

namespace WikipediaReferenceCleaner
{
    public class ReferencesProcessor
    {
        private readonly List<Reference> references = [];
        private string lastGroup = "";

        public string ProcessLines(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                ProcessLine(line);
            }

            return ConvertReferencesToString(references);
        }

        private void ProcessLine(string line)
        {
            if (SkipLine(line)) { return; }
            ProcessGroup(line);
            ProcessReference(line);
        }

        private static bool SkipLine(string line)
        {
            var skipLines = new List<string>() { "{{reflist", "|refs=", "refs=" };

            foreach (var skip in skipLines)
            {
                if (string.IsNullOrWhiteSpace(line)) { return true; }
                if (line.StartsWith(skip)) { return true; }
            }
            return false;
        }

        private void ProcessGroup(string line)
        {
            if (!line.StartsWith('#')) { return; }

            lastGroup = line[1..].Trim();
        }

        private void ProcessReference(string line)
        {
            //All references in a reflist must have a name,
            //otherwise they can't be used in the article.
            if (line.StartsWith("<ref>"))
            {
                var message = "All references MUST have a 'name' tag.";
                throw new InvalidOperationException(message);
            }

            if (!line.StartsWith("<ref ")) { return; }

            //Extract mandatory data.
            var reference = new Reference()
            {
                Group = lastGroup,
                Name = GetName(line),
                CiteType = GetCiteType(line),
            };

            //Extract the data inside the citation.
            var referenceDataStart = line.IndexOf('|');
            var foundallValues = false;
            while (!foundallValues)
            {
                var tagNameStart = referenceDataStart + 1;
                var tagNameEnd = line.IndexOf('=', tagNameStart);
                var tagName = line[tagNameStart..tagNameEnd];

                var tagValueStart = tagNameEnd + 1;
                var tagValueEnd = line.IndexOf('|', tagValueStart);
                if (tagValueEnd < 0)
                {
                    //We found the last tag.
                    tagValueEnd = line.IndexOf('}');
                    foundallValues = true;
                }
                var tagValue = line[tagValueStart..tagValueEnd].Trim();

                reference.Data.Add(tagName, tagValue);

                referenceDataStart = tagValueEnd;
            }

            references.Add(reference);
        }

        private static string GetName(string line)
        {
            var tagStart = "<ref name=\"";
            var tagEnd = "\">";
            var (start, end) = GetTagValueSpan(line, tagStart, tagEnd);
            return line[start..end].Trim();
        }

        private static string GetCiteType(string line)
        {
            var tagStart = "{{cite ";
            var tagEnd = "|";
            var (start, end) = GetTagValueSpan(line, tagStart, tagEnd);
            return line[start..end].Trim();
        }

        private static (int start, int end) GetTagValueSpan(string line, string tagStart, string tagEnd)
        {
            var startIndex = GetTagValueStartIndex(line, tagStart);
            var endIndex = GetTagValueEndIndex(line, tagEnd, startIndex);

            return (startIndex, endIndex);
        }

        private static int GetTagValueStartIndex(string line, string tagStart)
        {
            return line.IndexOf(tagStart, StringComparison.OrdinalIgnoreCase) + tagStart.Length;
        }

        private static int GetTagValueEndIndex(string line, string tagEnd, int tagStartIndex)
        {
            return line.IndexOf(tagEnd, tagStartIndex);
        }

        private static string ConvertReferencesToString(IEnumerable<Reference> references)
        {
            var refText = new StringBuilder();
            refText.AppendLine("{{reflist|refs=");

            var refGroups = references.GroupBy(r => r.Group);

            foreach (var group in refGroups)
            {
                refText.AppendLine();
                refText.AppendLine($"# {group.Key}");
                foreach (var reference in group)
                {
                    refText.Append(ConvertReferenceToString(reference));
                }
                refText.AppendLine();
            }

            refText.AppendLine();
            refText.AppendLine("}}");

            return refText.ToString();
        }

        private static string ConvertReferenceToString(Reference reference)
        {
            var refText = new StringBuilder();

            refText.AppendLine($"<ref name=\"{reference.Name}\">");
            refText.AppendLine("  {{cite " + reference.CiteType.ToString().ToLower());

            var items = reference.Data.OrderBy(item => item.Key, new ReferenceDataSorter());

            foreach(var item in items)
            {
                refText.AppendLine($"    |{item.Key}={item.Value}");
            }

            refText.Remove(refText.Length - Environment.NewLine.Length, Environment.NewLine.Length);
            refText.AppendLine(" }}</ref>");

            return refText.ToString();
        }
    }
}
