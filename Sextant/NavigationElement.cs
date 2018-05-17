using System;
using System.Diagnostics;

namespace Sextant
{
	[DebuggerDisplay("{ViewType.Name, ViewModelType.Name}")]
	public sealed class NavigationElement
    {
        public Type NavigationViewType
        {
            get;
            private set;
        }

        public Type NavigationViewModelType
        {
            get;
            private set;
        }

        public Type ViewType
        {
            get;
            private set;
        }

        public Type ViewModelType
        {
            get;
            private set;
        }

        public Func<object> ViewCreationFunc
        {
            get;
            private set;
        }

        public Func<object> ViewModelCreationFunc
        {
            get;
            private set;
        }

		public NavigationElement(Type viewType, Type viewModelType, Type navigationViewType, Type navigationViewModelType, Func<object> viewModelCreationFunc = null, Func<object> viewCreationFunc = null)
        {
            NavigationViewType = navigationViewType;
            NavigationViewModelType = navigationViewModelType;
            ViewType = viewType;
            ViewModelType = viewModelType;
            ViewModelCreationFunc = viewModelCreationFunc;
            ViewCreationFunc = viewCreationFunc;
        }

        public NavigationElement(Type viewType, Type viewModelType, Func<object> viewModelCreationFunc = null)
			: this(viewType, viewModelType, null, null, viewModelCreationFunc, null)
        {
        }

        public NavigationElement(Type viewModelType, Type navigationViewType, Type navigationViewModelType, Func<object> viewCreationFunc = null)
            : this(null, viewModelType, navigationViewType, navigationViewModelType, null, viewCreationFunc)
        {
        }
    }
}