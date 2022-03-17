using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

using CoreGraphics;
using Foundation;
using UIKit;

namespace TouchTracking.iOS
{
	class TouchRecognizer : UIGestureRecognizer
	{
		readonly Element element;
		readonly UIView view;
		readonly TouchTracking.TouchEffect touchEffect;
		bool capture;

		static readonly Dictionary<UIView, TouchRecognizer> viewDictionary = new();

		static readonly Dictionary<long, TouchRecognizer> idToTouchDictionary = new();

		public TouchRecognizer(Element element, UIView view, TouchTracking.TouchEffect touchEffect)
		{
			this.element = element;
			this.view = view;
			this.touchEffect = touchEffect;

			viewDictionary.Add(view, this);
		}

		public void Detach() => viewDictionary.Remove(view);
		

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);

			foreach (UITouch touch in touches.Cast<UITouch>())
			{
				long id = touch.Handle.ToInt64();
				FireEvent(this, id, TouchActionType.Pressed, touch, true);

				if (!idToTouchDictionary.ContainsKey(id))
					idToTouchDictionary.Add(id, this);
			}

			capture = touchEffect.Capture;
		}

		public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			base.TouchesMoved(touches, evt);

			foreach (UITouch touch in touches.Cast<UITouch>())
			{
				long id = touch.Handle.ToInt64();

				if (capture)
				{
					FireEvent(this, id, TouchActionType.Moved, touch, true);
				}
				else
				{
					CheckForBoundaryHop(touch);

					if (idToTouchDictionary[id] is not null)
						FireEvent(idToTouchDictionary[id], id, TouchActionType.Moved, touch, true);
				}
			}
		}

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			base.TouchesEnded(touches, evt);

			foreach (UITouch touch in touches.Cast<UITouch>())
			{
				long id = touch.Handle.ToInt64();

				if (capture)
				{
					FireEvent(this, id, TouchActionType.Released, touch, false);
				}
				else
				{
					CheckForBoundaryHop(touch);

					if (idToTouchDictionary[id] is not null)
						FireEvent(idToTouchDictionary[id], id, TouchActionType.Released, touch, false);
				}

				idToTouchDictionary.Remove(id);
			}
		}

		public override void TouchesCancelled(NSSet touches, UIEvent evt)
		{
			base.TouchesCancelled(touches, evt);

			foreach (UITouch touch in touches.Cast<UITouch>())
			{
				long id = touch.Handle.ToInt64();

				if (capture)
					FireEvent(this, id, TouchActionType.Cancelled, touch, false);
				else if (idToTouchDictionary[id] is not null)
					FireEvent(idToTouchDictionary[id], id, TouchActionType.Cancelled, touch, false);

				idToTouchDictionary.Remove(id);
			}
		}

		void CheckForBoundaryHop(UITouch touch)
		{
			long id = touch.Handle.ToInt64();
			TouchRecognizer recognizerHit = null;

			foreach (UIView view in viewDictionary.Keys)
			{
				CGPoint location = touch.LocationInView(view);

				if (new CGRect(new CGPoint(), view.Frame.Size).Contains(location))
					recognizerHit = viewDictionary[view];
			}

			if (recognizerHit != idToTouchDictionary[id])
			{
				if (idToTouchDictionary[id] is not null)
					FireEvent(idToTouchDictionary[id], id, TouchActionType.Exited, touch, true);

				if (recognizerHit is not null)
					FireEvent(recognizerHit, id, TouchActionType.Entered, touch, true);

				idToTouchDictionary[id] = recognizerHit;
			}
		}

		void FireEvent(TouchRecognizer recognizer, long id, TouchActionType actionType, UITouch touch, bool isInContact)
		{
			CGPoint cgPoint = touch.LocationInView(recognizer.View);
			Point xfPoint = new(cgPoint.X, cgPoint.Y);

			CGPoint absCgPoint = touch.LocationInView(touch.Window);
			Point absPoint = new(absCgPoint.X, absCgPoint.Y);

			recognizer.touchEffect.OnTouchAction(recognizer.element, new(id, actionType, xfPoint, isInContact, absPoint));
		}
	}
}