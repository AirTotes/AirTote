using FIS_J.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIS_J.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FCSPage : ContentPage
	{
		FCSPageViewModel Model { get; } = new();

		public FCSPage()
		{
			InitializeComponent();
			BindingContext = Model;
		}
	}
}