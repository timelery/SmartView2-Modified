using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SmartView2.Converters
{
	public class ImageWithDefaultSourceConverter : IValueConverter
	{
		public string DefaultSource
		{
			get;
			set;
		}

		public ImageWithDefaultSourceConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Uri uri = value as Uri;
			if (uri == null)
			{
				if (string.IsNullOrEmpty(this.DefaultSource))
				{
					return null;
				}
				return new BitmapImage(new Uri(this.DefaultSource));
			}
			if (!File.Exists(uri.LocalPath))
			{
				return new BitmapImage(new Uri(this.DefaultSource));
			}
			return new BitmapImage(uri);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}