using BruTile;
using BruTile.Cache;
using BruTile.Predefined;
using BruTile.Web;

using FIS_J.Services;

using Mapsui;
using Mapsui.Rendering.Skia.SkiaWidgets;
using Mapsui.Tiling.Layers;
using Mapsui.Widgets;

using SkiaSharp;

using Topten.RichTextKit;

namespace FIS_J.Components.Maps;

public static class TileProvider
{
	// TODO: Impl Cache System
	static IPersistentCache<byte[]>? DefaultCache { get; } = null;

	const string MAP_ADDITIONAL_ATTR
		= "Shoreline data is derived from: United States. National Imagery and Mapping Agency.\n\"Vector Map Level 0 (VMAP0).\" Bethesda, MD: Denver, CO: The Agency; USGS Information Services, 1997.";

	public static Attribution AttributionInfo { get; } = new("出典: 地理院タイル\n" + MAP_ADDITIONAL_ATTR, "https://maps.gsi.go.jp/development/ichiran.html");

	static string USER_AGENT => HttpService.HttpClient.DefaultRequestHeaders.UserAgent.ToString();

	static HttpTileSource TileSource { get; } = new(
		new GlobalSphericalMercator(),
		@"https://cyberjapandata.gsi.go.jp/xyz/pale/{z}/{x}/{y}.png",
		name: "国土地理院 淡色地図",
		persistentCache: DefaultCache,
		userAgent: USER_AGENT
		);


	public static TileLayer CreateLayer()
		=> new(TileSource);
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