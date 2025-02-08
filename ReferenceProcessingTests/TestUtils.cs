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

        public static string GetFullPathToTestFile(string filename)
        {
            var testProjectName = Assembly.GetCallingAssembly().GetName().Name;
            var projectFolder = new DirectoryInfo(Environment.CurrentDirectory);

            while (projectFolder.Name != testProjectName)
            {
                projectFolder = projectFolder.Parent!;
            }

            return Path.Combine(projectFolder.FullName, filename);
        }

        public static string CreateTempFolder()
        {
            string tempFolderPath;
            do
            {
                tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            } while (Directory.Exists(tempFolderPath));

            Directory.CreateDirectory(tempFolderPath);
            return tempFolderPath;
        }

        public static string CreateTempOutputFilepath(string pathToTempFolder)
        {
            var tempFilename = $"{Guid.NewGuid()}-output.txt";
            return Path.Combine(pathToTempFolder, tempFilename);
        }
    }
}
