using Mapsui.Layers;
using Mapsui.Nts.Extensions;
using Mapsui.Providers;
using Mapsui.Styles;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace FIS_J.Maps;

public static class MinimumVectoringAltitude
{
	public static ILayer CreateLayer()
	{
		return new Layer("MinimumVectoringAltitude")
		{
			DataSource = GetProvider(),
			Style = new VectorStyle
			{
				Fill = null,
				Outline = new Pen
				{
					Color = Mapsui.Styles.Color.Black,
					Width = 1,
				}
			}
		};
	}

	public static MemoryProvider GetProvider()
		=> new(CreatePolygon().ToFeatures());

	private static List<Geometry> CreatePolygon()
	{
		var result = new List<Geometry>();

		//var polygon = new WKTReader().Read("");
		//result.Add(polygon);

		return result;
	}
}

