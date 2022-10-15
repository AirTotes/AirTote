using AirTote.Pages.TabChild;
using AirTote.Services.JMA;
using AirTote.Services.JMA.Models;

using DependencyPropertyGenerator;

namespace AirTote.Components.Maps;

[DependencyProperty<TopPage>("Top")]
[DependencyProperty<string>("Value")]
[DependencyProperty<string>("Text")]
[DependencyProperty<bool>("IsChecked", DefaultBindingMode = DefaultBindingMode.TwoWay)]
public partial class JMATileSelectingViewCell : ViewCell
{
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
	partial void OnValueChanged()
		=> UpdateIsChecked();

	void UpdateIsChecked()
	{
		if (Top is null || string.IsNullOrEmpty(Value))
		{
			IsEnabled = false;
			return;
		}

		IsEnabled = true;

		if (Top.JMATileLayer is null)
		{
			IsChecked = (Value == "none");
			return;
		}

		IsChecked = Value switch
		{
			TargetTimes.TYPE_HIGH_RESOLUTION_PRECIPITATION_NOWCASTS => Top.CurrentNowC_Type == NowC_Types.HRPNs,
			TargetTimes.TYPE_THUNDER_NOWCASTS => Top.CurrentNowC_Type == NowC_Types.THNs,
			TargetTimes.TYPE_TORNADO_NOWCASTS => Top.CurrentNowC_Type == NowC_Types.TRNs,
			_ => false
		};
	}

	async void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		if (sender is not RadioButton radioButton || Top is null || !e.Value)
			return;

		if (Top.JMATiles is null)
			await ReloadJMATiles();
		if (Top.JMATiles is null)
			return;

		switch (radioButton.Value)
		{
			case TargetTimes.TYPE_HIGH_RESOLUTION_PRECIPITATION_NOWCASTS:
				if (Top.JMATiles.HRPNs_Latest is not null)
					Top.SetLayer(Top.JMATiles.HRPNs_Latest, NowC_Types.HRPNs);
				break;

			case TargetTimes.TYPE_THUNDER_NOWCASTS:
				if (Top.JMATiles.THNsTimeList.FirstOrDefault() is TargetTimes thns)
					Top.SetLayer(thns, NowC_Types.THNs);
				break;

			case TargetTimes.TYPE_TORNADO_NOWCASTS:
				if (Top.JMATiles.TRNsTimeList.FirstOrDefault() is TargetTimes trns)
					Top.SetLayer(trns, NowC_Types.TRNs);
				break;

			default:
				Top.JMATileLayer = null;
				break;
		}
	}

	async Task ReloadJMATiles()
	{
		if (Top is null)
			return;

		Top.JMATiles = await JMATilesProvider.Init();
		System.Diagnostics.Debug.WriteLine($"{nameof(JMATileSelectingViewCell)}.{nameof(ReloadJMATiles)}() ... Latest HRPNs = {Top.JMATiles.HRPNs_Latest}");
	}
}

