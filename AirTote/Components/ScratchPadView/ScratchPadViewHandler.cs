using Microsoft.Maui.Handlers;

namespace AirTote.Components;

public partial class ScratchPadViewHandler
{
	public static IPropertyMapper<ScratchPadView, ScratchPadViewHandler> PropertyMapper = new PropertyMapper<ScratchPadView, ScratchPadViewHandler>(ViewHandler.ViewMapper)
	{
		[nameof(ScratchPadView.HostPage)] = MapHostPage,
	};

	public ScratchPadViewHandler() : base(PropertyMapper) { }

	public static partial void MapHostPage(ScratchPadViewHandler handler, ScratchPadView video);
}
