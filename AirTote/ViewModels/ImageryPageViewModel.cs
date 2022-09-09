using AirTote.TwoPaneView;

namespace AirTote.ViewModels;

public class ImageryPageViewModel : BaseViewModel
{
	public ImageryPageViewModel()
	{
		Title = "Imagery";
	}

	private ChangeRightPaneViewCommand? _ChangeRightPaneViewCommand;
	public ChangeRightPaneViewCommand? ChangeRightPaneViewCommand
	{
		get => _ChangeRightPaneViewCommand;
		set => SetProperty(ref _ChangeRightPaneViewCommand, value);
	}
}
