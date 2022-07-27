using Mapsui.Styles;
using Mapsui.UI.Maui;

using SkiaSharp;

using Topten.RichTextKit;
using IStyle = Topten.RichTextKit.IStyle;
using Style = Topten.RichTextKit.Style;

namespace AirTote.Components.Maps;

public class CustomTextCalloutPin : Pin
{
	public static IStyle DefaultTextStyle { get; } = new Style()
	{
		BackgroundColor = SKColor.Empty,
		FontFamily = "BIZ UDGothic",
		FontSize = 16,
		TextColor = SKColors.Black,
	};

	public TextPaintOptions TextPaintOptions { get; } = new()
	{
		IsAntialias = true,
		LcdRenderText = true,
	};

	public CustomTextCalloutPin(MapView view) : base(view)
	{
		Callout.RectRadius = 5;
		Callout.ArrowHeight = 8;
		Callout.ArrowWidth = 24;
		Callout.ArrowAlignment = ArrowAlignment.Top;
		Callout.ArrowPosition = 0.5;
		Callout.BackgroundColor = Microsoft.Maui.Graphics.Color.FromRgb(0xff, 0xff, 0xff);

		Callout.TitleFontSize = 16;
		Callout.SubtitleFontSize = 12;

		Callout.Type = CalloutType.Detail;

		//Callout.Content = -1;
	}

	public void SetCalloutText(in IEnumerable<Func<RichString, RichString>> textGens)
	{
		RichString str = new();

		foreach (var func in textGens)
		{
			if (func is not null)
				str = func.Invoke(str);
		}

		SetCalloutText(str);
	}

	public void SetCalloutText(RichString richText)
	{
		if (Callout.Content > 0)
			BitmapRegistry.Instance.Unregister(Callout.Content);
		Callout.Content = -1;

		richText.DefaultStyle = DefaultTextStyle;

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

		Callout.Content = BitmapRegistry.Instance.Register(memStream);
		Callout.Type = CalloutType.Custom;
	}
}
