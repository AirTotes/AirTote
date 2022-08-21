using System;

namespace AirTote.Pages
{
	public partial class WetherInformation : ContentPage
	{
		public WetherInformation()
		{
			InitializeComponent();
		}

		private void Windy_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new weather.Windy());
		}

		private void Nomwether_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Nomwether());
		}

		private void Upperwether_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Upperwether());
		}

		private void SkyInformation_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new SkyInformation());
		}

		private void live_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new live());
		}

		private void weekly_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new weekly());
		}

		private void longterm_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new longterm());
		}

		private void satellite_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new satellite());
		}

		private void emagram_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new emagram());
		}
	}
}
