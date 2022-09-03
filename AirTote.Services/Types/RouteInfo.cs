using System.Text.Json.Serialization;

namespace AirTote.Services.Types;

public record RouteInfo(
	string Name,
	string? PreviousPointName,
	string? NextPointName,
	string? TrackingMagneticCourse,
	string? GeometricalCourse,
	string? Distance_NM,
	string? ChangeOverPoint,
	string? MinimumEnrouteAltitude,
	string? LateralLimits,
	string? DirectionOfCruisingLevels_Odd,
	string? DirectionOfCruisingLevels_Even,
	string? RemarksControllingUnitFrequency
)
{
	[JsonIgnore]
	public PointInfo? PreviousPoint { get; set; }
	[JsonIgnore]
	public PointInfo? NextPoint { get; set; }

	[JsonIgnore]
	public RouteInfo? PreviousRoute { get; set; }
	[JsonIgnore]
	public RouteInfo? NextRoute { get; set; }
}
