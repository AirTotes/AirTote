using AirTote.Components;
using AirTote.Models;
using AirTote.TwoPaneView;

namespace AirTote.Pages;

public partial class ThirdPartyLicenses
{
	public const string LICENSE_INFO_DIR = "ThirdPartyLicenses";
	public const string LICENSE_LIST_FILE_NAME = "license_list.json";

	public ThirdPartyLicenses()
	{
		InitializeComponent();

		TwoPaneView.RightPaneContent = new Label()
		{
			Text = "Select package to check license",
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};

		Task.Run(Init);
	}

	async Task Init()
	{
		List<LicenseJsonSchema> licenseList = new();
		await LoadJson(Path.Combine(LICENSE_INFO_DIR, LICENSE_LIST_FILE_NAME), licenseList);

		licenseList.Sort((x, y) => string.Compare(x.id, y.id));

		PackageListView.ItemsSource = licenseList;
	}

	static async Task LoadJson(string path, List<LicenseJsonSchema> licenses)
	{
		object? obj;
		try
		{
			using (var stream = await FileSystem.OpenAppPackageFileAsync(path))
			{
				if (stream is null)
					return;
				obj = await System.Text.Json.JsonSerializer.DeserializeAsync(stream, typeof(LicenseJsonSchema[]));
			}
		}
		catch (DirectoryNotFoundException)
		{
			return;
		}
		catch (FileNotFoundException)
		{
			return;
		}
		catch (Exception ex)
		{
			AirTote.Services.MsgBox.DisplayAlert("Load / Parse License Json Error", $"Cannot load file ({path})\n{ex}", "OK");
			return;
		}

		if (obj is not LicenseJsonSchema[] arr)
			return;

		licenses.AddRange(arr);
	}

	async void PackageListView_ItemTapped(object? sender, ItemTappedEventArgs e)
	{
		if (e.Item is not LicenseJsonSchema license)
			return;

		TwoPaneView.RightPaneContent = new LicenseView(license);
		await this.TwoPaneView.NotifyRightPaneContentChanged(license.id);
	}
}
