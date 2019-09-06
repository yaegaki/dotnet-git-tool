namespace DotnetGitTool
{
    public enum ExitCode
    {
        Success = 0,

        InvalidArguments = -1,
        GitCloneFailed = -2,
        GitPullFailed = -3,
        DotnetPackFailed = -4,
        DotnetInstallFailed = -5,
    }
}
