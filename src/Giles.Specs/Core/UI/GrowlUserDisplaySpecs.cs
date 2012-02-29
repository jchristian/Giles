using Giles.Core.Runners;
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
        public class when_subscribing_to_test_events
        {
            static FakeGilesTestListener testListener;
            static GrowlUserDisplay subject;

            Establish context = () =>
            {
                testListener = new FakeGilesTestListener();
                subject = new GrowlUserDisplay();
            };

            Because of = () =>
                subject.Register(testListener);

            It should_subscribe_to_the_test_failure_event = () =>
                testListener.SubscribedToTestFailureEvent.ShouldBeTrue();

            It should_subscribe_to_the_tests_completed_event = () =>
                testListener.SubscribedToTestsCompletedEvent.ShouldBeTrue();
        }

        [Subject(typeof(GrowlUserDisplay))]
        public class with_a_growl_user_display_registered_to_a_build_runner
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
        public class when_a_build_starts : with_a_growl_user_display_registered_to_a_build_runner
        {
            static string buildStartedMessage = "Build started";

            Because of = () =>
                fakeBuildRunner.RaiseBuildStartedEvent(buildStartedMessage);

            It should_contain_a_message_that_the_build_started = () =>
                growlAdapter.Messages.ShouldContain(buildStartedMessage);
        }

        [Subject(typeof(GrowlUserDisplay))]
        public class when_a_build_completes : with_a_growl_user_display_registered_to_a_build_runner
        {
            static string buildCompletedMessage = "Build completed";

            Because of = () =>
                fakeBuildRunner.RaiseBuildCompletedEvent(buildCompletedMessage);

            It should_contain_a_message_that_the_build_started = () =>
                growlAdapter.Messages.ShouldContain(buildCompletedMessage);
        }

        [Subject(typeof(GrowlUserDisplay))]
        public class when_a_build_fails : with_a_growl_user_display_registered_to_a_build_runner
        {
            static string buildFailedMessage = "Build failed";

            Because of = () =>
                fakeBuildRunner.RaiseBuildFailedEvent(buildFailedMessage);

            It should_contain_a_message_that_the_build_failed = () =>
                growlAdapter.Messages.ShouldContain(buildFailedMessage);
        }

        [Subject(typeof(GrowlUserDisplay))]
        public class with_a_growl_user_display_registered_to_a_test_listener
        {
            protected static GrowlUserDisplay subject;
            protected static FakeGilesTestListener fakeTestListener;
            protected static FakeGrowlAdapter growlAdapter;

            Establish context = () =>
            {
                fakeTestListener = new FakeGilesTestListener();
                growlAdapter = new FakeGrowlAdapter();
                subject = new GrowlUserDisplay();
                subject.Register(fakeTestListener);
                subject.GrowlAdapter = growlAdapter;
            };
        }

        [Subject(typeof(GrowlUserDisplay))]
        public class when_there_is_a_test_failure : with_a_growl_user_display_registered_to_a_test_listener
        {
            static string testFailureMessage = "The tests failed";

            Because of = () =>
                fakeTestListener.RaiseTestFailureEvent(new ExecutionResult { ExitCode = 1, Output = testFailureMessage });

            It should_contain_a_message_that_there_was_a_failure_running_the_tests = () =>
                growlAdapter.Messages.ShouldContain(testFailureMessage);
        }

        [Subject(typeof(GrowlUserDisplay))]
        public class when_the_tests_complete : with_a_growl_user_display_registered_to_a_test_listener
        {
            static string testsCompletedMessage = "The tests are done!";

            Because of = () =>
                fakeTestListener.RaiseTestsCompletedEvent(new ExecutionResult { ExitCode = 0, Output = testsCompletedMessage });

            It should_contain_a_message_that_the_tests_completed = () =>
                growlAdapter.Messages.ShouldContain(testsCompletedMessage);
        }
    }
}
