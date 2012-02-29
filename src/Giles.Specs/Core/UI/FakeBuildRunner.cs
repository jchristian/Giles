using System;
using System.Collections.Generic;
using Giles.Core.Runners;
using Giles.Core.UI;
using Giles.Core.Utility;

namespace Giles.Specs.Core.UI
{
    public class FakeBuildRunner : IBuildRunner
    {
        IList<EventHandler<BuildActionEventArgs>> buildStartedHandlers = new List<EventHandler<BuildActionEventArgs>>();
        IList<EventHandler<BuildActionEventArgs>> buildCompletedHandlers = new List<EventHandler<BuildActionEventArgs>>();
        IList<EventHandler<BuildActionEventArgs>> buildFailedHandlers = new List<EventHandler<BuildActionEventArgs>>();

        public bool SubscribedToStartedEvent;
        public bool SubscribedToCompletedEvent;
        public bool SubscribedToFailedEvent;

        public event EventHandler<BuildActionEventArgs> BuildStarted
        {
            add
            {
                buildStartedHandlers.Add(value);
                SubscribedToStartedEvent = true;
            }
            remove { }
        }

        public event EventHandler<BuildActionEventArgs> BuildCompleted
        {
            add
            {
                buildCompletedHandlers.Add(value);
                SubscribedToCompletedEvent = true;
            }
            remove { }
        }

        public event EventHandler<BuildActionEventArgs> BuildFailed
        {
            add
            {
                buildFailedHandlers.Add(value);
                SubscribedToFailedEvent = true;
            }
            remove { }
        }

        public bool Run()
        {
            throw new NotImplementedException();
        }

        public void RaiseBuildStartedEvent(string message)
        {
            buildStartedHandlers.Each(handler => handler.Invoke(null, new BuildActionEventArgs(message)));
        }

        public void RaiseBuildCompletedEvent(string message)
        {
            buildCompletedHandlers.Each(handler => handler.Invoke(null, new BuildActionEventArgs(message)));
        }

        public void RaiseBuildFailedEvent(string message)
        {
            buildFailedHandlers.Each(handler => handler.Invoke(null, new BuildActionEventArgs(message)));
        }
    }
}