using System;
using System.Collections.Generic;
using TouchTracking;

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

		enum TranslateMode
		{
			None,
			Move,
			Rotate,
		}

		readonly Dictionary<long, TranslateMode> FingerControls = new();

		public FlightComputerSim()
		{
			over_grid.Children.Add(TrueIndex);
			over_grid.Children.Add(Compass);

			base_grid.Children.Add(TASArc);
			base_grid.Children.Add(over_grid);

			TouchEffect CompassRotationEffect = new();

			CompassRotationEffect.TouchAction += CompassRotationEffect_TouchAction;
			CompassRotationEffect.Capture = true;

			over_grid.Effects.Add(CompassRotationEffect);

			Content = base_grid;
		}

		public new double Height => FCS_TASArc.E6BHeight;
		public new double Width => FCS_TrueIndex.RADIUS * 2;

		private void CompassRotationEffect_TouchAction(object sender, TouchActionEventArgs e)
		{
			if (e.Type == TouchActionType.Pressed)
			{
				OnOverGridTapped(e.Location);

				FingerControls[e.Id] = OnOverGridTapped(e.Location);
			}
			else if (FingerControls.TryGetValue(e.Id, out var CurrentMode) && CurrentMode != TranslateMode.None)
			{
				switch (CurrentMode)
				{
					case TranslateMode.Move:
						OverGridMoveFunction(e);
						break;

					case TranslateMode.Rotate:
						CompassRotationFunction(e);
						break;
				}

				switch (e.Type)
				{
					case TouchActionType.Cancelled:
					case TouchActionType.Entered:
					case TouchActionType.Exited:
					case TouchActionType.Released:
						FingerControls.Remove(e.Id);
						break;
				}
			}
		}

		private TranslateMode OnOverGridTapped(in Point loc)
		{
			double radius = Math.Sqrt(Math.Pow(FCS_TrueIndex.RADIUS - loc.X, 2) + Math.Pow(FCS_TrueIndex.RADIUS - loc.Y, 2));
			return radius switch
			{
				<= FCS_TrueIndex.RADIUS and > (FCS_TrueIndex.RADIUS - FCS_TrueIndex.ARC_THICKNESS) => TranslateMode.Move,
				<= FCS_Compass.RADIUS and > (FCS_Compass.RADIUS - FCS_Compass.ARC_THICKNESS) => TranslateMode.Rotate,
				_ => TranslateMode.None,
			};
		}

		private void OverGridMoveFunction(TouchActionEventArgs e)
		{
			double newTopTmp = over_grid.TranslationY + ((e.AbsoluteLocation.Y - e.LastAbsLocation.Y) / Scale);
			if (newTopTmp < OverGrid_MinTop)
				newTopTmp = OverGrid_MinTop;
			else if (newTopTmp > OverGrid_MaxTop)
				newTopTmp = OverGrid_MaxTop;

			over_grid.TranslationY = newTopTmp;
		}

		private void CompassRotationFunction(TouchActionEventArgs e)
		{
			double x1 = e.LastLocation.X - FCS_TrueIndex.RADIUS;
			double y1 = e.LastLocation.Y - FCS_TrueIndex.RADIUS;
			double x2 = e.Location.X - FCS_TrueIndex.RADIUS;
			double y2 = e.Location.Y - FCS_TrueIndex.RADIUS;

			if ((x1 == 0 && x2 == 0) || (y1 == 0 && y2 == 0))
				return;

			double rad1;
			double rad2;

			if (x1 == 0)
				rad1 = y1 > 0 ? Math.PI / 2 : Math.PI * 3 / 2;
			else
				rad1 = Math.Atan(y1 / x1);

			if (x2 == 0)
				rad2 = y2 > 0 ? Math.PI / 2 : Math.PI * 3 / 2;
			else
				rad2 = Math.Atan(y2 / x2);

			double newRotation = Compass.Rotation + ((rad2 - rad1) * 180 / Math.PI);
			Compass.Rotation = newRotation % 360;
		}
	}
}
