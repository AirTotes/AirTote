using System;

namespace FIS_J.FISJ
{
	public partial class SubmitReport : ContentPage
	{
		public SubmitReport()
		{
			InitializeComponent();
		}

		private void FlightPlan_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new FlightPlan());
		}

		private void AirportUse_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new AirportUse());
		}
	}
}