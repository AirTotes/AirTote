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
			=> Math.Min(max, Math.Max(value, min));

		public PinchToZoomContainer()
		{
			var pinchGesture = new PinchGestureRecognizer();
			pinchGesture.PinchUpdated += OnPinchUpdated;
			GestureRecognizers.Add(pinchGesture);

			AnchorX = 0;
			AnchorY = 0;
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
					double deltaWidth = Width / (Content.Width * startScale);
					double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

					double renderedY = Content.Y + yOffset;
					double deltaY = renderedY / Height;
					double deltaHeight = Height / (Content.Height * startScale);
					double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

					double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
					double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

					double translationXMax = Content.Width * (currentScale - 1);
					double translationYMax = Content.Height * (currentScale - 1);

					Content.TranslationX = ValueIn(-translationXMax, targetX, 0);
					Content.TranslationY = ValueIn(-translationYMax, targetY, 0);

					Content.Scale = currentScale;
					break;

				case GestureStatus.Completed:
					xOffset = Content.TranslationX;
					yOffset = Content.TranslationY;
					break;
			}
		}
	}
}