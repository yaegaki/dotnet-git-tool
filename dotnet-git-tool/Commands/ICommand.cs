using System.Threading.Tasks;

namespace DotnetGitTool.Commands
{
    public interface ICommand
    {
        Task<ExitCode> RunAsync(CommandLineArgs args, IProcessExecutor processExecutor);
    }
}
