using System.Collections.Generic;
using Giles.Core.Runners;
using Giles.Core.UI;

namespace Giles.Specs.Core.Runners
{
    public class FakeUserDisplay : IUserDisplay
    {
        public bool WasAskedToRegister;
        public bool WasAskedToRegisterTheTestListener;
        public IBuildRunner TheBuildRunnerThatWasRegistered;
        public GilesTestListener TheTestListenerThatWasRegistered;
        public IList<string> DisplayMessagesReceived = new List<string>();
        public IList<ExecutionResult> DisplayResultsReceived = new List<ExecutionResult>();

        public void Register(IBuildRunner buildRunner)
        {
            WasAskedToRegister = true;
            TheBuildRunnerThatWasRegistered = buildRunner;
        }

        public void Register(GilesTestListener testListener)
        {
            WasAskedToRegisterTheTestListener = true;
            TheTestListenerThatWasRegistered = testListener;
            testListener.TestFailure += (s, e) => DisplayResult(e.Result);
            testListener.TestsCompleted += (s, e) => DisplayResult(e.Result);
        }

        public void DisplayResult(ExecutionResult result)
        {
            DisplayResultsReceived.Add(result);
        }
    }
}