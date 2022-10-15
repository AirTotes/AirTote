using AirTote.Components.Maps;
using AirTote.Components.Maps.Layers;
using AirTote.Components.Maps.Widgets;
using AirTote.Interfaces;
using AirTote.Models;
using AirTote.Services;
using AirTote.Services.JMA;
using AirTote.Services.JMA.Models;
using AirTote.ViewModels.SettingPages;

using CommunityToolkit.Maui.Views;

using Mapsui.Extensions;
using Mapsui.Tiling.Layers;

using Topten.RichTextKit;

namespace AirTote.Pages.TabChild;

public partial class TopPage : ContentPage, IContainFlyoutPageInstance
{
	const float BUTTON_HEIGHT_WIDTH = 48;
	static TimeSpan LOCATION_TIMER_INTERVAL = new(0, 0, 0, 2);

	CancellationTokenSource? gpsCancelation;

	GetRemoteCsv METAR { get; } = new(@"https://fis-j.technotter.com/GetMetarTaf/metar_jp.csv");
	GetRemoteCsv TAF { get; } = new(@"https://fis-j.technotter.com/GetMetarTaf/taf_jp.csv");

	MVALayer MVA { get; set; } = new();
	MVALabelLayer MVAText { get; set; } = new();

	public FlyoutPage? FlyoutPage { get; set; }

	TopPageSettingViewModel Settings { get; } = new();

	public JMATilesProvider? JMATiles { get; set; }
	public NowC_Types CurrentNowC_Type { get; private set; }
	TileLayer? _JMATileLayer = null;
	public TileLayer? JMATileLayer
	{
		get => _JMATileLayer;
		set
		{
			if (_JMATileLayer == value)
				return;

			if (_JMATileLayer is not null)
				Map.Map?.Layers.Remove(_JMATileLayer);

			_JMATileLayer = value;
			if (value is not null)
				Map.Map?.Layers.Insert(1, value);
		}
	}

	public TopPage()
	{
		InitializeComponent();

		Appearing += (_, _) => ResetCalloutText();

		ResetCalloutText();

		Map.Map?.Layers.Add(MVA);
		Map.Map?.Layers.Add(MVAText);

		Map.Map?.Layers.Add(new AirRouteLayer(Map.Map));

		Map.Renderer.WidgetRenders[typeof(InfoWidget)] = new InfoWidgetRenderer();
		Map.Renderer.WidgetRenders[typeof(ButtonWidget)] = new ButtonWidgetRenderer();
		Map.Renderer.WidgetRenders[typeof(StatusBarBGWidget)] = new StatusBarBGWidgetRenderer();

		Map.Map?.Widgets.Add(new InfoWidget()
		{
			VerticalAlignment = Mapsui.Widgets.VerticalAlignment.Bottom,
			HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment.Left,
		});

		Map.Map?.Widgets.Add(new StatusBarBGWidget());

		Shell.SetNavBarIsVisible(this, false);
		Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);

		Task.Run(async () =>
		{
			ButtonWidget widget = await ButtonWidget.FromAppPackageFileAsync("Open Menu Button", "settings_FILL0_wght400_GRAD0_opsz48.svg");
			widget.MarginX = 0;
			widget.MarginY = StatusBarBGWidgetRenderer.Height;
			widget.Size = new(BUTTON_HEIGHT_WIDTH, BUTTON_HEIGHT_WIDTH);

			widget.WidgetTouched += (_, e) =>
			{
				OnSettingButtonClicked();
				e.Handled = true;
			};

			Map.Map?.Widgets.Add(widget);
		});

		Task.Run(async () =>
		{
			try
			{
				await MVA.ReloadAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				await MsgBox.DisplayAlertAsync("Failed to get Remote Resource", "MVAの取得に失敗しました。" + ex.Message, "OK");
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
					bool response = await MsgBox.DisplayAlertAsync("Failed to get Remote Resource", $"MVAの更新に失敗しました。\n{ex.Message}\n更新を継続しますか?", "Yes", "No");
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
				await MsgBox.DisplayAlertAsync("Failed to get Remote Resource", "MVA Textの取得に失敗しました。" + ex.Message, "OK");
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
					bool response = await MsgBox.DisplayAlertAsync("Failed to get Remote Resource", $"MVA Textの更新に失敗しました。\n{ex.Message}\n更新を継続しますか?", "Yes", "No");
					if (!response)
						return;
				}

				await Task.Delay(2000);
			}
#else
			return;
#endif
		});

		Task.Run(StartGPS);

		Map.MyLocationLayer.Clicked += (_, _) => Map.MyLocationFollow = true;

		Map.AirportSelected += Map_AirportSelected;
	}

	private async void Map_AirportSelected(object? sender, AirportSelectedEventArgs e)
		=> await Navigation.PushAsync(new AerodromeInfo.Aerodrome(e.SelectedAP));

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
			await MsgBox.DisplayAlertAsync("Failed to get Remote Resource", "METAR/TAFの更新に失敗しました。表示されている情報は、前回までに取得した情報です。\n" + ex.Message, "OK");
			return;
		}
		finally
		{
			ResetCalloutTextRunning = false;
		}
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		Settings.LoadValues();
		StartGPS();

		ReloadJMATileProvider();
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		gpsCancelation?.Cancel();

		Map.MyLocationEnabled = false;
		Map.MyLocationFollow = false;
		Map.IsMyLocationButtonVisible = false;
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

	async void ReloadJMATileProvider()
	{
		JMATiles = await JMATilesProvider.Init();

		System.Diagnostics.Debug.WriteLine($"ReloadJMATiles() JMATileLayer: {JMATiles.HRPNs_Latest}");
	}

	public void SetLayer(TargetTimes time, NowC_Types type)
	{
		JMATileLayer = JMATilesProvider.GetLayer(time, type);
		CurrentNowC_Type = type;

		System.Diagnostics.Debug.WriteLine($"{nameof(TopPage)}.{nameof(SetLayer)}({time}, {type})");
	}

	void OnSettingButtonClicked()
	{
		if (Map.Map is null)
			return;

		this.ShowPopup(new MapSettingPopup(this, Map.RefreshGraphics));
	}

	void UpdateMyLocation(Location loc)
	{
		Mapsui.UI.Maui.Position pos = new(loc.Latitude, loc.Longitude);
		Map.MyLocationLayer.UpdateMyLocation(pos, Map.MyLocationEnabled && Settings.IsLocationFollowAnimationEnabled);
		if (loc.Speed is double v)
			Map.MyLocationLayer.UpdateMySpeed(v);
	}

	public async void StartGPS()
	{
		gpsCancelation?.Dispose();
		gpsCancelation = null;
		if (!Settings.IsLocationEnabled)
			return;

		gpsCancelation = new CancellationTokenSource();

		await Task.Run(async () =>
		{
			while (!gpsCancelation.IsCancellationRequested)
			{
				// ref: https://docs.microsoft.com/en-us/dotnet/maui/platform-integration/device/geolocation
				GeolocationRequest req = new(GeolocationAccuracy.High, Settings.LocationRefleshInterval);

				Application.Current?.Dispatcher.DispatchAsync(async () =>
				{
					try
					{
						Location? loc = await Geolocation.Default.GetLocationAsync(req);
						if (loc is not null)
						{
							UpdateMyLocation(loc);

							if (!Map.MyLocationEnabled)
							{
								Map.MyLocationEnabled = true;
								Map.MyLocationFollow = true;
								Map.IsMyLocationButtonVisible = true;
							}
						}
						return;
					}
					catch (FeatureNotSupportedException)
					{
						MsgBox.DisplayAlert("Cannot Show Your Location", "この端末では位置情報サービスを使用できません", "OK");
					}
					catch (FeatureNotEnabledException)
					{
						MsgBox.DisplayAlert("Location Service Disabled", "OSの設定により、位置情報サービスが無効化されています", "OK");
					}
					catch (PermissionException)
					{
						MsgBox.DisplayAlert("Location Service Not Allowed", "アプリに位置情報の使用が許可されていません", "OK");
					}
					catch (Exception ex)
					{
						MsgBox.DisplayAlert("Failed to Get Your Location", "位置情報の取得でエラーが発生しました\n" + ex.Message, "OK");
					}

					Settings.IsLocationEnabled = false;
					gpsCancelation.Cancel();
				}).ConfigureAwait(false);

				await Task.Delay(Settings.LocationRefleshInterval).ConfigureAwait(false);
			}
		}, gpsCancelation.Token).ConfigureAwait(false);
	}
}

