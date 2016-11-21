using MediaLibrary.DataModels;
using SmartView2.ViewModels.MultimediaInner;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SmartView2.Converters
{
	internal class ReloadPreviewIfNotExistConverter : IValueConverter
	{
		public string DefaultSource
		{
			get;
			set;
		}

		public ReloadPreviewIfNotExistConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object bitmapImage;
			MultimediaFile multimediaFile = value as MultimediaFile;
			if (multimediaFile == null)
			{
				if (string.IsNullOrEmpty(this.DefaultSource))
				{
					return null;
				}
				return new BitmapImage(new Uri(this.DefaultSource));
			}
			if (!File.Exists(multimediaFile.Preview.LocalPath))
			{
				try
				{
					(parameter as VideoImageViewModel).DeleteItemCommand.Execute(multimediaFile);
					return new BitmapImage(multimediaFile.Preview);
				}
				catch
				{
					bitmapImage = new BitmapImage(new Uri(this.DefaultSource));
				}
				return bitmapImage;
			}
			return new BitmapImage(multimediaFile.Preview);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}