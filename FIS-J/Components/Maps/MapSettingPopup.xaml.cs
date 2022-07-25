using CommunityToolkit.Maui.Views;
using FIS_J.Components.Maps.Widgets;
using Mapsui;

namespace FIS_J.Components.Maps;

public partial class MapSettingPopup : Popup
{
	Mapsui.Map Map { get; }

	public MapSettingPopup(Mapsui.Map map, Action RefleshCanvas)
	{
		Map = map;
		InitializeComponent();

		foreach (var layer in map.Layers)
			LayersSection.Add(new MapSettingViewCell(layer));

		foreach (var widget in map.Widgets)
		{
			if (widget is not INamedWidget namedWidget)
				continue;
			WidgetsSection.Add(new MapSettingViewCell(namedWidget, RefleshCanvas));
		}
	}

	void CloseButton_Clicked(object sender, EventArgs e)
		=> this.Close();

	void SaveButton_Clicked(object sender, EventArgs e)
	{
		// 設定を保存して、次回起動、および各デバイスに設定を反映させる。
		throw new NotImplementedException();

		//CloseButton_Clicked(sender, e);
	}
}
