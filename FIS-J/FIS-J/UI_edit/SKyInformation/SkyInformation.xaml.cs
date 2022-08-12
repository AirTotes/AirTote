using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIS_J.FISJ
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
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