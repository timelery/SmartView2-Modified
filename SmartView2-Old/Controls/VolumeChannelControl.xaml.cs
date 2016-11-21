using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SmartView2.Controls
{
	public partial class VolumeChannelControl : UserControl
	{
		private readonly static DependencyProperty channelAvailableProperty;

		private readonly static DependencyProperty volumeAvailableProperty;

		public bool ChannelAvailable
		{
			get
			{
				return (bool)base.GetValue(VolumeChannelControl.channelAvailableProperty);
			}
			set
			{
				base.SetValue(VolumeChannelControl.channelAvailableProperty, value);
			}
		}

		public bool VolumeAvailable
		{
			get
			{
				return (bool)base.GetValue(VolumeChannelControl.volumeAvailableProperty);
			}
			set
			{
				base.SetValue(VolumeChannelControl.volumeAvailableProperty, value);
			}
		}

		static VolumeChannelControl()
		{
			VolumeChannelControl.channelAvailableProperty = DependencyProperty.Register("ChannelAvailable", typeof(bool), typeof(VolumeChannelControl), new PropertyMetadata(true));
			VolumeChannelControl.volumeAvailableProperty = DependencyProperty.Register("VolumeAvailable", typeof(bool), typeof(VolumeChannelControl), new PropertyMetadata(true));
		}

		public VolumeChannelControl()
		{
			this.InitializeComponent();
		}
	}
}