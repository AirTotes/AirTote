using System;
using System.Runtime.CompilerServices;
using FIS_J.Models;
using FIS_J.Services;

namespace FIS_J.ViewModels.PayLandingFee
{
	public class CalcFeeViewModel : BaseViewModel
	{
		public CalcFeeViewModel()
		{
			Title = "料金計算";
		}

		bool _IsJet = false;
		public bool IsJet
		{
			get => _IsJet;
			set => SetProperty(ref _IsJet, value);
		}

		double _Weight = 0;
		public double Weight
		{
			get => _Weight;
			set
			{
				SetProperty(ref _Weight, value);

				LandingCharge = getLandingCharge();
				ParkingCharge = getParkingCharge();
			}
		}

		double _Noise = 0;
		public double Noise
		{
			get => _Noise;
			set => SetProperty(ref _Noise, value);
		}

		int _Passenger = 0;
		public int Passenger
		{
			get => _Passenger;
			set => SetProperty(ref _Passenger, value);
		}

		double _FreightWeight = 0;
		public double FreightWeight
		{
			get => _FreightWeight;
			set => SetProperty(ref _FreightWeight, value);
		}

		bool _IsInternational = false;
		public bool IsInternational
		{
			get => _IsInternational;
			set => SetProperty(ref _IsInternational, value);
		}

		bool _IsCargoAircraft = false;
		public bool IsCargoAircraft
		{
			get => _IsCargoAircraft;
			set => SetProperty(ref _IsCargoAircraft, value);
		}

		double _StationaryTime = 0;
		public double StationaryTime
		{
			get => _StationaryTime;
			set
			{
				SetProperty(ref _StationaryTime, value);

				ParkingCharge = getParkingCharge();
			}
		}

		AirportInfo.APInfo _Station = null;
		public AirportInfo.APInfo Station
		{
			get => _Station;
			set => SetProperty(ref _Station, value);
		}

		int _LandingCharge;
		public int LandingCharge
		{
			get => _LandingCharge;
			set => SetCharge(ref _LandingCharge, value);
		}

		int _ParkingCharge;
		public int ParkingCharge
		{
			get => _ParkingCharge;
			set => SetCharge(ref _ParkingCharge, value);
		}

		int _SecurityCharge;
		public int SecurityCharge
		{
			get => _SecurityCharge;
			set => SetCharge(ref _SecurityCharge, value);
		}

		public int ConsumptionTax => (int)((LandingCharge + ParkingCharge + SecurityCharge) * 0.1);
		public int TotalCharge => LandingCharge + ParkingCharge + SecurityCharge + ConsumptionTax;

		void SetCharge(ref int local, int value,
			[CallerMemberName] string propertyName = "")
		{
			SetProperty(ref local, value, propertyName);

			OnPropertyChanged(nameof(ConsumptionTax));
			OnPropertyChanged(nameof(TotalCharge));
		}

		int getLandingCharge()
		{
			int weight = Math.Max((int)Weight, 0);

			if (weight <= 6)
				return 1000;
			else
				return 700 + (590 * (weight - 6));
		}

		int getParkingCharge()
		{
			int weight = Math.Max((int)Weight, 0);

			if (StationaryTime < 3)
				return 0;

			int fee = 0;
			int time = (int)Math.Ceiling(StationaryTime / 24);

			if (weight <= 23)
			{
				fee = 810;

				if (weight > 6)
					fee += (weight - 6) * 30;
				if (weight > 3)
					fee += 810;
			}
			else
			{
				if (weight > 100)
					fee += 70 * (weight - 100);
				if (weight > 25)
					fee += 80 * Math.Min(weight - 25, 75);

				fee += 90 * Math.Min(weight, 25);
			}

			return fee * time;
		}
	}
}

