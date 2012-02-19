using System;
using System.Diagnostics;
using Giles.Core.Configuration;
using Giles.Core.UI;
using Giles.Core.Utility;

namespace Giles.Core.Runners
{
    public interface IBuildRunner : IRunner
    {
        event Action<object, BuildStartedEventArgs> BuildStarted;
        event Action<object, object> BuildCompleted;
        event Action<object, object> BuildFailed;
    }

    public class BuildRunner : IBuildRunner
    {
        readonly GilesConfig config;
        readonly Settings settings;

        public event Action<object, BuildStartedEventArgs> BuildStarted = delegate { };
        public event Action<object, object> BuildCompleted = delegate { };
        public event Action<object, object> BuildFailed = delegate {};

        public BuildRunner(GilesConfig config, Settings settings)
        {
            this.config = config;
            this.settings = settings;
            this.config.UserDisplay.Each(display => display.Register(this));
        }

        public bool Run()
        {
            var watch = new Stopwatch();
            config.UserDisplay.Each(display => display.DisplayMessage("Building..."));

            BuildStarted(this, null);

            watch.Start();
            var result = CommandProcessExecutor.Execute(settings.MsBuild, "\"" + config.SolutionPath + "\"");
            watch.Stop();
            
            var message = FormatBuildMessages(watch, result);

            config.UserDisplay.Each(display => display.DisplayMessage(message, watch.Elapsed.TotalSeconds));
            BuildCompleted(this, null);

            if (result.ExitCode != 0)
            {
                BuildFailed(this, null);
            }
            return result.ExitCode == 0;
        }

        private static string FormatBuildMessages(Stopwatch watch, ExecutionResult result)
        {
            var message = string.Format("Build complete in {0} seconds. Result: {1}", 
                                        watch.Elapsed.TotalSeconds,
                                        result.ExitCode == 0 ? "Success" : "Failure");

            if (result.ExitCode != 0)
                message += string.Format("\n{0}", result.Output);
            return message;
        }
    }
}