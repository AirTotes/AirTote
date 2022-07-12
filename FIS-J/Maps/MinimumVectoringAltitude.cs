using FIS_J.Services;
using Mapsui.Layers;
using Mapsui.Nts.Extensions;
using Mapsui.Providers;
using Mapsui.Styles;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace FIS_J.Maps;

public static class MinimumVectoringAltitude
{
	const string SERVER_URL = @"http://192.168.63.22:8080";

	public static Layer CreateLayer()
	{
		return new Layer("MinimumVectoringAltitude")
		{
			DataSource = null,
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

	public static async Task<MemoryProvider> GetProvider(bool isLocal = false)
		=> new((await (isLocal ? CreateLocalPolygon() : CreatePolygon())).ToFeatures());

	public static async Task<List<Geometry>> CreatePolygon()
	{
		var result = new List<Geometry>();

		using var stream = await HttpService.HttpClient.GetStreamAsync(SERVER_URL + "/RJTT.txt");

		var polygon = new WKTReader().Read(stream);
		result.Add(polygon);

		return result;
	}

	public static async Task<List<Geometry>> CreateLocalPolygon()
	{
		var result = new List<Geometry>();

		using var stream = await FileSystem.OpenAppPackageFileAsync("RJTT.txt");

		var polygon = new WKTReader().Read(stream);
		result.Add(polygon);

		return result;
	}
}

