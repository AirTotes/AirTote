using System;

namespace AirTote.Pages
{
	public partial class SkyInformation : ContentPage
	{
		public SkyInformation()
		{
			InitializeComponent();
		}

		private void Badwetherforecast_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Badwetherforecast());
		}
		private void lowerbadwether_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new lowerbadwether());
		}
		private void signmet_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new signmet());
		}
		private void Internationalfukuoka_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Internationalfukuoka());
		}
		private void InternationalDomestic_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new InternationalDomestic());
		}

	}
}