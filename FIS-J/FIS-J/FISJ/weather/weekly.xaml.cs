using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIS_J.FISJ
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
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