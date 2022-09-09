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
		Console.WriteLine($"~~~~~~~~~ {nameof(SketchPads_Disappearing)} START ~~~~~~~~~~~~~~");
		Console.WriteLine($"~~~~~~~~~ => {filePath}");
		if (sketchPad.DrawingData is null)
			return;

		try
		{
			Directory.CreateDirectory(filePath);
			File.WriteAllBytes(filePath, sketchPad.DrawingData.GetBytes());

			Console.WriteLine($"~~~~~~~~~ {nameof(SketchPads_Disappearing)} TASK COMPLETE ~~~~~~~~~~~~~~");
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);

			AirTote.Services.MsgBox.DisplayAlert("Canvas Save Failed", ex.ToString(), "OK");
		}
		Console.WriteLine($"~~~~~~~~~ {nameof(SketchPads_Disappearing)} FINISH ~~~~~~~~~~~~~~");
	}

	private void SketchPads_Appearing(object? sender, EventArgs e)
	{
		if (!File.Exists(filePath) || sketchPad.DrawingData is null)
			return;

		var bytes = File.ReadAllBytes(filePath);
		sketchPad.DrawingData.FromBytes(bytes);
	}

}
