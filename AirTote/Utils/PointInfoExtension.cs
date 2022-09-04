using AirTote.Services.Types;

using Mapsui;
using Mapsui.Extensions;
using Mapsui.Projections;

using NetTopologySuite.Geometries;

namespace AirTote;

public static partial class Utils
{
	public static MPoint? ToMPoint(this PointInfo pt)
	{
		if (pt.HasLonLat())
			return SphericalMercator.FromLonLat((double)pt.Longitude_Deg, (double)pt.Latitude_Deg).ToMPoint();
		else
			return null;
	}

	public static Coordinate? ToCoordinate(this PointInfo pt)
	{
		if (pt.ToMPoint() is MPoint mpt)
			return new(mpt.X, mpt.Y);
		else
			return null;
	}
}

