using System;
using System.Threading.Tasks;

using AirTote.Services;
using AirTote.ViewModels;

namespace AirTote.Pages.TabChild
{
	public partial class aero : ContentPage
	{
		HtmlViewModel html = new();
		AISJapan? ais { get; set; }
		public aero()
		{
			InitializeComponent();
			BindingContext = html;

			Appearing += Aero_Appearing;
		}

		private async void Aero_Appearing(object? sender, EventArgs e)
		{
			html.HTML = null;

			try
			{
				ais = await AISJapan.FromSecureStorageIfNeededAsync(SecureStorage.Default, ais);
			}
			catch (Exception ex)
			{
				ais = null;

				MsgBox.DisplayAlert("AIS Japan Account Error", "設定画面にて自身のアカウントを設定してください。\n" + ex.Message, "OK");
			}
		}

		private void SUPsView_Clicked(object sender, EventArgs e)
			=> ShowPageAsync("https://aisjapan.mlit.go.jp/html/AIP/html/20220224/eSUP/JP-eSUPs-en-JP.html");

		private void AIPView_Clicked(object sender, EventArgs e)
			=> ShowPageAsync("https://aisjapan.mlit.go.jp/html/AIP/html/20220224/eAIP/20220301/JP-menu-en-JP.html");

		private void AICsView_Clicked(object sender, EventArgs e)
			=> ShowPageAsync("https://aisjapan.mlit.go.jp/html/AIP/html/20220324/eAIC/JP-eAICs-jp-JP.html");

		async void ShowPageAsync(string url)
		{
			if (ais is null)
				return;

			var result = await ais.GetPage(url);

			html.HTML = new HtmlWebViewSource()
			{
				Html = result
			};
		}
	}
}
