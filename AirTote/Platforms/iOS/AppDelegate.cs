using Foundation;

using UIKit;

namespace AirTote;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
	{
		bool result = base.FinishedLaunching(application, launchOptions);

		try
		{
			Firebase.Core.App.Configure();
			Firebase.Crashlytics.Crashlytics.SharedInstance.Init();
			Firebase.Crashlytics.Crashlytics.SharedInstance.SetCrashlyticsCollectionEnabled(true);
			Firebase.Crashlytics.Crashlytics.SharedInstance.SendUnsentReports();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			Firebase.Crashlytics.Crashlytics.SharedInstance.Dispose();
		}

		return result;
	}
}

