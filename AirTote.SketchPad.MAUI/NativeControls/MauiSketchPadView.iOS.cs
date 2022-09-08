using AirTote.SketchPad.Controls;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;

using PencilKit;

using System;

using UIKit;

namespace AirTote.PlatformSpecific;

public class MauiSketchPadView : PKCanvasView
{
	PKToolPicker? _toolPicker;

	SketchPadView _VirtualView { get; }

	Page? _hostPage;

	public MauiSketchPadView(SketchPadView virtualView)
	{
		if (DeviceInfo.Version < new Version(13, 0))
			throw new NotSupportedException("PencilKit only supported iOS 12 or above");

		_VirtualView = virtualView;

		if (DeviceInfo.Version < new Version(14, 0))
		{
			Console.WriteLine("iOS 13.0 Detected and Launching ToolPicker...");
			_toolPicker = PKToolPicker.GetSharedToolPicker(Window);
		}
		else
			_toolPicker = new();

		this.Tool = new PKInkingTool(PKInkType.Pencil, UIColor.Red);
		_toolPicker?.AddObserver(this);

		UpdateMaximumZoomScale();
		UpdateMinimumZoomScale();

		UpdateCanvasSize();
		this.ContentOffset = new(_CanvasWidth * 1.5, _CanvasWidth * 1.5);

		UpdateIsToolPickerVisible();

		BackgroundColor = UIColor.SystemGray6;
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

	double _DefaultHeight => _VirtualView.Height * 2;
	double _DefaultWidth => _VirtualView.Width * 2;

	double _CanvasHeight => _VirtualView.CanvasHeight < 0
		? (this.ContentSize.Height > _DefaultHeight ? this.ContentSize.Height : _DefaultHeight)
		: _VirtualView.CanvasHeight;
	double _CanvasWidth => _VirtualView.CanvasWidth < 0
		? (this.ContentSize.Width > _DefaultWidth ? this.ContentSize.Width : _DefaultWidth)
		: _VirtualView.CanvasWidth;

	public void UpdateCanvasHeight()
		=> this.ContentSize = new(this.ContentSize.Width, _CanvasHeight);
	public void UpdateCanvasWidth()
		=> this.ContentSize = new(_CanvasWidth, this.ContentSize.Height);
	public void UpdateCanvasSize()
		=> this.ContentSize = new(_CanvasWidth, _CanvasHeight);

	public void UpdateMaximumZoomScale()
		=> this.MaximumZoomScale = new(_VirtualView.MaximumZoomScale);
	public void UpdateMinimumZoomScale()
		=> this.MinimumZoomScale = new(_VirtualView.MinimumZoomScale);
}
