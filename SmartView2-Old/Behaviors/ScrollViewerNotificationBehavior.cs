using System;
using System.Windows;
using System.Windows.Controls;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class ScrollViewerNotificationBehavior : Behavior<ScrollViewer>
	{
		private readonly static DependencyProperty canScrollLeftProperty;

		private readonly static DependencyProperty canScrollRightProperty;

		private readonly static DependencyProperty canScrollUpProperty;

		private readonly static DependencyProperty canScrollDownProperty;

		public bool CanScrollDown
		{
			get
			{
				return (bool)base.GetValue(ScrollViewerNotificationBehavior.canScrollDownProperty);
			}
			set
			{
				base.SetValue(ScrollViewerNotificationBehavior.canScrollDownProperty, value);
			}
		}

		public bool CanScrollLeft
		{
			get
			{
				return (bool)base.GetValue(ScrollViewerNotificationBehavior.canScrollLeftProperty);
			}
			set
			{
				base.SetValue(ScrollViewerNotificationBehavior.canScrollLeftProperty, value);
			}
		}

		public bool CanScrollRight
		{
			get
			{
				return (bool)base.GetValue(ScrollViewerNotificationBehavior.canScrollRightProperty);
			}
			set
			{
				base.SetValue(ScrollViewerNotificationBehavior.canScrollRightProperty, value);
			}
		}

		public bool CanScrollUp
		{
			get
			{
				return (bool)base.GetValue(ScrollViewerNotificationBehavior.canScrollUpProperty);
			}
			set
			{
				base.SetValue(ScrollViewerNotificationBehavior.canScrollUpProperty, value);
			}
		}

		static ScrollViewerNotificationBehavior()
		{
			ScrollViewerNotificationBehavior.canScrollLeftProperty = DependencyProperty.Register("CanScrollLeft", typeof(bool), typeof(ScrollViewerNotificationBehavior));
			ScrollViewerNotificationBehavior.canScrollRightProperty = DependencyProperty.Register("CanScrollRight", typeof(bool), typeof(ScrollViewerNotificationBehavior));
			ScrollViewerNotificationBehavior.canScrollUpProperty = DependencyProperty.Register("CanScrollUp", typeof(bool), typeof(ScrollViewerNotificationBehavior));
			ScrollViewerNotificationBehavior.canScrollDownProperty = DependencyProperty.Register("CanScrollDown", typeof(bool), typeof(ScrollViewerNotificationBehavior));
		}

		public ScrollViewerNotificationBehavior()
		{
		}

		private void AssociatedObject_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			this.CanScrollLeft = e.HorizontalOffset > 0;
			this.CanScrollRight = e.HorizontalOffset + e.ViewportWidth < e.ExtentWidth;
			this.CanScrollUp = e.VerticalOffset > 0;
			this.CanScrollDown = e.VerticalOffset + e.ViewportWidth < e.ExtentHeight;
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.ScrollChanged += new ScrollChangedEventHandler(this.AssociatedObject_ScrollChanged);
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			base.AssociatedObject.ScrollChanged -= new ScrollChangedEventHandler(this.AssociatedObject_ScrollChanged);
		}
	}
}