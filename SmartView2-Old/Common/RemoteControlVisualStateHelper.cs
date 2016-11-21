using SmartView2.Core;
using System;
using System.Windows;

namespace SmartView2.Common
{
	public class RemoteControlVisualStateHelper : DependencyObject
	{
		public readonly static DependencyProperty StateProperty;

		static RemoteControlVisualStateHelper()
		{
			RemoteControlVisualStateHelper.StateProperty = DependencyProperty.RegisterAttached("State", typeof(RemoteControlType), typeof(RemoteControlVisualStateHelper), new UIPropertyMetadata(RemoteControlType.Tv, new PropertyChangedCallback(RemoteControlVisualStateHelper.StateChanged)));
		}

		public RemoteControlVisualStateHelper()
		{
		}

		public static RemoteControlType GetState(DependencyObject d)
		{
			return (RemoteControlType)d.GetValue(RemoteControlVisualStateHelper.StateProperty);
		}

		public static void SetState(DependencyObject d, RemoteControlType value)
		{
			d.SetValue(RemoteControlVisualStateHelper.StateProperty, value);
		}

		private static void StateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != null)
			{
				VisualStateManager.GoToState((FrameworkElement)d, e.NewValue.ToString(), true);
			}
		}
	}
}