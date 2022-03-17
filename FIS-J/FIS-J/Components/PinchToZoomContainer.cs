using System;
using FIS_J.Components;
using Xamarin.Forms;

//ref: https://github.com/xamarin/xamarin-forms-samples/blob/main/WorkingWithGestures/PinchGesture/PinchGesture/PinchToZoomContainer.cs

namespace PinchGesture
{
	public class PinchToZoomContainer : ContentView
	{
		double currentScale = 1;
		double startScale = 1;
		double xOffset = 0;
		double yOffset = 0;

		static double ValueIn(double min, double value, double max)
			=> value <= min ? min : (max <= value ? max : value);

		public PinchToZoomContainer()
		{
			var pinchGesture = new PinchGestureRecognizer();
			pinchGesture.PinchUpdated += OnPinchUpdated;
			GestureRecognizers.Add(pinchGesture);

			AnchorX = 0.5;
			AnchorY = 0;
		}

		double? _ContentHeight = null;
		double ContentHeight
		{
			get => _ContentHeight ?? Content.Height;
			set => _ContentHeight = value <= 0 ? null : value;
		}
		double? _ContentWidth = null;
		double ContentWidth
		{
			get => _ContentWidth ?? Content.Width;
			set => _ContentWidth = value <= 0 ? null : value;
		}

		void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
		{
			switch (e.Status)
			{
				case GestureStatus.Started:
					startScale = Content.Scale;
					Content.AnchorX = 0;
					Content.AnchorY = 0;
					break;

				case GestureStatus.Running:
					currentScale += (e.Scale - 1) * startScale;
					currentScale = Math.Max(1, currentScale);

					double renderedX = Content.X + xOffset;
					double deltaX = renderedX / Width;
					double deltaWidth = Width / (ContentWidth * startScale);
					double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

					double renderedY = Content.Y + yOffset;
					double deltaY = renderedY / Height;
					double deltaHeight = Height / (ContentHeight * startScale);
					double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

					double targetX = xOffset - (originX * ContentWidth) * (currentScale - startScale);
					double targetY = yOffset - (originY * ContentHeight) * (currentScale - startScale);

					Content.TranslationX = ValueIn(-ContentWidth * (currentScale - 1), targetX, 0);
					Content.TranslationY = ValueIn(-ContentHeight * (currentScale - 1), targetY, 0);

					Content.Scale = currentScale;
					break;

				case GestureStatus.Completed:
					xOffset = Content.TranslationX;
					yOffset = Content.TranslationY;
					break;
			}
		}

		protected override void OnChildAdded(Element child)
		{
			base.OnChildAdded(child);

			if (child is FlightComputerSim fcs)
			{
				ContentHeight = fcs.Height;
				ContentWidth = fcs.Width;
			}
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);

			if (width <= 0 || height <= 0)
				return;

			double scaleX = width / ContentWidth;
			double scaleY = height / ContentHeight;

			Scale = Math.Min(scaleX, scaleY);
		}
	}
}