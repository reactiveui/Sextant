using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sextant.UnitTests.MockedApp;
using Sextant.UnitTests.MockedApp.ViewModels;
using Sextant.UnitTests.MockedApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;

namespace Sextant.UnitTests
{
	[TestFixture]
	public class SextantNavigationServiceBaseTest
	{
		MockedSextantNavigationService m_navigationService;

		[SetUp]
		public void Setup()
		{
			MockForms.Init();
			var app = new App();
			m_navigationService = new MockedSextantNavigationService()
			{
				Logger = new BaseLogger()
			};
			SextantCore.SetCurrentFactory(m_navigationService);

		}

		[Test]
		public async Task RegisterNavigationPage_WithRegisteredPage_OnlyOneInstace()
		{
			var vm = new Mock<SecondViewModel>()
			{
				DefaultValue = DefaultValue.Mock
			};

			int i = 0;

			vm.Setup(x => x.VoidConctructorMethod()).Callback(() => i++);

			m_navigationService.RegisterPage<FirstView, FirstViewModel, FirstNavigationView, FirstNavigationViewModel>();
			m_navigationService.RegisterPage<SecondView, SecondViewModel, SecondNavigationView, SecondNavigationViewModel>();


			Application.Current.MainPage = SextantCore.Instance.GetPage<FirstNavigationViewModel>() as NavigationPage;
			var firstvm = Application.Current.MainPage.BindingContext as FirstNavigationViewModel;

			await firstvm.PushModalPageAsync<SecondNavigationViewModel, SecondViewModel>();

			vm.Verify(x => x.VoidConctructorMethod(), Times.Once);
			Assert.AreEqual(1, i);
		}
	}
}
