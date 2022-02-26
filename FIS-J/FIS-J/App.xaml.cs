using FIS_J.Services;
using FIS_J.Views;

using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIS_J
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			DependencyService.Register<MockDataStore>();
			MainPage = new AppShell();
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
