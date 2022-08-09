using AirTote.Components.Maps;
using AirTote.Models;
using AirTote.Services;
using AirTote.ViewModels.PayLandingFee;

namespace AirTote.Pages.PayLandingFee
{
	public partial class CalcFee : ContentPage
	{
		CalcFeeViewModel viewModel { get; } = new();

		public CalcFee()
		{
			InitializeComponent();
			BindingContext = viewModel;

			viewModel.AirportInfo = new()
			{
				icao = "RJTT"
			};
		}

		public CalcFee(AirportInfo.APInfo station)
		{
			InitializeComponent();
			viewModel.AirportInfo = station;
			BindingContext = viewModel;
		}

		public CalcFee(CalcFeeViewModel viewModel)
		{
			InitializeComponent();
			this.viewModel = viewModel;
			BindingContext = viewModel;
		}

		async void Button_Clicked(object sender, System.EventArgs e)
		{
			await Navigation.PushAsync(new SelectAirport(viewModel));
		}
	}
}
