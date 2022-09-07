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
		_VirtualView = virtualView;

		_toolPicker = new();

		this.Tool = new PKInkingTool(PKInkType.Pencil, UIColor.Red);
		_toolPicker?.AddObserver(this);

		ShowToolPicker(null, null);
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

	public void UpdateHostPage()
	{
		if (_hostPage is not null)
		{
			_hostPage.Appearing -= ShowToolPicker;
			_hostPage.Disappearing -= HideToolPicker;
		}

		_hostPage = _VirtualView.HostPage;

		if (_hostPage is not null)
		{
			_hostPage.Appearing += ShowToolPicker;
			_hostPage.Disappearing += HideToolPicker;
		}
	}

	private void ShowToolPicker(object? sender, EventArgs? e)
	{
		_toolPicker?.SetVisible(true, this);
		this.BecomeFirstResponder();
	}

	private void HideToolPicker(object? sender, EventArgs? e)
	{
		_toolPicker?.SetVisible(false, this);
	}
}
