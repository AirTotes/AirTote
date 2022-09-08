namespace AirTote.Pages.TabChild;

public partial class SketchPads : ContentPage
{
	static readonly string filePath = Path.Combine(FileSystem.Current.AppDataDirectory, "SketchPads", "Test.bin");
	public SketchPads()
	{
		InitializeComponent();

		this.Appearing += SketchPads_Appearing;
		this.Disappearing += SketchPads_Disappearing;
	}

	private void SketchPads_Disappearing(object? sender, EventArgs e)
	{
		if (sketchPad.DrawingData is null)
			return;

		try
		{
			Directory.CreateDirectory(filePath);
			sketchPad.DrawingData.SaveToFile(filePath);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}
	}

	private void SketchPads_Appearing(object? sender, EventArgs e)
	{
		if (!File.Exists(filePath) || sketchPad.DrawingData is null)
			return;

		sketchPad.DrawingData.FromFile(filePath);
	}

}
