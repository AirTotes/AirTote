using System.Collections.ObjectModel;

using FIS_J.ViewModels;

namespace FIS_J;

public partial class FlyoutMenuPage : ContentPage
{
	ObservableCollection<FlyoutMenuItem> _MenuItems { get; } = new();
	public IReadOnlyCollection<FlyoutMenuItem> MenuItems => _MenuItems;

	public FlyoutMenuPage(NavigationPage topNavPage)
	{
		InitializeComponent();

		WidthRequest = 300;

		PageListView.ItemsSource = _MenuItems;

		Task.Run(() =>
		{
			_MenuItems.Add(new(topNavPage));
			_MenuItems.Add(new(new FISJ.SubmitReport()));
			_MenuItems.Add(new(new FISJ.ReservePages.ReserveSpotAndFuel()));
			_MenuItems.Add(new(new FISJ.aero()));
			_MenuItems.Add(new(new FISJ.WetherInformation()));
			_MenuItems.Add(new(new FISJ.IcaoPage()));
			_MenuItems.Add(new(new FISJ.PayLandingFee.CalcFee()));
			_MenuItems.Add(new(new Views.SettingPage()));
		});
	}
}
