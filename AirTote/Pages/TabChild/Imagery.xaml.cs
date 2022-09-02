using AirTote.Services;
using AirTote.TwoPaneView;
using AirTote.ViewModels;

using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace AirTote.Pages.TabChild;

public class ImageryList : List<ImagerySourceReader>
{
	public string Name { get; private set; }

	public ImageryList(string name, List<ImagerySourceReader> ImageryList) : base(ImageryList)
	{
		Name = name;
	}
}

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

		try //例外処理
		{
			List<ImagerySourceReader> text = await ImagerySourceReader.ReadCsvFileAsync("ImagerySource.csv");

			string FullName = string.Empty;

			Dictionary<string, List<ImagerySourceReader>> GroupNameKeyDic = new();
			foreach (var textItem in text)
			{
				textItem.tpv = TPV;
				if (!GroupNameKeyDic.ContainsKey(textItem.GroupName))
					GroupNameKeyDic[textItem.GroupName] = new();

				GroupNameKeyDic[textItem.GroupName].Add(textItem);
			}
			List<ImageryList> imageryGroup = new();

			foreach (KeyValuePair<string, List<ImagerySourceReader>> textItem in GroupNameKeyDic)
			{
				ImageryList imageryList = new(textItem.Key, textItem.Value);
				imageryGroup.Add(imageryList);
			}
			MainThread.BeginInvokeOnMainThread(() =>
			{
				try
				{
					ImageryList.ItemsSource = imageryGroup;
				}
				catch (Exception ex)
				{
					MsgBox.DisplayAlert("Imagery Show Error", "エラーが発生しました\n" + ex.Message, "OK");
				}
			});
		}
		catch (Exception ex) //例外の種類
		{
			MsgBox.DisplayAlert("Read Asset Error", $"{ex}", "OK");
			Console.WriteLine(ex);
			return;
		}
	}
}
