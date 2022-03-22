﻿using FIS_J.Models;
using FIS_J.Services;
using FIS_J.ViewModels;

using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIS_J.FISJ
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Metar : ContentPage
	{
		AVWX api { get; } = new AVWX("KQuqTZ1D1BfSsuXu4eN2lc3DnC46-tGsU-l023G6q0w");
		MetarPageViewModel ViewModel { get; } = new MetarPageViewModel();

		public Metar()
		{
			InitializeComponent();
			BindingContext = ViewModel;

			Task.Run(async () => await SetMetarAndTaf(ViewModel.CurrentICAOCode));
		}

		async Task SetMetarAndTaf(ICAOCode code)
		{
			ViewModel.Metar = await api.GetSanitizedMETAR(code);
			ViewModel.taf = await api.GetSanitizedTAF(code);
		}

		async void Picker_SelectedIndexChanged(object sender, EventArgs e)
		{
			await SetMetarAndTaf(ViewModel.CurrentICAOCode);
		}
	}
}