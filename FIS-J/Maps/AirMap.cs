﻿using FIS_J.Models;

using Mapsui;
using Mapsui.Extensions;
using Mapsui.Projections;
using Mapsui.Tiling;
using Mapsui.UI.Maui;
using Mapsui.Utilities;

namespace FIS_J.Maps
{
	public class AirMap : MapView
	{
		const double DEFAULT_CENTER_LATITUDE = 35.5469298;
		const double DEFAULT_CENTER_LONGITUDE = 139.7719668;

		public AirMap() : this(DEFAULT_CENTER_LONGITUDE, DEFAULT_CENTER_LATITUDE) { }

		public AirMap(double longitude, double latitude)
		{
			Init(longitude, latitude);
		}

		private void Init(double longitude, double latitude)
		{
			if (Map is null)
				throw new NullReferenceException("Cannot use NULL Map");

			Map.Layers.Add(TileProvider.CreateLayer());

			Map.Widgets.Add(new TileLicenseWidget());
			Renderer.WidgetRenders[typeof(TileLicenseWidget)] = new TileLicenseWidgetRenderer();

			var reso = Map.Resolutions[Math.Min(Map.Resolutions.Count - 1, 9)];
			Map.Home = v => v.NavigateTo(SphericalMercator.FromLonLat(longitude, latitude).ToMPoint(), reso);

			IsMyLocationButtonVisible = false;
			UnSnapRotationDegrees = 15;
			ReSnapRotationDegrees = 5;
			MyLocationEnabled = false;
			MyLocationFollow = false;
			MyLocationLayer.Enabled = false;

			UniqueCallout = true;

			// MapのPinやCallout以外をタップされた時に、開いているCalloutを閉じる
			Info += (s, e) =>
			{
				if (s is not MapView map || map.SelectedPin is not Pin pin)
					return;

				pin.HideCallout();
				e.Handled = true;
			};
		}

		public void MoveTo(AirportInfo.LatLng latlng)
			=> MoveTo(longitude: latlng.longitude, latitude: latlng.latitude);
		public void MoveTo(in Position latlng)
			=> MoveTo(longitude: latlng.Longitude, latitude: latlng.Latitude);
		public void MoveTo(in double longitude, in double latitude)
			=> Navigator?.CenterOn(SphericalMercator.FromLonLat(longitude, latitude).ToMPoint(), 250, Mapsui.Utilities.Easing.SinInOut);
	}
}

