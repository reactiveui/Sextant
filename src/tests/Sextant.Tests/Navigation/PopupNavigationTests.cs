// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Sextant.Mocks;
using Sextant.Plugins.Popup;

namespace Sextant.Tests.Navigation;

/// <summary>Checks popup navigation, lifecycle events and view binding.</summary>
public sealed class PopupNavigationTests
{
    /// <summary>The optional navigation contract.</summary>
    private const string Contract = "popup";

    /// <summary>The number of overloads that push a popup.</summary>
    private const int PushOverloadCount = 12;

    /// <summary>Checks popup overloads and all four navigation events.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PopupOverloadsResolvePagesAndPublishEvents()
    {
        using var view = new NavigationViewMock();
        var model = new PopupModel();
        var page = new SextantPopupPage<IViewModel>();
        var locator = new PlatformViewLocator { Factory = () => page };
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        List<PopupNavigationEvent> pushing = [];
        List<PopupNavigationEvent> pushed = [];
        List<PopupNavigationEvent> popping = [];
        List<PopupNavigationEvent> popped = [];
        using var pushingSubscription = service.Pushing.Subscribe(pushing.Add);
        using var pushedSubscription = service.Pushed.Subscribe(pushed.Add);
        using var poppingSubscription = service.Popping.Subscribe(popping.Add);
        using var poppedSubscription = service.Popped.Subscribe(popped.Add);
        await service.PushPopup(model);
        await service.PushPopup(model, Contract);
        await service.PushPopup(model, Contract, false);
        await service.PushPopup(typeof(PopupModel));
        await service.PushPopup(typeof(PopupModel), Contract);
        await service.PushPopup(typeof(PopupModel), Contract, false);
        var parameter = new NavigationParameter();
        await service.PushPopup(model, parameter);
        await service.PushPopup(model, parameter, Contract);
        await service.PushPopup(model, parameter, Contract, false);
        await service.PushPopup(typeof(PopupModel), parameter);
        await service.PushPopup(typeof(PopupModel), parameter, Contract);
        await service.PushPopup(typeof(PopupModel), parameter, Contract, false);
        await Assert.That(service.PopupStack.Count).IsEqualTo(PushOverloadCount);
        await Assert.That(pushing.Count).IsEqualTo(PushOverloadCount);
        await Assert.That(pushed.Count).IsEqualTo(PushOverloadCount);
        await Assert.That(pushed[0].ViewModel).IsSameReferenceAs(model);
        await Assert.That(pushed[0].IsAnimated).IsTrue();
        await Assert.That(pushed[^1].IsAnimated).IsFalse();
        await service.PopPopup();
        await service.PopPopup(false);
        await service.RemovePopup(model);
        await service.RemovePopup(model, Contract);
        await service.RemovePopup(model, Contract, false);
        await Assert.That(popping.Count).IsEqualTo(PushOverloadCount - service.PopupStack.Count);
        await Assert.That(popped.Count).IsEqualTo(model.DestroyCount);
        await service.PopAllPopups();
        await Assert.That(service.PopupStack.Count).IsEqualTo(0);
        await service.PushPopup(model);
        await service.PopAllPopups(false);
        await Assert.That(service.PopupStack.Count).IsEqualTo(0);
    }

    /// <summary>Checks invalid popup registrations and non-view-model stack entries.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PopupNavigationRejectsInvalidViewsAndParameters()
    {
        using var view = new NavigationViewMock();
        var model = new PopupModel();
        var locator = new PlatformViewLocator();
        var navigation = new PopupNavigationMock();
        using var service = new PopupViewStackService(view, navigation, locator, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        await Assert.That(() => service.PushPopup(model)).Throws<InvalidOperationException>();
        locator.Factory = static () => new PageView();
        await Assert.That(() => service.PushPopup(model)).Throws<InvalidOperationException>();
        await Assert.That(() => service.PushPopup((IViewModel)null!)).Throws<ArgumentNullException>();
        await Assert.That(() => service.RemovePopup(null!)).Throws<ArgumentNullException>();
        await Assert.That(async () => await service.PushPopup((INavigable)null!, new NavigationParameter())).Throws<ArgumentNullException>();
        await Assert.That(async () => await service.PushPopup(model, (INavigationParameter)null!)).Throws<ArgumentNullException>();
        navigation.Seed(new());
        navigation.Seed(new SextantPopupPage<IViewModel>());
        await Assert.That(service.PopupStack.Count).IsEqualTo(0);
        await Assert.That(static () => new PopupNavigationEvent(null!, false)).Throws<ArgumentNullException>();
        await Assert.That(static () => new PopupNavigationEvent(new SextantPopupPage<IViewModel>(), false)).Throws<InvalidOperationException>();
    }

    /// <summary>Checks base, generic and untyped popup binding.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PopupPagesSynchronizeBindingContextsAndBackgroundClicks()
    {
        var model = new PopupModel();
        var page = new TestPopup();
        var clicks = 0;
        using var subscription = page.BackgroundClick.Subscribe(_ => clicks++);
        page.ViewModel = model;
        await Assert.That(page.BindingContext).IsSameReferenceAs(model);
        page.BindingContext = null;
        await Assert.That(page.ViewModel).IsNull();
        EventHandler? clickHandler = null;
        using (var clicksSubscription = PopupPageEvents.ObserveBackgroundClicks(handler => clickHandler = handler, _ => clickHandler = null).Subscribe(_ => clicks++))
        {
            clickHandler!(page, EventArgs.Empty);
            await Assert.That(clicks).IsEqualTo(1);
        }

        await Assert.That(clickHandler).IsNull();
        var generic = new SextantPopupPage<PopupModel> { ViewModel = model };
        await Assert.That(generic.BindingContext).IsSameReferenceAs(model);
        IViewFor untyped = generic;
        await Assert.That(untyped.ViewModel).IsSameReferenceAs(model);
        untyped.ViewModel = null;
        await Assert.That(generic.ViewModel).IsNull();
        generic.BindingContext = model;
        await Assert.That(generic.ViewModel).IsSameReferenceAs(model);
    }

    /// <summary>Provides an accessible base popup constructor.</summary>
    private sealed class TestPopup : SextantPopupPage
    {
    }

    /// <summary>Records popup lifecycle notifications.</summary>
    private sealed class PopupModel : INavigable, IDestructible
    {
        /// <inheritdoc />
        public string Id => nameof(PopupModel);

        /// <summary>Gets the number of destruction calls.</summary>
        public int DestroyCount { get; private set; }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) => Observable.Return(Unit.Default);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) => WhenNavigatingTo(parameter);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) => WhenNavigatedTo(parameter);

        /// <inheritdoc />
        public void Destroy() => DestroyCount++;
    }
}
