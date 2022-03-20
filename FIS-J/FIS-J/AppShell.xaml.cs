﻿using FIS_J.FISJ;
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
			Routing.RegisterRoute(nameof(SubmitFormxaml), typeof(SubmitFormxaml));
			Routing.RegisterRoute(nameof(SkyInformation), typeof(SkyInformation));
			Routing.RegisterRoute(nameof(WetherInformation), typeof(WetherInformation));
			Routing.RegisterRoute(nameof(IcaoPage), typeof(IcaoPage));
			Routing.RegisterRoute(nameof(FCSPage), typeof(FCSPage));
			Routing.RegisterRoute(nameof(Metar), typeof(Metar));
			Routing.RegisterRoute(nameof(aero), typeof(aero));
		}
	}
}
