using BruTile;
using BruTile.Web;

using Mapsui;
using Mapsui.Rendering.Skia.SkiaWidgets;
using Mapsui.Tiling.Layers;
using Mapsui.Widgets;

using SkiaSharp;

using Topten.RichTextKit;
using Mapsui.Layers;

namespace AirTote.Components.Maps.Widgets;

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