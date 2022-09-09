namespace AirTote;

public static partial class Utils
{
	static public string ToDmsString(double Latitude, double Longitude)
	{
		char latSign = Latitude >= 0 ? 'N' : 'S';
		char lonSigh = Longitude >= 0 ? 'E' : 'W';

		return $"{ToDmsString(Latitude)}{latSign}, {ToDmsString(Longitude)}{lonSigh}";
	}

	static public string ToDmsString(double _deg)
	{
		if (_deg < 0)
			_deg *= -1;
		decimal deg = (decimal)_deg;

		decimal d = decimal.Floor(deg);
		decimal m = decimal.Floor((deg - d) * 60);
		int s = (int)((deg - d - (m / 60)) * 3600);

		return $"{(int)d}° {(int)m}' {s}\"";
	}
}
