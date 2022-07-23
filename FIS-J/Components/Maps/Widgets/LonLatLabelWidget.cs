using FIS_J.Components.Maps.Layers;
using Mapsui;
using Mapsui.Projections;
using Mapsui.Rendering.Skia.SkiaWidgets;
using Mapsui.Widgets;
using SkiaSharp;

namespace FIS_J.Components.Maps.Widgets;

public class LonLatLabelWidget : Widget
{
	public override bool HandleWidgetTouched(INavigator navigator, MPoint position)
		=> false;
}

public class LonLatLabelWidgetRenderer : ISkiaWidgetRenderer, IDisposable
{
	const byte ALPHA = 0xA0;
	const int TEXT_POS = 30;
	const float RADIUS = 4;
	const float PADDING = 2;
	const string STR_FORMAT = "##0.#";

	SKPaint textPaint { get; } = new()
	{
		Color = SKColors.Black.WithAlpha(ALPHA),
		TextSize = 10,
		IsAntialias = true,
		LcdRenderText = true,
	};

	SKPaint bgPaint { get; } = new()
	{
		Color = SKColors.White.WithAlpha(ALPHA),
		Style = SKPaintStyle.Fill
	};

	float halfHeight { get; }

	public LonLatLabelWidgetRenderer()
	{
		halfHeight = textPaint.TextSize / 2;
	}

	public void Dispose()
	{
		textPaint.Dispose();
		bgPaint.Dispose();
	}

	static public double GetLabelStep(in double resolution)
	=> resolution switch
	{
		var i when i <= LatLngLayerGenerator.MAX_RESO_LV_2 / 5 => 0.1,
		var i when i <= LatLngLayerGenerator.MAX_RESO_LV_2 => 0.5,
		var i when i <= LatLngLayerGenerator.MAX_RESO_LV_1 => 2,
		_ => 10
	};

	public void Draw(SKCanvas canvas, IReadOnlyViewport viewport, IWidget _widget, float layerOpacity)
	{
		if (_widget is not LonLatLabelWidget widget)
			return;

		(double lon, double lat)? lastLonLat = null;

		double labelStep = GetLabelStep(viewport.Resolution);
		double labelStepInv = 1 / labelStep;

		void SetLonLatLabelPosIfNeeded(Dictionary<SKPoint, string> dic, int x, int y)
		{
			SKPoint screenPos = new(x, y);

			var lonlat = SphericalMercator.ToLonLat(viewport.ScreenToWorld(screenPos.X, screenPos.Y));

			double lon = Math.Floor(lonlat.X * labelStepInv) * labelStep;
			double lat = Math.Ceiling(lonlat.Y * labelStepInv) * labelStep;

			if (lastLonLat is not null)
			{
				// 面倒なので、1px未満の誤差は許容する
				if (-180 <= lon && lon <= 180 && lastLonLat?.lon != lon)
					dic[screenPos] = lon.ToString(STR_FORMAT) + "°";
				if (-LatLngLayerGenerator.LAT_LINE_MAX <= lat && lat <= LatLngLayerGenerator.LAT_LINE_MAX && lastLonLat?.lat != lat)
					dic[screenPos] = lat.ToString(STR_FORMAT) + "°";
			}

			lastLonLat = (lon, lat);
		}

		void DrawLonLatText(in IReadOnlyDictionary<SKPoint, string> labelsDic)
		{
			foreach (var kvp in labelsDic)
			{
				float halfWidth = textPaint.MeasureText(kvp.Value) / 2;

				canvas.DrawRoundRect(new(new(
					kvp.Key.X - halfWidth - PADDING,
					kvp.Key.Y - halfHeight - PADDING,
					kvp.Key.X + halfWidth + PADDING,
					kvp.Key.Y + halfHeight + PADDING
					), RADIUS), bgPaint);

				canvas.DrawText(kvp.Value,
					kvp.Key.X - halfWidth,
					kvp.Key.Y + halfHeight - (textPaint.FontMetrics.Descent / 2),
					textPaint);
			}
		}

		Dictionary<SKPoint, string> labels = new();

		lastLonLat = null;
		for (int y = TEXT_POS; y <= viewport.Height - TEXT_POS; y++)
			SetLonLatLabelPosIfNeeded(labels, TEXT_POS, y);

		lastLonLat = null;
		for (int x = TEXT_POS; x <= viewport.Width - TEXT_POS; x++)
			SetLonLatLabelPosIfNeeded(labels, x, (int)viewport.Height - TEXT_POS);

		DrawLonLatText(labels);
	}
}
