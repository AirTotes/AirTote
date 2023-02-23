using AirTote.Components;
using AirTote.SketchPad;

using CommunityToolkit.Maui;

namespace AirTote;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseAirToteSketchPad()
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
