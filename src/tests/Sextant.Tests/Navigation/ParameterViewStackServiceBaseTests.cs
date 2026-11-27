// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Sextant.Mocks;

namespace Sextant.Tests;

/// <summary>Checks which thread <see cref="ParameterViewStackServiceBase"/> calls a view model's navigation callbacks on.</summary>
public sealed class ParameterViewStackServiceBaseTests
{
    /// <summary>The record of a <c>WhenNavigatingTo</c> call.</summary>
    private const string Navigating = "navigating";

    /// <summary>The record of a <c>WhenNavigatedTo</c> call.</summary>
    private const string Navigated = "navigated";

    /// <summary>The record of a <c>WhenNavigatedFrom</c> call.</summary>
    private const string From = "from";

    /// <summary>The record of a <c>Destroy</c> call.</summary>
    private const string Destroyed = "destroy";

    /// <summary>
    /// Every navigation callback runs on the view's main thread, even when navigation starts on another thread and the
    /// platform finishes the push or pop on the thread pool. A callback that sets a bound property then updates its
    /// control on the thread that owns it.
    /// </summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task NavigationCallbacks_RunOnTheMainThread()
    {
        using var mainThread = new EventLoopScheduler();
        var mainThreadId = await Observable.Start(static () => Environment.CurrentManagedThreadId, mainThread);
        using var view = new ThreadPoolView(mainThread);
        var model = new ThreadRecordingModel();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var parameter = new NavigationParameter();

        await service.PushPage(model, parameter);
        await service.PushPage(model, parameter);
        await service.PushModal(model, parameter);
        await service.PopPage(parameter);

        await Assert.That(model.Callbacks).IsEquivalentTo([Navigating, Navigated, Navigating, Navigated, Navigating, Navigated, From, Destroyed, Navigated]);
        await Assert.That(model.Threads).All().Satisfy(threadId => threadId.IsEqualTo(mainThreadId));
    }

    /// <summary>Records each navigation callback and the thread it ran on.</summary>
    [System.Diagnostics.DebuggerDisplay("ThreadRecordingModel: {Callbacks.Count}")]
    private sealed class ThreadRecordingModel : INavigable, IDestructible
    {
        /// <summary>Gets the callbacks, in the order they ran.</summary>
        public ConcurrentQueue<string> Callbacks { get; } = new();

        /// <summary>Gets the managed thread each callback ran on.</summary>
        public ConcurrentQueue<int> Threads { get; } = new();

        /// <inheritdoc />
        public string Id => nameof(ThreadRecordingModel);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) => Record(Navigating);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) => Record(Navigated);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) => Record(From);

        /// <inheritdoc />
        public void Destroy() => _ = Record(Destroyed);

        /// <summary>Records a callback on the calling thread.</summary>
        /// <param name="callback">The callback's name.</param>
        /// <returns>A sequence that completes at once.</returns>
        private IObservable<Unit> Record(string callback)
        {
            Callbacks.Enqueue(callback);
            Threads.Enqueue(Environment.CurrentManagedThreadId);
            return Observable.Return(Unit.Default);
        }
    }

    /// <summary>A view whose main thread is a dedicated thread, and whose navigation finishes on the thread pool.</summary>
    /// <param name="mainThread">The scheduler that stands in for the UI thread.</param>
    [System.Diagnostics.DebuggerDisplay("ThreadPoolView")]
    private sealed class ThreadPoolView(IScheduler mainThread) : IView, IDisposable
    {
        /// <summary>The device pop events, which this view never raises.</summary>
        private readonly System.Reactive.Subjects.Subject<IViewModel?> _popped = new();

        /// <inheritdoc />
        public IScheduler MainThreadScheduler => mainThread;

        /// <inheritdoc />
        public IObservable<IViewModel?> PagePopped => _popped;

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> PopModal() => FinishOnThreadPool();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> PopPage(bool animate) => FinishOnThreadPool();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> PopToRootPage(bool animate) => PopPage(animate);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> PushModal(IViewModel modalViewModel, string? contract, bool withNavigationPage) => FinishOnThreadPool();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> PushPage(IViewModel viewModel, string? contract, bool resetStack, bool animate) => FinishOnThreadPool();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => _popped.Dispose();

        /// <summary>Completes on the thread pool, as a platform navigation task can.</summary>
        /// <returns>A sequence that produces one value on a thread-pool thread.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IObservable<Unit> FinishOnThreadPool() => Observable.Start(static () => Unit.Default, TaskPoolScheduler.Default);
    }
}
