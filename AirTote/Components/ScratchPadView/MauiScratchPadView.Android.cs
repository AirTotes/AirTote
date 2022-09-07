using AirTote.Components;

using Android.Content;

namespace AirTote.PlatformSpecific;

public class MauiScratchPadView : Android.Views.View
{
	ScratchPadView _VirtualView { get; }

	public MauiScratchPadView(Context ctx, ScratchPadView virtualView) : base(ctx)
	{
		_VirtualView = virtualView;
	}
}
