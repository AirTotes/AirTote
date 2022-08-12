﻿using System;

namespace AirTote.Pages
{
	public partial class AirportUse : ContentPage
	{
		public AirportUse()
		{
			InitializeComponent();
		}

		private void Kounan_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new kounan());
		}

		private void Haneda_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new haneda());
		}

		private void Takamatu_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new takamatu());
		}

		private void Hirosima_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new hirosima());
		}
	}
}