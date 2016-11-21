using SmartView2.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace SmartView2.Common
{
	public class CharacterSizeToListConverter : IValueConverter
	{
		public CharacterSizeToListConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is IEnumerable<float>))
			{
				throw new ArgumentException("value must be IEnumerable<float>.");
			}
			List<float> list = ((IEnumerable<float>)value).ToList<float>();
			List<KeyValuePair<string, float>> keyValuePairs = new List<KeyValuePair<string, float>>();
			foreach (float single in list)
			{
				if (single != 0f)
				{
					keyValuePairs.Add(new KeyValuePair<string, float>(string.Format("{0} %", (int)(single * 100f)), single));
				}
				else
				{
					keyValuePairs.Add(new KeyValuePair<string, float>(ResourcesModel.Instanse.COM_TV_SID_DEFAULT, single));
				}
			}
			return keyValuePairs;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is IEnumerable<KeyValuePair<string, float>>))
			{
				throw new ArgumentException("value must be IEnumerable<KeyValuePair<string, float>>");
			}
			List<KeyValuePair<string, float>> list = ((IEnumerable<KeyValuePair<string, float>>)value).ToList<KeyValuePair<string, float>>();
			List<float> singles = new List<float>();
			list.ForEach((KeyValuePair<string, float> item) => singles.Add(item.Value));
			return singles;
		}
	}
}