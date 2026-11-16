// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using BenchmarkDotNet.Running;

namespace Sextant.Benchmarks;

/// <summary>Class which hosts the main entry point into the application.</summary>
public static class Program
{
    /// <summary>The main entry point into the benchmarking application.</summary>
    /// <param name = "args">Arguments from the command line.</param>
    public static void Main(string[] args) => _ = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
}
