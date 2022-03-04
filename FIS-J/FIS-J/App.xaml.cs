using FIS_J.Services;

using Xamarin.Forms;

namespace FIS_J
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			DependencyService.Register<MockDataStore>();
			MainPage = new NavigationPage(new FIS_J.MainPagexaml());
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
