using System;

namespace FIS_J.Pages
{
	public partial class weekly : ContentPage
	{
		public weekly()
		{
			InitializeComponent();
		}

		private void Weeklyensembleforecast_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new weeklyensembleforecast());
		}

		private void Weeklyensemble_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new weeklyensemble());
		}

		private void Weeklysuppoert_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new weeklysuppoert());
		}
	}
}