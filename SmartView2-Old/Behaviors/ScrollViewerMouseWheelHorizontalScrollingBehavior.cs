using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class ScrollViewerMouseWheelHorizontalScrollingBehavior : Behavior<ScrollViewer>
	{
		public ScrollViewerMouseWheelHorizontalScrollingBehavior()
		{
		}

		private void AssociatedObject_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (!(sender is ScrollViewer))
			{
				return;
			}
			ScrollViewer scrollViewer = (ScrollViewer)sender;
			ScrollViewer scrollViewer1 = sender as ScrollViewer;
			if (e.Delta <= 0)
			{
				scrollViewer1.LineRight();
			}
			else
			{
				scrollViewer1.LineLeft();
			}
			e.Handled = true;
		}

		protected override void OnAttached()
		{
			base.AssociatedObject.PreviewMouseWheel += new MouseWheelEventHandler(this.AssociatedObject_PreviewMouseWheel);
		}
	}
}