using AirTote.Components;
using AirTote.PlatformSpecific;

using Microsoft.Maui.Handlers;

namespace AirTote.Components;

public partial class ScratchPadViewHandler : ViewHandler<ScratchPadView, MauiScratchPadView>
{
	protected override MauiScratchPadView CreatePlatformView() => new(Context, VirtualView);

	public static partial void MapHostPage(ScratchPadViewHandler handler, ScratchPadView video)
		=> throw new NotImplementedException();
}
