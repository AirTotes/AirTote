using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using FIS_J.Models;

using Mapsui.Projection;
using Mapsui.Rendering.Skia;
using Mapsui.UI.Forms;
using Mapsui.Utilities;

using Xamarin.Forms;

namespace FIS_J.FISJ.PayLandingFee
{
	public partial class SelectAirport : ContentPage
	{
		const double DEFAULT_CENTER_LATITUDE = 35.5469298;
		const double DEFAULT_CENTER_LONGITUDE = 139.7719668;

		const string AP_ICON_SVG = "FIS_J.Assets.MapIcons.flight.svg";

		const double EARTH_RADIUS_M = 6378137;

		IContainsAirportInfo airportInfo { get; } = null;
		Dictionary<string, AirportInfo.APInfo> StationsDic { get; set; } = null;

		MapView map { get; }

		private Pin SelectedPin { get; set; }
		private Pin _SelectedPin { get; set; }

		public SelectAirport(IContainsAirportInfo airportInfo)
		{
			this.airportInfo = airportInfo;

			map = new();
			map.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
			map.IsMyLocationButtonVisible = false;
			map.MyLocationLayer.Enabled = false;
			map.UnSnapRotationDegrees = 15;
			map.ReSnapRotationDegrees = 5;

			map.MapClicked += OnMapClicked;

			var latlng = airportInfo?.AirportInfo?.coordinates
				?? new() { latitude = DEFAULT_CENTER_LATITUDE, longitude = DEFAULT_CENTER_LONGITUDE };

			var reso = map.Map.Resolutions[Math.Min(map.Map.Resolutions.Count - 1, 9)];

			map.Map.Home = n => n.NavigateTo(SphericalMercator.FromLonLat(latlng.longitude, latlng.latitude), reso);

			Appearing += SelectAirport_Appearing;
			Disappearing += SelectAirport_Disappearing;

			Content = map;

			Title = "Please Select Airport";
		}

		private void SelectAirport_Disappearing(object sender, EventArgs e)
		{
			map.PinClicked -= OnPinClicked;
		}

		private async void SelectAirport_Appearing(object sender, EventArgs e)
		{
			await SetAirportPins();
			map.PinClicked += OnPinClicked;

			map.IsVisible = true;
			map.Refresh();
		}

		private async Task SetAirportPins()
		{
			StationsDic ??= await AirportInfo.getAPInfoDic();
			map.Pins.Clear();

			string svg_str = "";
			using (var stream = typeof(SelectAirport).GetTypeInfo().Assembly.GetManifestResourceStream(AP_ICON_SVG))
			using (var reader = new StreamReader(stream))
				svg_str = await reader.ReadToEndAsync();

			foreach (var ap in StationsDic.Values)
			{
				Pin pin = new(map)
				{
					Address = ap.name,
					Label = ap.icao,
					Type = PinType.Svg,

					Position = new(ap.coordinates.latitude, ap.coordinates.longitude),

					Scale = 0.5f,

					Svg = svg_str,
				};

				pin.Callout.Anchor = new Point(0, pin.Height * pin.Scale);
				pin.Callout.RectRadius = 5;
				pin.Callout.ArrowHeight = 8;
				pin.Callout.ArrowWidth = 24;
				pin.Callout.ArrowAlignment = ArrowAlignment.Top;
				pin.Callout.ArrowPosition = 1;
				pin.Callout.BackgroundColor = Color.White;
				pin.Callout.TitleFontSize = 16;
				pin.Callout.SubtitleFontSize = 12;

				pin.Callout.Type = CalloutType.Detail;

				map.Pins.Add(pin);
			}
		}

		private async void OnPinClicked(object sender, PinClickedEventArgs e)
		{
			var pin = e.Pin;
			if (pin is null)
				return;

			if (pin != SelectedPin)
			{
				SelectedPin?.HideCallout();
				SelectedPin = null;
			}
			if (pin != _SelectedPin)
			{
				_SelectedPin?.HideCallout();
				_SelectedPin = null;
			}

			e.Handled = true;

			if (!StationsDic.TryGetValue(pin.Label, out AirportInfo.APInfo apinfo) || apinfo is null)
				return;

			if (!e.Pin.IsCalloutVisible())
			{
				pin.ShowCallout();
				SelectedPin = pin;
				map.Navigator.CenterOn(SphericalMercator.FromLonLat(apinfo.coordinates.longitude, apinfo.coordinates.latitude));
				return;
			}
			else
			{
				airportInfo.AirportInfo = apinfo;
				await Navigation.PopAsync();
			}
		}

		private void OnMapClicked(object sender, MapClickedEventArgs e)
		{
			_SelectedPin = SelectedPin;
			SelectedPin?.HideCallout();
			SelectedPin = null;
		}
	}
}

