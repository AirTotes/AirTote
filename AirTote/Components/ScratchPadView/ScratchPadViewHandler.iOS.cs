using AirTote.PlatformSpecific;

using Microsoft.Maui.Handlers;

namespace AirTote.Components;

public partial class ScratchPadViewHandler : ViewHandler<ScratchPadView, MauiScratchPadView>
{
	protected override MauiScratchPadView CreatePlatformView() => new(VirtualView);

	protected override void ConnectHandler(MauiScratchPadView platformView) => base.ConnectHandler(platformView);

	public static partial void MapIsToolPickerVisible(ScratchPadViewHandler handler, ScratchPadView scratchPad)
		=> handler?.PlatformView.UpdateIsToolPickerVisible();
	public static partial void MapCanvasHeight(ScratchPadViewHandler handler, ScratchPadView scratchPad)
		=> handler?.PlatformView.UpdateCanvasHeight();
	public static partial void MapCanvasWidth(ScratchPadViewHandler handler, ScratchPadView scratchPad)
		=> handler?.PlatformView.UpdateCanvasWidth();
	public static partial void MapHeight(ScratchPadViewHandler handler, ScratchPadView scratchPad)
		=> handler?.PlatformView.UpdateCanvasHeight();
	public static partial void MapWidth(ScratchPadViewHandler handler, ScratchPadView scratchPad)
		=> handler?.PlatformView.UpdateCanvasWidth();
}
