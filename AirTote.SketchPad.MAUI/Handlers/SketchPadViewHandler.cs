using AirTote.SketchPad.Controls;

using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace AirTote.SketchPad.Handlers;

public partial class SketchPadViewHandler
{
	static readonly IPropertyMapper<SketchPadView, SketchPadViewHandler> PropertyMapper = new PropertyMapper<SketchPadView, SketchPadViewHandler>(ViewHandler.ViewMapper)
	{
		[nameof(SketchPadView.IsToolPickerVisible)] = MapIsToolPickerVisible,
		[nameof(SketchPadView.CanvasHeight)] = MapCanvasHeight,
		[nameof(SketchPadView.CanvasWidth)] = MapCanvasWidth,
		[nameof(SketchPadView.Height)] = MapCanvasHeight,
		[nameof(SketchPadView.Width)] = MapCanvasWidth,
	};

	public SketchPadViewHandler() : base(PropertyMapper) { }

	public static partial void MapIsToolPickerVisible(SketchPadViewHandler handler, SketchPadView sketchPad);
	public static partial void MapCanvasHeight(SketchPadViewHandler handler, SketchPadView sketchPad);
	public static partial void MapCanvasWidth(SketchPadViewHandler handler, SketchPadView sketchPad);
	public static partial void MapHeight(SketchPadViewHandler handler, SketchPadView sketchPad);
	public static partial void MapWidth(SketchPadViewHandler handler, SketchPadView sketchPad);
}
