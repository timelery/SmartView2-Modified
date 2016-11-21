using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmartView2.Behaviors
{
	public class ItemsControlDragItemBehavior : ItemsControlMoveItemBehavior
	{
		public readonly static DependencyProperty DragCommandProperty;

		public readonly static DependencyProperty SelectedItemsProperty;

		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(ItemsControlDragItemBehavior.DragCommandProperty);
			}
			set
			{
				base.SetValue(ItemsControlDragItemBehavior.DragCommandProperty, value);
			}
		}

		public IEnumerable<object> SelectedItems
		{
			get
			{
				return (IEnumerable<object>)base.GetValue(ItemsControlDragItemBehavior.SelectedItemsProperty);
			}
			set
			{
				base.SetValue(ItemsControlDragItemBehavior.SelectedItemsProperty, value);
			}
		}

		static ItemsControlDragItemBehavior()
		{
			ItemsControlDragItemBehavior.DragCommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ItemsControlDragItemBehavior), new PropertyMetadata(null));
			ItemsControlDragItemBehavior.SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(IEnumerable<object>), typeof(ItemsControlDragItemBehavior), new PropertyMetadata(null));
		}

		public ItemsControlDragItemBehavior()
		{
		}

		protected override void StartDrag(object dragObject)
		{
			if (this.Command != null && this.Command.CanExecute(null) && this.container != null)
			{
				if (base.IsSelectionMode && this.container.IsSelected)
				{
					if (this.SelectedItems != null)
					{
						this.Command.Execute(this.SelectedItems);
						return;
					}
				}
				else if (dragObject != null)
				{
					this.Command.Execute(dragObject);
				}
			}
		}
	}
}