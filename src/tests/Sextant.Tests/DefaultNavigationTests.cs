// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using Sextant.Mocks;
namespace Sextant.Tests;

/// <summary>Checks interface overloads against real navigation stack updates.</summary>
public sealed class DefaultNavigationTests
{
    /// <summary>The contract shared by registered view models.</summary>
    private const string Contract = "contract";

    /// <summary>Checks the PopModal interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PopModalDefault0()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        await service.PushModal(model);
        await ((IParameterViewStackService)sut).PopModal();
        await Assert.That((await sut.ModalStack.FirstAsync()).Count).IsEqualTo(0);
    }

    /// <summary>Checks the PopPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PopPageDefault1()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        await service.PushPage(model);
        await ((IParameterViewStackService)sut).PopPage();
        await Assert.That((await sut.PageStack.FirstAsync()).Count).IsEqualTo(0);
    }

    /// <summary>Checks the PopToRootPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PopToRootPageDefault2()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        await service.PushPage(model);
        await ((IParameterViewStackService)sut).PopToRootPage();
        await Assert.That((await sut.PageStack.FirstAsync()).Count).IsEqualTo(1);
    }

    /// <summary>Checks the PushModal interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushModalDefault3()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        await ((IParameterViewStackService)sut).PushModal((IViewModel)model, Contract);
        await Assert.That(await sut.TopModal()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushModal interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushModalDefault4()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        await ((IParameterViewStackService)sut).PushModal((IViewModel)model);
        await Assert.That(await sut.TopModal()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushModal interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushModalDefault5()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        await ((IParameterViewStackService)sut).PushModal(typeof(NavigableViewModelMock), Contract);
        await Assert.That(await sut.TopModal()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushModal interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushModalDefault6()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        await ((IParameterViewStackService)sut).PushModal(typeof(NavigableViewModelMock));
        await Assert.That(await sut.TopModal()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPageDefault7()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        await ((IParameterViewStackService)sut).PushPage(typeof(NavigableViewModelMock), Contract, false);
        await Assert.That(await sut.TopPage()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPageDefault8()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        await ((IParameterViewStackService)sut).PushPage(typeof(NavigableViewModelMock), Contract);
        await Assert.That(await sut.TopPage()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPageDefault9()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        await ((IParameterViewStackService)sut).PushPage(typeof(NavigableViewModelMock));
        await Assert.That(await sut.TopPage()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPageDefault10()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        await ((IParameterViewStackService)sut).PushPage((IViewModel)model, Contract, false);
        await Assert.That(await sut.TopPage()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPageDefault11()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        await ((IParameterViewStackService)sut).PushPage((IViewModel)model, Contract);
        await Assert.That(await sut.TopPage()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPageDefault12()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        await ((IParameterViewStackService)sut).PushPage((IViewModel)model);
        await Assert.That(await sut.TopPage()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPageDefault13()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        await ((IParameterViewStackService)sut).PushPage((INavigable)model, Contract, false);
        await Assert.That(await sut.TopPage()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPageDefault14()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        await ((IParameterViewStackService)sut).PushPage((INavigable)model, Contract);
        await Assert.That(await sut.TopPage()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPageDefault15()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        await ((IParameterViewStackService)sut).PushPage((INavigable)model);
        await Assert.That(await sut.TopPage()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPageDefault16()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        var parameter = new NavigationParameter();
        await ((IParameterViewStackService)sut).PushPage((INavigable)model, parameter, Contract, false);
        await Assert.That(await sut.TopPage()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPageDefault17()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        var parameter = new NavigationParameter();
        await ((IParameterViewStackService)sut).PushPage((INavigable)model, parameter, Contract);
        await Assert.That(await sut.TopPage()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPageDefault18()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        var parameter = new NavigationParameter();
        await ((IParameterViewStackService)sut).PushPage((INavigable)model, parameter);
        await Assert.That(await sut.TopPage()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPageDefault19()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        var parameter = new NavigationParameter();
        await ((IParameterViewStackService)sut).PushPage(typeof(NavigableViewModelMock), parameter, Contract, false);
        await Assert.That(await sut.TopPage()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPageDefault20()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        var parameter = new NavigationParameter();
        await ((IParameterViewStackService)sut).PushPage(typeof(NavigableViewModelMock), parameter, Contract);
        await Assert.That(await sut.TopPage()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushPageDefault21()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        var parameter = new NavigationParameter();
        await ((IParameterViewStackService)sut).PushPage(typeof(NavigableViewModelMock), parameter);
        await Assert.That(await sut.TopPage()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushModal interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushModalDefault22()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        var parameter = new NavigationParameter();
        await ((IParameterViewStackService)sut).PushModal((INavigable)model, parameter, Contract);
        await Assert.That(await sut.TopModal()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushModal interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushModalDefault23()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        var parameter = new NavigationParameter();
        await ((IParameterViewStackService)sut).PushModal((INavigable)model, parameter);
        await Assert.That(await sut.TopModal()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushModal interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushModalDefault24()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        var parameter = new NavigationParameter();
        await ((IParameterViewStackService)sut).PushModal(typeof(NavigableViewModelMock), parameter, Contract);
        await Assert.That(await sut.TopModal()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PushModal interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PushModalDefault25()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        var parameter = new NavigationParameter();
        await ((IParameterViewStackService)sut).PushModal(typeof(NavigableViewModelMock), parameter);
        await Assert.That(await sut.TopModal()).IsSameReferenceAs(model);
    }

    /// <summary>Checks the PopPage interface overload and its resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PopPageDefault26()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var sut = new DefaultNavigationService(service);
        var parameter = new NavigationParameter();
        await service.PushPage(model);
        await ((IParameterViewStackService)sut).PopPage(parameter);
        await Assert.That((await sut.PageStack.FirstAsync()).Count).IsEqualTo(0);
    }
}
