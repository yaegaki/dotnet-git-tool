using System.Diagnostics;
using System.Threading.Tasks;

namespace DotnetGitTool
{
    public static class ProcessUtil
    {
        public static async Task<ProcessResult> StartAsync (string fileName, string arguments, string workDir)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = fileName,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = arguments,
                WorkingDirectory = workDir,
            };

            using (var proc = new Process())
            {
                proc.StartInfo = startInfo;
                proc.Start();

                proc.StandardInput.Close();

                var stdOutTask = proc.StandardOutput.ReadToEndAsync();
                var stdErrTask = proc.StandardError.ReadToEndAsync();

                await Task.WhenAll(stdOutTask, stdErrTask);

                proc.WaitForExit();

                return new ProcessResult(proc.ExitCode, stdOutTask.Result, stdErrTask.Result);
            }
        }
    }
}
