using AirTote.PlatformSpecific;

using Microsoft.Maui.Handlers;

namespace AirTote.Components;

public partial class ScratchPadViewHandler : ViewHandler<ScratchPadView, MauiScratchPadView>
{
	protected override MauiScratchPadView CreatePlatformView() => new(VirtualView);

	protected override void ConnectHandler(MauiScratchPadView platformView) => base.ConnectHandler(platformView);

	public static partial void MapHostPage(ScratchPadViewHandler handler, ScratchPadView video)
		=> handler?.PlatformView.UpdateHostPage();
}
