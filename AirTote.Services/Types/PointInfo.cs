namespace AirTote.Services.Types;

public record PointInfo(
	string Type,
	string Name,
	float? Latitude_Deg,
	float? Longitude_Deg
);
