using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace SmartView2.Views.Popups
{
	public partial class OkCancelPopupView : UserControl
	{
		public OkCancelPopupView()
		{
			this.InitializeComponent();
		}

		private void Button_KeyDown(object sender, KeyEventArgs e)
		{
		}
	}
}