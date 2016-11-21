using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace SmartView2.Common
{
	public class RectConverter : IMultiValueConverter
	{
		public Thickness Offset
		{
			get;
			set;
		}

		public RectConverter()
		{
			this.Offset = new Thickness();
		}

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if ((int)values.Length >= 2)
			{
				double? nullable = (double?)(values[0] as double?);
				double num = (nullable.HasValue ? (double)((double)nullable.GetValueOrDefault()) : double.NaN);
				double? nullable1 = (double?)(values[1] as double?);
				double num1 = (nullable1.HasValue ? (double)((double)nullable1.GetValueOrDefault()) : double.NaN);
				if (!double.IsNaN(num) && !double.IsNaN(num1))
				{
					Thickness offset = this.Offset;
					if (num + this.Offset.Bottom > 0 && num1 + this.Offset.Right > 0)
					{
						double left = 0 + this.Offset.Left;
						double top = 0 + this.Offset.Top;
						double right = num + this.Offset.Right;
						Thickness thickness = this.Offset;
						return new Rect(left, top, right, num1 + thickness.Bottom);
					}
				}
			}
			return null;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}