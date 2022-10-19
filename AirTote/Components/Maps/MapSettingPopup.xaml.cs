using AirTote.Components.Maps.Widgets;
using AirTote.Pages.TabChild;

using CommunityToolkit.Maui.Views;

using DependencyPropertyGenerator;

using Mapsui;

namespace AirTote.Components.Maps;

[DependencyProperty<TopPage>("Top")]
public partial class MapSettingPopup : Popup
{
	AirMap Map { get; }

	public MapSettingPopup(TopPage top, Action RefleshCanvas)
	{
		Map = top.Map;
		if (Map.Map is null)
			throw new ArgumentNullException(nameof(Map.Map));

		// code from: https://github.com/CommunityToolkit/Maui/blob/68b82412a7e31d961b7ec0da2909f38e2a07ff9a/samples/CommunityToolkit.Maui.Sample/Models/PopupSize.cs#L10
		var deviceDisplay = DeviceDisplay.Current;
		Size = new(
			Math.Min(0.9 * (deviceDisplay.MainDisplayInfo.Width / deviceDisplay.MainDisplayInfo.Density), 350),
			0.8 * (deviceDisplay.MainDisplayInfo.Height / deviceDisplay.MainDisplayInfo.Density)
			);
		this.HorizontalOptions = Microsoft.Maui.Primitives.LayoutAlignment.Start;

		InitializeComponent();

		InitSections(Map, Map.Map, RefleshCanvas);

		this.Top = top;
	}

	void InitSections(AirMap airMap, Mapsui.Map map, Action RefleshCanvas)
	{
		List<MapTileSettingViewCell> MapTypeCells = new();
		foreach (var v in TileProvider.TileSources.Values)
		{
			MapTileSettingViewCell cell = new(MapTypeCells, airMap.CurrentMapTileLayer?.Name == v.Name, Map, v);
			MapTypeCells.Add(cell);
			MapTypeSection.Add(cell);
		}

		foreach (var layer in map.Layers)
		{
			if (layer.IsMapInfoLayer)
				LayersSection.Add(new MapSettingViewCell(layer));
		}

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
