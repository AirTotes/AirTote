using FIS_J.Services;

using Mapsui.Layers;
using Mapsui.Providers;

namespace FIS_J.Maps;

public class MVALabelLayer : Layer
{
	new MemoryProvider DataSource { get => base.DataSource as MemoryProvider; set => base.DataSource = value; }

	PointFeature[] Features { get; set; }

	public MVALabelLayer()
	{
		Name = nameof(MVALayer);

		DataSource = null;

		Style = null;

		Task.Run(async () => DataSource = new(await MinimumVectoringAltitude.GetMVALabels(true)));
	}

	public async Task ReloadAsync()
		=> DataSource = new(await MinimumVectoringAltitude.GetMVALabels());
}
