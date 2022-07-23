using System;

namespace FIS_J.FISJ
{
	public partial class emagram : ContentPage
	{
		public emagram()
		{
			InitializeComponent();
		}

		private void Nansei_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new nansei());
		}

		private void Kyuusyuminami_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new kyuusyuminami());
		}

		private void Kyuusyukita_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new kyuusyukita());
		}

		private void Sikoku_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new sikoku());
		}

		private void Tyuugoku_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new tyuugoku());
		}

		private void Kinki_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new kinki());
		}

		private void Toukai_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new toukai());
		}

		private void Hokuriku_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new hokuriku());
		}

		private void Kanto_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new kanto());
		}

		private void Touhoku_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new touhoku());
		}

		private void Hokkaido_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new hokkaido());
		}

		private void Zenkoku_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new zenkoku());
		}
	}
}