using System;
using System.Collections.Generic;

using AirTote.Interfaces;
using AirTote.Models;

namespace AirTote.ViewModels.ReservePages
{
	public class ReserveSpotAndFuelViewModel : BaseViewModel, IContainsAirportInfo
	{
		public ReserveSpotAndFuelViewModel()
		{
			Title = "スポット予約 / 給油調整";
		}

		public static DateTime Today => DateTime.Today;

		DateTime _SpotReserveTimeBegin_Date = DateTime.Today;
		public DateTime SpotReserveTimeBegin_Date
		{
			get => _SpotReserveTimeBegin_Date;
			set => SetProperty(ref _SpotReserveTimeBegin_Date, value);
		}

		TimeSpan _SpotReserveTimeBegin_Time;
		public TimeSpan SpotReserveTimeBegin_Time
		{
			get => _SpotReserveTimeBegin_Time;
			set => SetProperty(ref _SpotReserveTimeBegin_Time, value);
		}

		int _SpotStayTime_HH;
		public int SpotStayTime_HH
		{
			get => _SpotStayTime_HH;
			set => SetProperty(ref _SpotStayTime_HH, Math.Max(value, 0));
		}

		int _SpotStayTime_MM;
		public int SpotStayTime_MM
		{
			get => _SpotStayTime_MM;
			set
			{
				if (0 <= value && value < 60)
					SetProperty(ref _SpotStayTime_MM, value);

				int value_total_mm = SpotStayTime_HH * 60 + value;
				if (value_total_mm < 0)
					value_total_mm = 0;

				SpotStayTime_HH = value_total_mm / 60;
				SetProperty(ref _SpotStayTime_MM, value_total_mm % 60);
			}
		}

		FuelTypes _FuelType;
		public FuelTypes FuelType
		{
			get => _FuelType;
			set => SetProperty(ref _FuelType, value);
		}

		DateTime _ChargeFuelBegin_Date = DateTime.Today;
		public DateTime ChargeFuelBegin_Date
		{
			get => _ChargeFuelBegin_Date;
			set => SetProperty(ref _ChargeFuelBegin_Date, value);
		}
		TimeSpan _ChargeFuelBegin_Time;
		public TimeSpan ChargeFuelBegin_Time
		{
			get => _ChargeFuelBegin_Time;
			set => SetProperty(ref _ChargeFuelBegin_Time, value);
		}

		int _FuelToCharge_gal;
		public int FuelToCharge_gal
		{
			get => _FuelToCharge_gal;
			set => SetProperty(ref _FuelToCharge_gal, value);
		}

		AirportInfo.APInfo? _AirportInfo = null;
		public AirportInfo.APInfo? AirportInfo
		{
			get => _AirportInfo;
			set
			{
				SetProperty(ref _AirportInfo, value);
				AirportName = $"{value?.icao ?? ("Unknown")} ({value?.name})";
			}
		}

		string _AirportName = "Unknown";
		public string AirportName
		{
			get => _AirportName;
			set => SetProperty(ref _AirportName, value);
		}

		public static List<string> FuelNames { get; } = new()
		{
			"なし",
			"JET A-1 (航空タービン燃料油 1号)",
			"JET A (航空タービン燃料油 2号)",
			"JET B (航空タービン燃料油 3号)",
			"AVGAS 100",
		};

		public enum FuelTypes
		{
			None,
			JET_A1,
			JET_A,
			JET_B,
			AVGAS_100,
		}
	}
}
