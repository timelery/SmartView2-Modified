using SmartView2.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace SmartView2.Common
{
	public class CharacterSizeToStringConverter : IValueConverter
	{
		public CharacterSizeToStringConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is float))
			{
				return null;
			}
			float single = float.Parse(value.ToString());
			if (single == 0f)
			{
				return new KeyValuePair<string, float>(ResourcesModel.Instanse.COM_TV_SID_DEFAULT, single);
			}
			return new KeyValuePair<string, float>(string.Format("{0} %", (int)(single * 100f)), single);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is KeyValuePair<string, float>))
			{
				return null;
			}
			return ((KeyValuePair<string, float>)value).Value;
		}
	}
}