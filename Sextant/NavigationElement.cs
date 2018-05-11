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
            set;
        }

        public Type NavigationViewModelType
        {
            get;
            set;
        }

        public Type ViewType
        {
            get;
            set;
        }

        public Type ViewModelType
        {
            get;
            set;
        }

        public Func<object> ViewCreationFunc
        {
            get;
            set;
        }

        public Func<object> ViewModelCreationFunc
        {
            get;
            set;
        }

		public NavigationElement(Type viewType, Type viewModelType, Type navigationViewType, Type navigationViewModelType, Func<object> viewModelCreationFunc = null)
        {
            NavigationViewType = navigationViewType;
            NavigationViewModelType = navigationViewModelType;
            ViewType = viewType;
            ViewModelType = viewModelType;
            ViewModelCreationFunc = viewModelCreationFunc;
        }

        public NavigationElement(Type viewType, Type viewModelType, Func<object> viewModelCreationFunc = null)
			: this(viewType, viewModelType, null, null, viewModelCreationFunc)
        {
        }
    }
}