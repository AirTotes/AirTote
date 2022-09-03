using System.Diagnostics.CodeAnalysis;

namespace AirTote.Services.Types;

public record PointInfo(
	string Type,
	string Name,
	float? Latitude_Deg,
	float? Longitude_Deg
)
{
	[MemberNotNullWhen(true, nameof(Latitude_Deg))]
	[MemberNotNullWhen(true, nameof(Longitude_Deg))]
	public bool HasLonLat()
		=> Latitude_Deg is not null and not float.NaN && Longitude_Deg is not null and not float.NaN;
}
