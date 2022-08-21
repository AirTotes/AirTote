using System;

namespace AirTote.Pages
{
	public partial class hirosima : ContentPage
	{
		public hirosima()
		{
			InitializeComponent();
		}

		private async void hirosimaAirportUse_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.hij.airport.jp/assets/files/operation/airport_usage.pdf?202203211957");
		}

		private async void landingdiscount_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.hij.airport.jp/assets/files/operation/operator_information.pdf?202203211957");
		}

		private async void PilotInformation_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.hij.airport.jp/assets/files/operation/operator_notification.pdf?202203211957");
		}
	}
}