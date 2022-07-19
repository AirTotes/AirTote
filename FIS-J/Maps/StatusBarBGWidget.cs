using System.ComponentModel;
using Mapsui;
using Mapsui.Rendering.Skia.SkiaWidgets;
using Mapsui.UI.Maui;
using Mapsui.Widgets;
using SkiaSharp;

namespace FIS_J.Maps;

public class StatusBarBGWidget : Widget
{
	public override bool HandleWidgetTouched(INavigator navigator, MPoint position)
		=> false;
}

public class StatusBarBGWidgetRenderer : ISkiaWidgetRenderer, IDisposable
{
	public static readonly float Height = 24;

#if !__IOS__
	static StatusBarBGWidgetRenderer()
	{
	}
#endif

	const byte ALPHA_TOP = 0xAA;

	SKPaint bgPaint { get; } = new();
	SKColor[] LightThemeColors { get; } = new SKColor[]
	{
		SKColors.White.WithAlpha(ALPHA_TOP),
		SKColors.White.WithAlpha(0x00),
	};
	SKColor[] DarkThemeColors { get; } = new SKColor[]
	{
		SKColors.Black.WithAlpha(ALPHA_TOP),
		SKColors.Black.WithAlpha(0x00),
	};
	SKPoint start { get; } = new(0, 0);
	SKPoint end { get; } = new(0, Height);
	float[] colorPos { get; } = new float[]
	{
		0.5f,
		1f,
	};

	SKShader lightThemeShader { get; }
	SKShader darkThemeShader { get; }

	public StatusBarBGWidgetRenderer()
	{
		lightThemeShader = SKShader.CreateLinearGradient(
			start,
			end,
			LightThemeColors,
			colorPos,
			SKShaderTileMode.Repeat
			);
		darkThemeShader = SKShader.CreateLinearGradient(
			start,
			end,
			DarkThemeColors,
			colorPos,
			SKShaderTileMode.Repeat
			);

		bgPaint.Shader = lightThemeShader;
	}

	public void Dispose()
	{
		bgPaint.Dispose();
		lightThemeShader.Dispose();
		darkThemeShader.Dispose();
	}

	public void Draw(SKCanvas canvas, IReadOnlyViewport viewport, IWidget _widget, float layerOpacity)
	{
		if (_widget is not StatusBarBGWidget widget)
			return;

		if (Height == 0)
			return;

		if (Application.Current is not null)
		{
			switch (Application.Current.RequestedTheme)
			{
				case AppTheme.Dark:
					if (bgPaint.Shader != darkThemeShader)
						bgPaint.Shader = darkThemeShader;
					break;

				default:
					if (bgPaint.Shader != lightThemeShader)
						bgPaint.Shader = lightThemeShader;
					break;
			}
		}

		canvas.DrawRect(0, 0, (float)viewport.Width, Height, bgPaint);
	}
}
