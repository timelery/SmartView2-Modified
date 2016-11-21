using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using UIFoundation.Navigation;

namespace SmartView2.Controls
{
	public partial class MediaElementExtFullscreenWindow : Window
	{
		public readonly static DependencyProperty NotificationSourceProperty;

		public readonly static DependencyProperty PopupSourceProperty;

		public IEnumerable<TostMessage> NotificationSource
		{
			get
			{
				return (IEnumerable<TostMessage>)base.GetValue(MediaElementExtFullscreenWindow.NotificationSourceProperty);
			}
			set
			{
				base.SetValue(MediaElementExtFullscreenWindow.NotificationSourceProperty, value);
			}
		}

		public IEnumerable<PopupViewModel> PopupSource
		{
			get
			{
				return (IEnumerable<PopupViewModel>)base.GetValue(MediaElementExtFullscreenWindow.PopupSourceProperty);
			}
			set
			{
				base.SetValue(MediaElementExtFullscreenWindow.PopupSourceProperty, value);
			}
		}

		static MediaElementExtFullscreenWindow()
		{
			MediaElementExtFullscreenWindow.NotificationSourceProperty = DependencyProperty.Register("NotificationSource", typeof(IEnumerable<TostMessage>), typeof(MediaElementExtFullscreenWindow), new PropertyMetadata(null));
			MediaElementExtFullscreenWindow.PopupSourceProperty = DependencyProperty.Register("PopupSource", typeof(IEnumerable<PopupViewModel>), typeof(MediaElementExtFullscreenWindow), new PropertyMetadata(null));
		}

		public MediaElementExtFullscreenWindow()
		{
			this.InitializeComponent();
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				base.Close();
			}
		}
	}
}