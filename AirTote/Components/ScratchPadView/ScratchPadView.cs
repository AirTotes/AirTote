using DependencyPropertyGenerator;

namespace AirTote.Components;

public interface IScratchPadView
{
	Page? HostPage { get; set; }
	bool IsToolPickerVisible { get; set; }
}

[DependencyProperty<Page>("HostPage")]
[DependencyProperty<bool>("IsToolPickerVisible")]
public partial class ScratchPadView : View, IScratchPadView
{
}
