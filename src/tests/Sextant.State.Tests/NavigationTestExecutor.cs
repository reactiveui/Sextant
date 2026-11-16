// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Concurrency;
using Splat;
using Splat.Builder;
using TUnit.Core.Interfaces;

namespace Sextant.Tests;

/// <summary>Isolates ReactiveUI registration and scheduler state for each test.</summary>
public sealed class NavigationTestExecutor : ITestExecutor
{
    /// <inheritdoc />
    public async ValueTask ExecuteTest(TestContext context, Func<ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        var mainScheduler = RxSchedulers.MainThreadScheduler;
        var taskScheduler = RxSchedulers.TaskpoolScheduler;
        var previousResolver = (IDependencyResolver)AppLocator.Current;
        using var resolver = new ModernDependencyResolver();

        try
        {
            _ = global::Sextant.Sextant.Instance;
            AppBuilder.ResetBuilderStateForTests();
            AppLocator.SetLocator(resolver);
            _ = RxAppBuilder.CreateReactiveUIBuilder()
                .WithMainThreadScheduler(ImmediateScheduler.Instance)
                .WithTaskPoolScheduler(ImmediateScheduler.Instance)
                .WithCoreServices()
                .BuildApp();
            await action();
        }
        finally
        {
            AppBuilder.ResetBuilderStateForTests();
            AppLocator.SetLocator(previousResolver);
            RxSchedulers.MainThreadScheduler = mainScheduler;
            RxSchedulers.TaskpoolScheduler = taskScheduler;
        }
    }
}
