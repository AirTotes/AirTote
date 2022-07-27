using System.Linq;
using AirTote.Components.Maps.Layers;
using AirTote.Models;
using Mapsui.UI.Maui;
using Topten.RichTextKit;

using TCalloutTextGen = System.Func<AirTote.Models.AirportInfo.APInfo, Topten.RichTextKit.RichString, Topten.RichTextKit.RichString>;

namespace AirTote.Components.Maps;

public record AirportSelectedEventArgs(AirportInfo.APInfo SelectedAP);

public class AirportMap : AirMap
{
	const string AP_ICON_SVG = "MapIcons/flight.svg";

	static Dictionary<string, AirportInfo.APInfo> StationsDic { get; } = new();

	public event EventHandler<AirportSelectedEventArgs>? AirportSelected;
	public event EventHandler<AirportSelectedEventArgs>? AirportClicked;

	public AirportMap(Func<AirportInfo.APInfo, RichString, RichString>? setCalloutText = null)
	{
		Init(setCalloutText);
	}

	public AirportMap(double longitude, double latitude, TCalloutTextGen? setCalloutText = null)
		: base(longitude, latitude)
	{
		Init(setCalloutText);
	}

	private async void Init(TCalloutTextGen? setCalloutText)
	{
		Map?.Layers.Add(LatLngLayerGenerator.Generate());
		PinClicked += OnPinClicked;

		if (StationsDic.Count <= 0)
		{
			foreach (var kvp in await AirportInfo.getAPInfoDic())
				StationsDic[kvp.Key] = kvp.Value;
		}

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

				Anchor = new(0, 0),

				Svg = svg_str,
			};

			pin.Callout.CalloutClicked += OnCalloutClicked;

			SetCalloutText(pin, ap, setCalloutText);

			Pins.Add(pin);
		}
	}

	public void SetCalloutText(TCalloutTextGen? setCalloutText = null)
	{
		foreach (var pin in Pins)
		{
			if (pin is not CustomTextCalloutPin cpin)
				continue;

			if (!StationsDic.TryGetValue(pin.Label, out AirportInfo.APInfo? apinfo) || apinfo is null)
				return;

			SetCalloutText(cpin, apinfo, setCalloutText);
		}
	}

	private void SetCalloutText(CustomTextCalloutPin pin, AirportInfo.APInfo ap, TCalloutTextGen? setCalloutText)
	{
		RichString str = new();

		str.Alignment(Topten.RichTextKit.TextAlignment.Center)
			.Add($"{ap.icao} ({ap.name})", fontSize: 24, underline: UnderlineStyle.Solid)
			.MarginBottom(8);

		if (setCalloutText is not null)
		{
			str
				.Paragraph()
				.MarginBottom(0);

			str = setCalloutText(ap, str);
		}

		var tmpMaxWidth = (float)Math.Min(Height, Width) * 0.9f;
		str.MaxWidth = tmpMaxWidth < 0 ? 300 : tmpMaxWidth;

		pin.SetCalloutText(str);
	}

	private void OnPinClicked(object? sender, PinClickedEventArgs e)
	{
		var pin = e.Pin;
		if (pin is null)
			return;

		MoveTo(pin.Position);

		e.Handled = true;

		if (!StationsDic.TryGetValue(pin.Label, out AirportInfo.APInfo? apinfo) || apinfo is null)
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

	private void OnCalloutClicked(object? sender, CalloutClickedEventArgs e)
	{
		if (e.Callout is not Callout callout)
			return;

		callout.Pin.HideCallout();
		if (!StationsDic.TryGetValue(callout.Pin.Label, out AirportInfo.APInfo? apinfo) || apinfo is null)
			return;

		AirportSelected?.Invoke(this, new(apinfo));
	}
}
