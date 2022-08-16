using System.Threading.Tasks;
using AngleSharp;
using AirTote.Services;
using AirTote.ViewModels;

namespace AirTote.Pages
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
				if (result is null)
				{
					System.Diagnostics.Debug.WriteLine("result is NULL");
					return;
				}
				viewModel.WebPage = new HtmlWebViewSource()
				{
					Html = result.ToHtml()
				};
				System.Diagnostics.Debug.WriteLine("Complete");
			});
		}
	}
}
