using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using Mapsui.Styles;

using NetTopologySuite.Geometries;

namespace FIS_J.Maps
{
	public static class LatLngLayerGenerator
	{
		public const double MAX_RESO_LV_1 = 5000;
		public const double MAX_RESO_LV_2 = 1000;
		public const int LAT_LINE_MAX = 85;
		const double DEFAULT_OPACITY = 0.2;

		static readonly double[] WIDTH_SET = new double[]
		{
			1.5,

			1.3,
			1.1,

			0.9,
			0.7,
		};

		static public double GetLineStep(in double resolution)
			=> resolution switch
			{
				var i when i <= MAX_RESO_LV_2 => 0.1,
				var i when i <= MAX_RESO_LV_1 => 1,
				_ => 5
			};

		static public ILayer[] Generate()
		{
			List<ILayer> layers = new();

			return new ILayer[]
			{
				CreateLatLngLayer(10, MAX_RESO_LV_1, double.MaxValue, WIDTH_SET[0] / 2),
				CreateLatLngLayer(5, MAX_RESO_LV_1, double.MaxValue, WIDTH_SET[1] / 2, 5),

				CreateLatLngLayer(10, 0, MAX_RESO_LV_1, WIDTH_SET[0]),

				CreateLatLngLayer(5, 0, MAX_RESO_LV_1, WIDTH_SET[1], 10),
				CreateLatLngLayer(1, 0, MAX_RESO_LV_1, WIDTH_SET[2], 5),

				CreateLatLngLayer(0.5, 0, MAX_RESO_LV_2, WIDTH_SET[3], 1),
				CreateLatLngLayer(0.1, 0, MAX_RESO_LV_2, WIDTH_SET[4], 0.5),
			};
		}

		static ILayer CreateLatLngLayer(double step, double MinVisibleResolution, double MaxVisibleResolution, double Width, double skipStep = double.NaN)
		{
			List<GeometryFeature> latlngLines = new();

			bool isSkipStepNaN = double.IsNaN(skipStep);

			for (double i = 0; i <= 180; i += step)
			{
				if (isSkipStepNaN && (i % skipStep) == 0)
					continue;

				// positive longitude
				latlngLines.Add(new(new LineString(
					new[]
					{
						SphericalMercator.FromLonLat(i, -LAT_LINE_MAX).ToCoordinate(),
						SphericalMercator.FromLonLat(i, LAT_LINE_MAX).ToCoordinate()
					})
				));

				if (i == 0)
					continue;

				// negative longitude
				latlngLines.Add(new(new LineString(
					new[]
					{
						SphericalMercator.FromLonLat(-i, -LAT_LINE_MAX).ToCoordinate(),
						SphericalMercator.FromLonLat(-i, LAT_LINE_MAX).ToCoordinate()
					})
				));
			}

			for (double i = 0; i <= LAT_LINE_MAX; i += step)
			{
				if (isSkipStepNaN && (i % skipStep) == 0)
					continue;

				// positive latitude
				latlngLines.Add(new(new LineString(
					new[]
					{
						SphericalMercator.FromLonLat(-180, i).ToCoordinate(),
						SphericalMercator.FromLonLat(180, i).ToCoordinate()
					})
				));

				if (i == 0)
					continue;

				// negative latitude
				latlngLines.Add(new(new LineString(
					new[]
					{
						SphericalMercator.FromLonLat(-180, -i).ToCoordinate(),
						SphericalMercator.FromLonLat(180, -i).ToCoordinate()
					})
				));
			}

			return new MemoryLayer()
			{
				Features = latlngLines,
				IsMapInfoLayer = true,
				Name = "LatLngLines",
				Style = new VectorStyle()
				{
					Fill = null,
					Outline = null,
					Line = new(Mapsui.Styles.Color.Black, Width),
				},
				MaxVisible = MaxVisibleResolution,
				MinVisible = MinVisibleResolution,
				Opacity = DEFAULT_OPACITY,
			};
		}
	}
}

