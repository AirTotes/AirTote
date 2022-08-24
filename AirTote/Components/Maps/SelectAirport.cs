using AirTote.Components.Maps;
using AirTote.Interfaces;

using CommunityToolkit.Maui.Views;

namespace AirTote.Components.Maps;

public class SelectAirport : Popup
{
	const double DEFAULT_CENTER_LATITUDE = 35.5469298;
	const double DEFAULT_CENTER_LONGITUDE = 139.7719668;

	public SelectAirport(IContainsAirportInfo? airportInfo)
	{
		var latlng = airportInfo?.AirportInfo?.coordinates
			?? new() { latitude = DEFAULT_CENTER_LATITUDE, longitude = DEFAULT_CENTER_LONGITUDE };

		this.Size = new(
			Shell.Current.Width * 0.8,
			Shell.Current.Height * 0.9
		);

		AirportMap map = new(latlng.longitude, latlng.latitude);

		GradientStop gradientStopGen(double offset, byte alpha)
		{
			GradientStop ret = new(Colors.Transparent, (float)offset);

			ret.SetAppThemeColor(
				GradientStop.ColorProperty,
				Colors.White.WithAlpha(alpha),
				Colors.Black.WithAlpha(alpha)
			);

			return ret;
		}

		LinearGradientBrush labelBrush = new(
			new()
			{
				gradientStopGen(0.0, 0),
				gradientStopGen(0.2, 0xFF),
				gradientStopGen(0.8, 0xFF),
				gradientStopGen(1.0, 0),
			},
			new(0, 0),
			new(1, 0)
		);

		Label label = new()
		{
			Text = "Please Select Airport",
			VerticalOptions = LayoutOptions.Start,
			HorizontalOptions = LayoutOptions.Center,
			VerticalTextAlignment = TextAlignment.Center,
			Background = labelBrush,
			Padding = new(32, 4)
		};

		map.AirportSelected += (_, e) =>
		{
			if (airportInfo is not null)
				airportInfo.AirportInfo = e.SelectedAP;

			this.Close(e.SelectedAP);
		};

		Content = new Grid()
		{
			map,
			label,
		};
	}
}

