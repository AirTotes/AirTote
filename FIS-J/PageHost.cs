using FIS_J.FISJ;
using FIS_J.ViewModels;

namespace FIS_J;

public class PageHost : FlyoutPage
{
	TopPage topPage { get; }
	NavigationPage topNavPage { get; }
	FlyoutMenuPage MenuPage { get; }

	public PageHost()
	{
		topPage = new();
		topNavPage = new(topPage);
		MenuPage = new(topNavPage);

		Flyout = MenuPage;

		ChangeRootPage(topNavPage, topPage);
		MenuPage.PageListView.SelectionChanged += PageListView_SelectionChanged;

		// ref: https://github.com/dotnet/maui/issues/7520
		FlyoutLayoutBehavior = FlyoutLayoutBehavior.Popover;
	}

	private void PageListView_SelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.Count <= 0 || e.CurrentSelection[0] is not FlyoutMenuItem menuItem)
			return;

		ChangeRootPage(menuItem);
		MenuPage.PageListView.SelectedItem = null;
	}

	void ChangeRootPage(FlyoutMenuItem item)
		=> ChangeRootPage(item.Page, item.RootPage);

	async void ChangeRootPage(NavigationPage navP, Page contentP)
	{
		IsPresented = false;

#if !IOS
		navP = new(contentP);
#endif

		await navP.PopToRootAsync();

		Detail = navP;
	}
}
