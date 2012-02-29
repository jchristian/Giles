using System;
using System.Collections.Generic;
using Giles.Core.Runners;
using Giles.Core.Utility;

namespace Giles.Specs.Core.UI
{
    public class FakeGilesTestListener : GilesTestListener
    {
        IList<EventHandler<TestActionEventArgs>> testFailureHandlers = new List<EventHandler<TestActionEventArgs>>();
        IList<EventHandler<TestActionEventArgs>> testsCompletedHandlers = new List<EventHandler<TestActionEventArgs>>();

        public bool SubscribedToTestFailureEvent;
        public bool SubscribedToTestsCompletedEvent;
        
        public override event EventHandler<TestActionEventArgs> TestFailure
        {
            add
            {
                SubscribedToTestFailureEvent = true;
                testFailureHandlers.Add(value);
            }
            remove
            { }
        }

        public override event EventHandler<TestActionEventArgs> TestsCompleted
        {
            add
            {
                SubscribedToTestsCompletedEvent = true;
                testsCompletedHandlers.Add(value);
            }
            remove
            { }
        }

        public void RaiseTestFailureEvent(ExecutionResult result)
        {
            testFailureHandlers.Each(handler => handler.Invoke(null, new TestActionEventArgs(result)));
        }

        public void RaiseTestsCompletedEvent(ExecutionResult result)
        {
            testsCompletedHandlers.Each(handler => handler.Invoke(null, new TestActionEventArgs(result)));
        }
    }
}