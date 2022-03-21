using FIS_J.Services;
using FIS_J.ViewModels.PayLandingFee;
using Xamarin.Forms;

namespace FIS_J.FISJ.PayLandingFee
{
	public partial class CalcFee : ContentPage
	{
		CalcFeeViewModel viewModel { get; } = new();

		public CalcFee()
		{
			InitializeComponent();
			BindingContext = viewModel;

			viewModel.Station = new()
			{
				icao = "RJTT"
			};
		}

		public CalcFee(AVWX.Station station)
		{
			InitializeComponent();
			viewModel.Station = station;
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
