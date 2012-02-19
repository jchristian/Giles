using System.Linq;
using Giles.Core.Configuration;
using Giles.Core.Runners;
using Machine.Specifications;

namespace Giles.Specs.Core.Runners
{
    [Subject(typeof(BuildRunner))]
    public class with_a_build_runner
    {
        protected static GilesConfig config;
        protected static Settings settings;
        protected static BuildRunner subject;
        protected static FakeUserDisplay fakeUserDisplay;

        Establish context = () =>
            {
                fakeUserDisplay = new FakeUserDisplay();
                config = new GilesConfig();
                config.UserDisplay.Add(fakeUserDisplay);
                settings = new Settings();
                subject = new BuildRunner(config, settings);
            };
    }

    [Subject(typeof(BuildRunner))]
    public class when_asked_to_run_a_build_and_the_build_was_successful : with_a_build_runner
    {
        static ExecutionResult ExecuteHandler(string filename, string args)
        {
            ExecuteWasCalled = true; return new ExecutionResult { ExitCode = successExitCode };
        }

        static int successExitCode;
        static bool ExecuteWasCalled;
        static bool result;
        static bool wasNotifiedThatBuildStarted;
        static bool wasNotifiedThatBuildSuceeded;

        Establish context = () =>
            {
                successExitCode = 0;
                CommandProcessExecutor.Execute = (filename, args) => ExecuteHandler(filename, args);
                subject.BuildStarted += (sender, args) =>
                {
                    wasNotifiedThatBuildStarted = true;
                };

                subject.BuildCompleted += (sender, args) =>
                {
                    wasNotifiedThatBuildSuceeded = true;
                };
            };

        Because of = () =>
            result = subject.Run();

        It should_have_called_execute_on_the_command_executor = () =>
            ExecuteWasCalled.ShouldBeTrue();

        It should_return_success = () =>
            result.ShouldBeTrue();

        It should_raise_a_build_started_event = () => wasNotifiedThatBuildStarted.ShouldBeTrue();

        It should_raise_a_build_completed_event = () => wasNotifiedThatBuildSuceeded.ShouldBeTrue();

        It should_display_the_build_complete_message_to_the_user_display = () =>
            fakeUserDisplay.DisplayMessagesReceived.Any(x => x.Contains("Build complete")).ShouldBeTrue();

        It should_display_a_message_of_success_to_the_user_display = () =>
            fakeUserDisplay.DisplayMessagesReceived.Any(x => x.Contains("Success")).ShouldBeTrue();
    }

    [Subject(typeof(BuildRunner))]
    public class when_asked_to_run_a_build_and_the_build_was_failed : with_a_build_runner
    {
        static ExecutionResult ExecuteHandler(string filename, string args)
        { ExecuteWasCalled = true; return new ExecutionResult { ExitCode = failureExitCode }; }

        static int failureExitCode;
        static bool ExecuteWasCalled;
        static bool result;
        static bool wasNotifiedThatBuildFailed;

        Establish context = () =>
            {
                failureExitCode = 100;
                CommandProcessExecutor.Execute = (filename, args) => ExecuteHandler(filename, args);
                subject.BuildFailed += (sender, args) =>
                {
                    wasNotifiedThatBuildFailed = true;
                };
            };

        Because of = () =>
            result = subject.Run();

        It should_have_called_execute_on_the_command_executor = () =>
            ExecuteWasCalled.ShouldBeTrue();

        It should_return_failure = () =>
            result.ShouldBeFalse();

        It should_raise_a_build_failed_event = () =>
            wasNotifiedThatBuildFailed.ShouldBeTrue();

        It should_display_the_build_complete_message_to_the_user_display = () =>
            fakeUserDisplay.DisplayMessagesReceived.Any(x => x.Contains("Build complete")).ShouldBeTrue();

        It should_display_a_message_of_failure_to_the_user_display = () =>
            fakeUserDisplay.DisplayMessagesReceived.Any(x => x.Contains("Failure")).ShouldBeTrue();
    }

    [Subject(typeof(BuildRunner))]
    public class when_creating_the_build_runner : with_a_build_runner
    {
        It should_register_the_user_display_with_the_builder_runner = () =>
        {
            fakeUserDisplay.WasAskedToRegister.ShouldBeTrue();
            fakeUserDisplay.TheBuildRunnerThatWasRegistered.ShouldEqual(subject);
        };
    }
}