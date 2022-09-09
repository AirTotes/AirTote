using DependencyPropertyGenerator;

using Microsoft.Maui.Controls;

using System;

namespace AirTote.SketchPad.Controls;

public interface ISketchPadView
{
	Page? HostPage { get; set; }
	bool IsToolPickerVisible { get; set; }
	double CanvasHeight { get; set; }
	double CanvasWidth { get; set; }
	double MaximumZoomScale { get; set; }
	double MinimumZoomScale { get; set; }
}

[DependencyProperty<Page>("HostPage")]
[DependencyProperty<bool>("IsToolPickerVisible", DefaultValue = true)]
[DependencyProperty<double>("CanvasHeight", DefaultValue = -1)]
[DependencyProperty<double>("CanvasWidth", DefaultValue = -1)]
[DependencyProperty<double>("MaximumZoomScale", DefaultValue = 4.0)]
[DependencyProperty<double>("MinimumZoomScale", DefaultValue = 0.5)]
public partial class SketchPadView : View, ISketchPadView
{
	public IDrawingData? DrawingData { get; internal set; }

	partial void OnHostPageChanging(Page? oldValue, Page? newValue)
	{
		if (oldValue is not null)
		{
			oldValue.Appearing -= HostPageAppearing;
			oldValue.Disappearing -= HostPageDisappearing;
		}
		if (newValue is not null)
		{
			newValue.Appearing += HostPageAppearing;
			newValue.Disappearing += HostPageDisappearing;
		}
	}

	private void HostPageAppearing(object? sender, EventArgs e)
		=> IsToolPickerVisible = true;
	private void HostPageDisappearing(object? sender, EventArgs e)
		=> IsToolPickerVisible = false;
}
