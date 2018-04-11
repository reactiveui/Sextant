//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TOOLS
//////////////////////////////////////////////////////////////////////

#tool "GitReleaseManager"
#tool "GitVersion.CommandLine"
#tool "GitLink"

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var artifactsDir  = Directory("./artifacts/");
var rootAbsoluteDir = MakeAbsolute(Directory("./")).FullPath;

var local = BuildSystem.IsLocalBuild;

var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;
//var isRepository = StringComparer.OrdinalIgnoreCase.Equals("reactiveui/reactiveui", AppVeyor.Environment.Repository.Name);

var isDevelopBranch = StringComparer.OrdinalIgnoreCase.Equals("develop", AppVeyor.Environment.Repository.Branch);
var isReleaseBranch = StringComparer.OrdinalIgnoreCase.Equals("master", AppVeyor.Environment.Repository.Branch);
var isTagged = AppVeyor.Environment.Repository.Tag.IsTag;

var githubOwner = "giusepe";
var githubRepository = "sextant";
var githubUrl = string.Format("https://github.com/{0}/{1}", githubOwner, githubRepository);

// Version
var gitVersion = GitVersion();
var majorMinorPatch = gitVersion.MajorMinorPatch;
var semVersion = gitVersion.SemVer;
var informationalVersion = gitVersion.InformationalVersion;
var nugetVersion = gitVersion.NuGetVersion;
var buildVersion = gitVersion.FullBuildMetaData;

var packageWhitelist = new[] { "Sextant.PCL" };

//////////////////////////////////////////////////////////////////////
// Build
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
    {
        CleanDirectory(artifactsDir);
    });

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        NuGetRestore("Sextant.sln");
    });

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
    {
        MSBuild("Sextant.sln", settings =>
            settings.SetConfiguration("Release"));

    });

//////////////////////////////////////////////////////////////////////
// Packages
//////////////////////////////////////////////////////////////////////

Task("BuildPackages")
	.IsDependentOn("Build")
    .Does(() =>
    {
        var nuGetPackSettings = new NuGetPackSettings
        {
            OutputDirectory = rootAbsoluteDir + @"\artifacts\",
            IncludeReferencedProjects = true,
            Properties = new Dictionary<string, string>
            {
                { "Configuration", "Release" }
            }
        };

        NuGetPack("./Sextant.PCL/Sextant.PCL.nuspec", nuGetPackSettings);
    });

Task("PublishRelease")
    .IsDependentOn("BuildPackages")
    .WithCriteria(() => !local)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isRepository)
    .WithCriteria(() => isReleaseBranch)
    .WithCriteria(() => isTagged)
    .Does (() =>
    {
        var username = EnvironmentVariable("GITHUB_USERNAME");
        if (string.IsNullOrEmpty(username))
        {
            throw new Exception("The GITHUB_USERNAME environment variable is not defined.");
        }

        var token = EnvironmentVariable("GITHUB_TOKEN");
        if (string.IsNullOrEmpty(token))
        {
            throw new Exception("The GITHUB_TOKEN environment variable is not defined.");
        }

        // only push whitelisted packages.
        foreach(var package in packageWhitelist)
        {
            // only push the package which was created during this build run.
            var packagePath = artifactDirectory + File(string.Concat(package, ".", nugetVersion, ".nupkg"));

            GitReleaseManagerAddAssets(username, token, githubOwner, githubRepository, majorMinorPatch, packagePath);
        }

        GitReleaseManagerClose(username, token, githubOwner, githubRepository, majorMinorPatch);
    });

// TASK TARGETS
Task("Default").IsDependentOn("BuildPackages");

// EXECUTION
RunTarget(target);