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

			SubmitToOfficeButton.IsEnabled = Email.Default.IsComposeSupported;
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
				// PDF保存処理
				if (s is not null && await SavePDF(s) is string fpath)
				{
					try
					{
						await Share.Default.RequestAsync(new ShareFileRequest()
						{
							Title = "Open FLIGHT PLAN with...",
							File = new ShareFile(fpath)
						});
					}
					catch (Exception ex)
					{
						MsgBox.DisplayAlert("Open PDF", "Cannot open PDF\n" + ex.ToString(), "OK");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return;
			}

		}

		async void OpenMailApp(object? sender, EventArgs e)
		{
			if (!Email.Default.IsComposeSupported)
			{
				MsgBox.DisplayAlert("Submit to Office Failed", "This feature is not supported on this device.\nMaybe: Mail App is not configured?.", "OK");
				return;
			}

			string? dataUri;
			try
			{
				dataUri = await GetFPDataUriString();
			}
			catch (Exception ex)
			{
				MsgBox.DisplayAlert("Submit to Office Failed", "Generate PDF Failed.\n" + ex.ToString(), "OK");
				return;
			}

			if (string.IsNullOrEmpty(dataUri))
			{
				MsgBox.DisplayAlert("Submit to Office Failed", "Generate PDF Failed.\nThe return value was NULL or Empty.", "OK");
				return;
			}

			string? fpath;
			try
			{
				fpath = await SavePDF(dataUri);
			}
			catch (Exception ex)
			{
				MsgBox.DisplayAlert("Submit to Office Failed", "Save PDF Failed.\n" + ex.ToString(), "OK");
				return;
			}

			if (string.IsNullOrEmpty(fpath))
			{
				MsgBox.DisplayAlert("Submit to Office Failed", "Save PDF Failed.\nThe return value was NULL or Empty.", "OK");
				return;
			}

			EmailMessage emailMessage = new()
			{
				Subject = "飛行計画書の提出",
				Body = """
東京空港事務所
羽田空港 航空管制運航情報官 様


~~~~~~~~~~~~~~~~~~~~~~~~
Powered by: AirTote
https://airtote.jp
""",
				To = new List<string>()
				{
				},
				BodyFormat = EmailBodyFormat.PlainText,
				Attachments = new List<EmailAttachment>()
				{
					new EmailAttachment(fpath, "application/pdf")
				},
			};

			try
			{
				await Email.Default.ComposeAsync(emailMessage);
			}
			catch (Exception ex)
			{
				MsgBox.DisplayAlert("Submit to Office Failed", "Open Mail App Failed.\n" + ex.ToString(), "OK");
				return;
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
