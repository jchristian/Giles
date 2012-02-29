using Giles.Core.UI;
using Machine.Specifications;

namespace Giles.Specs.Core.UI
{
    public class GrowlUserDisplaySpecs
    {
        [Subject(typeof(GrowlUserDisplay))]
        public class when_subscribing_to_build_events
        {
            static FakeBuildRunner buildRunner;
            static GrowlUserDisplay subject;

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
        }

        [Subject(typeof(GrowlUserDisplay))]
        public class with_a_registered_growl_user_display
        {
            protected static GrowlUserDisplay subject;
            protected static FakeBuildRunner fakeBuildRunner;
            protected static FakeGrowlAdapter growlAdapter;

            Establish context = () =>
            {
                fakeBuildRunner = new FakeBuildRunner();
                growlAdapter = new FakeGrowlAdapter();
                subject = new GrowlUserDisplay();
                subject.Register(fakeBuildRunner);
                subject.GrowlAdapter = growlAdapter;
            };
        }

        [Subject(typeof(GrowlUserDisplay))]
        public class when_a_build_starts : with_a_registered_growl_user_display
        {
            static string buildStartedMessage = "Build started";

            Because of = () =>
                fakeBuildRunner.RaiseBuildStartedEvent(buildStartedMessage);

            It should_contain_a_message_that_the_build_started = () =>
                growlAdapter.Messages.ShouldContain(buildStartedMessage);
        }

        [Subject(typeof(GrowlUserDisplay))]
        public class when_a_build_completes : with_a_registered_growl_user_display
        {
            static string buildCompletedMessage = "Build completed";

            Because of = () =>
                fakeBuildRunner.RaiseBuildCompletedEvent(buildCompletedMessage);

            It should_contain_a_message_that_the_build_started = () =>
                growlAdapter.Messages.ShouldContain(buildCompletedMessage);
        }

        [Subject(typeof(GrowlUserDisplay))]
        public class when_a_build_fails : with_a_registered_growl_user_display
        {
            static string buildFailedMessage = "Build failed";

            Because of = () =>
                fakeBuildRunner.RaiseBuildFailedEvent(buildFailedMessage);

            It should_contain_a_message_that_the_build_failed = () =>
                growlAdapter.Messages.ShouldContain(buildFailedMessage);
        }
    }
}
