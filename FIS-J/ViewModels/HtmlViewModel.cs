using System;
using System.Collections.Generic;
using System.Text;

namespace FIS_J.ViewModels
{
    internal class HtmlViewModel:BaseViewModel
    {
		private HtmlWebViewSource _Html;
		public HtmlWebViewSource HTML
		{
			get => _Html;
			set => SetProperty(ref _Html, value);
		}
	}

}
