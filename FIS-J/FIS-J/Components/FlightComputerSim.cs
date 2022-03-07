using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Xamarin.Forms;

namespace FIS_J.Components
{
	public class FlightComputerSim : ContentView
	{
		readonly AbsoluteLayout base_grid = new()
		{
			HorizontalOptions = LayoutOptions.Center,
		};
		readonly AbsoluteLayout over_grid = new();
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
