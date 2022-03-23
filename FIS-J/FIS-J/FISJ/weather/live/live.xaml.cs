using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIS_J.FISJ
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class live : ContentPage
	{
		public live()
		{
			InitializeComponent();
		}

		private void Liveeast850_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new liveeast850());
		}

		private void Livenorthhemi_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new livenorthhemi());
		}

		private void Liveasiaground_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new liveasiaground());
		}

		private void Liveasia850_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new liveasia850());
		}

		private void Liveuppercut_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new liveuppercut());
		}

		private void Livenews_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new livenews());
		}

		private void Liveasia500_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new liveasia500());
		}

		private void Liveasia300_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new liveasia300());
		}

		private void Liveasia200_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new liveasia200());
		}
	}
}