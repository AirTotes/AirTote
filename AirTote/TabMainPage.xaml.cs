using AirTote.AISJapanParser.EAIP;
using AirTote.Services;

namespace AirTote;

public partial class TabMainPage : Shell
{
	public Task<AISJapan> AISJapanConnection { get; } = AISJapanUtils.FromSecureStorageAsync(SecureStorage.Default);

	public AIPDateInfo AIPDate { get; } = new(new(2022, 9, 8), new(2022, 9, 8));

	private AISJapanParser.EAIP.Menu? _EAIPMenu;
	public async Task<AISJapanParser.EAIP.Menu> GetEAIPMenuAsync()
	{
		if (_EAIPMenu is not null)
			return _EAIPMenu;
		else
			return await AISJapanParser.EAIP.Menu.GetAsync(await AISJapanConnection, AIPDate);
	}

	public TabMainPage()
	{
		InitializeComponent();

#if IOS
		if (new Version(13, 0) <= DeviceInfo.Version && DeviceInfo.Current.Idiom != DeviceIdiom.Phone)
			SketchPadTab.IsVisible = true;
#endif
	}

	public static new TabMainPage? Current => Shell.Current as TabMainPage;
}
