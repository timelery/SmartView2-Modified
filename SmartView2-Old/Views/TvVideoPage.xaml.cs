using SmartView2.Controls;
using SmartView2.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Threading;

namespace SmartView2.Views
{
	public partial class TvVideoPage : System.Windows.Controls.UserControl
	{
		private System.Windows.Threading.Dispatcher dispatcher;

		private bool isSotwareRenderingEnabled;

		public TvVideoPage()
		{
			this.InitializeComponent();
			this.dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
			base.Loaded += new RoutedEventHandler(this.TvVideoPage_Loaded);
			base.Unloaded += new RoutedEventHandler(this.TvVideoPage_Unloaded);
		}

		private void _comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			TvVideoViewModel dataContext = base.DataContext as TvVideoViewModel;
			if (dataContext == null)
			{
				return;
			}
			if (dataContext.SelectSourceCommand.CanExecute(this._comboBox.SelectedItem))
			{
				dataContext.SelectSourceCommand.Execute(this._comboBox.SelectedItem);
			}
		}

		private void dataContext_NeedRestartPlayer(object sender, EventArgs e)
		{
		}

		private void fullScreenButton_Click(object sender, RoutedEventArgs e)
		{
			this.mediaElement.SwitchFullScreenMode();
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			this.mediaElement.SwitchFullScreenMode();
		}

		private void TvVideoPage_Loaded(object sender, RoutedEventArgs e)
		{
			if ((int)Screen.AllScreens.Length > 1)
			{
				HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
				if (hwndSource != null)
				{
					HwndTarget compositionTarget = hwndSource.CompositionTarget;
					if (compositionTarget != null)
					{
						compositionTarget.RenderMode = RenderMode.SoftwareOnly;
					}
					this.isSotwareRenderingEnabled = true;
				}
			}
		}

		private void TvVideoPage_Unloaded(object sender, RoutedEventArgs e)
		{
		}
	}
}