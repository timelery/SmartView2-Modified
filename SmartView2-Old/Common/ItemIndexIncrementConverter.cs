using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using UIFoundation.Common;

namespace SmartView2.Common
{
	public class ItemIndexIncrementConverter : IValueConverter
	{
		public ItemIndexIncrementConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is ListBoxItem))
			{
				throw new ArgumentException("value is not ListBoxItem.");
			}
			ListBoxItem listBoxItem = (ListBoxItem)value;
			ListBox listBox = listBoxItem.FindAncestor<ListBox>();
			if (listBox == null)
			{
				return 0;
			}
			return listBox.ItemContainerGenerator.IndexFromContainer(listBoxItem) + 1;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}