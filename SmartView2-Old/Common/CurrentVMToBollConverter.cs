using SmartView2.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SmartView2.Common
{
	public class CurrentVMToBollConverter : IValueConverter
	{
		private Type[] vms = new Type[] { typeof(TvVideoViewModel), typeof(MultimediaViewModel), typeof(RemoteControlViewModel) };

		public CurrentVMToBollConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int num = int.Parse(parameter as string);
			return Array.IndexOf<Type>(this.vms, value.GetType()) == num;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}