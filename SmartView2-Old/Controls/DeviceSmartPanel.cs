using System;
using System.Windows;
using System.Windows.Controls;

namespace SmartView2.Controls
{
	internal class DeviceSmartPanel : StackPanel
	{
		public DeviceSmartPanel()
		{
		}

		protected override Size ArrangeOverride(Size arrangeSize)
		{
			int viewportWidth = (int)base.ViewportWidth;
			int horizontalOffset = (int)base.HorizontalOffset;
			Size width = new Size();
			int num = 0;
			int num1 = horizontalOffset + viewportWidth - 1;
			while (num < base.Children.Count)
			{
				if (num < horizontalOffset || num > num1)
				{
					base.Children[num].Visibility = System.Windows.Visibility.Hidden;
				}
				else
				{
					base.Children[num].Visibility = System.Windows.Visibility.Visible;
					double width1 = width.Width;
					Size desiredSize = base.Children[num].DesiredSize;
					width.Width = width1 + desiredSize.Width;
					width.Height = base.Children[num].DesiredSize.Height;
				}
				num++;
			}
			return base.ArrangeOverride(width);
		}
	}
}