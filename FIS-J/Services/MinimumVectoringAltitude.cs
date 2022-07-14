using System.Text.Json;
using System.Text.Json.Serialization;

using FIS_J.Services;

using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using Mapsui.Providers;
using Mapsui.Styles;

using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace FIS_J.Services;

public static class MinimumVectoringAltitude
{
#if DEBUG
	const string SERVER_URL = @"http://192.168.63.22:8080";
#else
	const string SERVER_URL = @"https://fis-j.technotter.com/mva";
#endif

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

	public static Layer CreateTextLayer()
	{
		return new Layer("MinimumVectoringAltitudeText")
		{
			DataSource = null,
			Style = null
		};
	}

	public static async Task<MemoryProvider> GetProvider(bool isLocal = false)
		=> new((await (isLocal ? CreateLocalPolygon() : CreatePolygon())).ToFeatures());

	public static async Task<MemoryProvider> GetTextProvider(bool isLocal = false)
		=> new(await (isLocal ? CreateLocalTexts() : CreateTexts()));

	public static async Task<List<Geometry>> CreatePolygon()
	{
		var result = new List<Geometry>();

		using var stream = await HttpService.HttpClient.GetStreamAsync(SERVER_URL + "/mva.txt");

		var polygon = new WKTReader().Read(stream);
		result.Add(polygon);

		return result;
	}

	public static async Task<List<Geometry>> CreateLocalPolygon()
	{
		var result = new List<Geometry>();

		using var stream = await FileSystem.OpenAppPackageFileAsync("mva.txt");

		var polygon = new WKTReader().Read(stream);
		result.Add(polygon);

		return result;
	}

	class LabelTextsRecord
	{
		[JsonPropertyName("text")]
		public string Text { get; set; }

		[JsonPropertyName("lon")]
		public double? Lon { get; set; }

		[JsonPropertyName("lat")]
		public double? Lat { get; set; }

		[JsonPropertyName("x")]
		public double? X { get; set; }

		[JsonPropertyName("y")]
		public double? Y { get; set; }

		[JsonPropertyName("min-visible")]
		public double? MinVisible { get; set; }

		[JsonPropertyName("max-visible")]
		public double? MaxVisible { get; set; }
	}

	public static async Task<List<IFeature>> CreateTexts()
	{
		using var stream = await HttpService.HttpClient.GetStreamAsync(SERVER_URL + "/mva.text.json");

		return await CreateTexts(stream);
	}

	public static async Task<List<IFeature>> CreateLocalTexts()
	{
		using var stream = await FileSystem.OpenAppPackageFileAsync("mva.text.json");

		return await CreateTexts(stream);
	}

	private static async Task<List<IFeature>> CreateTexts(Stream stream)
	{
		List<IFeature> texts = new();

		var result = await JsonSerializer.DeserializeAsync(stream, typeof(Dictionary<string, LabelTextsRecord[]>));

		if (result is Dictionary<string, LabelTextsRecord[]> dic)
		{
			foreach (var arr in dic)
				foreach (var v in arr.Value)
					texts.Add(CreateLabelFeature(v));
		}

		return texts;
	}

	private static IFeature CreateLabelFeature(LabelTextsRecord d)
	{
		MPoint pt = new();

		if (d.X is not null && d.Y is not null)
		{
			pt.X = d.X ?? 0;
			pt.Y = d.Y ?? 0;
		}
		else if (d.Lon is not null && d.Lat is not null)
			pt = SphericalMercator.FromLonLat(d.Lon ?? 0, d.Lat ?? 0).ToMPoint();
		else
			throw new ArgumentNullException("X-Y or Lon-Lat", "one of X-Y and Lon-Lat pairs must contain value");

		return new PointFeature(pt)
		{
			Styles = new List<IStyle>()
			{
				new LabelStyle()
				{
					Text = d.Text,
					MinVisible = d.MinVisible ?? 0,
					MaxVisible = d.MaxVisible ?? double.MaxValue
				}
			}
		};
	}
}

