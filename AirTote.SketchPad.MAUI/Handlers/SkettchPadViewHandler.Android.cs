using AirTote.SketchPad.Controls;
using AirTote.SketchPad.NativeControls;

using Microsoft.Maui.Handlers;

using System;

namespace AirTote.SketchPad.Handlers;

public partial class SketchPadViewHandler : ViewHandler<SketchPadView, MauiSketchPadView>
{
	protected override MauiSketchPadView CreatePlatformView() => new(Context, VirtualView);

	public static partial void MapIsToolPickerVisible(SketchPadViewHandler handler, SketchPadView sketchPad)
		=> throw new NotImplementedException();
	public static partial void MapCanvasHeight(SketchPadViewHandler handler, SketchPadView sketchPad)
		=> throw new NotImplementedException();
	public static partial void MapCanvasWidth(SketchPadViewHandler handler, SketchPadView sketchPad)
		=> throw new NotImplementedException();
	public static partial void MapHeight(SketchPadViewHandler handler, SketchPadView sketchPad)
		=> throw new NotImplementedException();
	public static partial void MapWidth(SketchPadViewHandler handler, SketchPadView sketchPad)
		=> throw new NotImplementedException();
	public static partial void MapMaximumZoomScale(SketchPadViewHandler handler, SketchPadView sketchPad)
		=> throw new NotImplementedException();
	public static partial void MapMinimumZoomScale(SketchPadViewHandler handler, SketchPadView sketchPad)
		=> throw new NotImplementedException();
}
