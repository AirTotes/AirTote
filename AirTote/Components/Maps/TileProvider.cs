using BruTile;
using BruTile.Cache;
using BruTile.Predefined;
using BruTile.Web;

using AirTote.Services;

using Mapsui;
using Mapsui.Rendering.Skia.SkiaWidgets;
using Mapsui.Tiling.Layers;
using Mapsui.Widgets;

using SkiaSharp;

using Topten.RichTextKit;

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

	static TileProvider()
	{
		_TileSources.Add(DEFAULT_MAP_SOURCE_KEY, new(
				@"https://cyberjapandata.gsi.go.jp/xyz/pale/{z}/{x}/{y}.png",
				"国土地理院 淡色地図",
				AttributionInfo
			));
	}

	public static TileLayer CreateLayer(string key = DEFAULT_MAP_SOURCE_KEY)
	{
		if (!TileSources.TryGetValue(key, out var value) || value is null)
			throw new KeyNotFoundException("Specified key was not found");

		return new(new HttpTileSource(
				new GlobalSphericalMercator(),
				value.UrlFormatter,
				name: value.Name,
				persistentCache: DefaultCache,
				userAgent: USER_AGENT,
				attribution: AttributionInfo
			))
		{
			Name = value.Name
		};
	}
}

public class TileLicenseWidget : Widget
{
	public override bool HandleWidgetTouched(INavigator navigator, MPoint position)
	{
		Launcher.OpenAsync(TileProvider.AttributionInfo.Url);
		return true;
	}
}

public class TileLicenseWidgetRenderer : ISkiaWidgetRenderer, IDisposable
{
	const float PADDING = 4;
	const float MARGIN = 4;
	const float RADIUS = 4;
	static float StrWidth { get; }
	static float StrHeight { get; }
	static RichString Str { get; }
	static SKColor BGColor { get; } = SKColors.White.WithAlpha(0x80);
	static SKColor TextColor { get; } = SKColors.Black;

	SKPaint BgPaint { get; } = new()
	{
		Color = BGColor,
		Style = SKPaintStyle.Fill
	};


	static TileLicenseWidgetRenderer()
	{
		Str = new(TileProvider.AttributionInfo.Text);
		Str.DefaultStyle = new Topten.RichTextKit.Style()
		{
			TextColor = TextColor,
			FontFamily = "BIZ UDGothic",
			FontSize = 10,
		};

		StrWidth = Str.MeasuredWidth;
		StrHeight = Str.MeasuredHeight;
	}

	public void Draw(SKCanvas canvas, IReadOnlyViewport viewport, IWidget _widget, float layerOpacity)
	{
		if (_widget is not TileLicenseWidget widget)
			return;

		float strLeft = (float)viewport.Width - MARGIN - PADDING - StrWidth;
		float strUp = (float)viewport.Height - MARGIN - PADDING - StrHeight;
		float rectLeft = strLeft - PADDING;
		float rectUp = strUp - PADDING;
		float rectHeight = StrHeight + (PADDING * 2);
		float rectWidth = StrWidth + (PADDING * 2);

		widget.Envelope = new(rectLeft, rectUp, rectLeft + rectWidth, rectUp + rectHeight);

		canvas.DrawRoundRect(strLeft - PADDING, strUp - PADDING, rectWidth, rectHeight, RADIUS, RADIUS, BgPaint);
		Str.Paint(canvas, new SKPoint(strLeft, strUp));
	}

	public void Dispose()
	{
		BgPaint.Dispose();
	}
}