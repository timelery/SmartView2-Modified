using System;
using System.Globalization;
using System.Windows.Data;

namespace SmartView2.Common
{
	public class TimeSpanToDoubleConverter : IValueConverter
	{
		public TimeSpanToDoubleConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is TimeSpan))
			{
				throw new InvalidOperationException("Invalid converter used.");
			}
			return ((TimeSpan)value).TotalSeconds;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object obj;
			try
			{
				TimeSpan timeSpan = new TimeSpan(0, 0, (int)double.Parse(value.ToString()));
				obj = timeSpan;
			}
			catch
			{
				throw new InvalidOperationException("Invalid converter used.");
			}
			return obj;
		}
	}
}