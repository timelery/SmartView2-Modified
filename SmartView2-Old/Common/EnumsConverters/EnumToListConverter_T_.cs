using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace SmartView2.Common.EnumsConverters
{
	public class EnumToListConverter<T> : IValueConverter
	where T : struct, IConvertible
	{
		public EnumToListConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is IEnumerable<T>))
			{
				throw new ArgumentException("value must be IEnumerable<T>.");
			}
			List<T> list = ((IEnumerable<T>)value).ToList<T>();
			List<KeyValuePair<string, T>> keyValuePairs = new List<KeyValuePair<string, T>>();
			list.ForEach((T item) => keyValuePairs.Add(new KeyValuePair<string, T>(EnumConverters<T>.GetLocalizatedString(item), item)));
			return keyValuePairs;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is IEnumerable<KeyValuePair<string, T>>))
			{
				throw new ArgumentException("value must be IEnumerable<KeyValuePair<string, T>>");
			}
			List<KeyValuePair<string, T>> list = ((IEnumerable<KeyValuePair<string, T>>)value).ToList<KeyValuePair<string, T>>();
			List<T> ts = new List<T>();
			list.ForEach((KeyValuePair<string, T> item) => ts.Add(item.Value));
			return ts;
		}
	}
}