#load nuget:https://www.myget.org/F/reactiveui/api/v2?package=ReactiveUI.Cake.Recipe&prerelease

Environment.SetVariableNames();

// Whitelisted Packages
var packageWhitelist = new[] 
{ 
    MakeAbsolute(File("./src/Sextant/Sextant.csproj")),
    MakeAbsolute(File("./src/Sextant.XamForms/Sextant.XamForms.csproj")),
};

var packageTestWhitelist = new[]
{
    MakeAbsolute(File("./src/Sextant.Tests/Sextant.Tests.csproj")),
    MakeAbsolute(File("./src/Sextant.XamForms.Tests/Sextant.XamForms.Tests.csproj")),
};

BuildParameters.SetParameters(context: Context, 
                            buildSystem: BuildSystem,
                            title: "Sextant",
                            whitelistPackages: packageWhitelist,
                            whitelistTestPackages: packageTestWhitelist,
                            artifactsDirectory: "./artifacts",
                            sourceDirectory: "./src");

var testCoverageExcludeFilters = new [] { string.Format("[{0}*Tests*]*", BuildParameters.Title), string.Format("[{0}*Mocks*]*", BuildParameters.Title) };

ToolSettings.SetToolSettings(context: Context, testCoverageExcludeFilters: testCoverageExcludeFilters);

Build.Run();