using FIS_J.ViewModels.PayLandingFee;
using Xamarin.Forms;

namespace FIS_J.FISJ.PayLandingFee
{
	public partial class CalcFee : ContentPage
	{
		CalcFeeViewModel viewModel { get; } = new();

		public CalcFee()
		{
			InitializeComponent();
			BindingContext = viewModel;
		}
	}
}
