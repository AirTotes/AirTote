using System.Collections.ObjectModel;
using FIS_J.ViewModels;
namespace FIS_J.Pages;

public partial class FlyoutMenuPage : ContentPage
{
	ObservableCollection<FlyoutMenuItem> _MenuItems { get; } = new();
	public IReadOnlyCollection<FlyoutMenuItem> MenuItems => _MenuItems;

	public SwipeGestureRecognizer CloseSwipeGesture { get; } = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };

	public FlyoutMenuPage(NavigationPage topNavPage)
	{
		InitializeComponent();

		WidthRequest = 300;

		PageListView.ItemsSource = _MenuItems;

		Task.Run(() =>
		{
			_MenuItems.Add(new(topNavPage));
			_MenuItems.Add(new(new SubmitReport()));
			_MenuItems.Add(new(new ReservePages.ReserveSpotAndFuel()));
			_MenuItems.Add(new(new aero()));
			_MenuItems.Add(new(new WetherInformation()));
			_MenuItems.Add(new(new IcaoPage()));
			_MenuItems.Add(new(new PayLandingFee.CalcFee()));
			_MenuItems.Add(new(new Views.SettingPage()));
		});

		Content.GestureRecognizers.Add(CloseSwipeGesture);
	}
}
