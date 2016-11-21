using System;
using System.Globalization;
using System.Windows.Data;

namespace SmartView2.Common
{
	public class TimeSpanToStringConverter : IValueConverter
	{
		public TimeSpanToStringConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is TimeSpan))
			{
				throw new InvalidOperationException("Wrong type was used.");
			}
			TimeSpan timeSpan = (TimeSpan)value;
			if (timeSpan.Hours > 0)
			{
				return timeSpan.ToString("hh\\:mm\\:ss");
			}
			return timeSpan.ToString("mm\\:ss");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}