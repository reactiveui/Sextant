using System;
namespace Sextant.UnitTests.MockedApp.ViewModels
{
	public class FirstViewModel : IBaseNavigationPageModel
    {
        public FirstViewModel()
        {
            VoidConctructorMethod();
        }

        public virtual void VoidConctructorMethod()
        {
            // just to check the call son Mock
        }
    }
}
