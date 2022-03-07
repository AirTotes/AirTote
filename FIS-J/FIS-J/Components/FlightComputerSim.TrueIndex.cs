using System;

using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace FIS_J.Components
{
	public class FCS_TrueIndex : ContentView
	{
		const double UNIT = FlightComputerSim.UNIT;

		public const double ORIG_ARC_THICKNESS = 12;
		public const double ORIG_RADIUS = FCS_Compass.ORIG_RADIUS + ORIG_ARC_THICKNESS;
		public const double ORIG_CENTER_RADIUS = 1.2;

		public const double CENTER_RADIUS = ORIG_CENTER_RADIUS * UNIT;
		public const double RADIUS = ORIG_RADIUS * UNIT;
		public const double ARC_THICKNESS = ORIG_ARC_THICKNESS * UNIT;

		const double SCALE_LEN_L = 3.5 * UNIT;
		const double SCALE_THICKNESS = FlightComputerSim.THICKNESS_BOLD;
		const double SCALE_LEN_S = 2 * UNIT;

		const double LABEL_RADIUS = 73 * UNIT;
		const double LABEL_HEIGHT = 5 * UNIT;
		const double LABEL_WIDTH = 40 * UNIT;
		const double LABEL_HEIGHT_HALF = LABEL_HEIGHT / 2;
		const double LABEL_WIDTH_HALF = LABEL_WIDTH / 2;

		const double TITLE_RADIUS = (ORIG_RADIUS - 4) * UNIT;
		const double TITLE_TRIANGLE_HEIGHT = 3 * UNIT;
		const double TITLE_TRIANGLE_WIDTH = 6 * UNIT;
		const double TITLE_TRIANGLE_RADIUS_TOP = (71 * UNIT) + TITLE_TRIANGLE_HEIGHT;


		static double ToRad(double deg) => deg * Math.PI / 180;

		AbsoluteLayout canvas = new();

		public FCS_TrueIndex()
		{
			Background = null;
			Content = canvas;
			DrawRing();
			DrawScales();
			DrawScaleLabels();

			canvas.Children.Add(new Polygon()
			{
				Points = new()
				{
					new(0, 0),
					new(TITLE_TRIANGLE_WIDTH, 0),
					new(TITLE_TRIANGLE_WIDTH / 2, TITLE_TRIANGLE_HEIGHT),
				},
				Fill = Brush.White,
				Margin = new(RADIUS - (TITLE_TRIANGLE_WIDTH / 2), RADIUS - TITLE_TRIANGLE_RADIUS_TOP)
			});

			ContentView elem = getTextElem("TRUE INDEX", 0, new(RADIUS, RADIUS - TITLE_RADIUS));
			(elem.Content as Label).TextColor = Color.Black;
			(elem.Content as Label).Background = Brush.White;
			(elem.Content as Label).Padding = new(UNIT, 0);
			canvas.Children.Add(elem);
		}

		void DrawRing()
		{
			double in_radius = RADIUS - 5;
			canvas.Children.Add(new Path()
			{
				Stroke = Brush.Black,
				StrokeThickness = FlightComputerSim.THICKNESS_SEMIBOLD,
				Fill = new SolidColorBrush(Color.FromRgba(0, 0, 0, 0.15)),
				Data = new PathGeometry()
				{
					FillRule = FillRule.Nonzero,
					Figures = new()
					{
						new()
						{
							StartPoint = new(RADIUS - in_radius, RADIUS),
							Segments = new()
							{
								new ArcSegment()
								{
									Point = new(RADIUS + in_radius, RADIUS),
									Size = new(in_radius, in_radius),
								},
								new ArcSegment()
								{
									Point = new(RADIUS - in_radius, RADIUS),
									Size = new(in_radius, in_radius),
									IsLargeArc = true,
								},
							}
						},
						new()
						{
							StartPoint = new(RADIUS - CENTER_RADIUS, RADIUS),
							Segments = new()
							{
								new ArcSegment()
								{
									Point = new(RADIUS + CENTER_RADIUS, RADIUS),
									Size = new(CENTER_RADIUS, CENTER_RADIUS),
									SweepDirection = SweepDirection.Clockwise,
								},
								new ArcSegment()
								{
									Point = new(RADIUS - CENTER_RADIUS, RADIUS),
									Size = new(CENTER_RADIUS, CENTER_RADIUS),
									IsLargeArc = true,
									SweepDirection = SweepDirection.Clockwise,
								},
							}
						},
					}
				},
			});

			canvas.Children.Add(new Path()
			{
				Stroke = null,
				Fill = Brush.Black,
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
			for (double deg = -50; deg <= 50; deg++)
			{
				double _height = (deg % 5) == 0 ? SCALE_LEN_L : SCALE_LEN_S;

				double r_in = (RADIUS - ARC_THICKNESS);
				double r_out = r_in + _height;

				canvas.Children.Add(new Line()
				{
					X1 = RADIUS + (r_out * Math.Sin(ToRad(deg))),
					Y1 = RADIUS - (r_out * Math.Cos(ToRad(deg))),
					X2 = RADIUS + (r_in * Math.Sin(ToRad(deg))),
					Y2 = RADIUS - (r_in * Math.Cos(ToRad(deg))),
					Stroke = Brush.White,
					StrokeThickness = SCALE_THICKNESS,
				});
			}
		}

		static ContentView getTextElem(string str, double deg, Point elemCenter)
			=> getTextElem(deg, elemCenter, new()
			{
				Text = str,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
				FontSize = FCS_Compass.DIRECTION_LABEL_FONTSIZE_S,
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

		void DrawScaleLabels()
		{
			for (int deg = 10; deg <= 50; deg += 10)
			{
				string s = deg.ToString();
				double d = deg;

				canvas.Children.Add(getTextElem(s, d,
					new(
						RADIUS + (LABEL_RADIUS * Math.Sin(ToRad(d))),
						RADIUS - (LABEL_RADIUS * Math.Cos(ToRad(d)))
					)));

				d *= -1;
				canvas.Children.Add(getTextElem(s, d,
					new(
						RADIUS + (LABEL_RADIUS * Math.Sin(ToRad(d))),
						RADIUS - (LABEL_RADIUS * Math.Cos(ToRad(d)))
					)));
			}
		}
	}
}
