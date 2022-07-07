﻿using FIS_J.Maps;
using FIS_J.Models;

namespace FIS_J.FISJ.PayLandingFee
{
	public partial class SelectAirport : ContentPage
	{
		const double DEFAULT_CENTER_LATITUDE = 35.5469298;
		const double DEFAULT_CENTER_LONGITUDE = 139.7719668;

		public SelectAirport(IContainsAirportInfo airportInfo)
		{
			var latlng = airportInfo?.AirportInfo?.coordinates
				?? new() { latitude = DEFAULT_CENTER_LATITUDE, longitude = DEFAULT_CENTER_LONGITUDE };

			AirportMap map = new(latlng.longitude, latlng.latitude);
			map.AirportSelected += async (_, e) =>
			{
				airportInfo.AirportInfo = e.SelectedAP;
				await Navigation.PopAsync();
			};

			Content = map;

			Title = "Please Select Airport";
		}
	}
}

