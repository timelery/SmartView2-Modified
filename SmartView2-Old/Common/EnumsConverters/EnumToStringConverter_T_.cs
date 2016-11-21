using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace SmartView2.Common.EnumsConverters
{
	public class EnumToStringConverter<T> : IValueConverter
	where T : struct, IConvertible
	{
		public EnumToStringConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is T))
			{
				throw new ArgumentException("value must be of T type");
			}
			T t = (T)value;
			return new KeyValuePair<string, T>(EnumConverters<T>.GetLocalizatedString(t), t);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}
			if (!(value is KeyValuePair<string, T>))
			{
				throw new ArgumentException("value must be KeyValuePair<string, T> type.");
			}
			return ((KeyValuePair<string, T>)value).Value;
		}
	}
}