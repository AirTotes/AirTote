using AirTote.Models;
using AirTote.ViewModels.AerodromeInfo;

using Mapsui.Extensions;
using Mapsui.Projections;

namespace AirTote.Pages.AerodromeInfo;

public partial class Aerodrome : ContentPage
{
	AerodromeViewModel VM { get; }

	public Aerodrome(AirportInfo.APInfo apInfo)
	{
		InitializeComponent();

		VM = new(apInfo);

		BindingContext = VM;

		Init();
	}

	private void Init()
	{
		this.APMap.Navigator?.NavigateTo(SphericalMercator.FromLonLat(VM.ApInfo.coordinates.longitude, VM.ApInfo.coordinates.latitude).ToMPoint(), 10);
	}
}
