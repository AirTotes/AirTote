﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using FIS_J.Maps;
using FIS_J.Models;

using Mapsui.Styles;
using Mapsui.UI.Forms;

using Xamarin.Forms;

namespace FIS_J.FISJ.PayLandingFee
{
	public partial class SelectAirport : ContentPage
	{
		const double DEFAULT_CENTER_LATITUDE = 35.5469298;
		const double DEFAULT_CENTER_LONGITUDE = 139.7719668;

		const string AP_ICON_SVG = "FIS_J.Assets.MapIcons.flight.svg";

		IContainsAirportInfo airportInfo { get; } = null;
		Dictionary<string, AirportInfo.APInfo> StationsDic { get; set; } = null;

		AirMap map { get; }

		private Pin SelectedPin { get; set; }
		private Pin _SelectedPin { get; set; }

		public SelectAirport(IContainsAirportInfo airportInfo)
		{
			this.airportInfo = airportInfo;

			var latlng = airportInfo?.AirportInfo?.coordinates
				?? new() { latitude = DEFAULT_CENTER_LATITUDE, longitude = DEFAULT_CENTER_LONGITUDE };

			map = new(latlng.longitude, latlng.latitude);

			map.Map.Layers.Add(LatLngLayerGenerator.Generate());

			map.MapClicked += OnMapClicked;

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

				pin.Callout.Anchor = new(0, pin.Height * pin.Scale);
				pin.Callout.RectRadius = 5;
				pin.Callout.ArrowHeight = 8;
				pin.Callout.ArrowWidth = 24;
				pin.Callout.ArrowAlignment = ArrowAlignment.Top;
				pin.Callout.ArrowPosition = 1;
				pin.Callout.BackgroundColor = Xamarin.Forms.Color.White;
				pin.Callout.TitleFontSize = 16;
				pin.Callout.SubtitleFontSize = 12;

				pin.Callout.CalloutClicked += Callout_CalloutClicked;

				pin.Callout.Type = CalloutType.Detail;

				map.Pins.Add(pin);
			}
		}

		private async void Callout_CalloutClicked(object sender, CalloutClickedEventArgs e)
		{
			e.Callout.Pin.HideCallout();
			if (!StationsDic.TryGetValue(e.Callout.Pin.Label, out AirportInfo.APInfo apinfo) || apinfo is null)
				return;

			airportInfo.AirportInfo = apinfo;
			await Navigation.PopAsync();
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

			if (!pin.IsCalloutVisible())
			{
				pin.ShowCallout();
				SelectedPin = pin;
				map.MoveTo(pin.Position);
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

