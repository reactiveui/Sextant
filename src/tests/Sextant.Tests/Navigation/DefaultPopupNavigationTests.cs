// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using Sextant.Mocks;
using Sextant.Plugins.Popup;
namespace Sextant.Tests.Navigation;

/// <summary>Checks popup interface defaults against real stack updates.</summary>
public sealed class DefaultPopupNavigationTests
{
    /// <summary>The popup registration contract.</summary>
    private const string Contract = "popup";

    /// <summary>Checks the PushPopup interface overload.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPopupDefault0()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultPopupNavigationService(service);
        await ((IPopupViewStackService)sut).PushPopup((IViewModel)model, Contract);
        await Assert.That(sut.PopupStack.Count).IsEqualTo(1);
    }

    /// <summary>Checks the PushPopup interface overload.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPopupDefault1()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultPopupNavigationService(service);
        await ((IPopupViewStackService)sut).PushPopup((IViewModel)model);
        await Assert.That(sut.PopupStack.Count).IsEqualTo(1);
    }

    /// <summary>Checks the PushPopup interface overload.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPopupDefault2()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultPopupNavigationService(service);
        await ((IPopupViewStackService)sut).PushPopup(typeof(NavigableViewModelMock), Contract);
        await Assert.That(sut.PopupStack.Count).IsEqualTo(1);
    }

    /// <summary>Checks the PushPopup interface overload.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task GenericPushPopupDefault2()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultPopupNavigationService(service);
        await ((IPopupViewStackService)sut).PushPopup<NavigableViewModelMock>(Contract);
        await Assert.That(sut.PopupStack.Count).IsEqualTo(1);
    }

    /// <summary>Checks the PushPopup interface overload.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPopupDefault3()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultPopupNavigationService(service);
        await ((IPopupViewStackService)sut).PushPopup(typeof(NavigableViewModelMock));
        await Assert.That(sut.PopupStack.Count).IsEqualTo(1);
    }

    /// <summary>Checks the PushPopup interface overload.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task GenericPushPopupDefault3()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultPopupNavigationService(service);
        await ((IPopupViewStackService)sut).PushPopup<NavigableViewModelMock>();
        await Assert.That(sut.PopupStack.Count).IsEqualTo(1);
    }

    /// <summary>Checks the PushPopup interface overload.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPopupDefault4()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultPopupNavigationService(service);
        var parameter = new NavigationParameter();
        await ((IPopupViewStackService)sut).PushPopup((INavigable)model, parameter, Contract);
        await Assert.That(sut.PopupStack.Count).IsEqualTo(1);
    }

    /// <summary>Checks the PushPopup interface overload.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPopupDefault5()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultPopupNavigationService(service);
        var parameter = new NavigationParameter();
        await ((IPopupViewStackService)sut).PushPopup((INavigable)model, parameter);
        await Assert.That(sut.PopupStack.Count).IsEqualTo(1);
    }

    /// <summary>Checks the PushPopup interface overload.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPopupDefault6()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultPopupNavigationService(service);
        var parameter = new NavigationParameter();
        await ((IPopupViewStackService)sut).PushPopup(typeof(NavigableViewModelMock), parameter, Contract);
        await Assert.That(sut.PopupStack.Count).IsEqualTo(1);
    }

    /// <summary>Checks the PushPopup interface overload.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task GenericPushPopupDefault6()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultPopupNavigationService(service);
        var parameter = new NavigationParameter();
        await ((IPopupViewStackService)sut).PushPopup<NavigableViewModelMock>(parameter, Contract);
        await Assert.That(sut.PopupStack.Count).IsEqualTo(1);
    }

    /// <summary>Checks the PushPopup interface overload.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPopupDefault7()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultPopupNavigationService(service);
        var parameter = new NavigationParameter();
        await ((IPopupViewStackService)sut).PushPopup(typeof(NavigableViewModelMock), parameter);
        await Assert.That(sut.PopupStack.Count).IsEqualTo(1);
    }

    /// <summary>Checks the PushPopup interface overload.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task GenericPushPopupDefault7()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultPopupNavigationService(service);
        var parameter = new NavigationParameter();
        await ((IPopupViewStackService)sut).PushPopup<NavigableViewModelMock>(parameter);
        await Assert.That(sut.PopupStack.Count).IsEqualTo(1);
    }

    /// <summary>Checks the PopPopup interface overload.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PopPopupDefault8()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultPopupNavigationService(service);
        await service.PushPopup(model);
        await ((IPopupViewStackService)sut).PopPopup();
        await Assert.That(sut.PopupStack.Count).IsEqualTo(0);
    }

    /// <summary>Checks the PopAllPopups interface overload.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PopAllPopupsDefault9()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultPopupNavigationService(service);
        await service.PushPopup(model);
        await ((IPopupViewStackService)sut).PopAllPopups();
        await Assert.That(sut.PopupStack.Count).IsEqualTo(0);
    }

    /// <summary>Checks the RemovePopup interface overload.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task RemovePopupDefault10()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultPopupNavigationService(service);
        await service.PushPopup(model);
        await ((IPopupViewStackService)sut).RemovePopup((IViewModel)model, Contract);
        await Assert.That(sut.PopupStack.Count).IsEqualTo(0);
    }

    /// <summary>Checks the RemovePopup interface overload.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task RemovePopupDefault11()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultPopupNavigationService(service);
        await service.PushPopup(model);
        await ((IPopupViewStackService)sut).RemovePopup((IViewModel)model);
        await Assert.That(sut.PopupStack.Count).IsEqualTo(0);
    }
}
