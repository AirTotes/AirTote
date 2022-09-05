using Mapsui.Styles;
using Mapsui.UI.Maui;

using SkiaSharp;

using Topten.RichTextKit;

using IStyle = Topten.RichTextKit.IStyle;

namespace AirTote;

public static partial class Utils
{
	public static void SetCalloutText(CalloutStyle Callout, RichString richText, IStyle? defaultStyle = null)
	{
		if (Callout.Content > 0)
			BitmapRegistry.Instance.Unregister(Callout.Content);
		Callout.Content = -1;

		Callout.Content = GetRichTextBitmapId(richText, defaultStyle);
		Callout.Type = CalloutType.Custom;
	}

	public static void SetCalloutText(Callout Callout, RichString richText, IStyle? defaultStyle = null)
	{
		if (Callout.Content > 0)
			BitmapRegistry.Instance.Unregister(Callout.Content);
		Callout.Content = -1;


		Callout.Content = GetRichTextBitmapId(richText, defaultStyle);
		Callout.Type = CalloutType.Custom;
	}
}
