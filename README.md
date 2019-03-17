## Sextant

[![NuGet Stats](https://img.shields.io/nuget/v/sextant.svg)](https://www.nuget.org/packages/sextant) [![Build Status](https://dev.azure.com/dotnet/ReactiveUI/_apis/build/status/Sextant-CI)](https://dev.azure.com/dotnet/ReactiveUI/_build/latest?definitionId=76) [![Code Coverage](https://codecov.io/gh/reactiveui/sextant/branch/master/graph/badge.svg)](https://codecov.io/gh/reactiveui/sextant)
<br>
<a href="https://www.nuget.org/packages/sextant">
        <img src="https://img.shields.io/nuget/dt/sextant.svg">
</a>
<a href="#backers">
        <img src="https://opencollective.com/reactiveui/backers/badge.svg">
</a>
<a href="#sponsors">
        <img src="https://opencollective.com/reactiveui/sponsors/badge.svg">
</a>
<a href="https://reactiveui.net/slack">
        <img src="https://img.shields.io/badge/chat-slack-blue.svg">
</a>

<p align="left"><img src="logo/vertical.png" alt="Sextant" height="180px"></p>

## A ReactiveUI navigation library for Xamarin.Forms

Sextant was born from a fork of [Xamvvm](https://github.com/xamvvm/xamvvm) which is nice and simple MVVM Framework with a good navigation system. The problem is, I just wanted a simple navigation system to use with [ReactiveUI](https://github.com/reactiveui/ReactiveUI) without all the things that come along an MVVM framework. Plus, I wanted to make it more "Reactive Friendly".

Then a wild [Rodney Littles](https://github.com/rlittlesii) appears, and with him an implementation of this [AMAZING POST](https://kent-boogaart.com/blog/custom-routing-in-reactiveui) by [Kent](https://github.com/kentcb)

Sextant is in a very initial stage and in constant change, so please be pantience with use... because we will break things.

This library is nothing more than me "standing on the shoulders of giants":
[Kent](https://github.com/kentcb) for been the original and true creator of this, I pretty much just copied and pasted it :)
[Geoffrey Huntley](https://github.com/ghuntley) maintainer on [ReactiveUI](https://github.com/reactiveui/ReactiveUI) (especially the build.cake) from where learned a lot about Cake + AppVeyor

## Usage

Install the nuget package on your Forms project and ViewModels project.

Register the views:
```csharp
SextantHelper.RegisterView<HomeView,HomeViewModel>();
SextantHelper.RegisterView<FirstModalView,FirstModalViewModel>();
SextantHelper.RegisterView<SecondModalView, SecondModalViewModel>();
SextantHelper.RegisterView<RedView, RedViewModel>();
```

(optional)If you need some especial configuration on the Navigation, like diferent colors, register a NavigationView for the VM:
```csharp
SextantHelper.RegisterNavigation<BlueNavigationView, SecondModalViewModel>();
```

Set the initial page:
```csharp
MainPage = SextantHelper.Initialize<HomeViewModel>();
```

After that all you have to do is call one of the methods inside your ViewModels:
```csharp
/// <summary>
/// Pops the <see cref="IPageViewModel"/> off the stack.
/// </summary>
/// <param name="animate">if set to <c>true</c> [animate].</param>
/// <returns></returns>
IObservable<Unit> PopModal(bool animate = true);

/// <summary>
/// Pops the <see cref="IPageViewModel"/> off the stack.
/// </summary>
/// <param name="animate">if set to <c>true</c> [animate].</param>
/// <returns></returns>
IObservable<Unit> PopPage(bool animate = true);

/// <summary>
/// Pushes the <see cref="IPageViewModel"/> onto the stack.
/// </summary>
/// <param name="modal">The modal.</param>
/// <param name="contract">The contract.</param>
/// <returns></returns>
IObservable<Unit> PushModal(IPageViewModel modal, string contract = null);

/// <summary>
/// Pushes the <see cref="IPageViewModel"/> onto the stack.
/// </summary>
/// <param name="page">The page.</param>
/// <param name="contract">The contract.</param>
/// <param name="resetStack">if set to <c>true</c> [reset stack].</param>
/// <param name="animate">if set to <c>true</c> [animate].</param>
/// <returns></returns>
IObservable<Unit> PushPage(IPageViewModel page, string contract = null, bool resetStack = false, bool animate = true);
```

For example:
```csharp
OpenModal = ReactiveCommand
    .CreateFromObservable(() =>
        this.ViewStackService.PushModal(new FirstModalViewModel(ViewStackService)),
        outputScheduler: RxApp.MainThreadScheduler);
```

For more examples, look inside the sample folder in the solution. 

## Contribute

Sextant is developed under an OSI-approved open source license, making it freely usable and distributable, even for commercial use. Because of our Open Collective model for funding and transparency, we are able to funnel support and funds through to our contributors and community. We ❤ the people who are involved in this project, and we’d love to have you on board, especially if you are just getting started or have never contributed to open-source before.

So here's to you, lovely person who wants to join us — this is how you can support us:

* [Responding to questions on StackOverflow](https://stackoverflow.com/questions/tagged/sextant)
* [Passing on knowledge and teaching the next generation of developers](http://ericsink.com/entries/dont_use_rxui.html)
* [Donations](https://reactiveui.net/donate) and [Corporate Sponsorships](https://reactiveui.net/sponsorship)
* [Asking your employer to reciprocate and contribute to open-source](https://github.com/github/balanced-employee-ip-agreement)
* Submitting documentation updates where you see fit or lacking.
* Making contributions to the code base.
