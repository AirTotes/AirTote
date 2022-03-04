using FIS_J.Models;
using FIS_J.ViewModels;

using Xamarin.Forms;

namespace FIS_J.Views
{
	public partial class NewItemPage : ContentPage
	{
		public Item Item { get; set; }

		public NewItemPage()
		{
			InitializeComponent();
			BindingContext = new NewItemViewModel();
		}
	}
}