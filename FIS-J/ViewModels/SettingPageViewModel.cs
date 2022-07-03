using System;

namespace FIS_J.ViewModels
{
	public class SettingPageViewModel : BaseViewModel
	{
		public SettingPageViewModel()
		{
			Title = "Settings";
		}

		HtmlWebViewSource _page = null;
		public HtmlWebViewSource WebPage
		{
			get => _page;
			set => SetProperty(ref _page, value);
		}
	}
}
