using System;

namespace FIS_J.FISJ
{
	public partial class Nomwether : ContentPage
	{
		public Nomwether()
		{
			InitializeComponent();
		}

		private async void asia250hpa_Clicked_1(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa252_00.pdf");
		}

		private async void asia300hpa_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa302_00.pdf");
		}

		private async void asia400hpa_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa402_00.pdf");
		}

		private async void asia500hpa_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa502_00.pdf");
		}

		private async void east850hpatem_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxfe5782_00.pdf");
		}

		private async void Groundpressure_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxfe502_00.pdf");
		}

		private async void asia250hpa12_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa252_12.pdf");
		}

		private async void asia300hpa12_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa302_12.pdf");
		}

		private async void asia400hpa12_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa402_12.pdf");
		}

		private async void asia500hpa12_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa502_12.pdf");
		}

		private async void east850hpatem12_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxfe5782_12.pdf");
		}

		private async void Groundpressure12_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxfe502_12.pdf");
		}

		private async void asiaGround_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/feas502_12.pdf");
		}

		private async void japan850hpa_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxjp854_00.pdf");
		}

		private async void japan850hpa12_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxjp854_12.pdf");
		}
	}
}