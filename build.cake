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

var treatWarningsAsErrors = false;

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

var packageWhitelist = new[] { "Sextant.PCL", "Sextant" };


// Resolve the API keys.
var apiKey = EnvironmentVariable("NUGET_APIKEY");
var source = EnvironmentVariable("NUGET_SOURCE");

var username = EnvironmentVariable("GITHUB_USERNAME");
var token = EnvironmentVariable("GITHUB_TOKEN");


Action<string, string> Package = (nuspec, basePath) =>
{
    CreateDirectory(artifactsDir);

    Information("Packaging {0} using {1} as the BasePath.", nuspec, basePath);

    NuGetPack($"{basePath}/{nuspec}", new NuGetPackSettings
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
        BasePath = basePath,
        IncludeReferencedProjects = true,
        Properties = new Dictionary<string, string> {{ "Configuration", "Release" }}
    });
};


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
        // Build the PCL version for legacy projects
        Package("Sextant.PCL.nuspec", "./Sextant.PCL");

        // Build the .Net Standard for the cool kids
        MSBuild("./Sextant/Sextant.csproj",
        new MSBuildSettings()
            .WithTarget("pack")
            .WithProperty("PackageOutputPath",  MakeAbsolute(Directory(artifactsDir)).ToString().Quote())
            .WithProperty("TreatWarningsAsErrors", treatWarningsAsErrors.ToString())
            .SetConfiguration("Release")
            // Due to https://github.com/NuGet/Home/issues/4790 and https://github.com/NuGet/Home/issues/4337 we
            // have to pass a version explicitly
            .WithProperty("Version", nugetVersion.ToString())
            .WithProperty("InformationalVersion", informationalVersion)
            .SetVerbosity(Verbosity.Minimal)
            .SetNodeReuse(false));
    });


//////////////////////////////////////////////////////////////////////
// Update AppVeyor Build Number
//////////////////////////////////////////////////////////////////////
Task("UpdateAppVeyorBuildNumber")
    .IsDependentOn("BuildPackages")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
{
    AppVeyor.UpdateBuildVersion(buildVersion);

}).ReportError(exception =>
{
    // When a build starts, the initial identifier is an auto-incremented value supplied by AppVeyor.
    // As part of the build script, this version in AppVeyor is changed to be the version obtained from
    // GitVersion. This identifier is purely cosmetic and is used by the core team to correlate a build
    // with the pull-request. In some circumstances, such as restarting a failed/cancelled build the
    // identifier in AppVeyor will be already updated and default behaviour is to throw an
    // exception/cancel the build when in fact it is safe to swallow.
    // See https://github.com/reactiveui/ReactiveUI/issues/1262

    Warning("Build with version {0} already exists.", buildVersion);
});

//////////////////////////////////////////////////////////////////////
// Publish Packages
//////////////////////////////////////////////////////////////////////

Task("CreateRelease")
    .IsDependentOn("UpdateAppVeyorBuildNumber")
    .IsDependentOn("BuildPackages")
    .WithCriteria(() => !local)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isRepository)
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

        GitReleaseManagerCreate(username, token, githubOwner, githubRepository, new GitReleaseManagerCreateSettings {
            Milestone         = majorMinorPatch,
            Name              = majorMinorPatch,
            Prerelease        = true,
            TargetCommitish   = "master"
        });
    });

Task("PublishPackages")
    .IsDependentOn("BuildPackages")
    .WithCriteria(() => !local)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isRepository)
    .WithCriteria(() => isDevelopBranch || isTagged)
    .Does (() =>
    {
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
            NuGetPush(packagePath, new NuGetPushSettings
            {
                Source = source,
                ApiKey = apiKey
            });
        }
    });

Task("PublishRelease")
    .IsDependentOn("BuildPackages")
    .WithCriteria(() => !local)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isRepository)
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
    .IsDependentOn("CreateRelease")
    .IsDependentOn("PublishPackages")
    .IsDependentOn("PublishRelease");

// EXECUTION
RunTarget(target);