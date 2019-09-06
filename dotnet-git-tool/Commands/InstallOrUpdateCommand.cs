using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetGitTool.Commands
{
    public class InstallOrUpdateCommand : ICommand
    {
        public async Task<ExitCode> RunAsync(CommandLineArgs args, IProcessExecutor processExecutor)
        {
            if (string.IsNullOrEmpty(args.Package))
            {
                Console.Error.WriteLine("missing package name.");
                return ExitCode.InvalidArguments;
            }

            Package package;
            try
            {
                package = await Package.ParseAsync(args.Package);
            }
            catch (PackageParseException e)
            {
                Console.Error.WriteLine(e.Message);
                return ExitCode.InvalidArguments;
            }

            // prepare directories
            Directory.CreateDirectory(PathUtil.SrcRoot);
            Directory.CreateDirectory(PathUtil.NupkgRoot);

            var packageNupkgDirectory = package.GetPackageNupkgDirectory();
            Directory.CreateDirectory(packageNupkgDirectory);
            // delete .nupkg if already exists
            DirectoryUtil.Delete(packageNupkgDirectory, "*.nupkg");

            var packageGitRootDirectory = package.GetPackageGitRootDirectory();
            if (Directory.Exists(packageGitRootDirectory))
            {
                var pullResult = await processExecutor.StartAsync("git", "pull", packageGitRootDirectory);
                if (pullResult.ExitCode != 0)
                {
                    Console.Error.WriteLine("git pull failed.");
                    Console.Error.WriteLine(pullResult.StandardError);
                    return ExitCode.GitPullFailed;
                }
            }
            else
            {
                var cloneArgs = $"clone --recursive \"{package.RepositoryUrl}\" \"{package.Hosting}/{package.RepositoryRoot}\"";
                var cloneResult = await processExecutor.StartAsync("git", cloneArgs, PathUtil.SrcRoot);
                if (cloneResult.ExitCode != 0)
                {
                    Console.Error.WriteLine("git clone failed.");
                    Console.Error.WriteLine(cloneResult.StandardError);
                    return ExitCode.GitCloneFailed;
                }
            }

            var packageSrcDirectory = package.GetPackageSrcDirectory();

            var globalJSONPath = Path.Join(packageSrcDirectory, "global.json");
            byte[] globalJSONBackup = null;
            if (args.AnySdk)
            {
                if (File.Exists(globalJSONPath))
                {
                    globalJSONBackup = File.ReadAllBytes(globalJSONPath);
                }

                // TODO: replace only 'sdk' property.
                File.WriteAllText(globalJSONPath, "{}");
            }

            try
            {
                var packArgs = BuildPackArgs(args, packageNupkgDirectory);
                var packResult = await processExecutor.StartAsync("dotnet", packArgs, packageSrcDirectory);
                if (packResult.ExitCode != 0)
                {
                    Console.Error.WriteLine("dotnet pack failed.");
                    Console.Error.WriteLine(packResult.StandardError);
                    return ExitCode.DotnetPackFailed;
                }
            }
            finally
            {
                if (args.AnySdk)
                {
                    if (globalJSONBackup == null)
                    {
                        File.Delete(globalJSONPath);
                    }
                    else
                    {
                        File.WriteAllBytes(globalJSONPath, globalJSONBackup);
                    }
                }
            }

            var (packageName, version) = GetPackageNameAndVersionFromDirectory(packageNupkgDirectory);
            if (string.IsNullOrEmpty(packageName))
            {
                Console.Error.WriteLine(".nupkg is not generated.");
                return ExitCode.DotnetPackFailed;
            }

            var installOrUpdateArgs = BuildInstallOrUpdateArgs(args, packageNupkgDirectory, packageName, version);
            var installOrUpdateResult = await processExecutor.StartAsync("dotnet", installOrUpdateArgs, packageNupkgDirectory);
            if (installOrUpdateResult.ExitCode != 0)
            {
                Console.Error.WriteLine($"dotnet {args.SubCommand} failed.");
                Console.Error.WriteLine(installOrUpdateResult.StandardError);
                return ExitCode.DotnetInstallFailed;
            }

            Console.WriteLine(installOrUpdateResult.StandardOutput);

            return ExitCode.Success;
        }

        private (string packageName, string version) GetPackageNameAndVersionFromDirectory(string nupkgDirectory)
        {
            var nupkgPath = Directory.EnumerateFiles(nupkgDirectory, "*.nupkg").FirstOrDefault();
            if (nupkgPath == null) return default;

            var fileName = Path.GetFileName(nupkgPath);
            var first = fileName.IndexOf(".");
            var last = fileName.LastIndexOf(".");

            var packageName = fileName.Substring(0, first);
            var version = fileName.Substring(first + 1, last - first - 1);

            return (packageName, version);
        }

        private string BuildPackArgs(CommandLineArgs args, string nupkgDirectory)
        {
            var sb = new StringBuilder();
            sb.Append($"pack --output \"{nupkgDirectory}\"");
            if (args.ForcePack)
            {
                sb.Append(" -p:PackAsTool=True -p:IsPackable=True");
            }

            return sb.ToString();
        }
        
        private string BuildInstallOrUpdateArgs(CommandLineArgs args, string nupkgDirectory, string packageName, string version)
            => $"tool {args.SubCommand} {packageName} -g --add-source \"{nupkgDirectory}\" --version {version}";
    }
}
