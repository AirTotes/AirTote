using Microsoft.Maui.Handlers;

namespace AirTote.Components;

public partial class ScratchPadViewHandler
{
	public static IPropertyMapper<ScratchPadView, ScratchPadViewHandler> PropertyMapper = new PropertyMapper<ScratchPadView, ScratchPadViewHandler>(ViewHandler.ViewMapper)
	{
		[nameof(ScratchPadView.IsToolPickerVisible)] = MapIsToolPickerVisible,
	};

	public ScratchPadViewHandler() : base(PropertyMapper) { }

	public static partial void MapIsToolPickerVisible(ScratchPadViewHandler handler, ScratchPadView scratchPad);
}
