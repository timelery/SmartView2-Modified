using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SmartView2.Controls
{
	public partial class NumPadControl : UserControl
	{
		private readonly static DependencyProperty channelAvailableProperty;

		public bool ChannelAvailable
		{
			get
			{
				return (bool)base.GetValue(NumPadControl.channelAvailableProperty);
			}
			set
			{
				base.SetValue(NumPadControl.channelAvailableProperty, value);
			}
		}

		static NumPadControl()
		{
			NumPadControl.channelAvailableProperty = DependencyProperty.Register("ChannelAvailable", typeof(bool), typeof(NumPadControl), new PropertyMetadata(true));
		}

		public NumPadControl()
		{
			this.InitializeComponent();
		}
	}
}