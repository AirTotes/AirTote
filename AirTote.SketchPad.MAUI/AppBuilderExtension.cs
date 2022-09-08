using AirTote.SketchPad.Controls;
using AirTote.SketchPad.Handlers;

using Microsoft.Maui.Hosting;

namespace AirTote.SketchPad;

public static class AppBuilderExtension
{
	public static MauiAppBuilder UseAirToteSketchPad(this MauiAppBuilder builder)
	{
		builder.ConfigureMauiHandlers(h =>
		{
			h.AddHandler<SketchPadView, SketchPadViewHandler>();
		});

		return builder;
	}
}
