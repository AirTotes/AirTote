using System.Collections.ObjectModel;

using FIS_J.ViewModels;

namespace FIS_J;

public partial class FlyoutMenuPage : ContentPage
{
	ObservableCollection<FlyoutMenuItem> MenuItems { get; } = new();

	public FlyoutMenuPage(Page topPage)
	{
		InitializeComponent();

		WidthRequest = 300;

		PageListView.ItemsSource = MenuItems;

		Task.Run(() =>
		{
			MenuItems.Add(new(topPage));
			MenuItems.Add(new(new FISJ.SubmitReport()));
			MenuItems.Add(new(new FISJ.ReservePages.ReserveSpotAndFuel()));
			MenuItems.Add(new(new FISJ.aero()));
			MenuItems.Add(new(new FISJ.WetherInformation()));
			MenuItems.Add(new(new FISJ.IcaoPage()));
			MenuItems.Add(new(new FISJ.PayLandingFee.CalcFee()));
			MenuItems.Add(new(new Views.SettingPage()));
		});
	}
}
