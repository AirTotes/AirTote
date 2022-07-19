using FIS_J.FISJ;
using FIS_J.Interfaces;
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

		MenuPage.CloseSwipeGesture.Swiped += (_, _) =>
		{
			if (!IsGestureEnabled)
				IsPresented = false;
		};

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

		IsGestureEnabled = contentP is not IDisableFlyoutGesture;
		if (contentP is IContainFlyoutPageInstance i)
			i.FlyoutPage = this;

#if !IOS
		navP = new(contentP);
#endif

		await navP.PopToRootAsync();

		Detail = navP;
	}
}
