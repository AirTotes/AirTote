using System;
using System.Globalization;
using Xamarin.Forms;

namespace FIS_J.ValueConverters
{
	public class BooleanInverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> !(bool)value;

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> !(bool)value;
	}

	public class StringToDoubleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> ((double)value).ToString();

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string s && double.TryParse(s, out var dValue))
				return dValue;
			else
				return 0;
		}
	}

	public class StringToIntegerConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> ((int)value).ToString();
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string s && int.TryParse(s, out var dValue))
				return dValue;
			else
				return 0;
		}

	}
}
