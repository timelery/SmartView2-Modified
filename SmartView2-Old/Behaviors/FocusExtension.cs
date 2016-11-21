using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SmartView2.Behaviors
{
	public class FocusExtension
	{
		private readonly static DependencyProperty isFocusedProperty;

		static FocusExtension()
		{
			FocusExtension.isFocusedProperty = DependencyProperty.RegisterAttached("IsFocused", typeof(bool), typeof(FocusExtension), new PropertyMetadata(false, new PropertyChangedCallback(FocusExtension.OnIsFocusedChanged)));
		}

		public FocusExtension()
		{
		}

		public static bool GetIsFocused(DependencyObject obj)
		{
			return (bool)obj.GetValue(FocusExtension.isFocusedProperty);
		}

		private static void OnIsFocusedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			UIElement uIElement = (UIElement)obj;
			if ((bool)e.NewValue)
			{
				obj.Dispatcher.BeginInvoke(new Action(() => {
					uIElement.Focus();
					Keyboard.Focus(uIElement);
				}), new object[0]);
			}
		}

		public static void SetIsFocused(DependencyObject obj, bool value)
		{
			obj.SetValue(FocusExtension.isFocusedProperty, value);
		}
	}
}