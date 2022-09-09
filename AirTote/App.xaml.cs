namespace AirTote;

public partial class App : Application
{
	static public Color LightSecondaryColor
		=> Current?.Resources[nameof(LightSecondaryColor)] as Color ?? Colors.Black;
	static public Color DarkSecondaryColor
		=> Current?.Resources[nameof(DarkSecondaryColor)] as Color ?? Colors.White;

	public App()
	{
		InitializeComponent();

		MainPage = new TabMainPage();
	}
}
