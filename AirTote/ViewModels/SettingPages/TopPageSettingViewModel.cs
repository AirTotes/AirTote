using CommunityToolkit.Mvvm.ComponentModel;
using AirTote.Services;

namespace AirTote.ViewModels.SettingPages;

public partial class TopPageSettingViewModel : ObservableObject
{
	static readonly TimeSpan IntervalDefaultValue = new(0, 0, 2);

	public TopPageSettingViewModel()
	{
		PreferenceManager.TryGet<bool>(PreferenceManager.Keys.TopPage_EnableLocationService, ref _IsLocationEnabled);
		PreferenceManager.TryGet<bool>(PreferenceManager.Keys.TopPage_EnableLocationFollowAnimation, ref _IsLocationFollowAnimationEnabled);
		PreferenceManager.TryGet(PreferenceManager.Keys.TopPage_LocationRefleshInterval, ref _LocationRefleshInterval);
	}

	[ObservableProperty]
	private bool _IsLocationEnabled = true;
	[ObservableProperty]
	private bool _IsLocationFollowAnimationEnabled = true;
	[ObservableProperty]
	private TimeSpan _LocationRefleshInterval = IntervalDefaultValue;

	public void SeveToPreference()
	{
		PreferenceManager.Set(PreferenceManager.Keys.TopPage_EnableLocationService, _IsLocationEnabled);
		PreferenceManager.Set(PreferenceManager.Keys.TopPage_EnableLocationFollowAnimation, _IsLocationFollowAnimationEnabled);
		PreferenceManager.Set(PreferenceManager.Keys.TopPage_LocationRefleshInterval, _LocationRefleshInterval);
	}
}
