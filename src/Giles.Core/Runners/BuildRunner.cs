using System;
using System.Diagnostics;
using Giles.Core.Configuration;
using Giles.Core.UI;
using Giles.Core.Utility;

namespace Giles.Core.Runners
{
    public interface IBuildRunner : IRunner
    {
        event EventHandler<BuildActionEventArgs> BuildStarted;
        event EventHandler<BuildActionEventArgs> BuildCompleted;
        event EventHandler<BuildActionEventArgs> BuildFailed;
    }

    public class BuildRunner : IBuildRunner
    {
        readonly GilesConfig config;
        readonly Settings settings;

        public event EventHandler<BuildActionEventArgs> BuildStarted = delegate { };
        public event EventHandler<BuildActionEventArgs> BuildCompleted = delegate { };
        public event EventHandler<BuildActionEventArgs> BuildFailed = delegate { };

        public BuildRunner(GilesConfig config, Settings settings)
        {
            this.config = config;
            this.settings = settings;
            this.config.UserDisplays.Each(display => display.Register(this));
        }

        public bool Run()
        {
            var watch = new Stopwatch();

            BuildStarted(this, new BuildActionEventArgs("Building..."));

            watch.Start();
            var result = CommandProcessExecutor.Execute(settings.MsBuild, "\"" + config.SolutionPath + "\"");
            watch.Stop();

            var args = new BuildActionEventArgs(FormatBuildMessages(watch, result), watch.Elapsed.TotalSeconds);
            if (result.ExitCode == 0)
                BuildCompleted(this, args);
            else
                BuildFailed(this, args);
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