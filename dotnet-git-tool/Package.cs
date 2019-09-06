using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DotnetGitTool
{
    [Serializable]
    public class PackageParseException : Exception
    {
        public PackageParseException() { }
        public PackageParseException(string message) : base(message) { }
        public PackageParseException(string message, Exception inner) : base(message, inner) { }
        protected PackageParseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    public class Package
    {
        /// <summary>
        /// Git hosting provider.
        /// ex) "github.com"
        /// </summary>
        /// <value></value>
        public string Hosting { get; }
        /// <summary>
        /// Git repository root.
        /// ex) "xxx/yyy"
        /// </summary>
        /// <value></value>
        public string RepositoryRoot { get; }
        /// <summary>
        /// Git hosting url.
        /// ex) "https://github.com/xxx/yyy"
        /// </summary>
        /// <value></value>
        public string RepositoryUrl { get; }
        /// <summary>
        /// Tool project's directory.
        /// If it is root directory, ToolProjectDirectroy is string.Empty.
        /// ex) "", "cmd/hoge"
        /// </summary>
        /// <value></value>
        public string ToolProjectDirectory { get; }

        public static ValueTask<Package> ParseAsync(string path)
        {
            if (!path.StartsWith("github.com"))
            {
                throw new PackageParseException("unknown hosting provider. currently only 'github.com' is supported.");
            }


            var temp = path.Split('/');
            if (temp.Length < 2)
            {
                throw new PackageParseException("invalid package.");
            }

            var hosting = temp[0];
            var repositoryRoot = $"{temp[1]}/{temp[2]}";
            var repositoryUrl = $"https://{hosting}/{temp[1]}/{temp[2]}";
            var toolProjectDirectory = temp.Length > 3 ? string.Join("/", temp.Skip(3)) : string.Empty;

            var package = new Package(hosting, repositoryRoot, repositoryUrl, toolProjectDirectory);
            return new ValueTask<Package>(package);
        }

        private Package(string hosting, string repositoryRoot, string repositoryUrl, string toolProjectDirectory)
        {
            this.Hosting = hosting;
            this.RepositoryRoot = repositoryRoot;
            this.RepositoryUrl = repositoryUrl;
            this.ToolProjectDirectory = toolProjectDirectory;
        }

        public string GetPackageGitRootDirectory()
            => Path.Combine(PathUtil.SrcRoot, Hosting, RepositoryRoot);

        public string GetPackageSrcDirectory()
            => Path.Combine(PathUtil.SrcRoot, Hosting, RepositoryRoot, ToolProjectDirectory);

        public string GetPackageNupkgDirectory()
            => Path.Combine(PathUtil.NupkgRoot, Hosting, RepositoryRoot, ToolProjectDirectory);
    }
}
