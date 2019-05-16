#load nuget:https://www.myget.org/F/reactiveui/api/v2?package=ReactiveUI.Cake.Recipe&version=1.0.114

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

ToolSettings.SetToolSettings(context: Context);

Build.Run();