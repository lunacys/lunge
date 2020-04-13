#tool nuget:?package=NUnit.Runners&version=2.6.4
#tool nuget:?package=GitVersion.CommandLine&version=5.0.1

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var solution = "./Source/lunge.sln";

var gitVersion = GitVersion();

TaskSetup(context => Information($"'{context.Task.Name}'"));
TaskTeardown(context => Information($"'{context.Task.Name}'"));

Task("Restore")
    .Does(() =>
{
    DotNetCoreRestore(solution);
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var buildSettings = new DotNetCoreBuildSettings 
    { 
        Configuration = configuration,
        ArgumentCustomization = args => args.Append($"/p:Version={gitVersion.AssemblySemVer}")
    };
    DotNetCoreBuild(solution, buildSettings);
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    var testRuns = 0;
    var failedRuns = 0;
    var testProjects = GetFiles($"./Source/Tests/**/*.Tests.csproj");
    var failedProjectNames = new List<string>();

    foreach (var project in testProjects)
    { 
        try
        {
            testRuns++;
            Information("Test Run {0} of {1} - {2}", testRuns, testProjects.Count, project.GetFilenameWithoutExtension());
            DotNetCoreTest(project.FullPath);            
        }
        catch
        {
            failedRuns++;
            failedProjectNames.Add(project.FullPath);
        }
    }

    if(failedRuns > 0)
    {
        string errorMsg = $"{failedRuns} of {testRuns} test runs failed. Failed tests:\n";

        foreach (var project in failedProjectNames)
        {
            errorMsg += $" > {project}\n";
        }

        throw new Exception(errorMsg);
    }
});

Task("Pack")
    .IsDependentOn("Test")
    .Does(() =>
{ 
    var artifactsDirectory = "./artifacts";

    CreateDirectory(artifactsDirectory);
    CleanDirectory(artifactsDirectory);    

    foreach (var project in GetFiles($"./Source/lunge.*/*.csproj"))
    {
        DotNetCorePack(project.FullPath, new DotNetCorePackSettings 
        {
            Configuration = configuration,
            IncludeSymbols = true,
            OutputDirectory = artifactsDirectory,
            ArgumentCustomization = args => args.Append($"/p:Version={gitVersion.NuGetVersion} /p:SymbolPackageFormat=snupkg")
        });
    }
});

Task("Publish")
    .IsDependentOn("Pack")
    .Does(() =>
{
    var availableConfigs = new List<string>() {
        "win-x86",
        "win-x64",
        "osx-x64",
        "linux-x64"
    };

    var publishDirectory = "./artifacts/publish";

    CreateDirectory(publishDirectory);
    CleanDirectory(publishDirectory);

    foreach (var cfg in availableConfigs)
    {
        var outputDir = $"./artifacts/publish/lunge.Library/{cfg}";

        CreateDirectory(outputDir);
        CleanDirectory(outputDir);

        var settings = new DotNetCorePublishSettings
        {
            Framework = "netstandard2.1",
            Configuration = "Release",
            Runtime = cfg,
            SelfContained = false,
            OutputDirectory = outputDir
        };

        DotNetCorePublish("./Source/lunge.Library/lunge.Library.csproj", settings);
    }
});

Task("Default")
    .IsDependentOn("Publish");

RunTarget(target);
