# Sextant repository guidance

Use the .NET 11 SDK. Libraries, tests, and benchmarks support .NET 10 and .NET 11. Keep the target-specific Microsoft package versions in the central package file's Choose branches.

## Layout

- `src/Sextant`, `src/Sextant.Avalonia`, `src/Sextant.Maui`, and `src/Sextant.Plugins.Popup` contain the libraries.
- `src/tests` contains the TUnit tests and hand-written mocks.
- `src/examples` contains the Avalonia and MAUI examples.
- `src/benchmarks/Sextant.Benchmarks` contains the navigation benchmarks.
- `Directory.Packages.props` manages shared package versions.

## Build and test

Run commands from `src`, selecting the target framework explicitly:

```sh
dotnet restore Sextant.slnx
dotnet build tests/Sextant.Tests/Sextant.Tests.csproj --configuration Release --framework net11.0
dotnet test --project tests/Sextant.Tests/Sextant.Tests.csproj --configuration Release --framework net11.0
```

Check both supported frameworks when changing shared code. Platform projects require the corresponding .NET workloads.

## Standards

Follow `.editorconfig` and fix analyzer diagnostics. The repository uses StyleSharp, PerformanceSharp, SecuritySharp, and PublicApiSharp. Update the target-specific public API baselines when changing public members.

Tests use TUnit on Microsoft.Testing.Platform with awaited assertions and hand-written mocks. Keep navigation tests independent of global state. Put registration tests in `Sextant.State.Tests`, whose TUnit executor resets the resolver and schedulers for each test and whose assembly runs serially. Do not use reflection to test private methods; expose testable internal helpers through MSBuild `InternalsVisibleTo` items.

The shared `src/testconfig.json` configures Microsoft's native Cobertura coverage and is copied beside both test executables. Exclude source-generated code through `GeneratedCodeAttribute`. Merge the reports from both test projects when measuring library coverage.

Use the existing reusable workflows in `reactiveui/actions-common`. Repository-specific changes belong in Sextant.
