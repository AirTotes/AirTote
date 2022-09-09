using AirTote.Models;

using CommunityToolkit.Mvvm.ComponentModel;

namespace AirTote.ViewModels.AerodromeInfo;

public partial class AerodromeViewModel : ObservableObject
{
	[ObservableProperty]
	private string _Title = "Aerodrome";

	public AirportInfo.APInfo ApInfo { get; }

	public string AerodromeName => ApInfo.name;
	public string IcaoCode => NAIfEmpty(ApInfo.icao);
	public string IataCode => NAIfEmpty(ApInfo.iata);
	public string Coordinate => Utils.ToDmsString(ApInfo.coordinates.latitude, ApInfo.coordinates.longitude);

	public AerodromeViewModel(AirportInfo.APInfo apInfo)
	{
		_Title = apInfo.name;
		ApInfo = apInfo;

		if (!string.IsNullOrWhiteSpace(apInfo.icao))
			_Title += $"({apInfo.icao})";
	}

	static string NAIfEmpty(string s) => string.IsNullOrWhiteSpace(s) ? "N/A" : s;
}
