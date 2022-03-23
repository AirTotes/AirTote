using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIS_J.FISJ
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class satellite : ContentPage
	{
		public satellite()
		{
			InitializeComponent();
		}

		private async void Japansatellite_Clicked_1(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("http://www.micosfit.jp/wakayama08/satellite/");
		}
	}
}