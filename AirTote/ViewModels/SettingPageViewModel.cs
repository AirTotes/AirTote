using AirTote.TwoPaneView;

namespace AirTote.ViewModels;

public class SettingPageViewModel : BaseViewModel
{
	public SettingPageViewModel()
	{
		Title = "Settings";
	}

	private ChangeRightPaneViewCommand? _ChangeRightPaneViewCommand;
	public ChangeRightPaneViewCommand? ChangeRightPaneViewCommand
	{
		get => _ChangeRightPaneViewCommand;
		set => SetProperty(ref _ChangeRightPaneViewCommand, value);
	}
}
