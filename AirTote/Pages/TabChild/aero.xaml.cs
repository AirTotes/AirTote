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

			Task.Run(async () =>
			{
				try
				{
					ais = await AISJapan.FromSecureStorageAsync(SecureStorage.Default);
				}
				catch (Exception ex)
				{
					MainThread.BeginInvokeOnMainThread(async () =>
						await Shell.Current.CurrentPage.DisplayAlert("AIS Japan Account Error", "設定画面にて自身のアカウントを設定してください。\n" + ex.Message, "OK")
					);
				}
			});
		}

		private async void SUPsView_Clicked(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("PASS Running");
			if (ais is null)
				return;
			var result = await ais.GetPage("https://aisjapan.mlit.go.jp/html/AIP/html/20220224/eSUP/JP-eSUPs-en-JP.html");
			System.Diagnostics.Debug.WriteLine(result);
			html.HTML = new HtmlWebViewSource()
			{
				Html = result
			};
		}

		private async void AIPView_Clicked(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("PASS Running");
			if (ais is null)
				return;
			var result = await ais.GetPage("https://aisjapan.mlit.go.jp/html/AIP/html/20220224/eAIP/20220301/JP-menu-en-JP.html");
			System.Diagnostics.Debug.WriteLine(result);
			html.HTML = new HtmlWebViewSource()
			{
				Html = result
			};
		}

		private async void AICsView_Clicked(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("PASS Running");
			if (ais is null)
				return;
			var result = await ais.GetPage("https://aisjapan.mlit.go.jp/html/AIP/html/20220324/eAIC/JP-eAICs-jp-JP.html");
			System.Diagnostics.Debug.WriteLine(result);
			html.HTML = new HtmlWebViewSource()
			{
				Html = result
			};
		}
	}
}
