using FIS_J.Maps;

namespace FIS_J.FISJ;

public class TopPage : ContentPage
{
	AirportMap Map { get; } = new();
	public TopPage()
	{
		Content = Map;
		Title = "Flight Information";
	}
}

