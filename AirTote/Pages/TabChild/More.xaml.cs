using AirTote.ViewModels;

namespace AirTote.Pages.TabChild;

public partial class More : ContentPage
{


	public More()
	{
		InitializeComponent();
		
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Pages.IcaoPage());
	}

	private void Button_Clicked_1(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Pages.ReservePages.ReserveSpotAndFuel());
	}

	private void Button_Clicked_2(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Pages.PayLandingFee.CalcFee());
	}

	private void Button_Clicked_3(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Pages.ThirdPartyLicenses());
	}

	private void Button_Clicked_4(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Pages.SettingPage());
	}
}
