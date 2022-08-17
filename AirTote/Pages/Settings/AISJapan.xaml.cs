using AirTote.ViewModels.SettingPages;

namespace AirTote.Pages.Settings;

public partial class AISJapan : ContentPage
{
	public const string AIS_JAPAN_CREATE_ACCOUNT = "https://aisjapan.mlit.go.jp/ShowAccount.do";

	AISJapanViewModel ViewModel { get; } = new();

	public AISJapan()
	{
		InitializeComponent();

		Content.BindingContext = ViewModel;
	}

	async void CreateAccount_Tapped(object sender, EventArgs e)
		=> await Browser.OpenAsync(AIS_JAPAN_CREATE_ACCOUNT);
}
