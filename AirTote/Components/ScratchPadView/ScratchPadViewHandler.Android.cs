using AirTote.Components;
using AirTote.PlatformSpecific;

using Microsoft.Maui.Handlers;

namespace AirTote.Components;

public partial class ScratchPadViewHandler : ViewHandler<ScratchPadView, MauiScratchPadView>
{
	protected override MauiScratchPadView CreatePlatformView() => new(Context, VirtualView);

	public static partial void MapIsToolPickerVisible(ScratchPadViewHandler handler, ScratchPadView scratchPad)
		=> throw new NotImplementedException();
	public static partial void MapCanvasHeight(ScratchPadViewHandler handler, ScratchPadView scratchPad)
		=> throw new NotImplementedException();
	public static partial void MapCanvasWidth(ScratchPadViewHandler handler, ScratchPadView scratchPad)
		=> throw new NotImplementedException();
	public static partial void MapHeight(ScratchPadViewHandler handler, ScratchPadView scratchPad)
		=> throw new NotImplementedException();
	public static partial void MapWidth(ScratchPadViewHandler handler, ScratchPadView scratchPad)
		=> throw new NotImplementedException();
}
