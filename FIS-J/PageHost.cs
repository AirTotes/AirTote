using FIS_J.FISJ;
using FIS_J.Interfaces;
using FIS_J.ViewModels;

namespace FIS_J;

public class PageHost : FlyoutPage
{
	TopPage topPage { get; }
	NavigationPage topNavPage { get; }
	FlyoutMenuPage MenuPage { get; }
	static FlyoutPage? _this { get; set; }

	static Dictionary<Type, bool> IsGestureEnabledDic { get; } = new();
	public static void SetIsGestureEnabled(Type target, bool value)
	 => IsGestureEnabledDic[target] = value;

	static public void OnNavigated(object? sender, NavigationEventArgs e)
	{
		if (sender is not NavigationPage navP)
			return;

		OnPageChanged(navP.CurrentPage);
	}

	static public void OnPageChanged(Page newPage)
	{
		if (_this is null)
			return;

		if (IsGestureEnabledDic.TryGetValue(newPage.GetType(), out bool value))
			_this.IsGestureEnabled = value;
		else
			_this.IsGestureEnabled = true;
	}

	public PageHost()
	{
		_this = this;
		topPage = new();
		topNavPage = new(topPage);
		MenuPage = new(topNavPage);

		MenuPage.CloseSwipeGesture.Swiped += (_, _) =>
		{
			if (!IsGestureEnabled)
				IsPresented = false;
		};

		topNavPage.Popped += OnNavigated;
		topNavPage.Pushed += OnNavigated;

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

		if (contentP is IContainFlyoutPageInstance i)
			i.FlyoutPage = this;

#if !IOS
		navP = new(contentP);
#endif

		await navP.PopToRootAsync();

		OnPageChanged(navP.CurrentPage);

		Detail = navP;
	}

}
