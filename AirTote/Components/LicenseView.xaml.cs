using AirTote.Models;
using AirTote.Pages;

namespace AirTote.Components;

public partial class LicenseView : ContentView
{
	public LicenseView(LicenseJsonSchema license)
	{
		this.BindingContext = license;

		InitializeComponent();

		Task.Run(() => SetLicenseText(license));
	}

	async Task SetLicenseText(LicenseJsonSchema license)
	{
		if (string.IsNullOrWhiteSpace(license.license))
			return;

		string fileContent = "";
		try
		{
			using (var stream = await FileSystem.OpenAppPackageFileAsync(Path.Combine(license.BaseDirectory, license.license)))
			{
				if (stream is null)
					return;
				fileContent = await new StreamReader(stream).ReadToEndAsync();
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}

		LicenseBodyLabel.TextType = fileContent.TrimEnd().EndsWith("</html>") ? TextType.Html : TextType.Text;
		LicenseBodyLabel.Text = fileContent;
		if (LicenseBodyLabel.TextType == TextType.Html)
			LicenseBodyLabel.BackgroundColor = Colors.White;
	}
}
