using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SmartView2.Controls
{
	public class GlobalMenu : Control
	{
		private static GlobalMenu instance;

		public readonly static DependencyProperty MenuItemsProperty;

		public GlobalMenuItemCollection MenuItems
		{
			get
			{
				return (GlobalMenuItemCollection)base.GetValue(GlobalMenu.MenuItemsProperty);
			}
			set
			{
				base.SetValue(GlobalMenu.MenuItemsProperty, value);
			}
		}

		static GlobalMenu()
		{
			GlobalMenu.MenuItemsProperty = DependencyProperty.Register("MenuItems", typeof(GlobalMenuItemCollection), typeof(GlobalMenu), new PropertyMetadata(null));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GlobalMenu), new FrameworkPropertyMetadata(typeof(GlobalMenu)));
		}

		public GlobalMenu()
		{
			GlobalMenu.instance = this;
		}

		public static void SetGlobalMenuItems(FrameworkElement d, GlobalMenuItemCollection val)
		{
			if (GlobalMenu.instance != null)
			{
				d.Loaded += new RoutedEventHandler((object s, RoutedEventArgs e) => GlobalMenu.instance.DataContext = d.DataContext);
				GlobalMenu.instance.MenuItems = val;
			}
		}
	}
}