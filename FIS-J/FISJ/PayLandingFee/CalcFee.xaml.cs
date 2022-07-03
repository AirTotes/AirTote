using FIS_J.Models;
using FIS_J.Services;
using FIS_J.ViewModels.PayLandingFee;

namespace FIS_J.FISJ.PayLandingFee
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
