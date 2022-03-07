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

		public FlightComputerSim()
		{
			over_grid.Children.Add(TrueIndex);
			over_grid.Children.Add(Compass);

			base_grid.Children.Add(TASArc);
			base_grid.Children.Add(over_grid);

			Content = base_grid;
		}
	}
}
