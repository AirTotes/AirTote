using System.Threading.Tasks;
using AngleSharp;
using FIS_J.Services;
using FIS_J.ViewModels;

namespace FIS_J.Views
{
	public partial class SettingPage : ContentPage
	{
		SettingPageViewModel viewModel { get; } = new();
		AISJapan ais = new("", "");

		public SettingPage()
		{
			InitializeComponent();
			BindingContext = viewModel;

			viewModel.WebPage = new HtmlWebViewSource()
			{
				Html = @"<!DOCTYPE html>
<html lang='ja'>
<body>
abcde
</body>
</html>"
			};

			Task.Run(async () =>
			{
				System.Diagnostics.Debug.WriteLine("Start");

				var result = await ais.GetWhatsNewAsync();
				viewModel.WebPage = new HtmlWebViewSource()
				{
					Html = result.ToHtml()
				};
				System.Diagnostics.Debug.WriteLine("Complete");
			});
		}
	}
}
