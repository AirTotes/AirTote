using Microsoft.Maui.Handlers;

namespace AirTote.Components;

public partial class ScratchPadViewHandler
{
	static readonly IPropertyMapper<ScratchPadView, ScratchPadViewHandler> PropertyMapper = new PropertyMapper<ScratchPadView, ScratchPadViewHandler>(ViewHandler.ViewMapper)
	{
		[nameof(ScratchPadView.IsToolPickerVisible)] = MapIsToolPickerVisible,
		[nameof(ScratchPadView.CanvasHeight)] = MapCanvasHeight,
		[nameof(ScratchPadView.CanvasWidth)] = MapCanvasWidth,
		[nameof(ScratchPadView.Height)] = MapCanvasHeight,
		[nameof(ScratchPadView.Width)] = MapCanvasWidth,
	};

	public ScratchPadViewHandler() : base(PropertyMapper) { }

	public static partial void MapIsToolPickerVisible(ScratchPadViewHandler handler, ScratchPadView scratchPad);
	public static partial void MapCanvasHeight(ScratchPadViewHandler handler, ScratchPadView scratchPad);
	public static partial void MapCanvasWidth(ScratchPadViewHandler handler, ScratchPadView scratchPad);
	public static partial void MapHeight(ScratchPadViewHandler handler, ScratchPadView scratchPad);
	public static partial void MapWidth(ScratchPadViewHandler handler, ScratchPadView scratchPad);
}
