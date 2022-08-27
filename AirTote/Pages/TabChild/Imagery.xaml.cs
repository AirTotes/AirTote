using AirTote.Services;
using AirTote.TwoPaneView;
using AirTote.ViewModels;

using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace AirTote.Pages.TabChild;

public partial class Imagery : ContentPage
{
	public Imagery()
	{
		InitializeComponent();

		BindingContext = viewModel;
		viewModel.ChangeRightPaneViewCommand = new(TPV);

		Task.Run(CsvToImageryListAsync);
	}
	internal static readonly OpenBrowserCommand OpenUri = new();
	ImageryPageViewModel viewModel { get; } = new();

	/// <summary>CsvファイルからImageryの中身を生成する関数</summary>
	private async void CsvToImageryListAsync()
	{
		MyCsvReader reader = new();

		try //例外処理
		{
			List<MyCsvReader> text = await reader.ReadCsvFileAsync("ImagerySource.csv");

			string FullName = string.Empty;

			Dictionary<string, List<MyCsvReader>> GroupNameKeyDic = new();
			foreach (var textItem in text)
			{
				if (!GroupNameKeyDic.ContainsKey(textItem.GroupName))
					GroupNameKeyDic[textItem.GroupName] = new();

				GroupNameKeyDic[textItem.GroupName].Add(textItem);
			}

			List<TableSection> sectionList = new();
			foreach (KeyValuePair<string, List<MyCsvReader>> kvp in GroupNameKeyDic)
			{
				TableSection sec = new(kvp.Key);

				List<MyCsvReader> recordList = kvp.Value;

				foreach (MyCsvReader record in recordList)
				{
					TextCell cell = new()
					{
						Text = record.ShortTitle,
						Detail = record.FullTitle,
					};

					if (record.IsPublic)
					{
						Microsoft.Maui.Controls.WebView webView = new()
						{
							Source = record.URL
						};

						webView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
								.EnableZoomControls(true);
						webView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
								.DisplayZoomControls(true);

						cell.Command = viewModel.ChangeRightPaneViewCommand;
						cell.CommandParameter = new ViewProps()
						{
							Title = record.FullTitle,
							Content = webView
						};
					}
					else
					{
						cell.Command = OpenUri;
						cell.CommandParameter = record.URL;
					}

					sec.Add(cell);
				}

				sectionList.Add(sec);
			}

			MainThread.BeginInvokeOnMainThread(() =>
			{
				foreach (TableSection sec in sectionList)
					ImageryList.Root.Add(sec);
			});
		}
		catch (Exception ex) //例外の種類
		{
			// MsgBox.DisplayAlert("Read Asset Error", "エラーが発生しました\n" + ex.Message, "OK");

			Console.WriteLine(ex);
			return;
		}
	}
}
