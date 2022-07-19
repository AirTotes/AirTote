namespace FIS_J.ViewModels;

public class FlyoutMenuItem : BaseViewModel
{
	public FlyoutMenuItem(Page page)
	{
		if (page is null)
			throw new ArgumentNullException(nameof(page));

		if (page is NavigationPage navPage)
		{
			_RootPage = navPage.RootPage;
			_Page = navPage;
		}
		else
		{
			_RootPage = page;
			_Page = new(RootPage);
		}
		Title = _RootPage.Title;

		Page.Pushed += PageHost.OnNavigated;
		Page.Popped += PageHost.OnNavigated;
	}

	private Page _RootPage;
	public Page RootPage => _RootPage;
	private NavigationPage _Page;
	public NavigationPage Page => _Page;
}

