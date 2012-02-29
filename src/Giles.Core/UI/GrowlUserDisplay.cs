using System;
using System.Drawing;
using System.Reflection;
using Giles.Core.Runners;
using Growl.CoreLibrary;

namespace Giles.Core.UI
{
    public class GrowlUserDisplay : IUserDisplay
    {
        public IGrowlAdapter GrowlAdapter { get; set; }

        public GrowlUserDisplay()
        {
            GrowlAdapter = new GrowlAdapter();
        }

        private const string successImage = "Giles.Core.Resources.checkmark.png";
        private const string failureImage = "Giles.Core.Resources.stop.png";

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

        private void DisplayBuildResult(object sender, BuildActionEventArgs args)
        {
            const string title = "Giles says...";
            var text = string.Format(args.Message.ScrubDisplayStringForFormatting(), args.Parameters);

            GrowlAdapter.Notify(DateTime.Now.Ticks.ToString(), title, text);
        }

        private void DisplayTestResult(object sender, TestActionEventArgs args)
        {
            var title = args.Result.ExitCode == 0 ? "Success!" : "Failures!";
            Resource icon = args.Result.ExitCode == 0 ? LoadImage(successImage) : LoadImage(failureImage);

            GrowlAdapter.Notify(DateTime.Now.Ticks.ToString(), title, args.Result.Output, icon);
        }

        private static Image LoadImage(string resourceName)
        {
            var file = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            return Image.FromStream(file);
        }
    }
}