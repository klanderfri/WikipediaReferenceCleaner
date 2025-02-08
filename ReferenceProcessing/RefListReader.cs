namespace ReferenceProcessing
{
    public class RefListReader(Messenger messenger)
    {
        public bool ReadReferences(string rawRefList, out List<Reference> references)
        {
            references = [];
            string lastGroup = "";
            var index = FindRefListStart(rawRefList);

            //Ceck that there is a reflist template to place the references in.
            if (index < 0)
            {
                var message = "There seems to be no reflist template in the article.";
                messenger.SendMessage(message);
                return false;
            }

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
                    messenger.SendMessage(refNameMissingMessage);
                    return false;
                }

                //Check if we have found a reference.
                if (line.StartsWith("<ref "))
                {
                    var endTag = "</ref>";
                    var lengthOfReference = rawRefList[index..].IndexOf(endTag) + endTag.Length;

                    //A reference may span over multiple lines, so update the line.
                    line = rawRefList.AsSpan(index, lengthOfReference);

                    var reference = ReadReference(line, lastGroup);
                    references.Add(reference);

                    index += lengthOfReference;
                    continue;
                }

                //Check if we have reched the closing tag of the reflist template.
                if (line.StartsWith("}}")) { break; }

                var unknownLineTypeMessage = "Unknown type of line encountered.";
                messenger.SendMessage(unknownLineTypeMessage);
                return false;
            }

            messenger.SendMessage($"Successfully read {references.Count} references.");
            return true;
        }

        private static int FindRefListStart(string rawRefList)
        {
            var index = rawRefList.IndexOf("{{reflist");

            if (index < 0)
            {
                //We can't process the references if there is no reflist
                //template to place them in.
                return -1;
            }

            index = rawRefList.IndexOf("refs", index);
            index = rawRefList.IndexOf('=', index);
            index++;

            return index;
        }

        private static Reference ReadReference(ReadOnlySpan<char> line, string lastGroup)
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

            return reference;
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
    }
}
