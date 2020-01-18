# Blazor quirks to keep in mind when using Sextant
1. Blazor can navigate using the url address bar.
2. Blazor can accept paramaters through the url.
3. Browsers have forward navigation buttons.
4. Browsers can skip pages in the navigation stack with the navigation buttons (right-click).
5. You can't clear the browser history with app commands.
6. You can still navigate pages via the browser buttons while a modal is showing.  (Although the modal will still be showing and blocking.)

All of this means you _can_ add in some special functionality that won't be available in other apps.  In other cases, you *must* support the extra functionality Blazor provides.

# To use

### In App.razor or App.razor.cs
```
using static Sextant.Sextant;

Instance.InitializeBlazor();

```
Then, you'll need to register Blazor routes, views, and viewmodels together.  Since you are navigating by viewmodel, the viewmodel must have some associated route that can be displayed in the url bar.
```
RegisterBlazorRoute<Pages.HomeView, HomeViewModel>("/", p => new HomeViewModel())
```

