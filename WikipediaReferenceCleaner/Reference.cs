namespace WikipediaReferenceCleaner
{
    public class Reference
    {
        public string? Group { get; set; }

        public required string Name { get; set; }
        public required string CiteType { get; set; }

        public Dictionary<string, string> Data { get; set; } = [];
    }
}
