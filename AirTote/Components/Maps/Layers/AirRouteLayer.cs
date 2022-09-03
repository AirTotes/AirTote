using AirTote.Services;

using Mapsui;
using Mapsui.Layers;
using Mapsui.Styles;
using Mapsui.UI;

namespace AirTote.Components.Maps.Layers;

public partial class AirRouteLayer : MemoryLayer
{
	Mapsui.Map Map { get; }
	CalloutStyle? LastTapped { get; set; } = null;

	public AirRouteLayer(Mapsui.Map map)
	{
		Map = map;

		Name = "Air Routes";
		IsMapInfoLayer = true;

		Map.Info += Map_OnInfo;

		Task.Run(LoadLowerATSRoute);
	}

	/// <summary>FeatureがTapされたときに呼び出される</summary>
	/// <param name="s">呼び出し元</param>
	/// <param name="e">イベントに関する情報</param>
	void Map_OnInfo(object? s, MapInfoEventArgs e)
	{
		if (LastTapped is not null)
			LastTapped.Enabled = false;

		if (e.MapInfo?.Layer != this || e.MapInfo.Feature is null)
			return;

		if (e.MapInfo.Feature.Styles.FirstOrDefault(v => v is CalloutStyle) is CalloutStyle calloutStyle && calloutStyle != LastTapped)
		{
			calloutStyle.Enabled = !calloutStyle.Enabled;
			LastTapped = calloutStyle;
		}
		else
			LastTapped = null;
	}

	async void LoadLowerATSRoute()
	{
		List<IFeature> features = new();

		var result = await AirRouteProvider.GetLowerATSRouteAsync(new(2022, 9, 8), new(2022, 9, 8));

		var ptList = result?.PointList.Where(v => v.Latitude_Deg is not null && v.Longitude_Deg is not null);
		if (ptList is not null)
			foreach (var pt in ptList)
				features.Add(new PointObject(pt));

		Features = features;
	}
}
