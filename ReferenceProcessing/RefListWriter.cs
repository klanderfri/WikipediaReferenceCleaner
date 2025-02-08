using System.Text;

namespace ReferenceProcessing
{
    public class RefListWriter(Messenger messenger)
    {
        public string ConvertReferencesToString(IEnumerable<Reference> references)
        {
            var refText = new StringBuilder();
            refText.AppendLine("{{reflist|refs=");
            refText.AppendLine();

            var refGroups = references.GroupBy(r => r.Group);

            var refCounter = 0;
            foreach (var group in refGroups)
            {
                refText.AppendLine($"# {group.Key}");
                foreach (var reference in group)
                {
                    refText.Append(ConvertReferenceToString(reference));
                    refCounter++;
                }
                refText.AppendLine();
            }

            refText.AppendLine("}}");

            messenger.SendMessage($"Successfully formatted {refCounter} references.");
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
