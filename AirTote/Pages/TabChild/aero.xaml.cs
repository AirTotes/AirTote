using System;
using System.Threading.Tasks;

using AirTote.Services;
using AirTote.AISJapanParser.EAIP;
using AirTote.ViewModels;
using AngleSharp.Dom;

namespace AirTote.Pages.TabChild
{
	public partial class aero : ContentPage
	{
		HtmlViewModel html = new();
		AISJapan? ais { get; set; }
		InfoDates? AIPInfoDates { get; set; }

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
				if (ais is null)
				{
					AIPInfoDates = null;
					ais = await AISJapanUtils.FromSecureStorageIfNeededAsync(SecureStorage.Default, ais);
				}

				if (AIPInfoDates is null && (await ais.GetAIPAsync()) is AngleSharp.Dom.IDocument doc)
					AIPInfoDates ??= new(doc);
			}
			catch (Exception ex)
			{
				ais = null;
				AIPInfoDates = null;

				MsgBox.DisplayAlert("AIS Japan Account Error", "設定画面にて自身のアカウントを設定してください。\n" + ex.Message, "OK");
			}
		}

		private void SUPsView_Clicked(object sender, EventArgs e)
		{
			if (AIPInfoDates?.Current is null)
			{
				MsgBox.DisplayAlert("AIS Japan Load Error", "AIS Japanへのアクセスに失敗しました。ページの再読み込みや、アカウントの再設定を行ってください。", "OK");
				return;
			}

			ShowPageAsync($"https://aisjapan.mlit.go.jp/html/AIP/html/{AIPInfoDates.Current.PublicationDateString}/eSUP/JP-eSUPs-en-JP.html");
		}

		private void AIPView_Clicked(object sender, EventArgs e)
			=> ShowPageAsync(AIPInfoDates?.Current?.GetUrl("JP-menu-en-JP.html"));

		private void AICsView_Clicked(object sender, EventArgs e)
		{
			if (AIPInfoDates?.Current is null)
			{
				MsgBox.DisplayAlert("AIS Japan Load Error", "AIS Japanへのアクセスに失敗しました。ページの再読み込みや、アカウントの再設定を行ってください。", "OK");
				return;
			}

			ShowPageAsync($"https://aisjapan.mlit.go.jp/html/AIP/html/{AIPInfoDates.Current.PublicationDateString}/eAIC/JP-eAICs-jp-JP.html");
		}

		async void ShowPageAsync(string? url)
		{
			if (ais is null || string.IsNullOrEmpty(url))
			{
				MsgBox.DisplayAlert("AIS Japan Load Error", "AIS Japanへのアクセスに失敗しました。ページの再読み込みや、アカウントの再設定を行ってください。", "OK");
				return;
			}

			var result = await ais.GetPage(url);

			MainThread.BeginInvokeOnMainThread(() =>
			{
				html.HTML = new HtmlWebViewSource()
				{
					Html = result,
					BaseUrl = Path.GetDirectoryName(url),
				};
			});
		}
	}
}
