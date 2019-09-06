using System.IO;

namespace DotnetGitTool
{
    public static class DirectoryUtil
    {
        public static void Delete(string path, string searchPattern)
        {
            foreach (var file in Directory.EnumerateFiles(path, searchPattern))
            {
                File.Delete(file);
            }
        }
    }
}
