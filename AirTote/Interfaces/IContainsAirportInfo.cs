using AirTote.Models;

namespace AirTote.Interfaces
{
	public interface IContainsAirportInfo
	{
		AirportInfo.APInfo? AirportInfo { get; set; }
	}
}
