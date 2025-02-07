namespace WikipediaReferenceCleaner
{
    public class ReferenceDataSorter : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            var xValue = GetValue(x);
            var yValue = GetValue(y);

            return xValue - yValue;
        }

        private static int GetValue(string? name) =>
            name switch
            {
                "title" => 1,
                "url" => 2,
                "website" => 3,
                "publisher" => 4,
                "date" => 5,
                "access-date" => 6,
                "language" => 7,
                "lang" => 7,
                "url-status" => 8,
                "archive-url" => 9,
                "archive-date" => 10,
                _ => 999
            };
    }
}
