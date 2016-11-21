using System;
using System.Globalization;
using System.Windows.Data;

namespace SmartView2.Common
{
	public class BoolToBoolNegativeConverter : IValueConverter
	{
		public BoolToBoolNegativeConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool? nullable = (bool?)(value as bool?);
			if (!nullable.HasValue)
			{
				return null;
			}
			return !nullable.Value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool? nullable = (bool?)(value as bool?);
			if (!nullable.HasValue)
			{
				return null;
			}
			return !nullable.Value;
		}
	}
}