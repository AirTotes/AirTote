using FIS_J.Models;
using FIS_J.Services;
using FIS_J.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

			Task.Run(async () =>
			{
				string _metar = await api.GetSanitizedMETAR(ICAOCode.RJAA);
				System.Diagnostics.Debug.WriteLine($"METAR: {_metar}");
				ViewModel.Metar = _metar;
			});
			Task.Run(async () =>
			{
				string _taf = await api.GetSanitizedTAF(ICAOCode.RJAA);
				System.Diagnostics.Debug.WriteLine($"TAF: {_taf}");
				ViewModel.taf = _taf;
			});
		}
	}
}