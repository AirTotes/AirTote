using SkiaSharp;

namespace FIS_J.Maps;

public class CalloutText : IDisposable
{
	private bool disposedValue;

	public SKPaint Paint { get; }

	public string Text { get; init; } = null;

	public SKColor FontColor { get; init; } = SKColors.Black;
	public string FontFamily { get; init; } = "BIZUDGothic-Regular";
	public int FontSize { get; init; } = 16;

	public float X { get; init; } = 0;
	public float Y { get; init; } = 0;

	private SKRect _TextBounds;
	public SKRect TextBounds => _TextBounds;

	private static SKTypeface _Typeface = null;
	private SKTypeface Typeface
	{
		get
		{
			if (_Typeface is null)
			{

				using var stream = FileSystem.OpenAppPackageFileAsync("Fonts/BIZUDGothic-Regular.ttf").Result;
				_Typeface = SKTypeface.FromStream(stream);
			}

			return _Typeface;
		}
	}

	public CalloutText(in string text = "", in CalloutText upper = null, in float linePadding = 2f)
	{
		Text ??= text;

		Paint = new()
		{
			Color = FontColor,
			Typeface = Typeface,
			TextSize = FontSize,
			IsLinearText = true,
			LcdRenderText = true,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High,
			SubpixelText = true,
		};

		if (upper is not null)
			Y += upper.Y + linePadding + Paint.FontSpacing;

		using var textPath = Paint.GetTextPath(Text, X, Y);
		textPath.GetTightBounds(out _TextBounds);

		if (upper is null)
			Y -= TextBounds.Top;
	}

	public void DrawTo(SKCanvas canvas)
		=> canvas.DrawText(Text, X, Y, Paint);

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
				Paint.Dispose();

			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}