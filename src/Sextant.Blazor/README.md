# Blazor quirks to keep in mind when using Sextant
1. Blazor can navigate using the url address bar.
2. Blazor can accept parameters through the url.
3. Browsers have forward navigation buttons.
4. Browsers can skip pages in the navigation stack with the navigation buttons (right-click).
5. You can't clear the browser history with app commands.
6. You can still navigate pages via the browser buttons while a modal is showing.  (Although the modal will still be showing and blocking.)

All of this means you _can_ add in some special functionality that won't be available in other apps.  In other cases, you *must* support the extra functionality Blazor provides.

# To use

### In index.html or \_Host.cshtml
Add the following script:
```
<script src="_content/Sextant.Blazor/navStack.js"></script>
```
The javascript contained within intercepts clicks to check for link-based navigation, notifies `onpopstate` changes, and alters the state of `History` objects.

### In App.razor

1.  Replace the default `Router` component with `SextantRouter`.  This will allow viewmodel-based navigation.  
2.  Replace the default `RouteView` component with `ReactiveRouteView`.  This allows your views to have their `ViewModel` parameters set automatically.
3.  Choose a modal that Sextant will use for your modal navigation.  Sextant.Blazor comes with a very basic `Sextant.Blazor.Modal.SimpleModel` that you can use.  Otherwise, implement your own using `Sextant.Blazor.Modal.IModalView`. Details below.

```
<SextantRouter AppAssembly="@typeof(Program).Assembly"
               ModalComponent="typeof(Sextant.Blazor.Modal.SimpleModal)">
    <Found Context="routeData">
       <ReactiveRouteView RouteData="routeData" DefaultLayout="typeof(MainLayout)" />
    </Found>
    <NotFound>
        <LayoutView Layout="@typeof(MainLayout)">
            <p>Sorry, there's nothing at this address.</p>
        </LayoutView>
    </NotFound>
</SextantRouter>
```
#### If Authorization needed
You can use `ReactiveAuthorizeRouteView` instead of `ReactiveRouteView`.  Everything should work as usual.

### In App.razor @code{ } or App.razor.cs
```
using static Sextant.Sextant;

Instance.InitializeBlazor();
```
#### RegisterBlazorRoute
Then, you'll need to register Blazor routes, views, and viewmodels together.  Since you are navigating by viewmodel, the viewmodel must have some associated route that can be displayed in the url bar.  The two method signatures available are:
```
IMutableDependencyResolver RegisterBlazorRoute<TView, TViewModel>(this IMutableDependencyResolver dependencyResolver, string route, string contract = null)
IMutableDependencyResolver RegisterBlazorRoute<TView, TViewModel>(this IMutableDependencyResolver dependencyResolver, string route, Func<Dictionary<string, string>, IViewModel> generator, string contract = null)
```
You will need to link the View and ViewModel via the generic type parameter and decide on a route that will allow the router to find the correct viewmodel when navigated directly.  You can even register the same View and ViewModel more than once for different routes.

Of particular importance is the second signature that contains `Func<Dictionary<string, string>, IViewModel> generator`.  You will use this to show Sextant how to create a particular viewmodel using parameters contained in the `Dictionary<string,string>`.  These are from the query parameters of the url that was typed into the url bar.  Usually, there will be no parameters, so you can just generate a viewmodel without them, but this allows you to retain one of the features of Blazor if you need it.

Finally, if you need to differentiate uses of a viewmodel for different views, there is always the `contract` parameter.

An example of `RegisterBlazorRoute` usage:
```
Splat.Locator.CurrentMutable
    .RegisterBlazorRoute<Pages.HomeView, HomeViewModel>("/", parameters => new HomeViewModel())
    .RegisterBlazorRoute<Pages.TestView, TestViewModel>("/test", parameters = new TestViewModel(parameters["id"]));
```

#### App start
Blazor apps start by loading an initial page with a url (usually baseUrl + "/"), so there is no need to push an initial page into the stack you would for other platforms using Sextant.

### In Views 
Views must either inherit `ReactiveComponentBase<IViewModel>` or you'll have to implement `IViewFor<IViewModel>` yourself.

Since there is no real binding, you need to reference ViewModel properties directly in your razor code.  You will also need to handle cases where the `ViewModel` parameter is `null` in your razor code.  The null-check operator (i.e. `ViewModel?.SomeProperty`) will work for normal property referencing.  If you use Blazor's `@bind-Value` for two-way binding, you cannot use the null-check operator and will have to explicitly check for `@if (ViewModel != null)` first.

### Implementing IModalView
`IModalView` has two methods to implement:
```
Task ShowViewAsync(Type viewType, IViewModel viewModel);
Task HideAsync();
```
You can view `SimpleModal` for an example of how you might implement `IModalView` into your own component. There is no need to manually add your modal component to the page.  `ReactiveRouteView` will automatically include it when it renders the page that was navigated to.

Below is an example of a BlazorFabric `Modal` component implementing `IModalView`

![BlazorFabric Modal](https://media.giphy.com/media/IeivavX2xDqscZublS/giphy.gif)
