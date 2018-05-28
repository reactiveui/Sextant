using ReactiveUI;
using Sextant.Abstraction;
using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Xamarin.Forms;

namespace Sextant
{
    public sealed class NavigationView : NavigationPage, IView
    {
        private readonly IScheduler backgroundScheduler;
        private readonly IScheduler mainScheduler;
        private readonly IObservable<IPageViewModel> pagePopped;
        private readonly IViewLocator viewLocator;
        public IObservable<IPageViewModel> PagePopped => this.pagePopped;

		public NavigationView(IScheduler mainScheduler, IScheduler backgroundScheduler, IViewLocator viewLocator)
        {
            //Ensure.ArgumentNotNull(backgroundScheduler, nameof(backgroundScheduler));
            //Ensure.ArgumentNotNull(mainScheduler, nameof(mainScheduler));
            //Ensure.ArgumentNotNull(viewLocator, nameof(viewLocator));

            this.backgroundScheduler = backgroundScheduler;
            this.mainScheduler = mainScheduler;
            this.viewLocator = viewLocator;

            this.pagePopped = Observable
                .FromEventPattern<NavigationEventArgs>(x => this.Popped += x, x => this.Popped -= x)
                .Select(ep => ep.EventArgs.Page.BindingContext as IPageViewModel)
                .WhereNotNull();
        }

        /// <summary>
        /// Pops the modal.
        /// </summary>
        /// <returns></returns>
        public IObservable<Unit> PopModal() =>
            this
                .Navigation
                .PopModalAsync()
                .ToObservable()
                .ToSignal()
                // XF completes the pop operation on a background thread :/
                .ObserveOn(this.mainScheduler);

        /// <summary>
        /// Pops the page.
        /// </summary>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns></returns>
        public IObservable<Unit> PopPage(bool animate) =>
            this
                .Navigation
                .PopAsync(animate)
                .ToObservable()
                .ToSignal()
                // XF completes the pop operation on a background thread :/
                .ObserveOn(this.mainScheduler);

        /// <summary>
        /// Pushes the modal.
        /// </summary>
        /// <param name="modalViewModel">The modal view model.</param>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        public IObservable<Unit> PushModal(IModalViewModel modalViewModel, string contract)
        {
            // Ensure.ArgumentNotNull(modalViewModel, nameof(modalViewModel));

            return Observable
                .Start(
                    () =>
                    {
                        var page = this.LocatePageFor(modalViewModel, contract);
                        this.SetPageTitle(page, modalViewModel.Id);
                        return page;
                    },
                    this.backgroundScheduler)
                .ObserveOn(this.mainScheduler)
                .SelectMany(
                    page =>
                        this
                            .Navigation
                            .PushModalAsync(page)
                            .ToObservable());
        }

        /// <summary>
        /// Pushes the page.
        /// </summary>
        /// <param name="pageViewModel">The page view model.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="resetStack">if set to <c>true</c> [reset stack].</param>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns></returns>
        public IObservable<Unit> PushPage(IPageViewModel pageViewModel, string contract, bool resetStack, bool animate)
        {
            // Ensure.ArgumentNotNull(pageViewModel, nameof(pageViewModel));

            // If we don't have a root page yet, be sure we create one and assign one immediately because otherwise we'll get an exception.
            // Otherwise, create it off the main thread to improve responsiveness and perceived performance.
            var hasRoot = this.Navigation.NavigationStack.Count > 0;
            var mainScheduler = hasRoot ? this.mainScheduler : CurrentThreadScheduler.Instance;
            var backgroundScheduler = hasRoot ? this.backgroundScheduler : CurrentThreadScheduler.Instance;

            return Observable
                .Start(
                    () =>
                    {
                        var page = this.LocatePageFor(pageViewModel, contract);
                        this.SetPageTitle(page, pageViewModel.Id);
                        return page;
                    },
                    backgroundScheduler)
                .ObserveOn(mainScheduler)
                .SelectMany(
                    page =>
                    {
                        if (resetStack)
                        {
                            if (this.Navigation.NavigationStack.Count == 0)
                            {
                                return this
                                    .Navigation
                                    .PushAsync(page, animated: false)
                                    .ToObservable();
                            }
                            else
                            {
                                // XF does not allow us to pop to a new root page. Instead, we need to inject the new root page and then pop to it.
                                this
                                        .Navigation
                                        .InsertPageBefore(page, this.Navigation.NavigationStack[0]);

                                return this
                                    .Navigation
                                    .PopToRootAsync(animated: false)
                                    .ToObservable();
                            }
                        }
                        else
                        {
                            return this
                                .Navigation
                                .PushAsync(page, animate)
                                .ToObservable();
                        }
                    });
        }

        private Page LocatePageFor(object viewModel, string contract)
        {
            // Ensure.ArgumentNotNull(viewModel, nameof(viewModel));

            var view = viewLocator.ResolveView(viewModel, contract);
            var viewFor = view as IViewFor;
            var page = view as Page;

            if (view == null)
            {
                throw new InvalidOperationException($"No view could be located for type '{viewModel.GetType().FullName}', contract '{contract}'. Be sure Splat has an appropriate registration.");
            }

            if (viewFor == null)
            {
                throw new InvalidOperationException($"Resolved view '{view.GetType().FullName}' for type '{viewModel.GetType().FullName}', contract '{contract}' does not implement IViewFor.");
            }

            if (page == null)
            {
                throw new InvalidOperationException($"Resolved view '{view.GetType().FullName}' for type '{viewModel.GetType().FullName}', contract '{contract}' is not a Page.");
            }

            viewFor.ViewModel = viewModel;

            return page;
        }

        private void SetPageTitle(Page page, string resourceKey)
        {
            // var title = Localize.GetString(resourceKey);
            // TODO: ensure resourceKey isn't null and is localized.
            page.Title = resourceKey;
        }
    }
}