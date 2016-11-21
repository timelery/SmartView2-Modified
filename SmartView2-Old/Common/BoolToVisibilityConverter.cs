using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SmartView2.Common
{
	public class BoolToVisibilityConverter : IValueConverter
	{
		public BoolToVisibilityConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool))
			{
				throw new InvalidOperationException("Invalid converter called.");
			}
			if ((bool)value)
			{
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}