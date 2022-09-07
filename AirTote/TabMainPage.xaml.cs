namespace AirTote;

public partial class TabMainPage : Shell
{
	public TabMainPage()
	{
		InitializeComponent();

#if IOS
		if (new Version(13, 0) <= DeviceInfo.Version)
			ScratchPadTab.IsVisible = true;
#endif
	}
}
