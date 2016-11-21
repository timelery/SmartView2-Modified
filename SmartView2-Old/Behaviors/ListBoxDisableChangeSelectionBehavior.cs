using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class ListBoxDisableChangeSelectionBehavior : Behavior<ListBox>
	{
		public ListBoxDisableChangeSelectionBehavior()
		{
		}

		private void AssociatedObject_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
			if (base.AssociatedObject.ContextMenu != null)
			{
				base.AssociatedObject.ContextMenu.IsOpen = true;
			}
		}

		protected override void OnAttached()
		{
			base.AssociatedObject.PreviewMouseRightButtonDown += new MouseButtonEventHandler(this.AssociatedObject_PreviewMouseRightButtonDown);
		}
	}
}