using AirTote.Services.Types;

using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;

namespace AirTote.Components.Maps.Layers;

public partial class AirRouteLayer
{
	private class PointObject : PointFeature
	{
		CalloutStyle CalloutStyle { get; } = new()
		{
			TitleFont = {
				Size = 12,
				Bold = true
			},
			RectRadius = 10,
			ShadowWidth = 4,
			Enabled = false,
			MaxWidth = 240,
			TitleFontColor = Mapsui.Styles.Color.Black,
		};
		VectorStyle IconStyle { get; } = new();

		public PointInfo PtInfo { get; }

		public PointObject(PointInfo ptInfo)
			: base(SphericalMercator.FromLonLat((double?)ptInfo.Longitude_Deg ?? double.NaN, (double?)ptInfo.Latitude_Deg ?? double.NaN).ToMPoint())
		{
			PtInfo = ptInfo;

			Styles.Add(this.IconStyle);
			Styles.Add(this.CalloutStyle);

			this.CalloutStyle.Title = ptInfo.Name;
		}
	}
}
