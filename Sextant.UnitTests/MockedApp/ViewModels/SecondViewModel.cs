using System;
namespace Sextant.UnitTests.MockedApp.ViewModels
{
	public class SecondViewModel : IBaseNavigationPageModel
    {
        public SecondViewModel()
        {
            VoidConctructorMethod();
        }

        public virtual void VoidConctructorMethod()
        {
            // just to check the call son Mock
        }
    }
}
