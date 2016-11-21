using MediaLibrary.DataModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using UIFoundation.Navigation.Controls;

namespace SmartView2.Views
{
	public partial class MultimediaPage : UserControl
	{
		public MultimediaPage()
		{
			this.InitializeComponent();
		}

		private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Button button = sender as Button;
			System.Windows.Controls.ContextMenu contextMenu = button.ContextMenu;
			if (contextMenu != null)
			{
				contextMenu.PlacementTarget = button;
				contextMenu.IsOpen = true;
			}
		}

		private void listBoxItem_Drop(object sender, DragEventArgs e)
		{
			e.Data.GetData(typeof(MediaLibrary.DataModels.Content));
			object dataContext = ((ListBoxItem)sender).DataContext;
			object obj = base.DataContext;
		}

		private void listBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (sender is ListBoxItem)
			{
				ListBoxItem listBoxItem = sender as ListBoxItem;
				DragDrop.DoDragDrop(listBoxItem, listBoxItem.DataContext, DragDropEffects.Move);
				listBoxItem.IsSelected = true;
			}
		}
	}
}