using System;

namespace AirTote.Pages
{
	public partial class kounan : ContentPage
	{
		public kounan()
		{
			InitializeComponent();
		}

		private async void kounanAirportUse_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://view.officeapps.live.com/op/view.aspx?src=https%3A%2F%2Fwww.pref.okayama.jp%2Fuploaded%2Flife%2F736586_6727276_misc.doc");
		}

		private async void kounandepart_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://view.officeapps.live.com/op/view.aspx?src=https%3A%2F%2Fwww.pref.okayama.jp%2Fuploaded%2Flife%2F736586_6727265_misc.docx");
		}

		private async void landingdiscount_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://view.officeapps.live.com/op/view.aspx?src=https%3A%2F%2Fwww.pref.okayama.jp%2Fuploaded%2Flife%2F736586_6727267_misc.docx");
		}

		private async void AirportCheck_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://view.officeapps.live.com/op/view.aspx?src=https%3A%2F%2Fwww.pref.okayama.jp%2Fuploaded%2Flife%2F736586_6727269_misc.docx");
		}
	}
}
