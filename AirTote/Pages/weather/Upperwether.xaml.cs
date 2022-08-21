using System;

namespace AirTote.Pages
{
	public partial class Upperwether : ContentPage
	{
		public Upperwether()
		{
			InitializeComponent();
		}

		private async void upperasia200_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupa20_00.pdf");
		}

		private async void upperasia200hpa12_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupa20_12.pdf");
		}

		private async void upperasia250hpa_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupa25_00.pdf");
		}

		private async void upperasia250hpa12_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupa25_12.pdf");
		}

		private async void upperasia300hpa00_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupn30_00.pdf");
		}

		private async void upperasia300hpa12_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupn30_12.pdf");
		}

		private async void upperasia500hpa00_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupq35_00.pdf");
		}

		private async void upperasia500hpa12_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupq35_12.pdf");
		}

		private async void upperasia850hpa00_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupq78_00.pdf");
		}

		private async void upperasia850hpa12_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupq78_12.pdf");
		}

		private async void upperNorthhemi00_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/auxn50_12.pdf");
		}

		private async void uppereast850hpa00_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/axfe578_00.pdf");
		}

		private async void uppereast850hpa12_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/axfe578_12.pdf");
		}

		private async void asiaGround850_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/feas50_12.pdf");
		}

		private async void uppercut00_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/axjp140_00.pdff");
		}

		private async void uppercut12_Clicked(object sender, EventArgs e)
		{
			await Launcher.OpenAsync("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/axjp140_12.pdf");
		}
	}
}