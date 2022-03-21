using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Android.Views;

[assembly: ResolutionGroupName("XamarinDocs")]
[assembly: ExportEffect(typeof(TouchTracking.Droid.TouchEffect), "TouchEffect")]

namespace TouchTracking.Droid
{
	public class TouchEffect : PlatformEffect
	{
		Android.Views.View view;
		Element formsElement;
		TouchTracking.TouchEffect libTouchEffect;
		bool capture;
		Func<double, double> fromPixels;
		readonly int[] twoIntArray = new int[2];

		static readonly Dictionary<Android.Views.View, TouchEffect> viewDictionary = new();

		static readonly Dictionary<int, TouchEffect> idToEffectDictionary = new();

		protected override void OnAttached()
		{
			view = Control ?? Container;

			if (Element.Effects.FirstOrDefault(e => e is TouchTracking.TouchEffect) is not TouchTracking.TouchEffect touchEffect || view is null)
				return;

			viewDictionary.Add(view, this);
			formsElement = Element;
			libTouchEffect = touchEffect;
			fromPixels = view.Context.FromPixels;
			view.Touch += OnTouch;
		}

		protected override void OnDetached()
		{
			if (viewDictionary.ContainsKey(view))
			{
				viewDictionary.Remove(view);
				view.Touch -= OnTouch;
			}
		}

		void OnTouch(object sender, Android.Views.View.TouchEventArgs args)
		{
			Android.Views.View senderView = sender as Android.Views.View;
			MotionEvent motionEvent = args.Event;

			int pointerIndex = motionEvent.ActionIndex;
			int id = motionEvent.GetPointerId(pointerIndex);


			senderView.GetLocationOnScreen(twoIntArray);
			Point screenPointerCoords = new(
				twoIntArray[0] + motionEvent.GetX(pointerIndex),
				twoIntArray[1] + motionEvent.GetY(pointerIndex)
			);

			switch (args.Event.ActionMasked)
			{
				case MotionEventActions.Down:
				case MotionEventActions.PointerDown:
					FireEvent(this, id, TouchActionType.Pressed, screenPointerCoords, true);

					idToEffectDictionary[id] = this;
					capture = libTouchEffect.Capture;
					break;

				case MotionEventActions.Move:
					for (pointerIndex = 0; pointerIndex < motionEvent.PointerCount; pointerIndex++)
					{
						id = motionEvent.GetPointerId(pointerIndex);

						if (capture)
						{
							senderView.GetLocationOnScreen(twoIntArray);

							screenPointerCoords = new(
								twoIntArray[0] + motionEvent.GetX(pointerIndex),
								twoIntArray[1] + motionEvent.GetY(pointerIndex)
							);

							FireEvent(this, id, TouchActionType.Moved, screenPointerCoords, true);
						}
						else
						{
							CheckForBoundaryHop(id, screenPointerCoords);

							if (idToEffectDictionary[id] is not null)
								FireEvent(idToEffectDictionary[id], id, TouchActionType.Moved, screenPointerCoords, true);
						}
					}
					break;

				case MotionEventActions.Up:
				case MotionEventActions.Pointer1Up:
					if (capture)
					{
						FireEvent(this, id, TouchActionType.Released, screenPointerCoords, false);
					}
					else
					{
						CheckForBoundaryHop(id, screenPointerCoords);

						if (idToEffectDictionary[id] is not null)
							FireEvent(idToEffectDictionary[id], id, TouchActionType.Released, screenPointerCoords, false);
					}

					idToEffectDictionary.Remove(id);
					break;

				case MotionEventActions.Cancel:
					if (capture)
						FireEvent(this, id, TouchActionType.Cancelled, screenPointerCoords, false);
					else if (idToEffectDictionary[id] is not null)
						FireEvent(idToEffectDictionary[id], id, TouchActionType.Cancelled, screenPointerCoords, false);

					idToEffectDictionary.Remove(id);
					break;
			}
		}

		void CheckForBoundaryHop(int id, Point pointerLocation)
		{
			TouchEffect touchEffectHit = null;

			foreach (Android.Views.View view in viewDictionary.Keys)
			{
				try
				{
					view.GetLocationOnScreen(twoIntArray);
				}
				catch (ObjectDisposedException)
				{
					continue;
				}

				Rectangle viewRect = new(twoIntArray[0], twoIntArray[1], view.Width, view.Height);
				if (viewRect.Contains(pointerLocation))
					touchEffectHit = viewDictionary[view];
			}

			if (touchEffectHit != idToEffectDictionary[id])
			{
				if (idToEffectDictionary[id] is not null)
					FireEvent(idToEffectDictionary[id], id, TouchActionType.Exited, pointerLocation, true);

				if (touchEffectHit is not null)
					FireEvent(touchEffectHit, id, TouchActionType.Entered, pointerLocation, true);

				idToEffectDictionary[id] = touchEffectHit;
			}
		}

		void FireEvent(TouchEffect touchEffect, int id, TouchActionType actionType, Point pointerLocation, bool isInContact)
		{
			Action<Element, TouchActionEventArgs> onTouchAction = touchEffect.libTouchEffect.OnTouchAction;

			touchEffect.view.GetLocationOnScreen(twoIntArray);

			double x = pointerLocation.X - twoIntArray[0];
			double y = pointerLocation.Y - twoIntArray[1];

			Point point = new(fromPixels(x), fromPixels(y));
			Point absPoint = new(fromPixels(pointerLocation.X), fromPixels(pointerLocation.Y));

			onTouchAction(touchEffect.formsElement, new(id, actionType, point, isInContact, absPoint));
		}
	}
}