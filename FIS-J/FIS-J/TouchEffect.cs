using System;
using System.Collections.Generic;
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

		readonly Dictionary<long, Point> LastLocationDic = new();
		readonly Dictionary<long, Point> LastAbsLocationDic = new();

		public void OnTouchAction(Element element, TouchActionEventArgs args)
		{
			long id = args.Id;
			args.LastLocation = LastLocationDic.TryGetValue(id, out var p) ? p : new();
			args.LastAbsLocation = LastAbsLocationDic.TryGetValue(id, out var p_abs) ? p_abs : new();
			LastLocationDic[id] = args.Location;
			LastAbsLocationDic[id] = args.AbsoluteLocation;
			TouchAction?.Invoke(element, args);
		}
	}
}