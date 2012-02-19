using System;
using System.Collections.Generic;
using Giles.Core.Runners;
using Giles.Core.UI;
using Growl.Connector;
using Machine.Specifications;
using Machine.Specifications.Utility;

namespace Giles.Specs.Core.UI
{
    public class GrowlUserDisplaySpecs
    {
        [Subject(typeof(GrowlUserDisplay))]
        public class when_subscribing_to_build_events
        {
            Establish context = () =>
            {
                buildRunner = new FakeBuildRunner();
                subject = new GrowlUserDisplay();
            };

            Because of = () =>
                subject.Register(buildRunner);

            It should_subscribe_to_the_build_started_event = () =>
                buildRunner.SubscribedToStartedEvent.ShouldBeTrue();

            It should_subscribe_to_the_build_completed_event = () =>
                buildRunner.SubscribedToCompletedEvent.ShouldBeTrue();

            It should_subscribe_to_the_build_failed_event = () =>
                buildRunner.SubscribedToFailedEvent.ShouldBeTrue();

            static FakeBuildRunner buildRunner;
            static GrowlUserDisplay subject;
        }

        [Subject(typeof(GrowlUserDisplay))]
        public class when_a_build_starts
        {
            Establish context = () =>
            {
                fakeBuildRunner = new FakeBuildRunner();
                subject = new GrowlUserDisplay();
                subject.Register(fakeBuildRunner);
                subject.GrowlAdapter = new FakeGrowlAdapter();
            };

            Because of = () =>
                fakeBuildRunner.RaiseBuildStartedEvent();

            It should_send_a_growl_notification = () =>
                growlAdapter.DidReceiveNotification.ShouldBeTrue();

            It should_contain_a_message_that_the_build_started = () =>
                growlAdapter.Messages.ShouldContain("Build Started");

            static GrowlUserDisplay subject;
            static FakeBuildRunner fakeBuildRunner;
            static FakeGrowlAdapter growlAdapter;
        }
    }

    public class FakeGrowlAdapter : IGrowlAdapter
    {
        public bool DidReceiveNotification;
        public IEnumerable<string> Messages;

        public void Notify(Notification notification)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeBuildRunner : IBuildRunner
    {
        IList<Action<object, BuildStartedEventArgs>> buildStartedHandlers = new List<Action<object, BuildStartedEventArgs>>();

        public bool SubscribedToStartedEvent;
        public bool SubscribedToCompletedEvent;
        public bool SubscribedToFailedEvent;

        public event Action<object, BuildStartedEventArgs> BuildStarted
        {
            add
            {
                buildStartedHandlers.Add(value);
                SubscribedToStartedEvent = true;
            }
            remove { }
        }

        public event Action<object, object> BuildCompleted
        {
            add { SubscribedToCompletedEvent = true; }
            remove { }
        }

        public event Action<object, object> BuildFailed
        {
            add { SubscribedToFailedEvent = true; }    
            remove { }
        }

        public bool Run()
        {
            throw new System.NotImplementedException();
        }

        public void RaiseBuildStartedEvent()
        {
            buildStartedHandlers.Each(handler => handler.Invoke(null, null));
        }
    }
}
