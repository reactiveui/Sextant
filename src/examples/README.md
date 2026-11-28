# Navigation examples

| Example | What to try |
| --- | --- |
| [NavigationLifecycle](NavigationLifecycle/Program.cs) | Open an order by contract, read its parameters, return a save result on pop, and inspect the page and modal stacks. The host runs without a window. |
| [Avalonia](SextantSample.Avalonia/App.axaml.cs) | Register a navigation view, pair views with view models, and navigate in a desktop window. |
| [MAUI](SextantSample.Maui/MauiProgram.cs) | Register the mobile navigation host, open a page or modal, or select **Show delivery note** to push a [typed popup](SextantSample.Maui/Views/DeliveryNotePopup.cs). Tap outside the note to close it. |

From `src`, run the page and modal example with
`dotnet run --project examples/NavigationLifecycle/NavigationLifecycle.csproj --framework net11.0`.
Use `net10.0` to run on .NET 10.

The [order page](NavigationLifecycle/OrderPage.cs) receives the values passed to each
navigation operation. The [local host](NavigationLifecycle/NavigationHost.cs) supplies the
scheduler and reports when a page is popped. The [shared view models](SextantSample.Core)
show the navigation commands used by the Avalonia and MAUI applications.
