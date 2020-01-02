using System;
using System.Reactive;
using ReactiveUI;
using Sextant;

namespace SextantSample.Core.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject, IViewModel
    {
        protected readonly IViewStackService ViewStackService;

        protected ViewModelBase(IViewStackService viewStackService)
        {
            ViewStackService = viewStackService;
        }

        public virtual string Id { get; }
    }
}
