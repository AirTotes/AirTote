using AirTote.Components.Maps;
using AirTote.Models;

namespace AirTote.Pages.PayLandingFee
{
	public partial class SelectAirport : ContentPage
	{
		const double DEFAULT_CENTER_LATITUDE = 35.5469298;
		const double DEFAULT_CENTER_LONGITUDE = 139.7719668;

		public SelectAirport(IContainsAirportInfo? airportInfo)
		{
			var latlng = airportInfo?.AirportInfo?.coordinates
				?? new() { latitude = DEFAULT_CENTER_LATITUDE, longitude = DEFAULT_CENTER_LONGITUDE };

			//PageHost.SetIsGestureEnabled(typeof(SelectAirport), false);

			AirportMap map = new(latlng.longitude, latlng.latitude);
			map.AirportSelected += async (_, e) =>
			{
				if (airportInfo is not null)
					airportInfo.AirportInfo = e.SelectedAP;
				await Navigation.PopAsync();
			};

			Content = map;

			Title = "Please Select Airport";
		}
	}
}

