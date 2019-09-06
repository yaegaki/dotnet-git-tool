using System;
using System.Reflection;
using System.Threading.Tasks;
using DotnetGitTool.Commands;

namespace DotnetGitTool 
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowUsage();
                return;
            }

            var cmdArgs = CommandLineArgs.Parse(args);

            if (!cmdArgs.IsValid)
            {
                var message = string.IsNullOrEmpty(cmdArgs.ErrorHint) ? cmdArgs.ErrorHint : "invalid arguments.";
                Console.Error.WriteLine(cmdArgs);
                Exit(ExitCode.InvalidArguments);
                return;
            }

            ICommand command;
            switch (cmdArgs.SubCommand)
            {
                case "install":
                case "update":
                    command = new InstallOrUpdateCommand();
                    break;
                default:
                    command = null;
                    break;
            }

            if (command == null)
            {
                Console.Error.WriteLine($"unknown sub-command '{cmdArgs.SubCommand}'.");
                Exit(ExitCode.InvalidArguments);
                return;
            }

            Exit(command.RunAsync(cmdArgs, new ProcessExecutor()).Result);
        }

        private static void ShowUsage()
        {
            var versionString = Assembly.GetEntryAssembly()
                                     .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                     .InformationalVersion
                                     .ToString();

            Console.WriteLine($"dotnet-git-tool v{versionString}");
            Console.WriteLine("-------------");
            Console.WriteLine("\nUsage:");
            Console.WriteLine("  dotnet git-tool install <repository>");
            Console.WriteLine("  dotnet git-tool install <repository>/<tool-path>");
        }

        private static void Exit(ExitCode exitCode)
            => Environment.Exit((int)exitCode);

        class ProcessExecutor : IProcessExecutor
        {
            public Task<ProcessResult> StartAsync(string fileName, string arguments, string workDir)
                => ProcessUtil.StartAsync(fileName, arguments, workDir);
        }
    }
}
