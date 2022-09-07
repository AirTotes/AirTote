using DependencyPropertyGenerator;

namespace AirTote.Components;

public interface IScratchPadView
{
	Page? HostPage { get; set; }
	bool IsToolPickerVisible { get; set; }
}

[DependencyProperty<Page>("HostPage")]
[DependencyProperty<bool>("IsToolPickerVisible", DefaultValue = true)]
[DependencyProperty<float>("CanvasHeight", DefaultValue = -1)]
[DependencyProperty<float>("CanvasWidth", DefaultValue = -1)]
public partial class ScratchPadView : View, IScratchPadView
{
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
