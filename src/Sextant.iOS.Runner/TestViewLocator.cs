using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using ReactiveUI;
using UIKit;

namespace Sextant.IOS.Runner
{
    internal class TestViewLocator
        : IViewLocator
    {
        public IViewFor ResolveView<T>(T viewModel, string contract = null)
            where T : class
        {
            return new PageUiViewController();
        }
    }
}
