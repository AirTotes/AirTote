using FIS_J.Services;

using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Providers;
using Mapsui.Styles;

namespace FIS_J.Components.Maps.Layers;

public class MVALayer : Layer
{
	static VectorStyle NormalStyle { get; } = new()
	{
		Fill = null,
		Outline = new()
		{
			Color = Mapsui.Styles.Color.FromArgb(0xAA, 0, 0, 0),
			Width = 0.7
		}
	};

	static VectorStyle UnselectedStyle { get; } = new()
	{
		Fill = null,
		Outline = new()
		{
			Color = Mapsui.Styles.Color.FromArgb(0x55, 0, 0, 0),
			Width = 0.5
		}
	};

	static VectorStyle SelectedStyle { get; } = new()
	{
		Fill = null,
		Outline = new()
		{
			Color = Mapsui.Styles.Color.FromArgb(0xEE, 0, 0, 0),
			Width = 1
		}
	};

	new MemoryProvider? DataSource { get => base.DataSource as MemoryProvider; set => base.DataSource = value; }

	Dictionary<string, GeometryFeature> Geometries { get; } = new();

	public MVALayer()
	{
		Name = nameof(MVALayer);

		DataSource = null;

		Style = null;

		Task.Run(async () =>
		{
			var geometries = await MinimumVectoringAltitude.GetMVALines(true);

			if (geometries is null)
				return;

			ApplyStyle(geometries.Values, NormalStyle);

			// 現状追加されている分 (おそらく、鯖から取得したもの) は上書きしない。
			// => ローカルの方が古いことが想定されるため
			foreach (var v in geometries)
				if (!Geometries.ContainsKey(v.Key))
					Geometries[v.Key] = v.Value;

			DataSource = new(Geometries.Values);
		});
	}

	public async Task ReloadAsync()
	{
		var geometries = await MinimumVectoringAltitude.GetMVALines();

		if (geometries is null)
			return;

		ApplyStyle(geometries.Values, NormalStyle);

		foreach (var v in geometries)
			Geometries[v.Key] = v.Value;

		DataSource = new(Geometries.Values);
	}

	public void OnAirportSelected(string ICAO)
	{
		foreach (var feature in Geometries)
		{
			if (
				(feature.Key == ICAO && (feature.Value.Styles.FirstOrDefault() as VectorStyle) != SelectedStyle)
				|| (feature.Key != ICAO && (feature.Value.Styles.FirstOrDefault() as VectorStyle) != UnselectedStyle)
			)
			{
				feature.Value.Styles = new List<IStyle>()
				{
					feature.Key == ICAO ? SelectedStyle : UnselectedStyle
				};
			}
		}
	}

	public void OnAirportUnselected()
		=> ApplyStyle(Geometries.Values, NormalStyle);


	private static void ApplyStyle(IEnumerable<GeometryFeature> targets, IStyle style)
	{
		foreach (var v in targets)
			targets.AsParallel().ForAll(v => v.Styles = new List<IStyle>() { style });
	}
}

