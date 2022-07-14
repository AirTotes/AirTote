using System.ComponentModel;
using Mapsui;
using Mapsui.Rendering.Skia.SkiaWidgets;
using Mapsui.UI.Maui;
using Mapsui.Widgets;
using SkiaSharp;

namespace FIS_J.Maps;

public class InfoWidget : Widget, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	private MPoint _TapPos;
	public MPoint TapPos
	{
		get => _TapPos;
		set
		{
			if (TapPos.X == value.X && TapPos.Y == TapPos.Y)
				return;
			_TapPos = value;
			PropertyChanged?.Invoke(this, new(nameof(TapPos)));
		}
	}

	public override bool HandleWidgetTouched(INavigator navigator, MPoint position)
	{
		return false;
	}
}

public class InfoWidgetRenderer : ISkiaWidgetRenderer, IDisposable
{
	const float TextSize = 12;
	const float Margin = 4;
	const float Padding = 4;
	const string Header = "Resolution: ";
	const string StrFormat = "#,###,###.##";
	SKPaint textPaint { get; } = new() { Color = SKColors.Black, TextSize = 12 };
	SKPaint bgPaint { get; } = new() { Color = SKColors.White, Style = SKPaintStyle.Fill };
	SKRect rect { get; }

	public InfoWidgetRenderer()
	{
		var width = textPaint.MeasureText(Header + StrFormat);
		rect = new(Margin, Margin, Margin + Padding + width + Padding, Margin + Padding + TextSize + Padding);
	}

	public void Dispose()
	{
		textPaint.Dispose();
		bgPaint.Dispose();
	}

	public void Draw(SKCanvas canvas, IReadOnlyViewport viewport, IWidget _widget, float layerOpacity)
	{
		if (_widget is not InfoWidget widget)
			return;

		canvas.DrawRect(rect, bgPaint);

		string text = Header + viewport.Resolution.ToString(StrFormat);
		canvas.DrawText(text, rect.Right - Padding - textPaint.MeasureText(text), rect.Top + Padding + TextSize, textPaint);
	}
}
