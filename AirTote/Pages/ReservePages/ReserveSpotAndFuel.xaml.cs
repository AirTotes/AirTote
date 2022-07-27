using AirTote.Pages.PayLandingFee;
using AirTote.ViewModels.ReservePages;

using System;

namespace AirTote.Pages.ReservePages
{
	public partial class ReserveSpotAndFuel : ContentPage
	{
		ReserveSpotAndFuelViewModel viewModel { get; } = new();

		public ReserveSpotAndFuel()
		{
			InitializeComponent();
			BindingContext = viewModel;
		}

		private async void ShowAirportSelectPage(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new SelectAirport(viewModel));
		}
	}
}
