namespace FIS_J.ViewModels;

public class FlyoutMenuItem : BaseViewModel
{
	public FlyoutMenuItem(Page page)
	{
		if (page is null)
			throw new ArgumentNullException(nameof(page));

		_Page = page;
		Title = page.Title;
	}

	private Page _Page;
	public Page Page => _Page;
}

