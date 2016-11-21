using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;

namespace SmartView2.Views
{
	public partial class MultimediaNowPlayingPage : UserControl
	{
		private DispatcherTimer timer;

		public MultimediaNowPlayingPage()
		{
			this.InitializeComponent();
		}
	}
}