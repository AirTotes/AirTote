using AirTote.Components.Maps.Widgets;

using Mapsui.Layers;
using Mapsui.Widgets;

namespace AirTote.Components.Maps;

public partial class MapSettingViewCell : ViewCell
{
	ILayer? Layer { get; } = null;
	IWidget? Widget { get; } = null;
	Action? RefleshCanvas { get; } = null;

	public MapSettingViewCell(ILayer layer)
	{
		InitializeComponent();

		NameLabel.Text = layer.Name;
		EnableSW.IsEnabled = layer.IsMapInfoLayer;
		EnableSW.IsToggled = layer.Enabled;

		Layer = layer;
	}

	public MapSettingViewCell(INamedWidget widget, Action? refleshCanvas)
	{
		InitializeComponent();

		NameLabel.Text = widget.Name;
		EnableSW.IsEnabled = widget is not ICannotDisableWidget;
		EnableSW.IsToggled = widget.Enabled;
		RefleshCanvas = refleshCanvas;

		Widget = widget;
	}

	void EnableSW_Toggled(object sender, ToggledEventArgs e)
	{
		if (Layer is not null)
			Layer.Enabled = e.Value;
		else if (Widget is not null)
		{
			Widget.Enabled = e.Value;
			RefleshCanvas?.Invoke();
		}
	}
}
