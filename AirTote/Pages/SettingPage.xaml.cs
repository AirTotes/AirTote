using AirTote.TwoPaneView;
using AirTote.ViewModels;

namespace AirTote.Pages;

public partial class SettingPage : ContentPage
{
	SettingPageViewModel viewModel { get; } = new();

	public SettingPage()
	{
		InitializeComponent();
		BindingContext = viewModel;

		viewModel.ChangeRightPaneViewCommand = new(TPV);
	}
}
