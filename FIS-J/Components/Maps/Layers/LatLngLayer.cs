using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using Mapsui.Styles;

using NetTopologySuite.Geometries;

namespace FIS_J.Components.Maps.Layers
{
	public static class LatLngLayerGenerator
	{
		public const double MAX_RESO_LV_1 = 5000;
		public const double MAX_RESO_LV_2 = 1000;
		public const int LAT_LINE_MAX = 85;
		public const int LON_LINE_MAX = 180;
		const double DEFAULT_OPACITY = 0.2;

		static Pen DashPen { get; } = new()
		{
			Color = Mapsui.Styles.Color.Black,
			PenStyle = PenStyle.Dash,
			PenStrokeCap = PenStrokeCap.Round,
		};

		static readonly double[] WIDTH_SET = new double[]
		{
			2,

			1.7,
			1.4,

			1,
		};

		static public double GetLineStep(in double resolution)
			=> resolution switch
			{
				var i when i <= MAX_RESO_LV_2 => 0.1,
				var i when i <= MAX_RESO_LV_1 => 1,
				_ => 5
			};

		static public MemoryLayer Generate()
		{
			IEnumerable<GeometryFeature> features
				= CreateLatLngLayer(10, MAX_RESO_LV_1, double.MaxValue, WIDTH_SET[0])
				.Concat(CreateLatLngLayer(5, MAX_RESO_LV_1, double.MaxValue, WIDTH_SET[2], 10))

				.Concat(CreateLatLngLayer(10, 0, MAX_RESO_LV_1, WIDTH_SET[0]))

				.Concat(CreateLatLngLayer(5, MAX_RESO_LV_2, MAX_RESO_LV_1, WIDTH_SET[2], 10))
				.Concat(CreateLatLngLayer(1, MAX_RESO_LV_2, MAX_RESO_LV_1, WIDTH_SET[3], 5, DashPen))

				.Concat(CreateLatLngLayer(5, 0, MAX_RESO_LV_2, WIDTH_SET[1], 10))
				.Concat(CreateLatLngLayer(1, 0, MAX_RESO_LV_2, WIDTH_SET[2], 5))

				.Concat(CreateLatLngLayer(0.5, 0, MAX_RESO_LV_2, WIDTH_SET[3], 1))
				.Concat(CreateLatLngLayer(0.1, 0, MAX_RESO_LV_2, WIDTH_SET[3], 0.5, DashPen));

			return new()
			{
				Features = features,
				IsMapInfoLayer = true,
				Name = "Longitude / Latitude Lines",
				Style = null
			};
		}

		static List<GeometryFeature> CreateLatLngLayer(double step, double MinVisibleResolution, double MaxVisibleResolution, double Width, double skipStep = double.NaN, Pen? pen = null)
		{
			List<GeometryFeature> latlngLines = new();

			bool isSkipStepNaN = double.IsNaN(skipStep);

			VectorStyle style = new()
			{
				Fill = null,
				Outline = null,
				Line = pen ?? new(Mapsui.Styles.Color.Black),
				MinVisible = MinVisibleResolution,
				MaxVisible = MaxVisibleResolution,
				Opacity = (float)DEFAULT_OPACITY,
			};
			style.Line.Width = Width;

			void AddNewLine(double lon1, double lat1, double lon2, double lat2)
			{
				GeometryFeature feature = new(new LineString(
					new[]
					{
						SphericalMercator.FromLonLat(lon1, lat1).ToCoordinate(),
						SphericalMercator.FromLonLat(lon2, lat2).ToCoordinate()
					})
				);

				feature.Styles.Add(style);
				latlngLines.Add(feature);
			}

			for (double i = 0; i <= 180; i += step)
			{
				if (!isSkipStepNaN && (i % skipStep) == 0)
					continue;

				// positive longitude
				AddNewLine(
					i, -LAT_LINE_MAX,
					i, LAT_LINE_MAX
					);
				if (i == 0)
					continue;

				// negative longitude
				AddNewLine(
					-i, -LAT_LINE_MAX,
					-i, LAT_LINE_MAX
					);
			}

			for (double i = 0; i <= LAT_LINE_MAX; i += step)
			{
				if (!isSkipStepNaN && (i % skipStep) == 0)
					continue;

				// positive latitude
				AddNewLine(
					-LON_LINE_MAX, i,
					LON_LINE_MAX, i
					);

				if (i == 0)
					continue;

				// negative latitude
				AddNewLine(
					-LON_LINE_MAX, -i,
					LON_LINE_MAX, -i
					);
			}

			return latlngLines;
		}
	}
}

