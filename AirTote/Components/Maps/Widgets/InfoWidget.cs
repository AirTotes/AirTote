using System.ComponentModel;

using Mapsui;
using Mapsui.Rendering.Skia.SkiaWidgets;
using Mapsui.UI.Maui;
using Mapsui.Widgets;

using SkiaSharp;

namespace AirTote.Components.Maps.Widgets;

public class InfoWidget : Widget, INotifyPropertyChanged, INamedWidget
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public float PaddingX { get; set; } = 4;
	public float PaddingY { get; set; } = 4;
	public string Name { get; } = "InfoWidget";

	public InfoWidget()
	{
		// set default value
		MarginX = 4;
		MarginY = 4;
	}

	private MPoint _TapPos = new();
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

	const string Header = "Resolution: ";
	const string StrFormat = "###,###.##";

	SKPaint textPaint { get; } = new() { Color = SKColors.Black, TextSize = 12 };
	SKPaint bgPaint { get; } = new() { Color = SKColors.White, Style = SKPaintStyle.Fill };
	float textWidth { get; }

	public InfoWidgetRenderer()
	{
		textWidth = textPaint.MeasureText(Header + StrFormat);
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

		var MarginX = widget.MarginX;
		var MarginY = widget.MarginY;
		var PaddingX = widget.PaddingX;
		var PaddingY = widget.PaddingY;

		SKRect rect = new(
			left: widget.HorizontalAlignment switch
			{
				Mapsui.Widgets.HorizontalAlignment.Left => MarginX,
				Mapsui.Widgets.HorizontalAlignment.Right => (float)viewport.Width - MarginX - PaddingX - textWidth - PaddingX,
				_ => (float)viewport.CenterX - (textWidth / 2) - PaddingX
			},
			top: widget.VerticalAlignment switch
			{
				Mapsui.Widgets.VerticalAlignment.Top => MarginY,
				Mapsui.Widgets.VerticalAlignment.Bottom => (float)viewport.Height - MarginY - PaddingY - TextSize - PaddingY,
				_ => (float)viewport.CenterY - (TextSize / 2) - PaddingY
			},
			right: widget.HorizontalAlignment switch
			{
				Mapsui.Widgets.HorizontalAlignment.Left => MarginX + PaddingX + textWidth + PaddingX,
				Mapsui.Widgets.HorizontalAlignment.Right => (float)viewport.Width - MarginX,
				_ => (float)viewport.CenterX + (textWidth / 2) + PaddingX
			},
			bottom: widget.VerticalAlignment switch
			{
				Mapsui.Widgets.VerticalAlignment.Top => MarginY + PaddingY + TextSize + PaddingY,
				Mapsui.Widgets.VerticalAlignment.Bottom => (float)viewport.Height - MarginY,
				_ => (float)viewport.CenterY + (TextSize / 2) + PaddingY
			}
			);

		canvas.DrawRect(rect, bgPaint);

		string text = Header + viewport.Resolution.ToString(StrFormat);
		canvas.DrawText(text, rect.Right - PaddingX - textPaint.MeasureText(text), rect.Top + PaddingY + TextSize, textPaint);
	}
}
