using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using AirTote.TwoPaneView;

using Csv;

using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace AirTote.Services;

public class ImagerySourceReader
{
	public string GroupName { get; set; } = "";
	public string ShortTitle { get; set; } = "";
	public string FullTitle { get; set; } = "";
	public string URL { get; set; } = "";
	public bool IsPublic { get; set; }
	public bool IsFullTitleLong => FullTitle.Length > 28;

	public TwoPaneView.TwoPaneView? tpv { get; set; }

	static readonly private OpenBrowserCommand obc = new();

	private ICommand? _Command;
	public ICommand Command
	{
		get
		{
			if (_Command is not null)
				return _Command;

			if (IsPublic && tpv is not null)
			{
				_Command = new ChangeRightPaneViewCommand(tpv);
			}
			else
			{
				_Command = obc;
			}

			return _Command;
		}
	}

	private object? _CommandParameter = null;
	public object? CommandParametor
	{
		get
		{
			if (_CommandParameter is not null)
				return _CommandParameter;

			if (IsPublic && tpv is not null)
			{
				Microsoft.Maui.Controls.WebView? webView;

				try
				{
					webView = new()
					{
						Source = URL
					};
				}
				catch (Exception ex)
				{
					MsgBox.DisplayAlert("Imagery Show Error", "内部エラーが発生しました\n(Generate Command Parametor with public url)\n" + ex.Message, "OK");
					return null;
				}

				webView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
						.EnableZoomControls(true);
				webView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
						.DisplayZoomControls(true);

				_CommandParameter = new ViewProps()
				{
					Title = FullTitle,
					Content = webView
				};
			}
			else
			{
				_CommandParameter = URL;
			}

			return _CommandParameter;
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

		List<ImagerySourceReader> records = new();
		await foreach (var row in CsvReader.ReadFromStreamAsync(fileStream))
		{
			records.Add(new()
			{
				GroupName = row[0] ?? "",
				ShortTitle = row[1] ?? "",
				FullTitle = row[2] ?? "",
				URL = row[3] ?? "",
				IsPublic = bool.Parse(row[4])
			});
		}

		return records;
	}

	public override string ToString() => $"[{this.GroupName}] '{this.FullTitle}'({this.ShortTitle}) => {this.URL} (IsPublic?:{this.IsPublic})";
}
