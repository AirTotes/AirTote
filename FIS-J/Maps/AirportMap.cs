using FIS_J.Models;
using Mapsui.UI.Maui;

namespace FIS_J.Maps;

public record AirportSelectedEventArgs(AirportInfo.APInfo SelectedAP);

public class AirportMap : AirMap
{
	const string AP_ICON_SVG = "MapIcons/flight.svg";

	static Dictionary<string, AirportInfo.APInfo> StationsDic { get; set; } = null;

	public event EventHandler<AirportSelectedEventArgs> AirportSelected;
	public event EventHandler<AirportSelectedEventArgs> AirportClicked;

	public AirportMap(Func<AirportInfo.APInfo, IEnumerable<CalloutText>, IEnumerable<CalloutText>> setCalloutText = null)
	{
		Init(setCalloutText);
	}

	public AirportMap(double longitude, double latitude, Func<AirportInfo.APInfo, IEnumerable<CalloutText>, IEnumerable<CalloutText>> setCalloutText = null)
		: base(longitude, latitude)
	{
		Init(setCalloutText);
	}

	private async void Init(Func<AirportInfo.APInfo, IEnumerable<CalloutText>, IEnumerable<CalloutText>> setCalloutText)
	{
		Map.Layers.Add(LatLngLayerGenerator.Generate());
		PinClicked += OnPinClicked;

		StationsDic ??= await AirportInfo.getAPInfoDic();

		Pins.Clear();

		string svg_str = "";
		using (var reader = new StreamReader(await FileSystem.OpenAppPackageFileAsync(AP_ICON_SVG)))
			svg_str = await reader.ReadToEndAsync();

		foreach (AirportInfo.APInfo ap in StationsDic.Values)
		{
			CustomTextCalloutPin pin = new(this)
			{
				Address = ap.name,
				Label = ap.icao,
				Type = PinType.Svg,

				Position = new(ap.coordinates.latitude, ap.coordinates.longitude),

				Scale = 0.5f,

				Svg = svg_str,
			};

			pin.Callout.CalloutClicked += OnCalloutClicked;

			SetCalloutText(pin, ap, setCalloutText);

			Pins.Add(pin);
		}
	}

	public void SetCalloutText(Func<AirportInfo.APInfo, IEnumerable<CalloutText>, IEnumerable<CalloutText>> setCalloutText = null)
	{
		foreach (var pin in Pins)
		{
			if (pin is not CustomTextCalloutPin cpin)
				continue;

			if (!StationsDic.TryGetValue(pin.Label, out AirportInfo.APInfo apinfo) || apinfo is null)
				return;

			SetCalloutText(cpin, apinfo, setCalloutText);
		}
	}

	private void SetCalloutText(CustomTextCalloutPin pin, AirportInfo.APInfo ap, Func<AirportInfo.APInfo, IEnumerable<CalloutText>, IEnumerable<CalloutText>> setCalloutText)
	{
		using CalloutText baseText = new($"{ap.icao} ({ap.name})");
		CalloutText[] baseTextArr = new[] { baseText };

		IEnumerable<CalloutText> calloutTexts = setCalloutText?.Invoke(ap, baseTextArr) ?? baseTextArr;

		pin.SetCalloutText(calloutTexts);
	}

	private void OnPinClicked(object sender, PinClickedEventArgs e)
	{
		var pin = e.Pin;
		if (pin is null)
			return;

		MoveTo(pin.Position);

		e.Handled = true;

		if (!StationsDic.TryGetValue(pin.Label, out AirportInfo.APInfo apinfo) || apinfo is null)
			return;

		if (pin.IsCalloutVisible())
			// Callout表示中 <=> 既にPinがClickされている
			AirportSelected?.Invoke(this, new(apinfo));
		else
		{
			pin.ShowCallout();
			AirportClicked?.Invoke(this, new(apinfo));
		}
	}

	private void OnCalloutClicked(object sender, CalloutClickedEventArgs e)
	{
		e.Callout.Pin.HideCallout();
		if (!StationsDic.TryGetValue(e.Callout.Pin.Label, out AirportInfo.APInfo apinfo) || apinfo is null)
			return;

		AirportSelected?.Invoke(this, new(apinfo));
	}
}
