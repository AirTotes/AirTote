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

	void Cell_Tapped(object sender, EventArgs e)
	{
		if ((sender as Cell)?.BindingContext is not ImagerySourceReader ctx)
			return;

		if (ctx.Command?.CanExecute(ctx.CommandParametor) == true)
			ctx.Command.Execute(ctx.CommandParametor);
	}

	internal static readonly OpenBrowserCommand OpenUri = new();
	ImageryPageViewModel viewModel { get; } = new();

	/// <summary>CsvファイルからImageryの中身を生成する関数</summary>
	private async void CsvToImageryListAsync()
	{

		try //例外処理
		{
			List<ImagerySourceReader> text = await ImagerySourceReader.ReadCsvFileAsync("ImagerySource.csv");

			MainThread.BeginInvokeOnMainThread(() =>
			{
				try
				{
					ImageryList.ItemsSource = text
						.Where(v => !string.IsNullOrEmpty(v.URL))
						.GroupBy(v => v.GroupName)
						.Select(v => new ImageryList(v.Key, v.ToList()));
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
			return;
		}
	}
}
