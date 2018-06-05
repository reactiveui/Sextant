using System;

namespace Sextant.Tests.Navigation
{
    internal static class ViewStackServiceFixtureExtensions
    {
        public static ViewStackServiceFixture WithModalStack(this ViewStackServiceFixture viewStackServiceFixture)
        {
            viewStackServiceFixture.ViewStackService.PushModal(viewStackServiceFixture.ModalViewModel, viewStackServiceFixture.ModalViewModel.Id).Subscribe();
            return viewStackServiceFixture;
        }

        public static ViewStackServiceFixture WithPageStack(this ViewStackServiceFixture viewStackServiceFixture)
        {
            viewStackServiceFixture.ViewStackService.PushPage(viewStackServiceFixture.PageViewModel, viewStackServiceFixture.PageViewModel.Id).Subscribe();
            return viewStackServiceFixture;
        }
    }
}