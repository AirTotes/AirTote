using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using FIS_J.Models;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace FIS_J.FISJ.PayLandingFee
{
	public partial class SelectAirport : ContentPage
	{
		const double DEFAULT_CENTER_LATITUDE = 35.5469298;
		const double DEFAULT_CENTER_LONGITUDE = 139.7719668;

		IContainsAirportInfo airportInfo { get; } = null;
		Dictionary<string, AirportInfo.APInfo> StationsDic { get; set; } = null;

		Map map { get; }

		public SelectAirport(IContainsAirportInfo airportInfo)
		{
			this.airportInfo = airportInfo;

			map = new();

			if (airportInfo?.AirportInfo?.coordinates is null)
				map.InitialCameraUpdate = CameraUpdateFactory.NewPosition(new(DEFAULT_CENTER_LATITUDE, DEFAULT_CENTER_LONGITUDE));
			else
			{
				var latlng = airportInfo.AirportInfo.coordinates;
				map.InitialCameraUpdate = CameraUpdateFactory.NewPosition(new(latlng.latitude, latlng.longitude));
			}

			Appearing += SelectAirport_Appearing;

			Content = map;

			Title = "Please Select Airport";
		}

		private async void SelectAirport_Appearing(object sender, EventArgs e)
		{
			await SetAirportPins();
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

				map.InfoWindowClicked += Pin_InfoWindowClicked;

				map.Pins.Add(pin);
			}
		}

		private async void Pin_InfoWindowClicked(object sender, InfoWindowClickedEventArgs e)
		{
			if (e.Pin is not Pin pin)
				return;

			if (StationsDic.TryGetValue(pin.Label, out AirportInfo.APInfo value) && value is not null)
			{
				airportInfo.AirportInfo = value;
				await Navigation.PopAsync();
			}
		}
	}
}

