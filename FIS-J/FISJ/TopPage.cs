using FIS_J.Maps;
using FIS_J.Models;
using FIS_J.Services;

namespace FIS_J.FISJ;

public class TopPage : ContentPage
{
	AirportMap Map { get; } = new();
	GetRemoteCsv METAR { get; } = new(@"https://fis-j.technotter.com/GetMetarTaf/metar_jp.csv");
	GetRemoteCsv TAF { get; } = new(@"https://fis-j.technotter.com/GetMetarTaf/taf_jp.csv");

	public TopPage()
	{
		Content = Map;
		Title = "Flight Information";

		Appearing += (_, _) => ResetCalloutText();

		ResetCalloutText();
	}

	private async void ResetCalloutText()
	{
		bool getMetarResult = await METAR.ReLoad();
		bool getTafResult = await TAF.ReLoad();

		if (getMetarResult != true && getTafResult != true)
			return;

		Map.SetCalloutText(setCalloutTextAction);
	}

	private IEnumerable<CalloutText> setCalloutTextAction(AirportInfo.APInfo ap, IEnumerable<CalloutText> upper)
	{
		List<CalloutText> texts = new(upper);

		METAR.Data.TryGetValue(ap.icao, out var _metar);
		TAF.Data.TryGetValue(ap.icao, out var _taf);

		CalloutText metar = new($"METAR:\t{_metar?.FirstOrDefault() ?? "N/A"}", upper.Last());
		CalloutText taf = new($"TAF:\t{_taf?.FirstOrDefault() ?? "N/A"}", metar);

		texts.Add(metar);
		texts.Add(taf);

		return texts;
	}
}

