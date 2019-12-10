﻿using System;
using ReactiveUI;
using Sextant;

namespace SextantSample.ViewModels
{
	public class FirstModalNavigationViewModel : ViewModelBase
	{
		public FirstModalNavigationViewModel(IViewStackService viewStackService)
            : base(viewStackService)
		{

		}
		public override string Id => nameof(FirstModalNavigationViewModel);
	}
}
