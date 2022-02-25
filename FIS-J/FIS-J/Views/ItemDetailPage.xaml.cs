using FIS_J.ViewModels;

using System.ComponentModel;

using Xamarin.Forms;

namespace FIS_J.Views
{
	public partial class ItemDetailPage : ContentPage
	{
		public ItemDetailPage()
		{
			InitializeComponent();
			BindingContext = new ItemDetailViewModel();
		}
	}
}