// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using System.Reactive.Linq;
using BenchmarkDotNet.Attributes;

namespace Sextant.Benchmarks;

/// <summary>Benchmarks navigation stack operations.</summary>
public static class ViewStackServiceBenchmark
{
    /// <summary>Benchmarks for the PopModal method.</summary>
    [System.Diagnostics.DebuggerDisplay("PopModalBenchmark: {_view}")]
    [SimpleJob]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    public sealed class PopModalBenchmark : IDisposable
    {
        /// <summary>The navigation view used by this benchmark.</summary>
        private global::Sextant.Mocks.NavigationViewMock? _view;

        /// <summary>The navigation state for this benchmark.</summary>
        private ViewStackService? _viewStackService;

        /// <summary>Setup method for when running all bench marks.</summary>
        [GlobalSetup]
        public void Setup()
        {
            _ = RxAppBuilder.CreateReactiveUIBuilder().WithCoreServices().BuildApp();
            _view = new();
            _viewStackService = new(_view, new DefaultViewModelFactory());
        }

        /// <summary>Setup method for pushing a page to the stack.</summary>
        /// <returns>A task that completes after navigation.</returns>
        [IterationSetup]
        public async Task BenchmarkSetup() => await ((IViewStackService)_viewStackService!).PushModal(new ViewModel());

        /// <summary>Benchmarks poping a modal from the view stack service.</summary>
        /// <returns>The dependency resolver.</returns>
        [Benchmark]
        public async Task PopModal() => await ((IViewStackService)_viewStackService!).PopModal();

        /// <summary>Benchmarks pushing a page onto the view stack service.</summary>
        /// <returns>The dependency resolver.</returns>
        [Benchmark]
        public async Task PopModalWithoutAnimation() => await ((IViewStackService)_viewStackService!).PopModal(false);

        /// <summary>Releases the navigation service and its view.</summary>
        [GlobalCleanup]
        public void Dispose()
        {
            _viewStackService?.Dispose();
            _view?.Dispose();
        }
    }

    /// <summary>Benchmarks for the PopPage method.</summary>
    [System.Diagnostics.DebuggerDisplay("PopPageBenchmark: {_view}")]
    [SimpleJob]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    public sealed class PopPageBenchmark : IDisposable
    {
        /// <summary>The navigation view used by this benchmark.</summary>
        private global::Sextant.Mocks.NavigationViewMock? _view;

        /// <summary>The navigation state for this benchmark.</summary>
        private ViewStackService? _viewStackService;

        /// <summary>Setup method for when running all bench marks.</summary>
        [GlobalSetup]
        public void Setup()
        {
            _ = RxAppBuilder.CreateReactiveUIBuilder().WithCoreServices().BuildApp();
            _view = new();
            _viewStackService = new(_view, new DefaultViewModelFactory());
        }

        /// <summary>Setup method for pushing a page to the stack.</summary>
        /// <returns>A task that completes after navigation.</returns>
        [IterationSetup]
        public async Task BenchmarkSetup() => await ((IViewStackService)_viewStackService!).PushPage(new ViewModel());

        /// <summary>Benchmarks pushing a page onto the view stack service.</summary>
        /// <returns>The dependency resolver.</returns>
        [Benchmark]
        public async Task PopPage() => await ((IViewStackService)_viewStackService!).PopPage();

        /// <summary>Benchmarks pushing a page onto the view stack service.</summary>
        /// <returns>The dependency resolver.</returns>
        [Benchmark]
        public async Task PopPageWithoutAnimation() => await ((IViewStackService)_viewStackService!).PopPage(false);

        /// <summary>Releases the navigation service and its view.</summary>
        [GlobalCleanup]
        public void Dispose()
        {
            _viewStackService?.Dispose();
            _view?.Dispose();
        }
    }

    /// <summary>Benchmarks for the PopToRootPage method.</summary>
    [System.Diagnostics.DebuggerDisplay("PopToRootPageBenchmark: {_view}")]
    [SimpleJob]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    public sealed class PopToRootPageBenchmark : IDisposable
    {
        /// <summary>The navigation view used by this benchmark.</summary>
        private global::Sextant.Mocks.NavigationViewMock? _view;

        /// <summary>The navigation state for this benchmark.</summary>
        private ViewStackService? _viewStackService;

        /// <summary>Setup method for when running all bench marks.</summary>
        /// <returns>A task that completes after navigation.</returns>
        [GlobalSetup]
        public async Task Setup()
        {
            _ = RxAppBuilder.CreateReactiveUIBuilder().WithCoreServices().BuildApp();
            _view = new();
            _viewStackService = new(_view, new DefaultViewModelFactory());
            await ((IViewStackService)_viewStackService).PushPage(new ViewModel());
            await ((IViewStackService)_viewStackService).PushPage(new ViewModel());
            await ((IViewStackService)_viewStackService).PushPage(new ViewModel());
        }

        /// <summary>Benchmarks pushing a page onto the view stack service.</summary>
        /// <returns>The dependency resolver.</returns>
        [Benchmark]
        public async Task PopToRootPage() => await ((IViewStackService)_viewStackService!).PopToRootPage();

        /// <summary>Releases the navigation service and its view.</summary>
        [GlobalCleanup]
        public void Dispose()
        {
            _viewStackService?.Dispose();
            _view?.Dispose();
        }
    }

    /// <summary>Benchmarks for the PushModal method.</summary>
    [System.Diagnostics.DebuggerDisplay("PushModalBenchmark: {_view}")]
    [SimpleJob]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    public sealed class PushModalBenchmark : IDisposable
    {
        /// <summary>The navigation view used by this benchmark.</summary>
        private global::Sextant.Mocks.NavigationViewMock? _view;

        /// <summary>The navigation state for this benchmark.</summary>
        private ViewStackService? _viewStackService;

        /// <summary>The navigation state for this benchmark.</summary>
        private ViewModel? _viewModel;

        /// <summary>Setup method for when running all bench marks.</summary>
        [GlobalSetup]
        public void Setup()
        {
            _ = RxAppBuilder.CreateReactiveUIBuilder().WithCoreServices().BuildApp();
            _view = new();
            _viewStackService = new(_view, new DefaultViewModelFactory());
        }

        /// <summary>Setup method for pushing a page to the stack.</summary>
        [IterationSetup]
        public void BenchmarkSetup() => _viewModel = new();

        /// <summary>Clean up method after pushing a page to the stack.</summary>
        /// <returns>A task that completes after navigation.</returns>
        [IterationCleanup]
        public async Task BenchmarkCleanup()
        {
            await ((IViewStackService)_viewStackService!).PopModal();
            _viewModel = null;
        }

        /// <summary>Benchmarks pushing a modal onto the view stack service.</summary>
        /// <returns>The dependency resolver.</returns>
        [Benchmark]
        public async Task PushModal() => await ((IViewStackService)_viewStackService!).PushModal(_viewModel!);

        /// <summary>Benchmarks pushing a modal onto the view stack service.</summary>
        /// <returns>The dependency resolver.</returns>
        [Benchmark]
        public async Task PushModalWithContract() => await ((IViewStackService)_viewStackService!).PushModal(_viewModel!, nameof(ViewModel));

        /// <summary>Releases the navigation service and its view.</summary>
        [GlobalCleanup]
        public void Dispose()
        {
            _viewStackService?.Dispose();
            _view?.Dispose();
        }
    }

    /// <summary>Benchmarks for the PushPage method.</summary>
    [System.Diagnostics.DebuggerDisplay("PushPageBenchmark: {_view}")]
    [SimpleJob]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    public sealed class PushPageBenchmark : IDisposable
    {
        /// <summary>The navigation view used by this benchmark.</summary>
        private global::Sextant.Mocks.NavigationViewMock? _view;

        /// <summary>The navigation state for this benchmark.</summary>
        private ViewStackService? _viewStackService;

        /// <summary>The navigation state for this benchmark.</summary>
        private ViewModel? _viewModel;

        /// <summary>Setup method for when running all bench marks.</summary>
        [GlobalSetup]
        public void Setup()
        {
            _ = RxAppBuilder.CreateReactiveUIBuilder().WithCoreServices().BuildApp();
            _view = new();
            _viewStackService = new(_view, new DefaultViewModelFactory());
        }

        /// <summary>Setup method for pushing a page to the stack.</summary>
        [IterationSetup]
        public void BenchmarkSetup() => _viewModel = new();

        /// <summary>Clean up method after pushing a page to the stack.</summary>
        /// <returns>A task that completes after navigation.</returns>
        [IterationCleanup]
        public async Task BenchmarkCleanup()
        {
            await ((IViewStackService)_viewStackService!).PopToRootPage();
            _viewModel = null;
        }

        /// <summary>Benchmarks pushing a page onto the view stack service.</summary>
        /// <returns>The dependency resolver.</returns>
        [Benchmark]
        public async Task PushPage() => await ((IViewStackService)_viewStackService!).PushPage(_viewModel!);

        /// <summary>Benchmarks pushing a page onto the view stack service.</summary>
        /// <returns>The dependency resolver.</returns>
        [Benchmark]
        public async Task PushPageWithContract() => await ((IViewStackService)_viewStackService!).PushPage(_viewModel!, nameof(ViewModel));

        /// <summary>Benchmarks pushing a page onto the view stack service.</summary>
        /// <returns>The dependency resolver.</returns>
        [Benchmark]
        public async Task PushPageAndAnimate() => await ((IViewStackService)_viewStackService!).PushPage(_viewModel!, null, false, true);

        /// <summary>Benchmarks pushing a page onto the view stack service.</summary>
        /// <returns>The dependency resolver.</returns>
        [Benchmark]
        public async Task PushPageAndResetStack() => await ((IViewStackService)_viewStackService!).PushPage(_viewModel!, null, resetStack: true);

        /// <summary>Releases the navigation service and its view.</summary>
        [GlobalCleanup]
        public void Dispose()
        {
            _viewStackService?.Dispose();
            _view?.Dispose();
        }
    }
}
