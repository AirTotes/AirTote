using System;
using System.Windows.Input;

namespace FIS_J.ViewModels
{
	internal class FCSPageViewModel : BaseViewModel
	{
		const double MINIMUM_MAGNIFICATION = 0.1;

		public FCSPageViewModel()
		{
			Title = "Flight Computer Simulator";
		}

		private double _Scale = 1;
		public double Scale
		{
			get => _Scale;
			set => SetProperty(ref _Scale, Math.Max(value, MINIMUM_MAGNIFICATION));
		}

		private double _AnchorX = 0.5;
		public double AnchorX
		{
			get => _AnchorX;
			set => SetProperty(ref _AnchorX, value);
		}

		private double _AnchorY = 0;
		public double AnchorY
		{
			get => _AnchorY;
			set => SetProperty(ref _AnchorY, value);
		}
	}
}
