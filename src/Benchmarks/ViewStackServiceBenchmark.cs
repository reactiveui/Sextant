// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Sextant.Benchmarks
{
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
    /// <summary>
    /// Benchmarks for the ViewStackService.
    /// </summary>
    [SimpleJob(runtimeMoniker: RuntimeMoniker.Net462)]
    [SimpleJob(runtimeMoniker: RuntimeMoniker.Net80)]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    public class ViewStackServiceBenchmark
    {
        /// <summary>
        /// Benchmarks for the PopModal method.
        /// </summary>
        public class PopModalBenchmark
        {
            private IViewStackService? _viewStackService;

            /// <summary>
            /// Setup method for when running all bench marks.
            /// </summary>
            [GlobalSetup]
            public void Setup()
            {
                _viewStackService = new ViewStackService(new BenchmarkView());
            }

            /// <summary>
            /// Setup method for pushing a page to the stack.
            /// </summary>
            [IterationSetup]
            public void BenchmarkSetup()
            {
                _viewStackService?.PushModal(new ViewModel());
            }

            /// <summary>
            /// Benchmarks poping a modal from the view stack service.
            /// </summary>
            /// <returns>The dependency resolver.</returns>
            [Benchmark]
            public IObservable<Unit> PopModal() => _viewStackService!.PopModal();

            /// <summary>
            /// Benchmarks pushing a page onto the view stack service.
            /// </summary>
            /// <returns>The dependency resolver.</returns>
            [Benchmark]
            public IObservable<Unit> PopModalWithoutAnimation() => _viewStackService!.PopModal(false);
        }

        /// <summary>
        /// Benchmarks for the PopPage method.
        /// </summary>
        public class PopPageBenchmark
        {
            private IViewStackService? _viewStackService;

            /// <summary>
            /// Setup method for when running all bench marks.
            /// </summary>
            [GlobalSetup]
            public void Setup()
            {
                _viewStackService = new ViewStackService(new BenchmarkView());
            }

            /// <summary>
            /// Setup method for pushing a page to the stack.
            /// </summary>
            [IterationSetup]
            public void BenchmarkSetup()
            {
                _viewStackService?.PushPage(new ViewModel());
            }

            /// <summary>
            /// Benchmarks pushing a page onto the view stack service.
            /// </summary>
            /// <returns>The dependency resolver.</returns>
            [Benchmark]
            public IObservable<Unit> PopPage() => _viewStackService!.PopPage();

            /// <summary>
            /// Benchmarks pushing a page onto the view stack service.
            /// </summary>
            /// <returns>The dependency resolver.</returns>
            [Benchmark]
            public IObservable<Unit> PopPageWithoutAnimation() => _viewStackService!.PopPage(false);
        }

        /// <summary>
        /// Benchmarks for the PopToRootPage method.
        /// </summary>
        public class PopToRootPageBenchmark
        {
            private IViewStackService? _viewStackService;

            /// <summary>
            /// Setup method for when running all bench marks.
            /// </summary>
            [GlobalSetup]
            public void Setup()
            {
                _viewStackService = new ViewStackService(new BenchmarkView());
                _viewStackService.PushPage(new ViewModel());
                _viewStackService.PushPage(new ViewModel());
                _viewStackService.PushPage(new ViewModel());
            }

            /// <summary>
            /// Benchmarks pushing a page onto the view stack service.
            /// </summary>
            /// <returns>The dependency resolver.</returns>
            [Benchmark]
            public IObservable<Unit> PopToRootPage() => _viewStackService!.PopToRootPage();
        }

        /// <summary>
        /// Benchmarks for the PushModal method.
        /// </summary>
        public class PushModalBenchmark
        {
            private IViewStackService? _viewStackService;
            private ViewModel? _viewModel;

            /// <summary>
            /// Setup method for when running all bench marks.
            /// </summary>
            [GlobalSetup]
            public void Setup()
            {
                _viewStackService = new ViewStackService(new BenchmarkView());
            }

            /// <summary>
            /// Setup method for pushing a page to the stack.
            /// </summary>
            [IterationSetup]
            public void BenchmarkSetup()
            {
                _viewModel = new ViewModel();
            }

            /// <summary>
            /// Clean up method after pushing a page to the stack.
            /// </summary>
            [IterationCleanup]
            public void BenchmarkCleanup()
            {
                _viewStackService?.PopModal();
                _viewModel = null;
            }

            /// <summary>
            /// Benchmarks pushing a modal onto the view stack service.
            /// </summary>
            /// <returns>The dependency resolver.</returns>
            [Benchmark]
            public IObservable<Unit> PushModal() => _viewStackService!.PushModal(_viewModel!);

            /// <summary>
            /// Benchmarks pushing a modal onto the view stack service.
            /// </summary>
            /// <returns>The dependency resolver.</returns>
            [Benchmark]
            public IObservable<Unit> PushModalWithContract() =>
                _viewStackService!.PushModal(_viewModel!, nameof(ViewModel));
        }

        /// <summary>
        /// Benchmarks for the PushPage method.
        /// </summary>
        public class PushPageBenchmark
        {
            private IViewStackService? _viewStackService;
            private ViewModel? _viewModel;

            /// <summary>
            /// Setup method for when running all bench marks.
            /// </summary>
            [GlobalSetup]
            public void Setup()
            {
                _viewStackService = new ViewStackService(new BenchmarkView());
            }

            /// <summary>
            /// Setup method for pushing a page to the stack.
            /// </summary>
            [IterationSetup]
            public void BenchmarkSetup()
            {
                _viewModel = new ViewModel();
            }

            /// <summary>
            /// Clean up method after pushing a page to the stack.
            /// </summary>
            [IterationCleanup]
            public void BenchmarkCleanup()
            {
                _viewStackService?.PopToRootPage();
                _viewModel = null;
            }

            /// <summary>
            /// Benchmarks pushing a page onto the view stack service.
            /// </summary>
            /// <returns>The dependency resolver.</returns>
            [Benchmark]
            public IObservable<Unit> PushPage() => _viewStackService!.PushPage(_viewModel!);

            /// <summary>
            /// Benchmarks pushing a page onto the view stack service.
            /// </summary>
            /// <returns>The dependency resolver.</returns>
            [Benchmark]
            public IObservable<Unit> PushPageWithContract() => _viewStackService!.PushPage(_viewModel!, nameof(ViewModel));

            /// <summary>
            /// Benchmarks pushing a page onto the view stack service.
            /// </summary>
            /// <returns>The dependency resolver.</returns>
            [Benchmark]
            public IObservable<Unit> PushPageAndAnimate() => _viewStackService!.PushPage(_viewModel!);

            /// <summary>
            /// Benchmarks pushing a page onto the view stack service.
            /// </summary>
            /// <returns>The dependency resolver.</returns>
            [Benchmark]
            public IObservable<Unit> PushPageAndResetStack() =>
                _viewStackService!.PushPage(_viewModel!, resetStack: true);
        }
    }
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
}
