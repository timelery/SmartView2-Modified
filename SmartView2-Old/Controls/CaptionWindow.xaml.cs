using SmartView2.Models.Settings;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SmartView2.Controls
{
	public partial class CaptionWindow : UserControl
	{
		public readonly static DependencyProperty TextProperty;

		public readonly static DependencyProperty SettingsModelProperty;

		public SmartView2.Models.Settings.SettingsModel SettingsModel
		{
			get
			{
				return (SmartView2.Models.Settings.SettingsModel)base.GetValue(CaptionWindow.SettingsModelProperty);
			}
			set
			{
				base.SetValue(CaptionWindow.SettingsModelProperty, value);
			}
		}

		public string Text
		{
			get
			{
				return (string)base.GetValue(CaptionWindow.TextProperty);
			}
			set
			{
				base.SetValue(CaptionWindow.TextProperty, value);
			}
		}

		static CaptionWindow()
		{
			CaptionWindow.TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(CaptionWindow), new PropertyMetadata("", new PropertyChangedCallback(CaptionWindow.OnText)));
			CaptionWindow.SettingsModelProperty = DependencyProperty.Register("SettingsModel", typeof(SmartView2.Models.Settings.SettingsModel), typeof(CaptionWindow), new PropertyMetadata(SmartView2.Models.Settings.SettingsModel.Defaults));
		}

		public CaptionWindow()
		{
			this.InitializeComponent();
			this.textBlock.Text = this.Text;
		}

		private static void OnText(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as CaptionWindow).textBlock.Text = (string)e.NewValue;
		}

		private void SettingsModel_SettingsSaved(object sender, EventArgs e)
		{
		}
	}
}