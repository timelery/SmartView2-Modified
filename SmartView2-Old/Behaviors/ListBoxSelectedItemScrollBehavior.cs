using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class ListBoxSelectedItemScrollBehavior : Behavior<ListBox>
	{
		public readonly static DependencyProperty IsEnabledProperty;

		public bool IsEnabled
		{
			get
			{
				return (bool)base.GetValue(ListBoxSelectedItemScrollBehavior.IsEnabledProperty);
			}
			set
			{
				base.SetValue(ListBoxSelectedItemScrollBehavior.IsEnabledProperty, value);
			}
		}

		static ListBoxSelectedItemScrollBehavior()
		{
			ListBoxSelectedItemScrollBehavior.IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(ListBoxSelectedItemScrollBehavior), new PropertyMetadata(true));
		}

		public ListBoxSelectedItemScrollBehavior()
		{
		}

		private void AssociatedObject_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			bool? newValue = (bool?)(e.NewValue as bool?);
			if ((newValue.HasValue ? newValue.GetValueOrDefault() : false))
			{
				ListBox listBox = sender as ListBox;
				if (listBox != null && this.IsEnabled)
				{
					base.AssociatedObject.ScrollIntoView(listBox.SelectedItem);
				}
			}
		}

		private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListBox listBox = sender as ListBox;
			if (listBox != null && this.IsEnabled)
			{
				base.AssociatedObject.ScrollIntoView(listBox.SelectedItem);
			}
		}

		private void ItemssSourceChanged(object sender, EventArgs e)
		{
			ListBox listBox = sender as ListBox;
			if (listBox != null)
			{
				base.AssociatedObject.ScrollIntoView(listBox.SelectedItem);
			}
		}

		protected override void OnAttached()
		{
			base.AssociatedObject.SelectionChanged += new SelectionChangedEventHandler(this.AssociatedObject_SelectionChanged);
			base.AssociatedObject.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.AssociatedObject_IsVisibleChanged);
			DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ListBox));
			if (dependencyPropertyDescriptor != null)
			{
				dependencyPropertyDescriptor.AddValueChanged(base.AssociatedObject, new EventHandler(this.ItemssSourceChanged));
			}
		}
	}
}