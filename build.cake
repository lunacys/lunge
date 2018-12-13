#tool nuget:?package=vswhere
#tool nuget:?package=NUnit.Runners&version=2.6.4
#tool nuget:?package=GitVersion.CommandLine&prerelease

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

    foreach (var project in GetFiles($"./Source/lunge.Library*/*.csproj"))
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

Task("Default")
    .IsDependentOn("Pack");

RunTarget(target);
