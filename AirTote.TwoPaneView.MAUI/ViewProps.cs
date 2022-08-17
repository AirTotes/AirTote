using Microsoft.Maui.Controls;

namespace AirTote.TwoPaneView;

[ContentProperty(nameof(Content))]
public partial class ViewProps
{
	public View? Content { get; set; }
	public string Title { get; set; } = "";
}
