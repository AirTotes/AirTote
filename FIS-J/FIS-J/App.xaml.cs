using Xamarin.Forms;

namespace FIS_J
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			MainPage = new FIS_J.FISJ.MainPagexaml();
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
