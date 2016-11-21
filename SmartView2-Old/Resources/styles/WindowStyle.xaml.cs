using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace SmartView2.Resources.Styles
{
	public partial class WindowStyle : ResourceDictionary
	{
		public WindowStyle()
		{
		}

		private void CloseButtonClick(object sender, RoutedEventArgs e)
		{
			sender.ForWindowFromTemplate((Window w) => SystemCommands.CloseWindow(w));
		}

		private void IconMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount > 1)
			{
				sender.ForWindowFromTemplate((Window w) => SystemCommands.CloseWindow(w));
			}
		}

		private void IconMouseUp(object sender, MouseButtonEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			Point screen = frameworkElement.PointToScreen(new Point(frameworkElement.ActualWidth / 2, frameworkElement.ActualHeight));
			sender.ForWindowFromTemplate((Window w) => SystemCommands.ShowSystemMenu(w, screen));
		}

		private void MaxButtonClick(object sender, RoutedEventArgs e)
		{
			sender.ForWindowFromTemplate((Window w) => {
				if (w.WindowState == WindowState.Maximized)
				{
					SystemCommands.RestoreWindow(w);
					return;
				}
				SystemCommands.MaximizeWindow(w);
			});
		}

		private void MinButtonClick(object sender, RoutedEventArgs e)
		{
			sender.ForWindowFromTemplate((Window w) => SystemCommands.MinimizeWindow(w));
		}
	}
}