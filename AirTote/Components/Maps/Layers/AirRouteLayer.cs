using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.UI;

using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace AirTote.Components.Maps.Layers;

public partial class AirRouteLayer : MemoryLayer
{
	Mapsui.Map Map { get; }
	CalloutStyle? LastTapped { get; set; } = null;
	static VectorStyle LINE_STYLE { get; } = new()
	{
		Line = new()
		{
			Color = Mapsui.Styles.Color.Red,
			Width = 2
		}
	};

	public AirRouteLayer(Mapsui.Map map)
	{
		Map = map;

		Name = "Air Routes";
		IsMapInfoLayer = true;

		Map.Info += Map_OnInfo;

		Style = LINE_STYLE;
		Features = CreateLines();
	}

	/// <summary>FeatureがTapされたときに呼び出される</summary>
	/// <param name="s">呼び出し元</param>
	/// <param name="e">イベントに関する情報</param>
	void Map_OnInfo(object? s, MapInfoEventArgs e)
	{
		if (LastTapped is not null)
			LastTapped.Enabled = false;

		if (e.MapInfo?.Layer != this || e.MapInfo.Feature is null)
			return;

		if (e.MapInfo.Feature.Styles.Where(v => v is CalloutStyle).FirstOrDefault() is CalloutStyle calloutStyle && calloutStyle != LastTapped)
		{
			calloutStyle.Enabled = !calloutStyle.Enabled;
			LastTapped = calloutStyle;
		}
		else
			LastTapped = null;
	}

	static GeometryFeature[] CreateLines()
	{
		return new[]
		{
			CreateFeatureFromWKT("LINESTRING(135 35, 140 40)"),
			CreateFeatureFromWKT("LINESTRING(130 35, 140 40)"),
			CreateFeatureFromWKT("LINESTRING(140 35, 140 40)"),
		};
	}

	private static GeometryFeature CreateFeatureFromWKT(string wkt)
	{
		WKTReader reader = new();

		GeometryFeature feat = new LineString(
			reader
				.Read(wkt)
				.Coordinates
				.Select(v => SphericalMercator.FromLonLat(v.X, v.Y)
					.ToCoordinate()
				)
				.ToArray()
			).ToFeature();

		feat.Styles.Add(CreateCalloutStyle("SAMPLE TEXT"));

		return feat;
	}

	private static CalloutStyle CreateCalloutStyle(string? name)
	{
		return new CalloutStyle
		{
			Title = name,
			TitleFont =
			{
				FontFamily = null,
				Size = 12
			},
			TitleFontColor = Mapsui.Styles.Color.Gray,
			MaxWidth = 120,
			RectRadius = 10,
			ShadowWidth = 4,
			Enabled = false,
		};
	}
}

