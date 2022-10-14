using System;
namespace AirTote.Services.JMA.Models;

public record TargetTimes(
	DateTime basetime,
	DateTime validtime,
	string[] elements
)
{
	public const string TYPE_HIGH_RESOLUTION_PRECIPITATION_NOWCASTS = "hrpns";
	public const string TYPE_HIGH_RESOLUTION_PRECIPITATION_NOWCASTS_NO_DATA = "hrpns_nd";
	public const string TYPE_THUNDER_NOWCASTS = "thns";
	public const string TYPE_THUNDER_NOWCASTS_NO_DATA = "thns_nd";
	public const string TYPE_TORNADO_NOWCASTS = "trns";
	public const string TYPE_TORNADO_NOWCASTS_NO_DATA = "trns_nd";
	public const string TYPE_LIGHTNING_DETECTION_NETWORK_SYSTEM = "liden";
}
