using CommunityToolkit.Mvvm.ComponentModel;
using AirTote.Services;

namespace AirTote.ViewModels.SettingPages;

public partial class TopPageSettingViewModel : ObservableObject
{
	public TopPageSettingViewModel()
	{
		_IsLocationEnabled = PreferenceManager.Get<bool>(PreferenceManager.Keys.TopPage_EnableLocationService);
		_IsLocationFollowAnimationEnabled = PreferenceManager.Get<bool>(PreferenceManager.Keys.TopPage_EnableLocationFollowAnimation);

		_LocationRefleshInterval = PreferenceManager.Get<TimeSpan>(PreferenceManager.Keys.TopPage_LocationRefleshInterval);

		if (
			!PreferenceManager.TryGet(PreferenceManager.Keys.TopPage_LocationRefleshInterval, out _LocationRefleshInterval)
			|| _LocationRefleshInterval == default
			)
			_LocationRefleshInterval = new(0, 0, 2);
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
