using System.Threading.Tasks;

namespace DotnetGitTool
{
    public interface IProcessExecutor
    {
        Task<ProcessResult> StartAsync (string fileName, string arguments, string workDir);
    }
}
