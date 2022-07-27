using FIS_J.Pages.PayLandingFee;
using FIS_J.ViewModels.ReservePages;

using System;

namespace FIS_J.Pages.ReservePages
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
