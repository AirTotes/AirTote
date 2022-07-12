using FIS_J.Maps;
using FIS_J.Models;
using FIS_J.Services;
using Mapsui.Layers;

namespace FIS_J.FISJ;

public class TopPage : ContentPage
{
	AirportMap Map { get; } = new();
	GetRemoteCsv METAR { get; } = new(@"https://fis-j.technotter.com/GetMetarTaf/metar_jp.csv");
	GetRemoteCsv TAF { get; } = new(@"https://fis-j.technotter.com/GetMetarTaf/taf_jp.csv");

	Layer MVA { get; set; }

	public TopPage()
	{
		Content = Map;
		Title = "Flight Information";

		Appearing += (_, _) => ResetCalloutText();

		ResetCalloutText();

		Task.Run(async () =>
		{
			try
			{
				MVA = await MinimumVectoringAltitude.CreateLayer();
			}
			catch (Exception ex)
			{
				await DisplayAlert("Failed to get Remote Resource", "MVAの取得に失敗しました。" + ex.Message, "OK");
				Console.WriteLine(ex);
			}

			Map.Map.Layers.Add(MVA);

			while (true)
			{
				try
				{
					MVA.DataSource = await MinimumVectoringAltitude.GetProvider();
					Console.WriteLine("MVA DataSource Updated");
				}
				catch (Exception ex)
				{
					await DisplayAlert("Failed to get Remote Resource", "MVAの更新に失敗しました。" + ex.Message, "OK");
					Console.WriteLine(ex);
				}

				await Task.Delay(2000);
			}
		});
	}

	bool ResetCalloutTextRunning = false;
	private async void ResetCalloutText()
	{
		if (ResetCalloutTextRunning)
			return;
		ResetCalloutTextRunning = true;
		try
		{
			bool getMetarResult = await METAR.ReLoad();
			bool getTafResult = await TAF.ReLoad();

			if (getMetarResult != true && getTafResult != true)
				return;

			Map.SetCalloutText(setCalloutTextAction);
		}
		catch (Exception ex)
		{
			await MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("Failed to get Remote Resource", "METAR/TAFの更新に失敗しました。表示されている情報は、前回までに取得した情報です。\n" + ex.Message, "OK"));
			return;
		}
		finally
		{
			ResetCalloutTextRunning = false;
		}
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

