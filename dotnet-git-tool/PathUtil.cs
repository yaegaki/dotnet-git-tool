using System;
using System.IO;

namespace DotnetGitTool
{
    public static class PathUtil
    {
        public static readonly string GitToolRoot = Path.Join(GetHomeDirectory(), ".dotnet-git-tool");
        public static readonly string SrcRoot = Path.Combine(GitToolRoot, "src");
        public static readonly string NupkgRoot = Path.Combine(GitToolRoot, "nupkg");

        private static string GetHomeDirectory()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    return Environment.GetEnvironmentVariable("HOME");
                default:
                    return Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            }
        }
    }
}
