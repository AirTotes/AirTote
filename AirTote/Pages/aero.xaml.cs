using System;
using System.Threading.Tasks;

using AirTote.Services;
using AirTote.ViewModels;

namespace AirTote.Pages
{
	public partial class aero : ContentPage
	{
		HtmlViewModel html = new();
		AISJapan ais = new("kojdai", "Kojdai0510");
		public aero()
		{
			InitializeComponent();
			BindingContext = html;
		}

		private async void SUPsView_Clicked(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("PASS Running");
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
			var result = await ais.GetPage("https://aisjapan.mlit.go.jp/html/AIP/html/20220324/eAIC/JP-eAICs-jp-JP.html");
			System.Diagnostics.Debug.WriteLine(result);
			html.HTML = new HtmlWebViewSource()
			{
				Html = result
			};
		}
	}
}