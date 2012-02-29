using Giles.Core.Runners;

namespace Giles.Core.UI
{
    public interface IUserDisplay
    {
        void Register(IBuildRunner buildRunner);
        void Register(GilesTestListener testListener);
    }
}