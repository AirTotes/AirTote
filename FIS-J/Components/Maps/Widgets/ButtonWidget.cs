using System.ComponentModel;

using Mapsui;
using Mapsui.Rendering.Skia.SkiaWidgets;
using Mapsui.Styles;
using Mapsui.UI.Maui;
using Mapsui.Widgets;

using SkiaSharp;

using Svg.Skia;

namespace FIS_J.Components.Maps.Widgets;

public class ButtonWidget : Widget
{
	public event EventHandler<WidgetTouchedEventArgs>? WidgetTouched;

	public SKPicture Picture { get; }

	public float Height { get; init; }
	public float Width { get; init; }

	public ButtonWidget(string svgString)
	{
		if (string.IsNullOrWhiteSpace(svgString))
			throw new ArgumentException("svgString cannot be null or empty");

		Picture = new SKSvg().FromSvg(svgString) ?? throw new FormatException("SvgString is not a valid SVG Format");
	}

	public override bool HandleWidgetTouched(INavigator navigator, MPoint position)
	{
		WidgetTouchedEventArgs e = new(position);
		WidgetTouched?.Invoke(this, e);
		return e.Handled;
	}
}

public class ButtonWidgetRenderer : ISkiaWidgetRenderer, IDisposable
{
	SKPaint bgPaint { get; } = new() { Color = SKColors.WhiteSmoke };

	public ButtonWidgetRenderer()
	{
	}

	public void Dispose()
	{
		bgPaint.Dispose();
	}

	public void Draw(SKCanvas canvas, IReadOnlyViewport viewport, IWidget _widget, float layerOpacity)
	{
		if (_widget is not ButtonWidget widget)
			return;

		if (widget.Picture is null)
			return;

		var rect = widget.Picture.CullRect;
		float TargetWidth = widget.Width == 0 ? rect.Width : widget.Width;
		float TargetHeight = widget.Height == 0 ? rect.Height : widget.Height;

		float scaleX = TargetWidth / rect.Width;
		float scaleY = TargetHeight / rect.Height;
		canvas.Scale(scaleX, scaleY);
		canvas.DrawRect(widget.MarginX, widget.MarginY, TargetWidth, TargetHeight, bgPaint);
		canvas.DrawPicture(widget.Picture, widget.MarginX, widget.MarginY);
		widget.Envelope ??= new(widget.MarginX, widget.MarginY, widget.MarginX + TargetWidth, widget.MarginY + TargetHeight);
	}
}
