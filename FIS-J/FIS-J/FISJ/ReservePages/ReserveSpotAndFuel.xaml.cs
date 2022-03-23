using FIS_J.FISJ.PayLandingFee;
using FIS_J.ViewModels.ReservePages;

using System;

using Xamarin.Forms;

namespace FIS_J.FISJ.ReservePages
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
