using System.Collections.Generic;
using System.IO;
using System.Linq;
using Giles.Core.Configuration;
using Giles.Core.IO;
using Machine.Specifications;
using NSubstitute;

namespace Giles.Specs.Core.Configuration
{
    [Subject(typeof(GilesConfigBuilder))]
    public class a_giles_config
    {
        protected static GilesConfigBuilder builder;
        protected static GilesConfig config;
        protected static IFileSystem fileSystem;
        protected static string solutionPath;
        protected static string solutionFolder;
        protected static string testRunnerExe;
        protected static string testAssemblyPath;
        protected static string projectRoot;

        Establish context = () =>
            {
                solutionFolder = @"c:\solutionRoot\solution";
                solutionPath = @"c:\solutionRoot\solution\mySolution.sln";
                testAssemblyPath = @"c:\solutionRoot\solution\tests\bin\debug\tests.dll";
                fileSystem = Substitute.For<IFileSystem>();
                testRunnerExe = @"c:\testAssembly.exe";
                fileSystem.GetFiles(Arg.Any<string>(), Arg.Any<string>(), SearchOption.AllDirectories)
                    .Returns(new[] { testRunnerExe });
                fileSystem.GetDirectoryName(solutionPath).Returns(solutionFolder);
                projectRoot = @"c:\solutionRoot";

                builder = new GilesConfigBuilder(solutionPath, new List<string> { testAssemblyPath });
            };
    }

    [Subject(typeof(GilesConfigBuilder))]
    public class when_building : a_giles_config
    {
        Because of = () =>
            config = builder.Build();

        It built_the_correct_config_test_runners = () =>
            config.TestRunners.All(x => x.Value.Path == testRunnerExe).ShouldBeTrue();

        It assigned_the_test_assembly_path = () =>
            config.TestAssemblies.ShouldContain(testAssemblyPath);

        It assigned_the_solution_path = () =>
            config.SolutionPath.ShouldEqual(solutionPath);

        It should_configure_the_console_user_display = () =>
            config.UserDisplays.Count().ShouldBeGreaterThan(0);
    }

}