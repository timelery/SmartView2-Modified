using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class ListBoxExtendedSelectionBehavior : Behavior<ListBox>
	{
		public readonly static DependencyProperty ExtendedSelectionEnabledProperty;

		private readonly static DependencyPropertyKey ExtendedSelectionEnabledAttachedPropertyKey;

		public readonly static DependencyProperty ExtendedSelectionEnabledAttachedProperty;

		public readonly static DependencyProperty SelectedItemsProperty;

		public readonly static DependencyProperty IsEnabledProperty;

		public readonly static DependencyPropertyKey IsEnabledAttachedPropertyKey;

		public readonly static DependencyProperty IsEnabledAttachedProperty;

		public bool ExtendedSelectionEnabled
		{
			get
			{
				return (bool)base.GetValue(ListBoxExtendedSelectionBehavior.ExtendedSelectionEnabledProperty);
			}
			set
			{
				base.SetValue(ListBoxExtendedSelectionBehavior.ExtendedSelectionEnabledProperty, value);
			}
		}

		public bool IsEnabled
		{
			get
			{
				return (bool)base.GetValue(ListBoxExtendedSelectionBehavior.IsEnabledProperty);
			}
			set
			{
				base.SetValue(ListBoxExtendedSelectionBehavior.IsEnabledProperty, value);
			}
		}

		public IEnumerable<object> SelectedItems
		{
			get
			{
				return (IEnumerable<object>)base.GetValue(ListBoxExtendedSelectionBehavior.SelectedItemsProperty);
			}
			set
			{
				base.SetValue(ListBoxExtendedSelectionBehavior.SelectedItemsProperty, value);
			}
		}

		static ListBoxExtendedSelectionBehavior()
		{
			ListBoxExtendedSelectionBehavior.ExtendedSelectionEnabledProperty = DependencyProperty.Register("ExtendedSelectionEnabled", typeof(bool), typeof(ListBoxExtendedSelectionBehavior), new PropertyMetadata(false, new PropertyChangedCallback(ListBoxExtendedSelectionBehavior.OnExtendedSelectionEnabledChanged)));
			ListBoxExtendedSelectionBehavior.ExtendedSelectionEnabledAttachedPropertyKey = DependencyProperty.RegisterAttachedReadOnly("ExtendedSelectionEnabledAttached", typeof(bool), typeof(ListBoxExtendedSelectionBehavior), new PropertyMetadata(false));
			ListBoxExtendedSelectionBehavior.ExtendedSelectionEnabledAttachedProperty = ListBoxExtendedSelectionBehavior.ExtendedSelectionEnabledAttachedPropertyKey.DependencyProperty;
			ListBoxExtendedSelectionBehavior.SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(IEnumerable<object>), typeof(ListBoxExtendedSelectionBehavior), new PropertyMetadata(null));
			ListBoxExtendedSelectionBehavior.IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(ListBoxExtendedSelectionBehavior), new PropertyMetadata(true, new PropertyChangedCallback(ListBoxExtendedSelectionBehavior.OnIsEnabledChanged)));
			ListBoxExtendedSelectionBehavior.IsEnabledAttachedPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsEnabledAttached", typeof(bool), typeof(ListBoxExtendedSelectionBehavior), new PropertyMetadata(true));
			ListBoxExtendedSelectionBehavior.IsEnabledAttachedProperty = ListBoxExtendedSelectionBehavior.IsEnabledAttachedPropertyKey.DependencyProperty;
		}

		public ListBoxExtendedSelectionBehavior()
		{
		}

		private void AssociatedObject_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Right && this.ExtendedSelectionEnabled)
			{
				this.ExtendedSelectionEnabled = false;
			}
		}

		private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.IsEnabled)
			{
				this.SelectedItems = base.AssociatedObject.SelectedItems.Cast<object>().ToArray<object>();
				if (base.AssociatedObject.SelectedItems.Count > 1 && e.AddedItems.Count > 0)
				{
					this.ExtendedSelectionEnabled = true;
				}
			}
		}

		public static bool GetExtendedSelectionEnabledAttached(ListBox obj)
		{
			return (bool)obj.GetValue(ListBoxExtendedSelectionBehavior.ExtendedSelectionEnabledAttachedProperty);
		}

		public static bool GetIsEnabledAttached(DependencyObject obj)
		{
			return (bool)obj.GetValue(ListBoxExtendedSelectionBehavior.IsEnabledAttachedProperty);
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.SelectionChanged += new SelectionChangedEventHandler(this.AssociatedObject_SelectionChanged);
			base.AssociatedObject.PreviewMouseDown += new MouseButtonEventHandler(this.AssociatedObject_PreviewMouseDown);
		}

		protected override void OnDetaching()
		{
			base.AssociatedObject.SelectionChanged -= new SelectionChangedEventHandler(this.AssociatedObject_SelectionChanged);
			base.AssociatedObject.PreviewMouseDown -= new MouseButtonEventHandler(this.AssociatedObject_PreviewMouseDown);
		}

		private static void OnExtendedSelectionEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ListBoxExtendedSelectionBehavior listBoxExtendedSelectionBehavior = d as ListBoxExtendedSelectionBehavior;
			bool? newValue = (bool?)(e.NewValue as bool?);
			if (listBoxExtendedSelectionBehavior != null && newValue.HasValue && listBoxExtendedSelectionBehavior.IsEnabled)
			{
				if (!newValue.Value)
				{
					object selectedItem = listBoxExtendedSelectionBehavior.AssociatedObject.SelectedItem;
					if (listBoxExtendedSelectionBehavior.AssociatedObject.SelectedItems.Count > 1)
					{
						listBoxExtendedSelectionBehavior.AssociatedObject.SelectedItems.Clear();
					}
					if (selectedItem != null)
					{
						listBoxExtendedSelectionBehavior.AssociatedObject.SelectedItems.Add(selectedItem);
					}
				}
				else if (listBoxExtendedSelectionBehavior.AssociatedObject.SelectedItems.Count == 1)
				{
					listBoxExtendedSelectionBehavior.AssociatedObject.SelectedItem = null;
				}
				ListBoxExtendedSelectionBehavior.SetExtendedSelectionEnabledAttached(listBoxExtendedSelectionBehavior.AssociatedObject, newValue.Value);
			}
		}

		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ListBoxExtendedSelectionBehavior listBoxExtendedSelectionBehavior = d as ListBoxExtendedSelectionBehavior;
			bool? newValue = (bool?)(e.NewValue as bool?);
			if (listBoxExtendedSelectionBehavior != null && newValue.HasValue && listBoxExtendedSelectionBehavior.AssociatedObject != null)
			{
				ListBoxExtendedSelectionBehavior.SetIsEnabledAttached(listBoxExtendedSelectionBehavior.AssociatedObject, newValue.Value);
				bool? nullable = newValue;
				if ((nullable.GetValueOrDefault() ? false : nullable.HasValue))
				{
					listBoxExtendedSelectionBehavior.ExtendedSelectionEnabled = false;
					listBoxExtendedSelectionBehavior.SelectedItems = null;
					ListBoxExtendedSelectionBehavior.SetExtendedSelectionEnabledAttached(listBoxExtendedSelectionBehavior.AssociatedObject, false);
				}
			}
		}

		public static void SetExtendedSelectionEnabledAttached(ListBox obj, bool value)
		{
			obj.SetValue(ListBoxExtendedSelectionBehavior.ExtendedSelectionEnabledAttachedPropertyKey, value);
		}

		private static void SetIsEnabledAttached(DependencyObject obj, bool value)
		{
			obj.SetValue(ListBoxExtendedSelectionBehavior.IsEnabledAttachedPropertyKey, value);
		}
	}
}