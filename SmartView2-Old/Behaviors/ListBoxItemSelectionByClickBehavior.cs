using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UIFoundation.Common;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class ListBoxItemSelectionByClickBehavior : Behavior<UIElement>
	{
		public readonly static DependencyProperty IsEnabledProperty;

		private ListBoxItem item;

		private Point? downPoint = null;

		public bool IsEnabled
		{
			get
			{
				return (bool)base.GetValue(ListBoxItemSelectionByClickBehavior.IsEnabledProperty);
			}
			set
			{
				base.SetValue(ListBoxItemSelectionByClickBehavior.IsEnabledProperty, value);
			}
		}

		static ListBoxItemSelectionByClickBehavior()
		{
			ListBoxItemSelectionByClickBehavior.IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(ListBoxItemSelectionByClickBehavior), new PropertyMetadata(false));
		}

		public ListBoxItemSelectionByClickBehavior()
		{
		}

		private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (this.item != null && this.IsEnabled && e.ChangedButton == MouseButton.Left)
			{
				this.downPoint = new Point?(e.GetPosition(null));
				e.Handled = true;
			}
		}

		private void AssociatedObject_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (this.item != null && this.downPoint.HasValue)
			{
				Point position = e.GetPosition(null);
				Vector value = position - this.downPoint.Value;
				if (Math.Abs(value.X) < SystemParameters.MinimumHorizontalDragDistance && Math.Abs(value.Y) < SystemParameters.MinimumVerticalDragDistance)
				{
					this.item.IsSelected = !this.item.IsSelected;
				}
			}
			this.downPoint = null;
		}

		protected override void OnAttached()
		{
			this.item = base.AssociatedObject.FindAncestor<ListBoxItem>();
			base.AssociatedObject.MouseDown += new MouseButtonEventHandler(this.AssociatedObject_MouseDown);
			base.AssociatedObject.MouseUp += new MouseButtonEventHandler(this.AssociatedObject_MouseUp);
		}
	}
}