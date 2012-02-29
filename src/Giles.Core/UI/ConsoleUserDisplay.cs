using System;
using Giles.Core.Runners;

namespace Giles.Core.UI
{
    public class ConsoleUserDisplay : IUserDisplay
    {
        readonly ConsoleColor defaultConsoleColor;

        public ConsoleUserDisplay()
        {
            defaultConsoleColor = Console.ForegroundColor;
        }

        public void Register(IBuildRunner buildRunner)
        {
            buildRunner.BuildStarted += DisplayBuildResult;
            buildRunner.BuildCompleted += DisplayBuildResult;
            buildRunner.BuildFailed += DisplayBuildResult;
        }

        public void Register(GilesTestListener testListener)
        {
            testListener.TestFailure += DisplayTestResult;
            testListener.TestsCompleted += DisplayTestResult;
        }

        private void DisplayTestResult(object sender, TestActionEventArgs args)
        {
            Console.WriteLine("\n\n======= {0} TEST RUNNER RESULTS =======", args.Result.Runner);
            Console.ForegroundColor = args.Result.ExitCode != 0 ?
                                      ConsoleColor.Red : defaultConsoleColor;

            Console.WriteLine(args.Result.Output);

            Console.ForegroundColor = defaultConsoleColor;
        }

        private void DisplayBuildResult(object sender, BuildActionEventArgs args)
        {
            Console.WriteLine(args.Message.ScrubDisplayStringForFormatting(), args.Parameters);
        }
    }
}