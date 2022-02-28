using FIS_J.ViewModels;

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