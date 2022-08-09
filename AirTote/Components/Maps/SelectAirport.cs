using AirTote.Components.Maps;
using AirTote.Models;

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

		map.AirportSelected += (_, e) =>
		{
			if (airportInfo is not null)
				airportInfo.AirportInfo = e.SelectedAP;

			this.Close(e.SelectedAP);
		};

		Content = map;
	}
}

