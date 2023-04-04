using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AirTote.Services;
using AirTote.Utils;

using AngleSharp;

namespace AirTote.Pages
{
	public partial class FlightPlan : ContentPage
	{
		string? fpHtml = null;

		public FlightPlan()
		{
			InitializeComponent();

			Loaded += FlightPlan_Loaded;

			FPWebView.Navigated += async (s, e) => await FPWebView.EvaluateJavaScriptAsync("window.isDownloadButtonHidden = true; window.HideDownloadButton();");
		}

		static private Task<string> getFlightPlanHtml()
			=> getFlightPlanHtml(new CancellationTokenSource().Token);
		static private async Task<string> getFlightPlanHtml(CancellationToken token)
		{
			using Stream f = await FileSystem.OpenAppPackageFileAsync("flight_plan_sheet.html");
			using StreamReader reader = new(f);

			return await reader.ReadToEndAsync(token);
		}

		private async void FlightPlan_Loaded(object? sender, EventArgs e)
		{
			fpHtml ??= await getFlightPlanHtml();

			FPWebView.Source = new HtmlWebViewSource()
			{
				Html = fpHtml,
				BaseUrl = "https://flightplan.airtote.jp/flight_plan_sheet.html",
			};
		}

		async Task<string?> GetFPDataUriString()
		{
			string? s = null;
			await FPWebView.EvaluateJavaScriptAsync("setTimeout(async()=>await GetFlightPlanPdfDataUri(),0)");

			for (int i = 0; s is null && i < 10; i++)
			{
				await Task.Delay(200);
				s = await FPWebView.EvaluateJavaScriptAsync("GetFlightPlanPdfDataUriResult");
			}

			return s;
		}

		async void Button_Clicked(object sender, EventArgs e)
		{
			try
			{
				string? s = await GetFPDataUriString();
				{
					await Task.Delay(200);
					s = await FPWebView.EvaluateJavaScriptAsync("GetFlightPlanPdfDataUriResult");
				}
				// PDF保存処理
				if (s is not null)
					SavePDF(s);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		[GeneratedRegex(@"^data:application/pdf;filename=.{1,256}\.pdf;base64,[\w\+/=]+$")]
		static private partial Regex pdfDataUriCheckRegex();

		async Task<string?> SavePDF(string datauri)
		{
			PdfDataUri pdf;

			try
			{
				pdf = PdfDataUriParser.Parse(datauri);
			}
			catch (Exception ex)
			{
				MsgBox.DisplayAlert("Generate PDF", "Cannot generate PDF\n" + ex.ToString(), "OK");
				return null;
			}

			string fpath;
			try
			{
				DirectoryInfo dir = new(
					Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
						"FLIGHT_PLAN"
					)
				);
				if (!dir.Exists)
					dir.Create();

				string fname = $"FlightPlan_{DateTime.UtcNow:yyyyMMddTHHmmssZ}.pdf";
				fpath = Path.Combine(dir.FullName, fname);

				await File.WriteAllBytesAsync(fpath, pdf.data);

				return fpath;
			}
			catch (Exception ex)
			{
				MsgBox.DisplayAlert("Save PDF", "Cannot save PDF\n" + ex.ToString(), "OK");
				return null;
			}
		}
	}
}
