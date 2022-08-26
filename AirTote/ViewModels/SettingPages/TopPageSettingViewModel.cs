using CommunityToolkit.Mvvm.ComponentModel;
using AirTote.Services;

namespace AirTote.ViewModels.SettingPages;

public partial class TopPageSettingViewModel : ObservableObject
{
	public TopPageSettingViewModel()
	{
		PreferenceManager.TryGet<bool>(PreferenceManager.Keys.TopPage_EnableLocationService, ref _IsLocationEnabled);
		PreferenceManager.TryGet<bool>(PreferenceManager.Keys.TopPage_EnableLocationFollowAnimation, ref _IsLocationFollowAnimationEnabled);
		PreferenceManager.TryGet(PreferenceManager.Keys.TopPage_LocationRefleshInterval, ref _LocationRefleshInterval);
	}

	[ObservableProperty]
	private bool _IsLocationEnabled;
	[ObservableProperty]
	private bool _IsLocationFollowAnimationEnabled;
	[ObservableProperty]
	private TimeSpan _LocationRefleshInterval;

	public void SeveToPreference()
	{
		PreferenceManager.Set(PreferenceManager.Keys.TopPage_EnableLocationService, _IsLocationEnabled);
		PreferenceManager.Set(PreferenceManager.Keys.TopPage_EnableLocationFollowAnimation, _IsLocationFollowAnimationEnabled);
		PreferenceManager.Set(PreferenceManager.Keys.TopPage_LocationRefleshInterval, _LocationRefleshInterval);
	}
}
