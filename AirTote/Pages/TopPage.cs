﻿using AirTote.Components.Maps;
using AirTote.Components.Maps.Layers;
using AirTote.Components.Maps.Widgets;
using AirTote.Interfaces;
using AirTote.Models;
using AirTote.Services;
using Mapsui.Extensions;
using Mapsui.Layers;
using Topten.RichTextKit;

namespace AirTote.Pages;

public class TopPage : ContentPage, IContainFlyoutPageInstance
{
	AirportMap Map { get; } = new();
	GetRemoteCsv METAR { get; } = new(@"https://fis-j.technotter.com/GetMetarTaf/metar_jp.csv");
	GetRemoteCsv TAF { get; } = new(@"https://fis-j.technotter.com/GetMetarTaf/taf_jp.csv");

	MVALayer MVA { get; set; } = new();
	MVALabelLayer MVAText { get; set; } = new();

	public FlyoutPage? FlyoutPage { get; set; }

	public TopPage()
	{
		Content = Map;
		Title = "Flight Information";

		Appearing += (_, _) => ResetCalloutText();

		ResetCalloutText();

		Map.Map?.Layers.Add(MVA);
		Map.Map?.Layers.Add(MVAText);

		Map.Renderer.WidgetRenders[typeof(InfoWidget)] = new InfoWidgetRenderer();
		Map.Renderer.WidgetRenders[typeof(ButtonWidget)] = new ButtonWidgetRenderer();
		Map.Renderer.WidgetRenders[typeof(StatusBarBGWidget)] = new StatusBarBGWidgetRenderer();

		Map.Map?.Widgets.Add(new InfoWidget()
		{
			VerticalAlignment = Mapsui.Widgets.VerticalAlignment.Bottom,
			HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment.Left,
		});

		Map.Map?.Widgets.Add(new StatusBarBGWidget());

		Task.Run(async () =>
		{
			ButtonWidget widget;

			try
			{
				using (var stream = await FileSystem.OpenAppPackageFileAsync("menu_FILL0_wght700_GRAD0_opsz48.svg"))
					widget = new(await new StreamReader(stream).ReadToEndAsync())
					{
						MarginX = 4,
						MarginY = StatusBarBGWidgetRenderer.Height,
					};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return;
			}

			widget.WidgetTouched += (s, e) =>
			{
				OnMenuButtonClicked();
				e.Handled = true;
			};

			Map.Map?.Widgets.Add(widget);
		});

		NavigationPage.SetHasNavigationBar(this, false);
		PageHost.SetIsGestureEnabled(typeof(TopPage), false);

		Task.Run(async () =>
		{
			try
			{
				await MVA.ReloadAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				await MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("Failed to get Remote Resource", "MVAの取得に失敗しました。" + ex.Message, "OK"));
				return;
			}


#if DEBUG
			while (true)
			{
				try
				{
					await MVA.ReloadAsync();
					Console.WriteLine("MVA DataSource Updated");
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
					bool response = await MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("Failed to get Remote Resource", $"MVAの更新に失敗しました。\n{ex.Message}\n更新を継続しますか?", "Yes", "No"));
					if (!response)
						return;
				}

				await Task.Delay(2000);
			}
#else
			return;
#endif
		});

		Task.Run(async () =>
		{
			try
			{
				await MVAText.ReloadAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				await MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("Failed to get Remote Resource", "MVA Textの取得に失敗しました。" + ex.Message, "OK"));
				return;
			}


#if DEBUG
			while (true)
			{
				try
				{
					await MVAText.ReloadAsync();
					Console.WriteLine("MVA-Text DataSource Updated");
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
					bool response = await MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("Failed to get Remote Resource", $"MVA Textの更新に失敗しました。\n{ex.Message}\n更新を継続しますか?", "Yes", "No"));
					if (!response)
						return;
				}

				await Task.Delay(2000);
			}
#else
			return;
#endif
		});
	}

	private void OnMenuButtonClicked()
	{
		if (MainThread.IsMainThread)
		{
			if (FlyoutPage is not null)
				FlyoutPage.IsPresented = !FlyoutPage.IsPresented;
		}
		else
			MainThread.BeginInvokeOnMainThread(OnMenuButtonClicked);
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

	private RichString setCalloutTextAction(AirportInfo.APInfo ap, RichString str)
	{
		METAR.Data.TryGetValue(ap.icao, out var _metar);
		TAF.Data.TryGetValue(ap.icao, out var _taf);

		str
			.Alignment(Topten.RichTextKit.TextAlignment.Left)
			.Add($"METAR: {_metar?.FirstOrDefault() ?? "N/A"}", fontSize: 14);
		str
			.Paragraph()
			.Alignment(Topten.RichTextKit.TextAlignment.Left)
			.Add($"  TAF: {_taf?.FirstOrDefault() ?? "N/A"}", fontSize: 14);

		return str;
	}
}

