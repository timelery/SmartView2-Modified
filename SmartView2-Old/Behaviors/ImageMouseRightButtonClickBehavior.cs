using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class ImageMouseRightButtonClickBehavior : Behavior<Image>
	{
		public ImageMouseRightButtonClickBehavior()
		{
		}

		private void AssociatedObject_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		private void AssociatedObject_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		protected override void OnAttached()
		{
			base.AssociatedObject.MouseRightButtonDown += new MouseButtonEventHandler(this.AssociatedObject_PreviewMouseDoubleClick);
			base.AssociatedObject.MouseRightButtonUp += new MouseButtonEventHandler(this.AssociatedObject_MouseRightButtonUp);
		}
	}
}