// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Sextant;
using Splat;

namespace NavigationLifecycle;

/// <summary>Opens an order by contract and delivers the edit result to the previous page.</summary>
internal static class Program
{
    /// <summary>Runs the page and modal flows against a local navigation host.</summary>
    /// <returns>Completion after both stacks have been checked.</returns>
    /// <exception cref="InvalidOperationException">A page, modal or navigation result differs from the expected state.</exception>
    private static async Task Main()
    {
        const string Editor = "editor";
        const int OrderNumber = 42;
        const int TimeoutSeconds = 30;
        using var deadline = new CancellationTokenSource(TimeSpan.FromSeconds(TimeoutSeconds));
        var token = deadline.Token;
        using var host = new NavigationHost();
        _ = RxAppBuilder.CreateReactiveUIBuilder().WithCoreServices().BuildApp();
        _ = AppLocator.CurrentMutable.RegisterViewModelFactory().RegisterViewModel(static () => new OrderPage(), Editor);
        using var navigation = new ParameterViewStackService(host, ViewModelFactory.Current);
        var home = new OrderPage();
        _ = await navigation.PushPage(home, new NavigationParameter(), null, resetStack: true, animate: false).ToTask(token);
        var selected = new NavigationParameter { [OrderPage.Order] = OrderNumber };
        _ = await navigation.PushPage<OrderPage>(selected, Editor).ToTask(token);
        var editor = (OrderPage)await navigation.TopPage().ToTask(token);
        var pages = await navigation.PageStack.FirstAsync().ToTask(token);
        if (editor.Number != OrderNumber || pages.Count != 2)
        {
            throw new InvalidOperationException("The selected order should appear above the home page.");
        }

        _ = await navigation.PopPage(new NavigationParameter { [OrderPage.Saved] = true }, animate: false).ToTask(token);
        if (!home.WasSaved || !editor.WasDestroyed || await navigation.TopPage().ToTask(token) != home)
        {
            throw new InvalidOperationException("The editor should close and return its save result to the home page.");
        }

        _ = await navigation.PushModal(editor.GetType(), selected, Editor, withNavigationPage: false).ToTask(token);
        var modal = (OrderPage)await navigation.TopModal().ToTask(token);
        _ = await navigation.PopModal().ToTask(token);
        var modals = await navigation.ModalStack.FirstAsync().ToTask(token);
        if (modal.Number != OrderNumber || !modal.WasDestroyed || modals.Count != 0 || await navigation.TopPage().ToTask(token) != home)
        {
            throw new InvalidOperationException("Closing the order modal should leave the modal stack empty.");
        }
    }
}
