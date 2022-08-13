using AirTote.Components;
using AirTote.Models;

namespace AirTote.Pages;

public partial class ThirdPartyLicenses
{
	public const string LICENSE_INFO_DIR = "ThirdPartyLicenses";
	public const string THIRD_PARTY_LICENSE_LIST_FILE_NAME = "third_party_license_list.json";

	public ThirdPartyLicenses()
	{
		InitializeComponent();

		TwoPaneView.RightPaneContent = new Label()
		{
			Text = "Select package to check license"
		};

		Task.Run(Init);
	}

	async Task Init()
	{
		List<LicenseJsonSchema> licenseList = new();
		await LoadJson(Path.Combine(LICENSE_INFO_DIR, THIRD_PARTY_LICENSE_LIST_FILE_NAME), licenseList);

		licenseList.Sort((x, y) => string.Compare(x.id, y.id));

		PackageListView.ItemsSource = licenseList;
	}

	static async Task LoadJson(string path, List<LicenseJsonSchema> licenses)
	{
		object? obj;
		using (var stream = await FileSystem.OpenAppPackageFileAsync(path))
		{
			if (stream is null)
				return;
			obj = await System.Text.Json.JsonSerializer.DeserializeAsync(stream, typeof(LicenseJsonSchema[]));
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
