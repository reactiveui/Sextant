using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace Sextant.XamForms
{
    public class NavigationPageSystemPopBehavior : BehaviorBase<NavigationPage>
    {
        /// <inheritdoc/>
        protected override void OnAttachedTo(NavigationPage bindable)
        {
            Observable
                .FromEvent<EventHandler<NavigationEventArgs>, NavigationEventArgs>(
                    handler =>
                    {
                        void Handler(object sender, NavigationEventArgs args) => handler(args);
                        return Handler;
                    },
                    x => bindable.Popped += x,
                    x => bindable.Popped -= x)
                .Where(x => x.Page.BindingContext is INavigated)
                .Where(_ => true) // TODO: [rlittlesii: January 10, 2021] Verify this was done by the system and not the consumer of Sextant
                .Select(x => x.Page.BindingContext)
                .Cast<INavigated>()
                .Subscribe(navigated =>
                {
                    INavigationParameter navigationParameter = new NavigationParameter();
                    navigated
                        .WhenNavigatedFrom(navigationParameter)
                        .Subscribe()
                        .DisposeWith(BehaviorDisposable);

                    bindable
                        .CurrentPage
                        .BindingContext
                        .InvokeViewModelAction<INavigated>(x =>
                            x.WhenNavigatedTo(navigationParameter)
                                .Subscribe()
                                .DisposeWith(BehaviorDisposable));

                    navigated
                        .InvokeViewModelAction<IDestructible>(x => x.Destroy());
                })
                .DisposeWith(BehaviorDisposable);

            base.OnAttachedTo(bindable);
        }

        private void NavigationPage_Popped(object sender, NavigationEventArgs e)
        {
            if (PageNavigationService.NavigationSource == PageNavigationSource.Device)
            {
                PageUtilities.HandleSystemGoBack(e.Page, AssociatedObject.CurrentPage);
            }
        }
    }
}
