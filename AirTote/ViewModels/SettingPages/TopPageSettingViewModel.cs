using CommunityToolkit.Mvvm.ComponentModel;
using AirTote.Services;

namespace AirTote.ViewModels.SettingPages;

public partial class TopPageSettingViewModel : ObservableObject
{
	static readonly TimeSpan IntervalDefaultValue = new(0, 0, 2);

	static List<TimeSpan> _IntervalList { get; } = new List<TimeSpan>
	{
		new(0, 0, 0, 0, 100),
		new(0, 0, 0, 0, 200),
		new(0, 0, 0, 0, 500),
		new(0, 0, 1),
		new(0, 0, 2),
		new(0, 0, 5),
		new(0, 0, 10),
		new(0, 0, 30),
		new(0, 1, 0),
	};

	public static System.Collections.IList IntervalList => _IntervalList;
	public static System.Collections.IList IntervalStrList => _IntervalList.Select(v => v.ToString(@"m\分s\.f\秒")).ToList();

	public TopPageSettingViewModel()
	{
		PreferenceManager.TryGet<bool>(PreferenceManager.Keys.TopPage_EnableLocationService, ref _IsLocationEnabled);
		PreferenceManager.TryGet<bool>(PreferenceManager.Keys.TopPage_EnableLocationFollowAnimation, ref _IsLocationFollowAnimationEnabled);
		PreferenceManager.TryGet(PreferenceManager.Keys.TopPage_LocationRefleshInterval, ref _LocationRefleshInterval);

		LocationRefleshIntervalIndex = _IntervalList.IndexOf(LocationRefleshInterval);
	}

	[ObservableProperty]
	private bool _IsLocationEnabled = true;
	[ObservableProperty]
	private bool _IsLocationFollowAnimationEnabled = true;
	[ObservableProperty]
	private TimeSpan _LocationRefleshInterval = IntervalDefaultValue;
	[ObservableProperty]
	private int _LocationRefleshIntervalIndex;

	partial void OnLocationRefleshIntervalIndexChanged(int value)
		=> LocationRefleshInterval = _IntervalList[value];

	partial void OnIsLocationEnabledChanged(bool value)
		=> PreferenceManager.Set(PreferenceManager.Keys.TopPage_EnableLocationService, value);
	partial void OnIsLocationFollowAnimationEnabledChanged(bool value)
		=> PreferenceManager.Set(PreferenceManager.Keys.TopPage_EnableLocationFollowAnimation, value);
	partial void OnLocationRefleshIntervalChanged(TimeSpan value)
		=> PreferenceManager.Set(PreferenceManager.Keys.TopPage_LocationRefleshInterval, _LocationRefleshInterval);
}
