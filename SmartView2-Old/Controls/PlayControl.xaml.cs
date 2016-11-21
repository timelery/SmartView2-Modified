using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SmartView2.Controls
{
	public partial class PlayControl : UserControl
	{
		private readonly static DependencyProperty recordingAvailableProperty;

		public bool RecordingAvailable
		{
			get
			{
				return (bool)base.GetValue(PlayControl.recordingAvailableProperty);
			}
			set
			{
				base.SetValue(PlayControl.recordingAvailableProperty, value);
			}
		}

		static PlayControl()
		{
			PlayControl.recordingAvailableProperty = DependencyProperty.Register("RecordingAvailable", typeof(bool), typeof(PlayControl), new PropertyMetadata(true));
		}

		public PlayControl()
		{
			this.InitializeComponent();
		}
	}
}