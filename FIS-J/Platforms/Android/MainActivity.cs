
using Android.App;
using Android.Content.PM;

namespace FIS_J
{
	[Activity(Label = "FIS-J", Icon = "@mipmap/appicon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
	public class MainActivity : MauiAppCompatActivity
	{
	}
}