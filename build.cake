#tool nuget:?package=vswhere
#tool nuget:?package=NUnit.Runners&version=2.6.4
#tool nuget:?package=GitVersion.CommandLine&prerelease
#tool nuget:?package=Microsoft.Packaging.Tools.Trimming&prerelease&version=1.1.0-preview1-26619-01

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var solution = "./Source/lunge.sln";

var vsLatest  = VSWhereLatest();
var msBuildPath = vsLatest?.CombineWithFilePath("./MSBuild/15.0/Bin/amd64/MSBuild.exe");
var gitVersion = GitVersion();

//TaskSetup(context => Information($"'{context.Task.Name}'"));
//TaskTeardown(context => Information($"'{context.Task.Name}'"));

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
        }
    }

    if(failedRuns > 0)
        throw new Exception($"{failedRuns} of {testRuns} test runs failed.");
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
            ArgumentCustomization = args => args.Append($"/p:Version={gitVersion.NuGetVersion}")
        });
    }
});

Task("Publish")
    .IsDependentOn("Pack")
    .Does(() =>
{
    var artifactsDirectory = "./artifacts";
    var publishDirectory = "./artifacts/publish";

    CreateDirectory(publishDirectory);
    CleanDirectory(publishDirectory);

    var projectsToPublish = new KeyValuePair<string, string>[] 
    { 
        //new KeyValuePair<string, string>("./Source/Demos/Nightly/Nightly.csproj", "DemoNightly"),
        //new KeyValuePair<string, string>("./Source/lunge.Library/lunge.Library.csproj", "lunge.Library"),
        new KeyValuePair<string, string>("./Source/lunge.MapEditor/lunge.MapEditor.csproj", "lunge.MapEditor")
    };

    foreach (var projKv in projectsToPublish)
    {
        var outputDir = publishDirectory + "/" + projKv.Value;

        CreateDirectory(outputDir);
        CleanDirectory(outputDir);

        var settings = new DotNetCorePublishSettings
        {
            Framework = "netcoreapp3.1",
            Configuration = "Release",
            Runtime = "win-x86",
            SelfContained = true,
            OutputDirectory = "./artifacts/publish/" + projKv.Value
        };

        DotNetCorePublish(projKv.Key, settings);
    }

    CreateDirectory("./artifacts/publish/lunge.Library");
    CleanDirectory("./artifacts/publish/lunge.Library");

    var lungeSettings = new DotNetCorePublishSettings
    {
        Framework = "netstandard2.1",
        Configuration = "Release",
        Runtime = "win-x86",
        SelfContained = false,
        OutputDirectory = "./artifacts/publish/lunge.Library"
    };

    DotNetCorePublish("./Source/lunge.Library/lunge.Library.csproj", lungeSettings);
    
});

Task("Default")
    .IsDependentOn("Publish");

RunTarget(target);
