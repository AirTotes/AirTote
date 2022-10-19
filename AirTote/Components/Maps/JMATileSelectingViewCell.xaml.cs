using AirTote.Pages.TabChild;
using AirTote.Services.JMA;
using AirTote.Services.JMA.Models;

using DependencyPropertyGenerator;

namespace AirTote.Components.Maps;

[DependencyProperty<TopPage>("Top")]
[DependencyProperty<NowC_Types?>("NowCTypes")]
[DependencyProperty<string>("Text")]
[DependencyProperty<bool>("IsChecked", DefaultBindingMode = DefaultBindingMode.TwoWay)]
[DependencyProperty<TextCell>("CurrentTimeTextCell")]
[DependencyProperty<Slider>("CurrentTimeSlider")]
public partial class JMATileSelectingViewCell : ViewCell
{
	IReadOnlyList<TargetTimes>? TargetTimeList { get; set; }

	public JMATileSelectingViewCell()
	{
		InitializeComponent();
	}

	public JMATileSelectingViewCell(TopPage top) : this()
	{
		Top = top;
	}

	partial void OnTopChanged()
		=> UpdateIsChecked();
	partial void OnNowCTypesChanged()
		=> UpdateIsChecked();

	void SetCurrentTimeText(string s)
	{
		if (CurrentTimeTextCell is not null)
			CurrentTimeTextCell.Text = s;
	}

	void ExecWhenNotNull<T>(T? v, Action<T> action)
	{
		if (v is not null)
			action.Invoke(v);
	}

	void UpdateIsChecked()
	{
		if (Top is null)
		{
			IsEnabled = false;
			SetCurrentTimeText("Disabled");
			return;
		}

		IsEnabled = true;

		if (Top.JMATileLayer is null)
		{
			IsChecked = (NowCTypes is null);
			return;
		}

		IsChecked = (Top.CurrentNowC_Type == NowCTypes);
	}

	async void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		if (sender is not RadioButton radioButton || Top is null || !e.Value)
			return;

		if (Top.JMATiles is null)
			await ReloadJMATiles();
		if (Top.JMATiles is null)
			return;

		// 既にJMA TileLayerを表示しているのであれば、再度の表示更新は行わない
		TargetTimes? time = null;
		if (NowCTypes is not null && Top.JMATileLayer is not null)
		{
			if (Top.CurrentNowC_Type == NowCTypes)
				time = Top.CurrentTargetTimes;
		}

		switch (NowCTypes)
		{
			case NowC_Types.HRPNs:
				time ??= Top.JMATiles.HRPNs_Latest;
				SetSlider(Top.JMATiles.HRPNsTimeList, time);
				break;

			case NowC_Types.THNs:
				SetSlider(Top.JMATiles.THNsTimeList, time);
				break;

			case NowC_Types.TRNs:
				SetSlider(Top.JMATiles.TRNsTimeList, time);
				break;

			default:
				Top.JMATileLayer = null;
				SetCurrentTimeText("非表示中");

				ExecWhenNotNull(CurrentTimeSlider, v => v.IsEnabled = false);
				break;
		}
	}

	void SetSlider(IReadOnlyList<TargetTimes> timesList, TargetTimes? time)
	{
		if (CurrentTimeSlider is null)
			return;

		if (timesList.Count <= 0)
		{
			CurrentTimeSlider.IsEnabled = false;
			SetCurrentTimeText("時刻データ取得失敗");
			return;
		}

		TargetTimeList = timesList;

		CurrentTimeSlider.IsEnabled = true;
		CurrentTimeSlider.Minimum = 0;
		CurrentTimeSlider.Maximum = timesList.Count - 1;

		int index = -1;
		if (time is not null)
		{
			for (int i = 0; i < timesList.Count; i++)
			{
				if (timesList[i] == time)
				{
					index = i;
					break;
				}
			}
		}

		if (index < 0)
		{
			// 0寄りがLatest or Feature
			// `time`指定が無い場合は、最新の情報を表示するようにする。
			index = 0;
			time = timesList[index];
		}

		CurrentTimeSlider.Value = timesList.Count - index - 1;

		SetLayer(time!);
	}

	partial void OnCurrentTimeSliderChanged(Slider? oldValue, Slider? newValue)
	{
		if (oldValue is not null)
			oldValue.ValueChanged -= SliderValueChanged;
		if (newValue is not null)
			newValue.ValueChanged += SliderValueChanged;
	}

	private void SliderValueChanged(object? sender, ValueChangedEventArgs e)
	{
		if (!IsChecked || TargetTimeList is null || NowCTypes is null)
			return;

		if ((int)e.OldValue != (int)e.NewValue)
			SetLayer(TargetTimeList[TargetTimeList.Count - (int)e.NewValue - 1]);
	}
	private void SetLayer(TargetTimes time)
	{
		if (NowCTypes is null)
			return;

		Top?.SetLayer(time, NowCTypes.Value);

		if (DateTime.TryParseExact(time.validtime, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var value))
			SetCurrentTimeText($"{value:yyyy/MM/dd HH:mm} UTC");
		else
			SetCurrentTimeText(time.validtime);
	}

	async Task ReloadJMATiles()
	{
		if (Top is null)
			return;

		Top.JMATiles = await JMATilesProvider.Init();
		System.Diagnostics.Debug.WriteLine($"{nameof(JMATileSelectingViewCell)}.{nameof(ReloadJMATiles)}() ... Latest HRPNs = {Top.JMATiles.HRPNs_Latest}");
	}
}

