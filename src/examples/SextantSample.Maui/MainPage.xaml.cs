// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
namespace SextantSample.Maui;

/// <summary>Displays the sample counter page.</summary>
[System.Diagnostics.DebuggerDisplay("MainPage: {_count}")]
public partial class MainPage : ContentPage
{
    /// <summary>The number of counter button clicks.</summary>
    private int _count;

    /// <summary>Initializes a new instance of the <see cref="MainPage"/> class.</summary>
    public MainPage()
    {
        InitializeComponent();
        CounterBtn.Clicked += OnCounterClicked;
    }

    /// <summary>Updates the counter and its accessibility announcement.</summary>
    /// <param name="sender">The clicked button.</param>
    /// <param name="e">The click event data.</param>
    private void OnCounterClicked(object? sender, EventArgs e)
    {
        _count++;
        CounterBtn.Text = _count == 1 ? $"Clicked {_count} time" : $"Clicked {_count} times";
        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}
