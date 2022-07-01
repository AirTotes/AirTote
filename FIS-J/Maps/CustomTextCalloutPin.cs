using Mapsui.Styles;
using Mapsui.UI.Maui;

using SkiaSharp;

namespace FIS_J.Maps;

public class CustomTextCalloutPin : Pin
{
	public CustomTextCalloutPin(MapView view) : base(view)
	{
		Callout.RectRadius = 5;
		Callout.ArrowHeight = 8;
		Callout.ArrowWidth = 24;
		Callout.ArrowAlignment = ArrowAlignment.Top;
		Callout.ArrowPosition = 1;
		Callout.BackgroundColor = Microsoft.Maui.Graphics.Color.FromRgb(0xff, 0xff, 0xff);

		Callout.TitleFontSize = 16;
		Callout.SubtitleFontSize = 12;

		Callout.Type = CalloutType.Detail;

		//Callout.Content = -1;
	}

	public void SetCalloutText(in IEnumerable<CalloutText> texts)
	{
		if (Callout.Content > 0)
			BitmapRegistry.Instance.Unregister(Callout.Content);
		Callout.Content = -1;

		MemoryStream memStream = new();
		using (SKBitmap bitmap = new(
			(int)texts.Max(v => v.X + v.TextBounds.Width) + 8,
			(int)texts.Max(v => v.TextBounds.Top + v.TextBounds.Height))
		)
		using (SKCanvas canvas = new(bitmap))
		{
			canvas.Clear();
			foreach (var calloutText in texts)
				calloutText.DrawTo(canvas);

			using var wStream = new SKManagedWStream(memStream);
			bitmap.Encode(wStream, SKEncodedImageFormat.Png, 100);
		}

		Callout.Content = BitmapRegistry.Instance.Register(memStream);
		Callout.Type = CalloutType.Custom;
	}
}
