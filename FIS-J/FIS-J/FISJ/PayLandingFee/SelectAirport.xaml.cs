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
		Dictionary<string, AirportInfo.APInfo> StationsDic { get; } = new();

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

		private async Task SetAirportPins()
		{
			var dic = await AirportInfo.getAPInfoDic();
			foreach (var ap in dic.Values)
			{
				StationsDic.Add(ap.icao, ap);
				Pin pin = new()
				{
					Address = ap.name,
					Label = ap.icao,
					Type = PinType.SearchResult,
					Position = new(ap.coordinates.latitude, ap.coordinates.longitude)
				};

				pin.InfoWindowClicked += Pin_InfoWindowClicked;

				map.Pins.Add(pin);
			}
		}

		private async void Pin_InfoWindowClicked(object sender, PinClickedEventArgs e)
		{
			if (sender is not Pin pin)
				return;

			if (StationsDic.TryGetValue(pin.Label, out AirportInfo.APInfo value) && value is not null)
			{
				if (CalcFeeViewModel is null)
					await Navigation.PushAsync(new CalcFee(value));
				else
				{
					CalcFeeViewModel.AirportInfo = value;
					await Navigation.PopAsync();
				}
			}
		}
	}
}

