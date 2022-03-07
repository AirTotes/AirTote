using System;

using Xamarin.Forms;

namespace FIS_J.Components
{
	public class FlightComputerSim : ContentView
	{
		readonly AbsoluteLayout base_grid = new()
		{
			HorizontalOptions = LayoutOptions.Center,
			WidthRequest = FCS_TrueIndex.RADIUS * 2,
			HeightRequest = FCS_TASArc.E6BHeight,
		};
		readonly AbsoluteLayout over_grid = new()
		{
			HeightRequest = FCS_TrueIndex.RADIUS * 2,
			WidthRequest = FCS_TrueIndex.RADIUS * 2,
			HorizontalOptions = LayoutOptions.Center,
		};
		readonly FCS_TASArc TASArc = new()
		{
			Margin = new(FCS_TrueIndex.RADIUS - FCS_TASArc.HalfWidth, 0),
		};
		readonly FCS_TrueIndex TrueIndex = new()
		{
			Margin = new(0),
		};
		readonly FCS_Compass Compass = new()
		{
			Margin = new(FCS_TrueIndex.ARC_THICKNESS),
			AnchorX = 0.5,
			AnchorY = 0.5,
			HeightRequest = FCS_Compass.RADIUS * 2,
			WidthRequest = FCS_Compass.RADIUS * 2,
		};

		readonly double OverGrid_MinTop = -FCS_TrueIndex.RADIUS;
		readonly double OverGrid_MaxTop = FCS_TASArc.E6BHeight - FCS_TrueIndex.RADIUS;

		Thickness OverGridMargin = new();
		double _CompassRotation = 0;
		double CompassRotation
		{
			get => _CompassRotation;
			set => _CompassRotation = value % 360;
		}

		enum TranslateMode
		{
			None,
			Move,
			Rotate,
		}

		TranslateMode CurrentMode = TranslateMode.None;

		public FlightComputerSim()
		{
			over_grid.Children.Add(TrueIndex);
			over_grid.Children.Add(Compass);

			base_grid.Children.Add(TASArc);
			base_grid.Children.Add(over_grid);

			TouchTracking.TouchEffect CompassRotationEffect = new();

			CompassRotationEffect.TouchAction += (s, e) =>
			{
				if (e.Type == TouchTracking.TouchActionType.Pressed)
				{
					Point loc = e.Location;
					double radius = Math.Sqrt(Math.Pow(FCS_TrueIndex.RADIUS - loc.X, 2) + Math.Pow(FCS_TrueIndex.RADIUS - loc.Y, 2));
					CurrentMode = radius switch
					{
						<= FCS_TrueIndex.RADIUS and > (FCS_TrueIndex.RADIUS - FCS_TrueIndex.ARC_THICKNESS) => TranslateMode.Move,
						<= FCS_Compass.RADIUS and > (FCS_Compass.RADIUS - FCS_Compass.ARC_THICKNESS) => TranslateMode.Rotate,
						_ => TranslateMode.None,
					};
				}
				else if (CurrentMode != TranslateMode.None)
				{
					switch (CurrentMode)
					{
						case TranslateMode.Move:
							double newTopTmp = OverGridMargin.Top + (e.AbsoluteLocation.Y - e.LastAbsLocation.Y);

							if (newTopTmp < OverGrid_MinTop)
								newTopTmp = OverGrid_MinTop;
							else if (newTopTmp > OverGrid_MaxTop)
								newTopTmp = OverGrid_MaxTop;

							OverGridMargin = new(0, newTopTmp, 0, 0);
							break;

						case TranslateMode.Rotate:
							double x1 = e.LastLocation.X - FCS_TrueIndex.RADIUS;
							double y1 = e.LastLocation.Y - FCS_TrueIndex.RADIUS;
							double x2 = e.Location.X - FCS_TrueIndex.RADIUS;
							double y2 = e.Location.Y - FCS_TrueIndex.RADIUS;

							if ((x1 == 0 && x2 == 0) || (y1 == 0 && y2 == 0))
								break;

							double rad1 = 0;
							double rad2 = 0;
							if (x1 == 0)
								rad1 = y1 > 0 ? Math.PI / 2 : Math.PI * 3 / 2;
							else
								rad1 = Math.Atan(y1 / x1);

							if (x2 == 0)
								rad2 = y2 > 0 ? Math.PI / 2 : Math.PI * 3 / 2;
							else
								rad2 = Math.Atan(y2 / x2);

							CompassRotation += (rad2 - rad1) * 180 / Math.PI;
							break;
					}

					switch (e.Type)
					{
						case TouchTracking.TouchActionType.Cancelled:
						case TouchTracking.TouchActionType.Entered:
						case TouchTracking.TouchActionType.Exited:
						case TouchTracking.TouchActionType.Released:
							CurrentMode = TranslateMode.None;
							break;
					}
				}
			};

			Device.StartTimer(new(0, 0, 0, 0, 20), () =>
					{
						if (over_grid.Margin.Top != OverGridMargin.Top)
							over_grid.Margin = OverGridMargin;

						if (Compass.Rotation != CompassRotation)
							Compass.Rotation = CompassRotation;
						return true;
					});

			over_grid.Effects.Add(CompassRotationEffect);

			Content = base_grid;
		}
	}
}
