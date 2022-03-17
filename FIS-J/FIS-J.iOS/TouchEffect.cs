using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using UIKit;

[assembly: ResolutionGroupName("XamarinDocs")]
[assembly: ExportEffect(typeof(TouchTracking.iOS.TouchEffect), "TouchEffect")]

namespace TouchTracking.iOS
{
	public class TouchEffect : PlatformEffect
	{
		UIView view;
		TouchRecognizer touchRecognizer;

		protected override void OnAttached()
		{
			view = Control ?? Container;

			if (Element.Effects.FirstOrDefault(e => e is TouchTracking.TouchEffect) is not TouchTracking.TouchEffect effect || view is null)
				return;

			touchRecognizer = new TouchRecognizer(Element, view, effect);
			view.AddGestureRecognizer(touchRecognizer);
		}

		protected override void OnDetached()
		{
			if (touchRecognizer is not null)
			{
				touchRecognizer.Detach();
				view.RemoveGestureRecognizer(touchRecognizer);
			}
		}
	}
}