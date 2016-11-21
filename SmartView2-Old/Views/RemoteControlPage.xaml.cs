using SmartView2.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

namespace SmartView2.Views
{
	public partial class RemoteControlPage : UserControl
	{
		public RemoteControlPage()
		{
			this.InitializeComponent();
		}

		private void _comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			RemoteControlViewModel dataContext = base.DataContext as RemoteControlViewModel;
			if (dataContext == null)
			{
				return;
			}
			if (dataContext.SetSourceCommand.CanExecute(null))
			{
				dataContext.SetSourceCommand.Execute(this._comboBox.SelectedItem);
			}
		}
	}
}