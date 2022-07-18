using FIS_J.FISJ;
using FIS_J.ViewModels;

namespace FIS_J;

public class PageHost : FlyoutPage
{
	TopPage topPage { get; }
	FlyoutMenuPage MenuPage { get; }

	public PageHost()
	{
		topPage = new();
		MenuPage = new(topPage);

		Flyout = MenuPage;

		ChangeRootPage(topPage);
		MenuPage.PageListView.SelectionChanged += PageListView_SelectionChanged;

		// ref: https://github.com/dotnet/maui/issues/7520
		FlyoutLayoutBehavior = FlyoutLayoutBehavior.Popover;
	}

	private void PageListView_SelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is not FlyoutMenuItem menuItem)
			return;

		ChangeRootPage(menuItem.Page);
	}

	void ChangeRootPage(Page page)
	{
		Detail = new NavigationPage(page);
		IsPresented = false;
	}
}
