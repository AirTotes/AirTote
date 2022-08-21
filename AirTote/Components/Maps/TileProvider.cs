using AirTote.Services;

using BruTile;
using BruTile.Cache;
using BruTile.Predefined;
using BruTile.Web;

using Mapsui.Tiling.Layers;

namespace AirTote.Components.Maps;

public record MapTileSourceInfo(string UrlFormatter, string Name, Attribution Attr);

public static class TileProvider
{
	// TODO: Impl Cache System
	static IPersistentCache<byte[]>? DefaultCache { get; } = null;

	const string MAP_ADDITIONAL_ATTR
		= "Shoreline data is derived from: United States. National Imagery and Mapping Agency.\n\"Vector Map Level 0 (VMAP0).\" Bethesda, MD: Denver, CO: The Agency; USGS Information Services, 1997.";

	public static Attribution AttributionInfo { get; } = new("出典: 地理院タイル\n" + MAP_ADDITIONAL_ATTR, "https://maps.gsi.go.jp/development/ichiran.html");

	static string USER_AGENT => HttpService.HttpClient.DefaultRequestHeaders.UserAgent.ToString();

	static Dictionary<string, MapTileSourceInfo> _TileSources { get; } = new();
	public static IReadOnlyDictionary<string, MapTileSourceInfo> TileSources => _TileSources;
	public const string DEFAULT_MAP_SOURCE_KEY = "gsi_jp_pale";
	public const string BLANK_MAP_SOURCE_KEY = "gsi_jp_blank";

	static TileProvider()
	{
		_TileSources.Add(DEFAULT_MAP_SOURCE_KEY, new(
				@"https://cyberjapandata.gsi.go.jp/xyz/pale/{z}/{x}/{y}.png",
				"国土地理院 淡色地図",
				AttributionInfo
			));
		_TileSources.Add(BLANK_MAP_SOURCE_KEY, new(
				@"https://cyberjapandata.gsi.go.jp/xyz/blank/{z}/{x}/{y}.png",
				"国土地理院 白地図",
				new("出典: 地理院タイル", "https://maps.gsi.go.jp/development/ichiran.html")
			));
	}

	public static TileLayer CreateLayer(string key = DEFAULT_MAP_SOURCE_KEY)
	{
		if (string.IsNullOrWhiteSpace(key))
			key = DEFAULT_MAP_SOURCE_KEY;

		if (!TileSources.TryGetValue(key, out var value) || value is null)
			throw new KeyNotFoundException("Specified key was not found");

		return CreateLayer(value);
	}

	public static TileLayer CreateLayer(MapTileSourceInfo value)
	{
		TileLayer layer = new(new HttpTileSource(
				new GlobalSphericalMercator(),
				value.UrlFormatter,
				name: value.Name,
				persistentCache: DefaultCache,
				userAgent: USER_AGENT,
				attribution: AttributionInfo
			))
		{
			Name = value.Name,
		};

		layer.Attribution.Enabled = false;

		return layer;
	}
}
