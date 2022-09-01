using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using AirTote.TwoPaneView;

using CsvHelper.Configuration.Attributes;

using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace AirTote.Services;

public class ImagerySourceReader
{
	public string GroupName { get; set; } = "";
	public string ShortTitle { get; set; } = "";
	public string FullTitle { get; set; } = "";
	public string URL { get; set; } = "";
	public bool IsPublic { get; set; }

	[Ignore]
	public TwoPaneView.TwoPaneView? tpv { get; set; }

	static readonly private OpenBrowserCommand obc = new();

	[Ignore]
	public ICommand Command
	{
		get
		{
			if (IsPublic && tpv is not null)
			{
				return new ChangeRightPaneViewCommand(tpv);
			}
			else
			{
				return obc;
			}
		}
	}

	[Ignore]
	public object CommandParametor
	{
		get
		{
			if (IsPublic && tpv is not null)
			{
				Microsoft.Maui.Controls.WebView webView = new()
				{
					Source = URL
				};

				webView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
						.EnableZoomControls(true);
				webView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
						.DisplayZoomControls(true);

				return new ViewProps()
				{
					Title = FullTitle,
					Content = webView
				};
			}
			else
			{
				return URL;
			}
		}
	}
	public static async Task<string> ReadTextFileAsync(string filePath)
	{
		using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(filePath);
		using StreamReader reader = new StreamReader(fileStream);

		return await reader.ReadToEndAsync();
	}


	// List<T>
	public static async Task<List<ImagerySourceReader>> ReadCsvFileAsync(string filePath)
	{
		using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(filePath);
		using StreamReader reader = new StreamReader(fileStream);

		using CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture);
		IEnumerable<ImagerySourceReader> records = csv.GetRecords<ImagerySourceReader>();

		return records.ToList();
	}
}
