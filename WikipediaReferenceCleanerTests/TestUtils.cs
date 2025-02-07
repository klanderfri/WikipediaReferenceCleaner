using System.Reflection;

namespace ReferenceProcessingTests
{
    public static class TestUtils
    {
        public static string GetDataFromTestFile(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{assembly.GetName().Name}.{filename}";

            var filestream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
                ?? throw new NullReferenceException("Could not extract filestream.");

            var reader = new StreamReader(filestream);
            return reader.ReadToEnd();
        }
    }
}
