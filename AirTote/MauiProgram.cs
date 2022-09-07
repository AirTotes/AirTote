using AirTote.Components;

using CommunityToolkit.Maui;

using SkiaSharp.Views.Maui.Controls.Hosting;

namespace AirTote;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseSkiaSharp()
			.UseMauiCommunityToolkit()
			.ConfigureMauiHandlers(h =>
			{
				h.AddHandler<ScratchPadView, ScratchPadViewHandler>();
			})
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("BIZUDGothic-Bold.ttf", "BIZ UDGothic Bold");
				fonts.AddFont("BIZUDGothic-Regular.ttf", "BIZ UDGothic");
				fonts.AddFont("BIZUDPGothic-Bold.ttf", "BIZ UDPGothic Bold");
				fonts.AddFont("BIZUDPGothic-Regular.ttf", "BIZ UDPGothic");
				fonts.AddFont("MaterialSymbolsRounded.ttf", "MaterialSymbolsRounded");
			});

		return builder.Build();
	}
}
