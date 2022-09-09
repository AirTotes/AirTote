namespace AirTote.Pages;

public class WebViewPage : ContentPage
{
	WebView _WebView { get; } = new();

	public WebViewPage(HtmlWebViewSource src, string title)
	{
		this._WebView.Source = src;
		this.Content = this._WebView;
		this.Title = title;
	}
}
