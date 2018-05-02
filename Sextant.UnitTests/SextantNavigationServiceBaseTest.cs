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
			m_navigationService = new MockedSextantNavigationService(Application.Current, false)
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

			m_navigationService.RegisterPage(() => new FirstViewModel(), () => new FirstView());
			//m_navigationService.RegisterPage(() => vm.Object, () => new SecondView());
			m_navigationService.RegisterPage(() => new SecondViewModel(), () => new SecondView());
			//var a = new SecondViewModel();
			//m_navigationService.RegisterPage(() => a, () => new SecondView());

			m_navigationService.RegisterNavigationPage(() => SextantCore.Instance.GetPage<FirstViewModel>(),
													   () => new FirstNavigationViewModel(),
													   page => new FirstNavigationView(page as Page));

			m_navigationService.RegisterNavigationPage(() => SextantCore.Instance.GetPage<SecondViewModel>(),
													   () => new SecondNavigationViewModel(),
													   page => new SecondNavigationView(page as Page));


			Application.Current.MainPage = SextantCore.Instance.GetPage<FirstNavigationViewModel>() as NavigationPage;
			var firstvm = Application.Current.MainPage.BindingContext as FirstNavigationViewModel;

			await firstvm.PushModalPageAsNewInstanceAsync<SecondNavigationViewModel, SecondViewModel>();

			vm.Verify(x => x.VoidConctructorMethod(), Times.Once);
			Assert.AreEqual(1, i);
		}
	}
}
