using System;

namespace AirTote.Pages
{
	public partial class longterm : ContentPage
	{
		public longterm()
		{
			InitializeComponent();
		}

		private void Torrid_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new torrid());
		}

		private void Monthevery_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new monthevery());
		}

		private void Monthspread_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new monthspread());
		}

		private void Monthensemble_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new monthensemble());
		}

		private void Monthcirculate_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new monthcirculate());
		}
	}
}