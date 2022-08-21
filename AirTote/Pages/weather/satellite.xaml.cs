using System;

namespace AirTote.Pages
{
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