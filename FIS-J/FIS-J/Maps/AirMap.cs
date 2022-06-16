using System;

using FIS_J.Models;

using Mapsui;
using Mapsui.Extensions;
using Mapsui.Projections;
using Mapsui.Tiling;
using Mapsui.UI.Forms;
using Mapsui.Utilities;

namespace FIS_J.Maps
{
	public class AirMap : MapView
	{
		const double DEFAULT_CENTER_LATITUDE = 35.5469298;
		const double DEFAULT_CENTER_LONGITUDE = 139.7719668;

		public AirMap()
		{
			Init(DEFAULT_CENTER_LONGITUDE, DEFAULT_CENTER_LATITUDE);
		}

		public AirMap(double longitude, double latitude)
		{
			Init(longitude, latitude);
		}

		private void Init(double longitude, double latitude)
		{
			Map.Layers.Add(OpenStreetMap.CreateTileLayer());

			var reso = Map.Resolutions[Math.Min(Map.Resolutions.Count - 1, 9)];
			Map.Home = v => v.NavigateTo(SphericalMercator.FromLonLat(longitude, latitude).ToMPoint(), reso);

			IsMyLocationButtonVisible = false;
			UnSnapRotationDegrees = 15;
			ReSnapRotationDegrees = 5;
			MyLocationEnabled = false;
			MyLocationFollow = false;
			MyLocationLayer.Enabled = false;

			UniqueCallout = true;
		}

		public void MoveTo(AirportInfo.LatLng latlng)
			=> MoveTo(longitude: latlng.longitude, latitude: latlng.latitude);
		public void MoveTo(in Position latlng)
			=> MoveTo(longitude: latlng.Longitude, latitude: latlng.Latitude);
		public void MoveTo(in double longitude, in double latitude)
			=> Navigator.CenterOn(SphericalMercator.FromLonLat(longitude, latitude).ToMPoint(), 250, Easing.SinInOut);
	}
}

