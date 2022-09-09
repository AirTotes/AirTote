using AirTote.ViewModels.SettingPages;

namespace AirTote.Pages.Settings;

public partial class TopPage : ContentPage
{
	TopPageSettingViewModel VM { get; } = new();

	public TopPage()
	{
		InitializeComponent();

		Content.BindingContext = VM;
	}
}
