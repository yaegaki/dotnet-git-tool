namespace DotnetGitTool
{
    public readonly struct ProcessResult
    {
        public readonly int ExitCode;
        public readonly string StandardOutput;
        public readonly string StandardError;

        public ProcessResult(int exitCode, string stdOut, string stdErr)
        {
            this.ExitCode = exitCode;
            this.StandardOutput = stdOut;
            this.StandardError = stdErr;
        }
    }
}
