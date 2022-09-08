using AirTote.PlatformSpecific;
using AirTote.SketchPad.Controls;

using Microsoft.Maui.Handlers;

namespace AirTote.SketchPad.Handlers;

public partial class SketchPadViewHandler : ViewHandler<SketchPadView, MauiSketchPadView>
{
	protected override MauiSketchPadView CreatePlatformView() => new(VirtualView);

	protected override void ConnectHandler(MauiSketchPadView platformView) => base.ConnectHandler(platformView);

	public static partial void MapIsToolPickerVisible(SketchPadViewHandler handler, SketchPadView sketchPad)
		=> handler?.PlatformView.UpdateIsToolPickerVisible();
	public static partial void MapCanvasHeight(SketchPadViewHandler handler, SketchPadView sketchPad)
		=> handler?.PlatformView.UpdateCanvasHeight();
	public static partial void MapCanvasWidth(SketchPadViewHandler handler, SketchPadView sketchPad)
		=> handler?.PlatformView.UpdateCanvasWidth();
	public static partial void MapHeight(SketchPadViewHandler handler, SketchPadView sketchPad)
		=> handler?.PlatformView.UpdateCanvasHeight();
	public static partial void MapWidth(SketchPadViewHandler handler, SketchPadView sketchPad)
		=> handler?.PlatformView.UpdateCanvasWidth();
}
