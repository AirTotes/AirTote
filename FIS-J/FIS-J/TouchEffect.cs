using System;

using Xamarin.Forms;

namespace TouchTracking
{
	public enum TouchActionType
	{
		Entered,
		Pressed,
		Moved,
		Released,
		Exited,
		Cancelled
	}
	public delegate void TouchActionEventHandler(object sender, TouchActionEventArgs args);

	public class TouchActionEventArgs : EventArgs
	{
		public TouchActionEventArgs(long id, TouchActionType type, Point location, bool isInContact, Point absoluteLocation)
		{
			Id = id;
			Type = type;
			Location = location;
			IsInContact = isInContact;
			AbsoluteLocation = absoluteLocation;
		}

		public long Id { private set; get; }

		public TouchActionType Type { private set; get; }

		public Point Location { private set; get; }
		public Point LastLocation { set; get; }
		public Point AbsoluteLocation { set; get; }
		public Point LastAbsLocation { set; get; }

		public bool IsInContact { private set; get; }
	}

	public class TouchEffect : RoutingEffect
	{
		public event TouchActionEventHandler TouchAction;

		public TouchEffect() : base("XamarinDocs.TouchEffect")
		{
		}

		public bool Capture { set; get; }
		public Point LastLocation { private get; set; }
		public Point LastAbsLocation { private get; set; }

		public void OnTouchAction(Element element, TouchActionEventArgs args)
		{
			args.LastLocation = LastLocation;
			args.LastAbsLocation = LastAbsLocation;
			LastLocation = args.Location;
			LastAbsLocation = args.AbsoluteLocation;
			TouchAction?.Invoke(element, args);
		}
	}
}