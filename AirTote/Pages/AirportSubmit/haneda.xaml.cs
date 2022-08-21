using System;

namespace AirTote.Pages
{
	public partial class haneda : ContentPage
	{
		public haneda()
		{
			InitializeComponent();
		}

		private async void hanedaAirportuse_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.kouwan.metro.tokyo.lg.jp/business/R3011.pdf");
		}

		private async void hanedaAirportusepermittion_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.kouwan.metro.tokyo.lg.jp/business/R3021.pdf");
		}

		private async void hanedaWeightOverpermittion_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.kouwan.metro.tokyo.lg.jp/business/R3031.pdf");
		}

		private async void hanedaUseInstitutionpermmition_1_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.kouwan.metro.tokyo.lg.jp/business/R3041.pdf");
		}
		private async void hanedaUseInstitutionpermmition_2_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.kouwan.metro.tokyo.lg.jp/business/R3051.pdf");
		}
		private async void hanedaUseInstitutionpermmition_3_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.kouwan.metro.tokyo.lg.jp/business/R3061.pdf");
		}
		private async void hanedaDiscount_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.kouwan.metro.tokyo.lg.jp/business/R3071.pdf");
		}
		private async void hanedarestrictmap_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.kouwan.metro.tokyo.lg.jp/business/download/d/pdf/hikouseigenn.pdf");
		}
	}
}
