
using System;

using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace FIS_J.Components
{
	public class FCS_Compass : ContentView
	{
		const double UNIT = FlightComputerSim.UNIT;
		const double ORIG_ARC_THICKNESS = 14;
		const double ORIG_RADIUS = 66.5;
		const double RADIUS = ORIG_RADIUS * UNIT;
		const double ARC_THICKNESS = ORIG_ARC_THICKNESS * UNIT;

		const double SCALE_LEN_L = 3.5 * UNIT;
		const double SCALE_THICKNESS = FlightComputerSim.THICKNESS_BOLD;
		const double SCALE_LEN_S = 2 * UNIT;

		const double LABEL_RADIUS = 61 * UNIT;
		const double LABEL_HEIGHT = 20;
		const double LABEL_WIDTH = 40;
		const double LABEL_HEIGHT_HALF = LABEL_HEIGHT / 2;
		const double LABEL_WIDTH_HALF = LABEL_WIDTH / 2;

		static double ToRad(double deg) => deg * Math.PI / 180;

		AbsoluteLayout canvas = new();

		public FCS_Compass()
		{
			Background = null;
			Content = canvas;
			DrawRing();
			DrawScales();
			DrawScaleNums();
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
				Fill = Brush.Red,
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
		void DrawScaleNums()
		{
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
		}

		void DrawOrientationLabels()
		{

		}
	}
}