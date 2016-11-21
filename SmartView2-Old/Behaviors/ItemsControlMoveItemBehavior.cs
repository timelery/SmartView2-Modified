using MediaLibrary.DataModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UIFoundation.Common;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class ItemsControlMoveItemBehavior : Behavior<FrameworkElement>
	{
		public readonly static DependencyProperty IsSelectionModeProperty;

		public readonly static DependencyProperty DragDropEffectProperty;

		private bool isMouseClicked;

		protected ListBoxItem container;

		public DragDropEffects DragDropEffect
		{
			get
			{
				return (DragDropEffects)base.GetValue(ItemsControlMoveItemBehavior.DragDropEffectProperty);
			}
			set
			{
				base.SetValue(ItemsControlMoveItemBehavior.DragDropEffectProperty, value);
			}
		}

		public bool IsSelectionMode
		{
			get
			{
				return (bool)base.GetValue(ItemsControlMoveItemBehavior.IsSelectionModeProperty);
			}
			set
			{
				base.SetValue(ItemsControlMoveItemBehavior.IsSelectionModeProperty, value);
			}
		}

		static ItemsControlMoveItemBehavior()
		{
			ItemsControlMoveItemBehavior.IsSelectionModeProperty = DependencyProperty.Register("IsSelectionMode", typeof(bool), typeof(ItemsControlMoveItemBehavior), new PropertyMetadata(false));
			ItemsControlMoveItemBehavior.DragDropEffectProperty = DependencyProperty.Register("DragDropEffect", typeof(DragDropEffects), typeof(ItemsControlMoveItemBehavior), new PropertyMetadata(DragDropEffects.Move));
		}

		public ItemsControlMoveItemBehavior()
		{
		}

		private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
		{
			if (this.isMouseClicked)
			{
				this.StartDrag(base.AssociatedObject.DataContext);
				this.isMouseClicked = false;
			}
		}

		private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.isMouseClicked = true;
		}

		private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.isMouseClicked = false;
		}

		protected override void OnAttached()
		{
			base.AssociatedObject.MouseLeftButtonDown += new MouseButtonEventHandler(this.AssociatedObject_MouseLeftButtonDown);
			base.AssociatedObject.MouseLeftButtonUp += new MouseButtonEventHandler(this.AssociatedObject_MouseLeftButtonUp);
			base.AssociatedObject.MouseLeave += new MouseEventHandler(this.AssociatedObject_MouseLeave);
			this.container = base.AssociatedObject.FindAncestor<ListBoxItem>();
		}

		protected virtual void StartDrag(object dragObject)
		{
			if (dragObject != null)
			{
				DataObject dataObject = new DataObject(typeof(ItemBase), dragObject);
				DragDrop.DoDragDrop(base.AssociatedObject, dataObject, this.DragDropEffect);
			}
		}
	}
}