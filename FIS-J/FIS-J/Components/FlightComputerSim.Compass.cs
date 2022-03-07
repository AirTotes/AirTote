
using System;

using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace FIS_J.Components
{
	public class FCS_Compass : ContentView
	{
		const double UNIT = FCS_TASArc.UNIT;
		const double ORIG_ARC_THICKNESS = 14;
		public const double ORIG_RADIUS = 66.5;
		const double RADIUS = ORIG_RADIUS * UNIT;
		public const double ARC_THICKNESS = ORIG_ARC_THICKNESS * UNIT;

		const double SCALE_LEN_L = 3.5 * UNIT;
		const double SCALE_THICKNESS = FCS_TASArc.THICKNESS_BOLD;
		const double SCALE_LEN_S = 2 * UNIT;

		const double LABEL_RADIUS = 61 * UNIT;
		const double LABEL_HEIGHT = 6 * UNIT;
		const double LABEL_WIDTH = 16 * UNIT;
		const double LABEL_HEIGHT_HALF = LABEL_HEIGHT / 2;
		const double LABEL_WIDTH_HALF = LABEL_WIDTH / 2;

		const double DIRECTION_LABEL_RADIUS_S = 55 * UNIT;
		const double DIRECTION_LABEL_RADIUS_L = 56 * UNIT;

		const double DIRECTION_LABEL_FONTSIZE_L = 5 * UNIT;
		public const double DIRECTION_LABEL_FONTSIZE_S = 3.2 * UNIT;

		const double DIRECTION_TRIANGLE_WIDTH_L = 3 * UNIT;
		const double DIRECTION_TRIANGLE_HEIGHT_L = 2 * UNIT;
		const double DIRECTION_TRIANGLE_WIDTH_S = 0.75 * UNIT;
		const double DIRECTION_TRIANGLE_HEIGHT_S = 2 * UNIT;
		const double DIRECTION_TRIANGLE_RADIUS_L = 61 * UNIT;
		const double DIRECTION_TRIANGLE_RADIUS_S = 58 * UNIT;

		static readonly string[] SCALE_DIRECTION_LABEL_TEXTS =
		{
			"N", "NNE", "NE", "ENE",
			"E", "ESE", "SE", "SSE",
			"S", "SSW", "SW", "WSW",
			"W", "WNW", "NW", "NNW",
		};

		static double ToRad(double deg) => deg * Math.PI / 180;

		AbsoluteLayout canvas = new();

		public FCS_Compass()
		{
			Background = null;
			Content = canvas;
			DrawRing();
			DrawScales();
			DrawScaleLabels();
		}

		static ContentView getTextElem(string str, double deg, Point elemCenter)
			=> getTextElem(deg, elemCenter, new()
			{
				Text = str,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.Black,
				FontSize = DIRECTION_LABEL_FONTSIZE_S,
			});
		static ContentView getTextElem(double deg, Point elemCenter, Label label)
			=> new()
			{
				HeightRequest = LABEL_HEIGHT,
				WidthRequest = LABEL_WIDTH,
				Margin = new(elemCenter.X - LABEL_WIDTH_HALF, elemCenter.Y - LABEL_HEIGHT_HALF),
				Content = label,
				AnchorX = 0.5,
				AnchorY = 0.5,
				Rotation = deg,
			};

		void DrawRing()
		{
			canvas.Children.Add(new Path()
			{
				Stroke = null,
				Fill = Brush.White,
				Data = new PathGeometry()
				{
					FillRule = FillRule.Nonzero,
					Figures = new()
					{
						new()
						{
							StartPoint = new(0, RADIUS),
							Segments = new()
							{
								new ArcSegment()
								{
									Point = new(RADIUS * 2, RADIUS),
									Size = new(RADIUS, RADIUS),
								},
								new ArcSegment()
								{
									Point = new(0, RADIUS),
									Size = new(RADIUS, RADIUS),
									IsLargeArc = true,
								},
							}
						},
						new()
						{
							StartPoint = new(ARC_THICKNESS, RADIUS),
							Segments = new()
							{
								new ArcSegment()
								{
									Point = new(RADIUS * 2 - ARC_THICKNESS, RADIUS),
									Size = new(RADIUS - ARC_THICKNESS, RADIUS - ARC_THICKNESS),
									SweepDirection = SweepDirection.Clockwise,
								},
								new ArcSegment()
								{
									Point = new(ARC_THICKNESS, RADIUS),
									Size = new(RADIUS - ARC_THICKNESS, RADIUS - ARC_THICKNESS),
									IsLargeArc = true,
									SweepDirection = SweepDirection.Clockwise,
								},
							}
						},
					}
				},
			});
		}

		void DrawScales()
		{
			for (double deg = 0; deg < 360; deg++)
			{
				double _width = (deg % 5) == 0 ? SCALE_LEN_L : SCALE_LEN_S;

				double r_in = RADIUS - _width;

				canvas.Children.Add(new Line()
				{
					X1 = RADIUS + (r_in * Math.Cos(ToRad(deg))),
					Y1 = RADIUS - (r_in * Math.Sin(ToRad(deg))),
					X2 = RADIUS + (RADIUS * Math.Cos(ToRad(deg))),
					Y2 = RADIUS - (RADIUS * Math.Sin(ToRad(deg))),
					Stroke = Brush.Black,
					StrokeThickness = SCALE_THICKNESS,
				});
			}
		}

		static PointCollection getUpTriangleS() => new()
		{
			new(0, DIRECTION_TRIANGLE_HEIGHT_S),
			new(DIRECTION_TRIANGLE_WIDTH_S / 2, 0),
			new(DIRECTION_TRIANGLE_WIDTH_S, DIRECTION_TRIANGLE_HEIGHT_S),
		};
		static PointCollection getRightTriangleS() => new()
		{
			new (0, 0),
			new (DIRECTION_TRIANGLE_HEIGHT_S, DIRECTION_TRIANGLE_WIDTH_S / 2),
			new (0, DIRECTION_TRIANGLE_WIDTH_S),
		};
		static PointCollection getDownTriangleS() => new()
		{
			new(0, 0),
			new(DIRECTION_TRIANGLE_WIDTH_S / 2, DIRECTION_TRIANGLE_HEIGHT_S),
			new(DIRECTION_TRIANGLE_WIDTH_S, 0),
		};
		static PointCollection getLeftTriangleS() => new()
		{
			new (DIRECTION_TRIANGLE_HEIGHT_S, 0),
			new (0, DIRECTION_TRIANGLE_WIDTH_S / 2),
			new (DIRECTION_TRIANGLE_HEIGHT_S, DIRECTION_TRIANGLE_WIDTH_S),
		};

		void DrawScaleLabels()
		{
			static Polygon getPolygonElem(PointCollection points, double deg, Thickness margin)
				=> new()
				{
					Points = points,
					Margin = margin,
					AnchorX = 0.5,
					AnchorY = 0.5,
					Fill = Brush.Black,
					Rotation = deg,
				};
			static Polygon getUpPolygonElem(double deg)
				=> getPolygonElem(getUpTriangleS(), deg, new(
						RADIUS + (DIRECTION_TRIANGLE_RADIUS_S * Math.Sin(ToRad(deg))) - (DIRECTION_TRIANGLE_WIDTH_S / 2),
						RADIUS - (DIRECTION_TRIANGLE_RADIUS_S * Math.Cos(ToRad(deg))) - (DIRECTION_TRIANGLE_HEIGHT_S / 2)
					));
			static Polygon getRightPolygonElem(double deg)
				=> getPolygonElem(getRightTriangleS(), deg, new(
						RADIUS + (DIRECTION_TRIANGLE_RADIUS_S * Math.Cos(ToRad(deg))) - (DIRECTION_TRIANGLE_HEIGHT_S / 2),
						RADIUS + (DIRECTION_TRIANGLE_RADIUS_S * Math.Sin(ToRad(deg))) - (DIRECTION_TRIANGLE_WIDTH_S / 2)
					));
			static Polygon getDownPolygonElem(double deg)
				=> getPolygonElem(getDownTriangleS(), deg, new(
						RADIUS - (DIRECTION_TRIANGLE_RADIUS_S * Math.Sin(ToRad(deg))) - (DIRECTION_TRIANGLE_WIDTH_S / 2),
						RADIUS + (DIRECTION_TRIANGLE_RADIUS_S * Math.Cos(ToRad(deg))) - (DIRECTION_TRIANGLE_HEIGHT_S / 2)
					));
			static Polygon getLeftPolygonElem(double deg)
				=> getPolygonElem(getLeftTriangleS(), deg, new(
						RADIUS - (DIRECTION_TRIANGLE_RADIUS_S * Math.Cos(ToRad(deg))) - (DIRECTION_TRIANGLE_HEIGHT_S / 2),
						RADIUS - (DIRECTION_TRIANGLE_RADIUS_S * Math.Sin(ToRad(deg))) - (DIRECTION_TRIANGLE_WIDTH_S / 2)
					));

			for (double deg = 0; deg < 360; deg += 10)
			{
				if ((deg % 90) == 0)
					continue;

				canvas.Children.Add(getTextElem(deg.ToString(), deg,
					new(
						RADIUS + (LABEL_RADIUS * Math.Sin(ToRad(deg))),
						RADIUS - (LABEL_RADIUS * Math.Cos(ToRad(deg)))
					)));
			}

			for (int i = 0; i < SCALE_DIRECTION_LABEL_TEXTS.Length; i++)
			{
				double deg = i * 22.5;
				double SIN = Math.Sin(ToRad(deg));
				double COS = Math.Cos(ToRad(deg));

				if (i % 4 == 0)
				{
					canvas.Children.Add(getTextElem(deg,
						new(
							RADIUS + (DIRECTION_LABEL_RADIUS_L * SIN),
							RADIUS - (DIRECTION_LABEL_RADIUS_L * COS)
						), new()
						{
							Text = SCALE_DIRECTION_LABEL_TEXTS[i],
							HorizontalOptions = LayoutOptions.Center,
							VerticalOptions = LayoutOptions.Center,
							HorizontalTextAlignment = TextAlignment.Center,
							VerticalTextAlignment = TextAlignment.Center,
							TextColor = Color.White,
							Background = Brush.Black,
							FontSize = DIRECTION_LABEL_FONTSIZE_L,
							WidthRequest = LABEL_HEIGHT,
							HeightRequest = LABEL_HEIGHT,
						}));
				}
				else
				{
					canvas.Children.Add(getTextElem(SCALE_DIRECTION_LABEL_TEXTS[i], deg,
						new(
							RADIUS + (DIRECTION_LABEL_RADIUS_S * SIN),
							RADIUS - (DIRECTION_LABEL_RADIUS_S * COS)
						)));
				}
			}

			// NNW
			canvas.Children.Add(getUpPolygonElem(-22.5));
			// North
			canvas.Children.Add(new Polygon() {
				Points = {
					new(0, DIRECTION_TRIANGLE_HEIGHT_L),
					new(DIRECTION_TRIANGLE_WIDTH_L / 2, 0),
					new(DIRECTION_TRIANGLE_WIDTH_L, DIRECTION_TRIANGLE_HEIGHT_L),
				},
				Margin = new(RADIUS - (DIRECTION_TRIANGLE_WIDTH_L / 2), RADIUS - DIRECTION_TRIANGLE_RADIUS_L - (DIRECTION_TRIANGLE_HEIGHT_L / 2)),
				Fill = Brush.Black,
			});
			// NNE
			canvas.Children.Add(getUpPolygonElem(22.5));
			// NE
			canvas.Children.Add(getUpPolygonElem(45));

			// ENE
			canvas.Children.Add(getRightPolygonElem(-22.5));
			// East
			canvas.Children.Add(new Polygon() {
				Points = {
					new(0, 0),
					new(DIRECTION_TRIANGLE_HEIGHT_L, DIRECTION_TRIANGLE_WIDTH_L / 2),
					new(0, DIRECTION_TRIANGLE_WIDTH_L),
				},
				Margin = new(RADIUS + DIRECTION_TRIANGLE_RADIUS_L - (DIRECTION_TRIANGLE_HEIGHT_L / 2), RADIUS - (DIRECTION_TRIANGLE_WIDTH_L / 2)),
				Fill = Brush.Black,
			});
			// ESE
			canvas.Children.Add(getRightPolygonElem(22.5));
			// SE
			canvas.Children.Add(getRightPolygonElem(45));

			// SSE
			canvas.Children.Add(getDownPolygonElem(-22.5));
			// South
			canvas.Children.Add(new Polygon() {
				Points = {
					new(0, 0),
					new(DIRECTION_TRIANGLE_WIDTH_L / 2, DIRECTION_TRIANGLE_HEIGHT_L),
					new(DIRECTION_TRIANGLE_WIDTH_L, 0),
				},
				Margin = new(RADIUS - (DIRECTION_TRIANGLE_WIDTH_L / 2), RADIUS + DIRECTION_TRIANGLE_RADIUS_L - (DIRECTION_TRIANGLE_HEIGHT_L / 2)),
				Fill = Brush.Black,
			});
			// SSW
			canvas.Children.Add(getDownPolygonElem(22.5));
			// SW
			canvas.Children.Add(getDownPolygonElem(45));

			// WSW
			canvas.Children.Add(getLeftPolygonElem(-22.5));
			// West
			canvas.Children.Add(new Polygon() {
				Points = {
					new(DIRECTION_TRIANGLE_HEIGHT_L, 0),
					new(DIRECTION_TRIANGLE_HEIGHT_L, DIRECTION_TRIANGLE_WIDTH_L),
					new(0, DIRECTION_TRIANGLE_WIDTH_L / 2),
				},
				Margin = new(RADIUS - DIRECTION_TRIANGLE_RADIUS_L - (DIRECTION_TRIANGLE_HEIGHT_L / 2), RADIUS - (DIRECTION_TRIANGLE_WIDTH_L / 2)),
				Fill = Brush.Black,
			});
			// WNW
			canvas.Children.Add(getLeftPolygonElem(22.5));
			// NW
			canvas.Children.Add(getLeftPolygonElem(45));

		}
	}
}