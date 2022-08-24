using System.ComponentModel;

using Mapsui;
using Mapsui.Rendering.Skia.SkiaWidgets;
using Mapsui.Styles;
using Mapsui.UI.Maui;
using Mapsui.Widgets;

using SkiaSharp;

using Svg.Skia;

namespace AirTote.Components.Maps.Widgets;

public class ButtonWidget : Widget, INamedWidget
{
	public event EventHandler<WidgetTouchedEventArgs>? WidgetTouched;

	public SKPicture Picture { get; }
	public SKBitmap? Bitmap { get; private set; }

	SKSize _Size = new(0, 0);
	public SKSize Size
	{
		get => _Size;
		set
		{
			if (value == _Size)
				return;
			_Size = value;

			var rect = Picture.CullRect;
			float scaleX = value.Width == 0 ? 1 : value.Width / Picture.CullRect.Width;
			float scaleY = value.Height == 0 ? 1 : value.Height / Picture.CullRect.Height;

			Bitmap?.Dispose();
			Bitmap = Picture.ToBitmap(
				SKColor.Empty,
				scaleX,
				scaleY,
				SKColorType.Bgra8888,
				SKAlphaType.Premul,
				SKColorSpace.CreateSrgb()
				);
		}
	}

	public string Name { get; }

	public ButtonWidget(string name, string svgString)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("name cannot be null or empty");
		if (string.IsNullOrWhiteSpace(svgString))
			throw new ArgumentException("svgString cannot be null or empty");

		Name = name;
		Picture = new SKSvg().FromSvg(svgString) ?? throw new FormatException("SvgString is not a valid SVG Format");
	}

	static public async Task<ButtonWidget> FromAppPackageFileAsync(string name, string path)
	{
		using var stream = await FileSystem.OpenAppPackageFileAsync(path);
		return new ButtonWidget(name, await new StreamReader(stream).ReadToEndAsync());
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

	public void Dispose()
	{
		bgPaint.Dispose();
	}

	public void Draw(SKCanvas canvas, IReadOnlyViewport viewport, IWidget _widget, float layerOpacity)
	{
		if (_widget is not ButtonWidget widget)
			return;

		if (widget.Bitmap is null)
			return;

		canvas.DrawRect(widget.MarginX, widget.MarginY, widget.Bitmap.Width, widget.Bitmap.Height, bgPaint);
		canvas.DrawBitmap(widget.Bitmap, widget.MarginX, widget.MarginY);
		widget.Envelope = new(widget.MarginX, widget.MarginY, widget.MarginX + widget.Bitmap.Width, widget.MarginY + widget.Bitmap.Height);
	}
}
