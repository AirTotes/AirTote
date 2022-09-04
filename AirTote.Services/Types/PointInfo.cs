using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AirTote.Services.Types;

[Flags]
public enum PointTypes
{
	UNKNOWN,
	VORTAC,
	CRP,
	ORRP,
	VOR,
	DME,
	TACAN,
	NDB
}

public record PointInfo(
	int Type,
	string Name,
	float? Latitude_Deg,
	float? Longitude_Deg
)
{
	[JsonIgnore]
	public IReadOnlyDictionary<string, int>? PointTypeDict { get; internal set; }

	[MemberNotNullWhen(true, nameof(Latitude_Deg))]
	[MemberNotNullWhen(true, nameof(Longitude_Deg))]
	public bool HasLonLat()
		=> Latitude_Deg is not null and not float.NaN && Longitude_Deg is not null and not float.NaN;

	public bool HasFlag(PointTypes type)
		=> HasFlag(type.ToString());
	public bool HasFlag(string type)
	{
		if (PointTypeDict?.TryGetValue(type, out int flagValue) != true)
			return false;

		return (Type & flagValue) > 0;
	}
}
