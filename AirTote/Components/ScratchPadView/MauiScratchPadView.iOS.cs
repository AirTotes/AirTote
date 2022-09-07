using AirTote.Components;

using PencilKit;

using UIKit;

namespace AirTote.PlatformSpecific;

public class MauiScratchPadView : PKCanvasView
{
	PKToolPicker? _toolPicker;

	ScratchPadView _VirtualView { get; }

	Page? _hostPage;

	public MauiScratchPadView(ScratchPadView virtualView)
	{
		if (DeviceInfo.Version < new Version(13, 0))
			throw new NotSupportedException("PencilKit only supported iOS 12 or above");

		_VirtualView = virtualView;

		_toolPicker = new();

		this.Tool = new PKInkingTool(PKInkType.Pencil, UIColor.Red);
		_toolPicker?.AddObserver(this);

		this.MaximumZoomScale = new(4.0);
		this.MinimumZoomScale = new(0.2);

		double dispHeight = DeviceDisplay.MainDisplayInfo.Height;
		double dispWidth = DeviceDisplay.MainDisplayInfo.Width;
		double maxHeightWidth = Math.Max(dispHeight, dispWidth);

		double canvasHeightWidth = maxHeightWidth * 4;

		this.ContentSize = new(canvasHeightWidth, canvasHeightWidth);
		this.ContentOffset = new(canvasHeightWidth / 2, canvasHeightWidth / 2);

		UpdateIsToolPickerVisible();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (_toolPicker is not null)
				_toolPicker.Dispose();

			_toolPicker = null;
		}

		base.Dispose(disposing);
	}

	public void UpdateIsToolPickerVisible()
	{
		if (_VirtualView.IsToolPickerVisible)
		{
			_toolPicker?.SetVisible(true, this);
			this.BecomeFirstResponder();
		}
		else
		{
			_toolPicker?.SetVisible(false, this);
		}
	}
}
