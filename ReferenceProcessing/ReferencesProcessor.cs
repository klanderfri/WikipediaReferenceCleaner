using System.Text;

namespace ReferenceProcessing
{
    public class ReferencesProcessor
    {
        private readonly List<Reference> references = [];
        private string lastGroup = "";

        public string ProcessLines(string rawRefList)
        {
            var index = FindRefListStart(rawRefList);

            //Process all lines.
            while (true)
            {
                //Check if we have found an empty line.
                var indexNextNewline = rawRefList.IndexOf(Environment.NewLine, index);
                if (indexNextNewline == index)
                {
                    index += Environment.NewLine.Length;
                    continue;
                }

                var line = rawRefList.AsSpan(index, indexNextNewline - index).TrimStart();

                //Check if we have found a group name.
                if (line.StartsWith('#'))
                {
                    lastGroup = line.ToString()[1..].Trim();

                    index = indexNextNewline + Environment.NewLine.Length;
                    continue;
                }

                //All references in a reflist must have a name,
                //otherwise they can't be used in the article.
                if (line.StartsWith("<ref>"))
                {
                    var refNameMissingMessage = "All references MUST have a 'name' tag.";
                    throw new InvalidOperationException(refNameMissingMessage);
                }

                //Check if we have found a reference.
                if (line.StartsWith("<ref "))
                {
                    var endTag = "</ref>";
                    var lengthOfReference = rawRefList[index..].IndexOf(endTag) + endTag.Length;

                    //A reference may span over multiple lines, so update the line.
                    line = rawRefList.AsSpan(index, lengthOfReference);

                    ProcessReference(line);

                    index += lengthOfReference;
                    continue;
                }

                //Check if we have reched the closing tag of the reflist template.
                if (line.StartsWith("}}")) { break; }

                var unknownLineTypeMessage = "Unknown type of line encountered.";
                throw new NotImplementedException(unknownLineTypeMessage);
            }

            return ConvertReferencesToString(references);
        }

        private static int FindRefListStart(string rawRefList)
        {
            var index = rawRefList.IndexOf("{{reflist");

            if (index < 0)
            {
                var message = "There seems to be no reflist template in the article.";
                throw new InvalidOperationException(message);
            }

            index = rawRefList.IndexOf("refs", index);
            index = rawRefList.IndexOf('=', index);
            index++;

            return index;
        }

        private void ProcessReference(ReadOnlySpan<char> line)
        {
            //Extract mandatory data.
            var reference = new Reference()
            {
                Group = lastGroup,
                Name = GetName(line),
                CiteType = GetCiteType(line),
            };

            //Extract the data inside the citation.
            var referenceDataStart = line.IndexOf('|');
            line = line[referenceDataStart..line.IndexOf('}')].Trim();
            while (line.Length > 0)
            {
                var tagNameStart = 1;
                var tagNameEnd = line.IndexOf('=');
                var tagName = line[tagNameStart..tagNameEnd].Trim();

                var tagValueStart = tagNameEnd + 1;
                var tagValueEnd = GetTagValueEndIndex(line, tagValueStart);
                var tagValue = line[tagValueStart..tagValueEnd].Trim();

                reference.Data.Add(tagName.ToString(), tagValue.ToString());

                referenceDataStart = tagValueEnd;
                line = line[referenceDataStart..];
            }

            references.Add(reference);
        }

        private static int GetTagValueEndIndex(ReadOnlySpan<char> line, int tagValueStart)
        {
            ReadOnlySpan<char> tagValue;
            int endIndex = tagValueStart;
            int wikilinkStarts;
            int wikilinkEnds;

            while (true)
            {
                var nextPipe = line[(endIndex)..].IndexOf('|');
                if (nextPipe < 0)
                {
                    //We found the last tag.
                    endIndex = line.Length;
                    break;
                }

                endIndex += nextPipe;
                tagValue = line[tagValueStart..endIndex];

                wikilinkStarts = CountOccurances(tagValue, "[[");
                wikilinkEnds = CountOccurances(tagValue, "]]");

                if (wikilinkStarts == wikilinkEnds) { break; }
                else
                {
                    endIndex++;
                }
            }

            return endIndex;
        }

        private static int CountOccurances(ReadOnlySpan<char> heystack, string needle)
        {
            var count = 0;
            var index = 0;

            while (true)
            {
                heystack = heystack[index..];
                index = heystack.IndexOf(needle);

                if (index < 0) { break; }

                count++;
                index++;
            }

            return count;
        }

        private static string GetName(ReadOnlySpan<char> line)
        {
            var tagStart = "<ref name=\"";
            var tagEnd = "\">";
            var tagValue = GetTagValueSpan(line, tagStart, tagEnd);
            return tagValue.Trim().ToString();
        }

        private static string GetCiteType(ReadOnlySpan<char> line)
        {
            var tagStart = "{{cite ";
            var tagEnd = "|";
            var tagValue = GetTagValueSpan(line, tagStart, tagEnd);
            return tagValue.Trim().ToString();
        }

        private static ReadOnlySpan<char> GetTagValueSpan(ReadOnlySpan<char> line, string tagStart, string tagEnd)
        {
            var startIndex = line.IndexOf(tagStart, StringComparison.OrdinalIgnoreCase) + tagStart.Length;
            line = line[startIndex..];
            var endIndex = line.IndexOf(tagEnd);
            line = line[..endIndex];

            return line;
        }

        private static string ConvertReferencesToString(IEnumerable<Reference> references)
        {
            var refText = new StringBuilder();
            refText.AppendLine("{{reflist|refs=");
            refText.AppendLine();

            var refGroups = references.GroupBy(r => r.Group);

            foreach (var group in refGroups)
            {
                refText.AppendLine($"# {group.Key}");
                foreach (var reference in group)
                {
                    refText.Append(ConvertReferenceToString(reference));
                }
                refText.AppendLine();
            }

            refText.AppendLine("}}");

            return refText.ToString();
        }

        private static string ConvertReferenceToString(Reference reference)
        {
            var refText = new StringBuilder();

            refText.AppendLine($"<ref name=\"{reference.Name}\">");
            refText.AppendLine("  {{cite " + reference.CiteType.ToString().ToLower());

            var items = reference.Data.OrderBy(item => item.Key, new ReferenceDataSorter());
            var longestItemName = items
                .Select(i => i.Key)
                .Max(k => k.Length);

            foreach (var item in items)
            {
                refText.Append($"    |");
                refText.Append(item.Key.PadRight(longestItemName + 1));
                refText.Append($"= {item.Value}");
                refText.AppendLine();
            }

            refText.Remove(refText.Length - Environment.NewLine.Length, Environment.NewLine.Length);
            refText.AppendLine(" }}</ref>");

            return refText.ToString();
        }
    }
}
