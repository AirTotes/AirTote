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
		IContainsAirportInfo airportInfo { get; } = null;
		Dictionary<string, AirportInfo.APInfo> StationsDic { get; set; } = null;

		public SelectAirport()
		{
			InitializeComponent();
			BindingContext = viewModel;

			SetAirportPins();
		}

		public SelectAirport(IContainsAirportInfo airportInfo)
		{
			InitializeComponent();
			BindingContext = viewModel;
			this.airportInfo = airportInfo;

			SetAirportPins();
		}

		private async Task SetAirportPins()
		{
			StationsDic ??= await AirportInfo.getAPInfoDic();
			map.Pins.Clear();

			foreach (var ap in StationsDic.Values)
			{
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
				airportInfo.AirportInfo = value;
				await Navigation.PopAsync();
			}
		}
	}
}

