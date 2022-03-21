using FIS_J.FISJ;
using FIS_J.FISJ.PayLandingFee;
using FIS_J.Views;

using System;

using Xamarin.Forms;

namespace FIS_J
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute(nameof(MainPagexaml), typeof(MainPagexaml));
			Routing.RegisterRoute(nameof(FlightPlan), typeof(FlightPlan));
			Routing.RegisterRoute(nameof(SkyInformation), typeof(SkyInformation));
			Routing.RegisterRoute(nameof(WetherInformation), typeof(WetherInformation));
			Routing.RegisterRoute(nameof(IcaoPage), typeof(IcaoPage));
			Routing.RegisterRoute(nameof(FCSPage), typeof(FCSPage));
			Routing.RegisterRoute(nameof(SelectAirport), typeof(SelectAirport));
			Routing.RegisterRoute(nameof(SettingPage), typeof(SettingPage));
		}
	}
}
