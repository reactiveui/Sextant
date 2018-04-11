//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var artifactsDir  = Directory("./artifacts/");
var rootAbsoluteDir = MakeAbsolute(Directory("./")).FullPath;

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

// TASK TARGETS
Task("Default").IsDependentOn("BuildPackages");

// EXECUTION
RunTarget(target);