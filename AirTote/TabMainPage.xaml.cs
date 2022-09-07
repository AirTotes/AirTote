namespace AirTote;

public partial class TabMainPage : Shell
{
	public TabMainPage()
	{
		InitializeComponent();

#if IOS
		ScratchPadTab.IsVisible = true;
#endif
	}
}
