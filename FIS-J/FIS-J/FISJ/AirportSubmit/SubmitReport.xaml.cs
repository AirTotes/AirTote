using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIS_J.FISJ
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
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