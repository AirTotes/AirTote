using Mapsui.Styles;

using SkiaSharp;

using Topten.RichTextKit;

using IStyle = Topten.RichTextKit.IStyle;

namespace AirTote;

public static partial class Utils
{
	static TextPaintOptions TextPaintOptions { get; } = new()
	{
		Edging = SKFontEdging.SubpixelAntialias,
	};

	public static int GetRichTextBitmapId(RichString richText, IStyle? defaultStyle)
	{
		if (defaultStyle is not null)
			richText.DefaultStyle = defaultStyle;

		var width = richText.MeasuredWidth;
		if (width * 1.1 < richText.MaxWidth)
		{
			width *= 1.1f;
			richText.MaxWidth = width;
		}

		MemoryStream memStream = new();
		using (SKBitmap bitmap = new(
			(int)width,
			(int)richText.MeasuredHeight)
		)
		using (SKCanvas canvas = new(bitmap))
		{
			canvas.Clear();
			richText.Paint(canvas, TextPaintOptions);

			using var wStream = new SKManagedWStream(memStream);
			bitmap.Encode(wStream, SKEncodedImageFormat.Png, 100);
		}

		return BitmapRegistry.Instance.Register(memStream);
	}

}

