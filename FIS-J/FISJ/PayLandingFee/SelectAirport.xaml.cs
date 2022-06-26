using FIS_J.Maps;
using FIS_J.Models;

using Mapsui.Styles;
using Mapsui.UI.Maui;
using Color = Microsoft.Maui.Graphics.Color;

namespace FIS_J.FISJ.PayLandingFee
{
	public partial class SelectAirport : ContentPage
	{
		const double DEFAULT_CENTER_LATITUDE = 35.5469298;
		const double DEFAULT_CENTER_LONGITUDE = 139.7719668;

		const string AP_ICON_SVG = "MapIcons/flight.svg";

		IContainsAirportInfo airportInfo { get; } = null;
		Dictionary<string, AirportInfo.APInfo> StationsDic { get; set; } = null;

		AirMap map { get; }

		public SelectAirport(IContainsAirportInfo airportInfo)
		{
			this.airportInfo = airportInfo;

			var latlng = airportInfo?.AirportInfo?.coordinates
				?? new() { latitude = DEFAULT_CENTER_LATITUDE, longitude = DEFAULT_CENTER_LONGITUDE };

			map = new(latlng.longitude, latlng.latitude);

			map.Map.Layers.Add(LatLngLayerGenerator.Generate());

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
			using (var reader = new StreamReader(await FileSystem.OpenAppPackageFileAsync(AP_ICON_SVG)))
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

				// アイコンは、マークの周りにEllipseを配置しているため、その分の高さを除いてあげる
				pin.Callout.Anchor = new(0, pin.Height * pin.Scale * 0.5);
				pin.Callout.RectRadius = 5;
				pin.Callout.ArrowHeight = 8;
				pin.Callout.ArrowWidth = 24;
				pin.Callout.ArrowAlignment = ArrowAlignment.Top;
				pin.Callout.ArrowPosition = 1;
				pin.Callout.BackgroundColor = Color.FromRgb(0xff, 0xff, 0xff);
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

			e.Handled = true;

			if (!StationsDic.TryGetValue(pin.Label, out AirportInfo.APInfo apinfo) || apinfo is null)
				return;

			if (!pin.IsCalloutVisible())
			{
				pin.ShowCallout();
				map.MoveTo(pin.Position);
				return;
			}
			else
			{
				airportInfo.AirportInfo = apinfo;
				await Navigation.PopAsync();
			}
		}
	}
}

