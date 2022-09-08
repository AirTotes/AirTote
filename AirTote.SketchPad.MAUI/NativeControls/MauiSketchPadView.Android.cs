using AirTote.SketchPad.Controls;

using Android.Content;

namespace AirTote.SketchPad.NativeControls;

public class MauiSketchPadView : Android.Views.View
{
	SketchPadView _VirtualView { get; }

	public MauiSketchPadView(Context ctx, SketchPadView virtualView) : base(ctx)
	{
		_VirtualView = virtualView;
	}
}
