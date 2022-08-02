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
using Mapsui.Layers;

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

public class TileLicenseWidget : Widget
{
	public RichString? Str { get; private set; } = null;
	public Attribution? Link { get; private set; } = null;

	static Topten.RichTextKit.Style MapAttrTextStyle { get; } = new Topten.RichTextKit.Style()
	{
		TextColor = SKColors.Black,
		FontFamily = "BIZ UDGothic",
		FontSize = 10,
	};


	public TileLicenseWidget(Mapsui.Map map)
	{
		map.Layers.Changed += Layers_Changed;

		SetAttrText(map.Layers.AsEnumerable());
	}

	private void Layers_Changed(object sender, LayerCollectionChangedEventArgs args)
	{
		bool isTileLayerRemoved = args.RemovedLayers?.Any(v => v is TileLayer) ?? false;
		if (!SetAttrText(args.AddedLayers) && isTileLayerRemoved)
		{
			Str = null;
			Link = null;
		}
	}

	private bool SetAttrText(IEnumerable<ILayer>? layers)
	{
		if (layers?.FirstOrDefault(v => v is TileLayer) is not TileLayer layer || layer.TileSource is not HttpTileSource src)
			return false;

		Str = new(src.Attribution.Text)
		{
			DefaultStyle = MapAttrTextStyle
		};

		Link = src.Attribution;
		return true;
	}

	public override bool HandleWidgetTouched(INavigator navigator, MPoint position)
	{
		if (Link?.Url is null)
			return false;

		Launcher.OpenAsync(Link.Url);
		return true;
	}
}

public class TileLicenseWidgetRenderer : ISkiaWidgetRenderer, IDisposable
{
	const float PADDING = 4;
	const float MARGIN = 4;
	const float RADIUS = 4;

	static SKColor BGColor { get; } = SKColors.White.WithAlpha(0x80);

	SKPaint BgPaint { get; } = new()
	{
		Color = BGColor,
		Style = SKPaintStyle.Fill
	};


	public void Draw(SKCanvas canvas, IReadOnlyViewport viewport, IWidget _widget, float layerOpacity)
	{
		if (_widget is not TileLicenseWidget widget || widget.Str is null)
			return;
		// 割合設定の数値は、iPhone SEでInfoWidgetに被らない程度に設定している
		widget.Str.MaxWidth = (float)(viewport.Width * 0.6);

		float strLeft = (float)viewport.Width - MARGIN - PADDING - widget.Str.MeasuredWidth;
		float strUp = (float)viewport.Height - MARGIN - PADDING - widget.Str.MeasuredHeight;
		float rectLeft = strLeft - PADDING;
		float rectUp = strUp - PADDING;
		float rectHeight = widget.Str.MeasuredHeight + (PADDING * 2);
		float rectWidth = widget.Str.MeasuredWidth + (PADDING * 2);

		widget.Envelope = new(rectLeft, rectUp, rectLeft + rectWidth, rectUp + rectHeight);

		canvas.DrawRoundRect(strLeft - PADDING, strUp - PADDING, rectWidth, rectHeight, RADIUS, RADIUS, BgPaint);
		widget.Str.Paint(canvas, new SKPoint(strLeft, strUp));
	}

	public void Dispose()
	{
		BgPaint.Dispose();
	}
}