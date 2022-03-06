using System;

using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace FIS_J.Components
{
	public partial class FlightComputerSim : ContentView
	{
		public const double UNIT = 5;
		const double BASE_RADIUS = 270;
		const double BASE_RADIUS_MIN = 36;
		const double BASE_WIDTH = 104;
		const double Radius = BASE_RADIUS * UNIT;
		const double E6BWidth = BASE_WIDTH * UNIT;

		static double ToRad(double deg) => deg * Math.PI / 180;

		const double HalfWidth = E6BWidth / 2;
		const double Per2DegStartRadius = BASE_RADIUS_MIN * UNIT;
		const double Per1DegStartRadius = 100 * UNIT;
		const double MAX_DEG = 89.0;
		const double E6BHeight = Radius - Per2DegStartRadius;
		static readonly double NON_FULL_ARC_RADIUS_LESS_THAN = Math.Sqrt(Math.Pow(Per2DegStartRadius, 2) + Math.Pow(HalfWidth, 2));

		const double THICKNESS_BOLD = 3;
		const double THICKNESS_SEMIBOLD = 2;
		const double THICKNESS_NORMAL = 1;

		const double LABEL_HEIGHT = 20;
		const double LABEL_WIDTH = 40;
		const double LABEL_HEIGHT_HALF = LABEL_HEIGHT / 2;
		const double LABEL_WIDTH_HALF = LABEL_WIDTH / 2;

		AbsoluteLayout canvas = new();

		public FlightComputerSim()
		{
			Background = Brush.White;
			Content = canvas;
			HeightRequest = canvas.HeightRequest = E6BHeight;
			WidthRequest = canvas.WidthRequest = E6BWidth;

			DrawRadians();
			DrawArcs();
			DrawRadTexts();
		}

		void DrawRadians()
		{
			for (double i = -MAX_DEG; i <= MAX_DEG; i += 1)
			{
				double RAD_THETA = ToRad(i);
				double TAN_THETA = Math.Tan(RAD_THETA);
				double LINE_END_RADIUS = (i % 2) == 0 ? Per2DegStartRadius : Per1DegStartRadius;

				double x1tmp = HalfWidth + (Radius * TAN_THETA);
				double x2tmp = HalfWidth + (LINE_END_RADIUS * Math.Sin(RAD_THETA));
				double y1tmp = Radius - (TAN_THETA == 0 ? Radius : (HalfWidth / Math.Abs(TAN_THETA)));
				double y2tmp = Radius - Math.Abs(LINE_END_RADIUS * Math.Cos(RAD_THETA));

				if ((x1tmp <= 0 && x2tmp <= 0) || (E6BWidth <= x1tmp && E6BWidth <= x2tmp))
					continue;

				if (y1tmp >= E6BHeight)
					continue;

				double x1 = x1tmp switch
				{
					(<= 0) => 0,
					(>= E6BWidth) => E6BWidth,
					(var v) => v,
				};
				double x2 = x2tmp switch
				{
					(<= 0) => 0,
					(>= E6BWidth) => E6BWidth,
					(var v) => y2tmp <= E6BHeight ? v : (HalfWidth + (Per2DegStartRadius * TAN_THETA)),
				};

				double y1 = y1tmp <= 0 ? 0 : y1tmp;
				double y2 = E6BHeight <= y2tmp ? E6BHeight : y2tmp;

				Line line = new()
				{
					X1 = x1,
					X2 = x2,
					Y1 = y1,
					Y2 = y2,
					Stroke = Brush.Black,
					StrokeThickness =
						(i % 10) == 0
						? THICKNESS_BOLD
						: (
							(i % 5) == 0
							? THICKNESS_SEMIBOLD
							: THICKNESS_NORMAL
						),
				};
				canvas.Children.Add(line);
			}
		}

		void DrawArcs()
		{
			PathGeometry pathGeo_Normal = new();
			PathGeometry pathGeo_SemiBold = new();
			PathGeometry pathGeo_Bold = new();

			for (double r = BASE_RADIUS_MIN; r <= BASE_RADIUS; r += 1)
			{
				double i = r * UNIT;

				double x = i < NON_FULL_ARC_RADIUS_LESS_THAN ? (HalfWidth - Math.Sqrt(Math.Pow(i, 2) - Math.Pow(Per2DegStartRadius, 2))) : 0;
				double y = x == 0 ? (Radius - Math.Sqrt(Math.Pow(i, 2) - Math.Pow(HalfWidth, 2))) : E6BHeight;

				((r % 10) == 0
					? pathGeo_Bold
					: ((r % 5) == 0 ? pathGeo_SemiBold : pathGeo_Normal))
					.Figures.Add(
					new PathFigure()
					{
						StartPoint = new(E6BWidth - x, y),
						Segments = new()
						{
							new ArcSegment()
							{
								Point = new(x, y),
								Size = new(i, i),
							}
						}
					}
				);
			}

			canvas.Children.Add(new Path()
			{
				Stroke = Brush.Black,
				StrokeThickness = THICKNESS_NORMAL,
				Data = pathGeo_Normal,
			});
			canvas.Children.Add(new Path()
			{
				Stroke = Brush.Black,
				StrokeThickness = THICKNESS_SEMIBOLD,
				Data = pathGeo_SemiBold,
			});
			canvas.Children.Add(new Path()
			{
				Stroke = Brush.Black,
				StrokeThickness = THICKNESS_BOLD,
				Data = pathGeo_Bold,
			});
		}

		void DrawRadTexts()
		{
			ContentView getElem(double deg, Thickness margin) => new()
			{
				HeightRequest = LABEL_HEIGHT,
				WidthRequest = LABEL_WIDTH,
				Margin = margin,
				Content = new Label()
				{
					Text = $"{Math.Abs(deg)}°",
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					HorizontalTextAlignment = TextAlignment.Center,
					VerticalTextAlignment = TextAlignment.Center,
					Background = Brush.White,
					TextColor = Color.Black,
				},
				AnchorX = 0.5,
				AnchorY = 0.5,
				Rotation = deg,
			};

			for (double orig_radius = 55; orig_radius < BASE_RADIUS; orig_radius += 40)
			{
				for (double deg = 5; deg <= 40; deg += 5)
				{
					if (orig_radius <= 100 && (deg % 10) != 0)
						continue;
					if (HalfWidth <= (orig_radius * UNIT * Math.Tan(ToRad(deg))))
						continue;

					double radius = orig_radius * UNIT;
					double move_x = radius * Math.Sin(ToRad(deg));
					double new_center_from_bottom = radius * Math.Cos(ToRad(deg));

					canvas.Children.Add(getElem(deg, new(HalfWidth + move_x - LABEL_WIDTH_HALF, Radius - new_center_from_bottom - LABEL_HEIGHT_HALF)));
					canvas.Children.Add(getElem(-deg, new(HalfWidth - move_x - LABEL_WIDTH_HALF, Radius - new_center_from_bottom - LABEL_HEIGHT_HALF)));
				}
			}
		}
	}
}