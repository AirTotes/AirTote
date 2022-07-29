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
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("BIZUDGothic-Bold.ttf", "BIZ UDGothic Bold");
				fonts.AddFont("BIZUDGothic-Regular.ttf", "BIZ UDGothic");
				fonts.AddFont("BIZUDPGothic-Bold.ttf", "BIZ UDPGothic Bold");
				fonts.AddFont("BIZUDPGothic-Regular.ttf", "BIZ UDPGothic");
			});

		return builder.Build();
	}
}
