
using System.Threading.Tasks;
using FIS_J.Services;
using Xamarin.Forms;

namespace FIS_J.Views
{
	public partial class AboutPage : ContentPage
	{
		public AboutPage()
		{
			InitializeComponent();

			Task.Run(async () =>
			{
				var ais = new AISJapan("your_id", "your_password");
				System.Diagnostics.Debug.WriteLine("PASS Running");
				var result = await ais.GetPage("https://aisjapan.mlit.go.jp/html/AIP/html/20220324/eAIP/20220324/JP-menu-jp-JP.html");
				System.Diagnostics.Debug.WriteLine(result);
				webView.Source = new HtmlWebViewSource()
				{
					Html = result
				};
			});
		}
	}
}