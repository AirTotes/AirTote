using AirTote.Models;

namespace AirTote.ViewModels
{
	internal class MetarPageViewModel : BaseViewModel
	{
		public MetarPageViewModel()
		{
			Title = "Metar Page";
		}

		private string _Metar = "Initial Value";
		private string _taf = "Initial Value";
		public string Metar
		{
			get => _Metar;
			set => SetProperty(ref _Metar, value);
		}
		public string taf
		{
			get => _taf;
			set => SetProperty(ref _taf, value);
		}

		private ICAOCode _CurrentICAOCode = ICAOCode.RJAA;
		public ICAOCode CurrentICAOCode
		{
			get => _CurrentICAOCode;
			set => SetProperty(ref _CurrentICAOCode, value);
		}
	}
}
