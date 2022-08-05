using Mapsui.Layers;
using Mapsui.Widgets;

namespace AirTote.Components.Maps;

public partial class MapTileSettingViewCell : ViewCell
{
	IReadOnlyList<MapTileSettingViewCell> OtherCells { get; }
	AirMap Map { get; }
	MapTileSourceInfo SrcInfo { get; }

	public MapTileSettingViewCell(IReadOnlyList<MapTileSettingViewCell> otherCells, bool isSelected, AirMap map, MapTileSourceInfo src)
	{
		InitializeComponent();

		OtherCells = otherCells;
		Map = map;
		SrcInfo = src;

		EnableSW.IsChecked = isSelected;
		EnableSW.IsEnabled = !isSelected;
		NameLabel.Text = src.Name;

		this.Tapped += (_, e) =>
		{
			if (this.EnableSW.IsEnabled)
				this.EnableSW.IsChecked = !this.EnableSW.IsChecked;
		};
	}

	void EnableSW_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		// 自身が選択されたのであれば、他のCellへの選択を解除させる
		if (e.Value == true)
		{
			foreach (var elem in OtherCells)
			{
				if (elem != this)
				{
					elem.EnableSW.IsChecked = false;
					elem.EnableSW.IsEnabled = true;
				}
			}

			this.EnableSW.IsEnabled = false;
			Map.ChangeMapTile(SrcInfo);
		}
		// 自身が選択解除された場合でも、他者がすべて選択されていない状態に移行するような操作は無効にする
		else
		{
			if (OtherCells.All(v => !v.EnableSW.IsChecked))
			{
				this.EnableSW.IsChecked = true;
				this.EnableSW.IsEnabled = false;
			}
		}
	}
}
