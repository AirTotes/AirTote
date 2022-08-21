using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using Mapsui.Styles;

using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace AirTote.Services;

public static class MinimumVectoringAltitude
{
#if DEBUG
	const string SERVER_URL = @"http://192.168.63.23:8080";
#else
	const string SERVER_URL = @"https://fis-j.technotter.com/mva";
#endif
	const string OFFLINE_BASE_PATH = "";

	class MVASourceRecord
	{
		[JsonPropertyName("chart")]
		public string? Chart { get; set; }

		[JsonPropertyName("label")]
		public string? Label { get; set; }
	}

	static Dictionary<string, MVASourceRecord>? _OnlineSourceRecordDic { get; set; } = null;
	static Dictionary<string, MVASourceRecord>? _OfflineSourceRecordDic { get; set; } = null;
	static async Task<Dictionary<string, MVASourceRecord>?> GetSourceRecordDic(bool isOffline)
	{
		if (isOffline)
		{
			if (_OfflineSourceRecordDic is not null)
				return _OfflineSourceRecordDic;
		}
		else
		{
			if (_OnlineSourceRecordDic is not null)
				return _OnlineSourceRecordDic;
		}

		Dictionary<string, MVASourceRecord>? result;
		using (var stream = await GetAssetStreamAsync(isOffline, "mva.json"))
			result = await JsonSerializer.DeserializeAsync(stream, typeof(Dictionary<string, MVASourceRecord>)) as Dictionary<string, MVASourceRecord>;

		if (isOffline)
			_OfflineSourceRecordDic = result;
		else
			_OnlineSourceRecordDic = result;

		return result;
	}

	static WKTReader WKTReader { get; } = new();
	static WKBReader WKBReader { get; } = new();

	static Task<Stream> GetAssetStreamAsync(bool isOffline, string fileName)
		=> isOffline ? FileSystem.OpenAppPackageFileAsync(Path.Combine(OFFLINE_BASE_PATH, fileName))
		: HttpService.HttpClient.GetStreamAsync($"{SERVER_URL}/{fileName}");
	static Task<Stream> GetAssetStreamAsync(bool isOffline, string fileName, CancellationToken cToken)
		=> isOffline ? FileSystem.OpenAppPackageFileAsync(Path.Combine(OFFLINE_BASE_PATH, fileName))
		: HttpService.HttpClient.GetStreamAsync($"{SERVER_URL}/{fileName}", cToken);

	public static async Task<Dictionary<string, GeometryFeature>?> GetMVALines(bool isOffline = false)
	{
		ConcurrentBag<KeyValuePair<string, GeometryFeature>> MVALines = new();

		Dictionary<string, MVASourceRecord>? SourceList = await GetSourceRecordDic(isOffline);

		if (SourceList is null)
			return null;

		await Parallel.ForEachAsync(SourceList, async (kvp, cToken) =>
		{
			Geometry geometry;

			if (string.IsNullOrWhiteSpace(kvp.Value.Chart))
				return;

			using (var stream = await GetAssetStreamAsync(isOffline, kvp.Value.Chart, cToken))
			{
				try
				{
					if (kvp.Value.Chart.EndsWith(".bin"))
						geometry = WKBReader.Read(stream);
					else
						geometry = WKTReader.Read(stream);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Failed to load '{kvp.Key}' Texts (at {kvp.Value}) (isOffline?: {isOffline})\n{ex}");
					return;
				}
			}

			MVALines.Add(new(kvp.Key, geometry.ToFeature()));
		});

		return MVALines.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
	}

	class LabelTextsRecord
	{
		[JsonPropertyName("text")]
		public string Text { get; set; } = String.Empty;

		[JsonPropertyName("lon")]
		public double? Lon { get; set; }

		[JsonPropertyName("lat")]
		public double? Lat { get; set; }

		[JsonPropertyName("x")]
		public double? X { get; set; }

		[JsonPropertyName("y")]
		public double? Y { get; set; }

		[JsonPropertyName("minVisible")]
		public double? MinVisible { get; set; }

		[JsonPropertyName("maxVisible")]
		public double? MaxVisible { get; set; }
	}

	public static async Task<PointFeature[]?> GetMVALabels(bool isOffline = false)
	{
		ConcurrentBag<PointFeature> MVALabels = new();

		Dictionary<string, MVASourceRecord>? SourceList = await GetSourceRecordDic(isOffline);

		if (SourceList is null)
			return null;

		await Parallel.ForEachAsync(SourceList, async (kvp, cToken) =>
		{
			LabelTextsRecord[]? labelsRecord = null;

			if (string.IsNullOrWhiteSpace(kvp.Value.Label))
				return;

			using (var stream = await GetAssetStreamAsync(isOffline, kvp.Value.Label, cToken))
			{
				try
				{
					labelsRecord = await JsonSerializer.DeserializeAsync(stream, typeof(LabelTextsRecord[]), cancellationToken: cToken) as LabelTextsRecord[];
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Failed to load '{kvp.Key}' Labels JSON (at {kvp.Value}) (isOffline?: {isOffline})\n{ex}");
					return;
				}

				if (labelsRecord is null)
					return;

				await Parallel.ForEachAsync(labelsRecord, (v, cToken) =>
				{
					var feature = CreateLabelFeature(v);

					feature["ICAO"] = kvp.Key;

					MVALabels.Add(feature);
					return ValueTask.CompletedTask;
				});

			}
		});

		return MVALabels.ToArray();
	}

	private static PointFeature CreateLabelFeature(LabelTextsRecord d)
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

		return new(pt)
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

