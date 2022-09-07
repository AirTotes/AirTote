using DependencyPropertyGenerator;

namespace AirTote.Components;

public interface IScratchPadView
{
	Page? HostPage { get; set; }
}

[DependencyProperty<Page>("HostPage")]
public partial class ScratchPadView : View, IScratchPadView
{
}
