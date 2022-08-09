using AirTote.Components.Maps;
using AirTote.Pages.PayLandingFee;
using AirTote.ViewModels.ReservePages;

using CommunityToolkit.Maui.Views;

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
			await this.ShowPopupAsync(new SelectAirport(viewModel));
		}
	}
}
