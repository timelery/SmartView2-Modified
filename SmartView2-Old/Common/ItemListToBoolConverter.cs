using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace SmartView2.Common
{
	public class ItemListToBoolConverter : IValueConverter
	{
		public ItemListToBoolConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return false;
			}
			if (!(value is IEnumerable<object>))
			{
				throw new InvalidOperationException("Wrong type was used.");
			}
			if ((value as IEnumerable<object>).Count<object>() != 0)
			{
				return true;
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}