using Giles.Core.Runners;

namespace Giles.Core.UI
{
    public interface IUserDisplay
    {
        void DisplayResult(ExecutionResult result);
        void Register(IBuildRunner buildRunner);
    }
}