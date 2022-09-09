using AirTote.AISJapanParser.EAIP;
using AirTote.Models;
using AirTote.ViewModels.AerodromeInfo;

using Mapsui.Extensions;
using Mapsui.Projections;

namespace AirTote.Pages.AerodromeInfo;

public partial class Aerodrome : ContentPage
{
	AerodromeViewModel VM { get; }
	TabMainPage Host { get; }

	public Aerodrome(AirportInfo.APInfo apInfo)
	{
		InitializeComponent();

		VM = new(apInfo);

		BindingContext = VM;

		this.APMap.Navigator?.NavigateTo(SphericalMercator.FromLonLat(VM.ApInfo.coordinates.longitude, VM.ApInfo.coordinates.latitude).ToMPoint(), 10);

		Host = TabMainPage.Current ?? throw new Exception("TabMainPage.Current is null");

		Task.Run(async () =>
		{
			if (TabMainPage.Current is null)
				return;

			var result = await TabMainPage.Current.GetEAIPMenuAsync();

			VM.AIPItems = result.ItemList.OfType<MenuItem_Aerodrome>().Where(v => v.ICAO == VM.IcaoCode).ToList();
		});
	}

	async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
	{
		if (e.Item is not MenuItem_Aerodrome item)
			return;

		string url = Host.AIPDate.GetUrl(item.FileName);
		await Navigation.PushAsync(new WebViewPage(new()
		{
			BaseUrl = url,
			Html = await (await Host.AISJapanConnection).GetPage(url)
		}, item.ItemTitle));
	}
}
