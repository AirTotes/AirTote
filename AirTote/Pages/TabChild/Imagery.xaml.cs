using AirTote.Services;
using AirTote.TwoPaneView;
using AirTote.ViewModels;

namespace AirTote.Pages.TabChild;

public partial class Imagery : ContentPage
{
	public Imagery()
	{
		InitializeComponent();

		BindingContext = viewModel;
		viewModel.ChangeRightPaneViewCommand = new(TPV);
	}
	//internal static readonly OpenBrowserCommand OpenUri = new();
	ImageryPageViewModel viewModel { get; } = new();
}
