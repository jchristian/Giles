using System;

namespace Giles.Core.Runners
{
    public class TestActionEventArgs : EventArgs
    {
        public ExecutionResult Result { get; set; }

        public TestActionEventArgs(ExecutionResult result)
        {
            Result = result;
        }
    }
}