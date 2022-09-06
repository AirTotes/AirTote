using AirTote.Services;
using AirTote.Services.Types;

using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;

namespace AirTote.Components.Maps.Layers;

public partial class AirRouteLayer
{
	private class PointObject : PointFeature
	{
		CalloutStyle CalloutStyle { get; } = new()
		{
			TitleFont = {
				Size = 12,
				Bold = true
			},
			RectRadius = 10,
			ShadowWidth = 4,
			Enabled = false,
			MaxWidth = 240,
			TitleFontColor = Mapsui.Styles.Color.Black,
		};
		SymbolStyle IconStyle { get; } = new()
		{
			Fill = null,
		};

		public PointInfo PtInfo { get; }

		public PointObject(PointInfo ptInfo)
			: base(SphericalMercator.FromLonLat((double?)ptInfo.Longitude_Deg ?? double.NaN, (double?)ptInfo.Latitude_Deg ?? double.NaN).ToMPoint())
		{
			PtInfo = ptInfo;

			Styles.Add(this.IconStyle);
			Styles.Add(this.CalloutStyle);

			this.CalloutStyle.Title = ptInfo.Name;

			Task.Run(SetIcon);
		}

		async void SetIcon()
		{
			int id = await GetPointIconId(PtInfo);

			(IconStyle.SymbolType, IconStyle.BitmapId)
				= id < 0
				? (SymbolType.Ellipse, -1)
				: (SymbolType.Image, id);
		}

		static Task<int> GetPointIconId(PointInfo ptInfo)
		{
			if (GetPointServiceType(ptInfo) is not string ptService || GetPointRepType(ptInfo) is not string ptRepType)
				return Task.FromResult(-1);

			return BitmapIdManager.GetSvgFromMauiAssetAsync($"MapIcons/{ptService}.{ptRepType}.svg");
		}

		static string? GetPointServiceType(PointInfo ptInfo)
			=> ptInfo switch
			{
				PointInfo v when v.HasFlag(PointTypes.VOR) && v.HasFlag(PointTypes.DME) => "VORDME",
				PointInfo v when v.HasFlag(PointTypes.VOR) && v.HasFlag(PointTypes.TACAN) => "VORTAC",
				PointInfo v when v.HasFlag(PointTypes.VOR) => "VOR",
				PointInfo v when v.HasFlag(PointTypes.TACAN) => "TACAN",
				PointInfo v when v.HasFlag(PointTypes.CRP) || v.HasFlag(PointTypes.ORRP) => "REP",
				_ => null
			};

		static string? GetPointRepType(PointInfo ptInfo)
			=> ptInfo switch
			{
				PointInfo v when v.HasFlag(PointTypes.CRP) => "CRP",
				PointInfo v when v.HasFlag(PointTypes.ORRP) => "ORRP",
				_ => null
			};
	}
}
