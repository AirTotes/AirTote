using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FIS_J.Models;
using FIS_J.Services;
using FIS_J.ViewModels.PayLandingFee;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FIS_J.FISJ.PayLandingFee
{
	public partial class SelectAirport : ContentPage
	{
		SelectAirportViewModel viewModel { get; } = new();
		CalcFeeViewModel CalcFeeViewModel { get; } = null;
		Dictionary<string, AVWX.Station> StationsDic { get; } = new();

		public SelectAirport()
		{
			InitializeComponent();
			BindingContext = viewModel;

			SetAirportPins();
		}

		public SelectAirport(CalcFeeViewModel calcFeeViewModel)
		{
			InitializeComponent();
			BindingContext = viewModel;
			CalcFeeViewModel = calcFeeViewModel;

			SetAirportPins();
		}

		private void SetAirportPins()
		{
			AVWX avwx = new("KQuqTZ1D1BfSsuXu4eN2lc3DnC46-tGsU-l023G6q0w");
			List<Task<AVWX.Station>> tasks = new();
			foreach (var code in ICAOCodes.Codes)
			{
				tasks.Add(avwx.GetStationInformation(code));
			}

			Task.Run(async () =>
			{
				var results = await Task.WhenAll(tasks);
				foreach (var result in results)
				{
					StationsDic.Add(result.icao, result);
					Pin pin = new()
					{
						Address = result.name,
						Label = result.icao,
						Type = PinType.SearchResult,
						Position = new(result.latitude, result.longitude)
					};

					pin.InfoWindowClicked += Pin_InfoWindowClicked;

					map.Pins.Add(pin);
				}
			});
		}

		private async void Pin_InfoWindowClicked(object sender, PinClickedEventArgs e)
		{
			if (sender is not Pin pin)
				return;

			if (StationsDic.TryGetValue(pin.Label, out AVWX.Station value) && value is not null)
			{
				if (CalcFeeViewModel is null)
					await Navigation.PushAsync(new CalcFee(value));
				else
				{
					CalcFeeViewModel.Station = value;
					await Navigation.PopAsync();
				}
			}
		}
	}
}

