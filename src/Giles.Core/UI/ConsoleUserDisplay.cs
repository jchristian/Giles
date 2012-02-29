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
        public void DisplayMessage(string message, params object[] parameters)
        {
            Console.WriteLine(message.ScrubDisplayStringForFormatting(), parameters);
        }

        public void DisplayResult(ExecutionResult result)
        {
            Console.WriteLine("\n\n======= {0} TEST RUNNER RESULTS =======", result.Runner);
            Console.ForegroundColor = result.ExitCode != 0 ?
                                      ConsoleColor.Red : defaultConsoleColor;

            Console.WriteLine(result.Output);

            Console.ForegroundColor = defaultConsoleColor;
        }

        public void Register(IBuildRunner buildRunner)
        {
            buildRunner.BuildStarted += DisplayBuildResult;
            buildRunner.BuildCompleted += DisplayBuildResult;
            buildRunner.BuildFailed += DisplayBuildResult;
        }

        void DisplayBuildResult(object sender, BuildActionEventArgs e)
        {
            Console.WriteLine(e.Message.ScrubDisplayStringForFormatting(), e.Parameters);
        }
    }
}