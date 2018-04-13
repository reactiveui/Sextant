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
var isRepository = StringComparer.OrdinalIgnoreCase.Equals("giusepe/sextant", AppVeyor.Environment.Repository.Name);

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


// Resolve the API keys.
var apiKey = EnvironmentVariable("NUGET_APIKEY");
var source = EnvironmentVariable("NUGET_SOURCE");

var username = EnvironmentVariable("GITHUB_USERNAME");
var token = EnvironmentVariable("GITHUB_TOKEN");


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
// Build Packages
//////////////////////////////////////////////////////////////////////

Task("BuildPackages")
	.IsDependentOn("Build")
    .Does(() =>
    {
        var nuGetPackSettings = new NuGetPackSettings
        {
            Authors = new [] {"Giusepe Casagrande"},
            Owners = new [] {"giusepe"},
            ProjectUrl = new Uri("https://github.com/giusepe/Sextant"),
            LicenseUrl = new Uri("https://github.com/giusepe/Sextant/blob/master/LICENSE"),
            RequireLicenseAcceptance = false,
            Version = nugetVersion,
            Tags = new [] {"mvvm", "reactiveui", "Rx", "Reactive Extensions", "Observable", "xamarin", "android", "ios", "forms", "monodroid", "monotouch", "xamarin.android", "xamarin.ios", "xamarin.forms"},
            ReleaseNotes = new [] { string.Format("{0}/releases", githubUrl) },
            Symbols = false,
            Verbosity = NuGetVerbosity.Detailed,
            OutputDirectory = artifactsDir,
            BasePath = "./Sextant.PCL",
            IncludeReferencedProjects = true,
            Properties = new Dictionary<string, string> {{ "Configuration", "Release" }}
        };

        NuGetPack("./Sextant.PCL/Sextant.PCL.nuspec", nuGetPackSettings);
    });

//////////////////////////////////////////////////////////////////////
// Publish Packages
//////////////////////////////////////////////////////////////////////

Task("PublishPackages")
    .IsDependentOn("BuildPackages")
    .WithCriteria(() => !local)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isRepository)
    .WithCriteria(() => isDevelopBranch || isReleaseBranch)
    .Does (() =>
    {
        if (isReleaseBranch && !isTagged)
        {
            Information("Packages will not be published as this release has not been tagged.");
            return;
        }

        if (string.IsNullOrEmpty(apiKey))
        {
            throw new Exception("The NUGET_APIKEY environment variable is not defined.");
        }

        if (string.IsNullOrEmpty(source))
        {
            throw new Exception("The NUGET_SOURCE environment variable is not defined.");
        }

        // only push whitelisted packages.
        foreach(var package in packageWhitelist)
        {
            // only push the package which was created during this build run.
            var packagePath = artifactsDir + File(string.Concat(package, ".", nugetVersion, ".nupkg"));

            // Push the package.
            NuGetPush(packagePath, new NuGetPushSettings {
                Source = source,
                ApiKey = apiKey
            });
        }
    });

Task("CreateRelease")
    .IsDependentOn("BuildPackages")
    .WithCriteria(() => !local)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isRepository)
    .WithCriteria(() => isReleaseBranch)
    .WithCriteria(() => !isTagged)
    .Does (() =>
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new Exception("The GITHUB_USERNAME environment variable is not defined.");
        }

        if (string.IsNullOrEmpty(token))
        {
            throw new Exception("The GITHUB_TOKEN environment variable is not defined.");
        }

        GitReleaseManagerCreate(username, token, githubOwner, githubRepository, new GitReleaseManagerCreateSettings {
            Milestone         = majorMinorPatch,
            Name              = majorMinorPatch,
            Prerelease        = true,
            TargetCommitish   = "master"
        });
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
        
        if (string.IsNullOrEmpty(username))
        {
            throw new Exception("The GITHUB_USERNAME environment variable is not defined.");
        }

        if (string.IsNullOrEmpty(token))
        {
            throw new Exception("The GITHUB_TOKEN environment variable is not defined.");
        }

        // only push whitelisted packages.
        foreach(var package in packageWhitelist)
        {
            // only push the package which was created during this build run.
            var packagePath = artifactsDir + File(string.Concat(package, ".", nugetVersion, ".nupkg"));

            GitReleaseManagerAddAssets(username, token, githubOwner, githubRepository, majorMinorPatch, packagePath);
        }

        GitReleaseManagerClose(username, token, githubOwner, githubRepository, majorMinorPatch);
    });

// TASK TARGETS
Task("Default")
    .IsDependentOn("PublishPackages")
    .IsDependentOn("CreateRelease")
    .IsDependentOn("PublishRelease");

// EXECUTION
RunTarget(target);