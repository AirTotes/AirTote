using FIS_J.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIS_J.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();
			this.BindingContext = new LoginViewModel();
		}
	}
}