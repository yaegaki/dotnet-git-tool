using System;

namespace DotnetGitTool
{
    public class CommandLineArgs
    {
        public bool IsValid { get; set; } = true;
        public string ErrorHint { get; set; }

        public string SubCommand { get; set; }
        public string Package { get; set; }
        public bool ForcePack { get; set; }
        public bool AnySdk { get; set; }
        public bool IsOverrideToolName { get; set; }
        public string OverrideToolName { get; set; }

        public static CommandLineArgs Parse (string[] args)
        {

            var result = new CommandLineArgs();

            if (args.Length == 0)
            {
                throw new ArgumentException(nameof(args));
            }

            result.SubCommand = args[0];

            var i = 1;
            while (i < args.Length && result.IsValid)
            {
                switch (args[i])
                {
                    case "-f":
                        result.ForcePack = true;
                        break;
                    case "--any-sdk":
                        result.AnySdk = true;
                        break;
                    default:
                        if (string.IsNullOrEmpty(result.Package))
                        {
                            result.Package = args[i];
                        }
                        else
                        {
                            result.IsValid = false;
                            result.ErrorHint = $"unknown option '{args[i]}'.";
                        }
                        break;
                }

                i++;
            }

            return result;
        }
    }
}
