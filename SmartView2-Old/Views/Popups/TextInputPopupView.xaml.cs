using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SmartView2.Views.Popups
{
	public partial class TextInputPopupView : UserControl
	{
		public TextInputPopupView()
		{
			this.InitializeComponent();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			this.textBox.Focus();
		}
	}
}