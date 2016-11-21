using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UIFoundation.Common;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class ItemsControlRemoveOrReplaceItemBehavior : Behavior<FrameworkElement>
	{
		private bool isMousePressed;

		private Point startPoint;

		private ListBoxItem listBoxItem;

		private bool isDragStarted;

		private Point currentPoint;

		public readonly static DependencyProperty ReplaceCommandProperty;

		public readonly static DependencyProperty RemoveCommandProperty;

		private Point windowPoint;

		public ICommand RemoveCommand
		{
			get
			{
				return (ICommand)base.GetValue(ItemsControlRemoveOrReplaceItemBehavior.RemoveCommandProperty);
			}
			set
			{
				base.SetValue(ItemsControlRemoveOrReplaceItemBehavior.RemoveCommandProperty, value);
			}
		}

		public ICommand ReplaceCommand
		{
			get
			{
				return (ICommand)base.GetValue(ItemsControlRemoveOrReplaceItemBehavior.ReplaceCommandProperty);
			}
			set
			{
				base.SetValue(ItemsControlRemoveOrReplaceItemBehavior.ReplaceCommandProperty, value);
			}
		}

		static ItemsControlRemoveOrReplaceItemBehavior()
		{
			ItemsControlRemoveOrReplaceItemBehavior.ReplaceCommandProperty = DependencyProperty.Register("ReplaceCommand", typeof(ICommand), typeof(ItemsControlRemoveOrReplaceItemBehavior), new PropertyMetadata(null));
			ItemsControlRemoveOrReplaceItemBehavior.RemoveCommandProperty = DependencyProperty.Register("RemoveCommand", typeof(ICommand), typeof(ItemsControlRemoveOrReplaceItemBehavior), new PropertyMetadata(null));
		}

		public ItemsControlRemoveOrReplaceItemBehavior()
		{
		}

		private void AssociatedObject_DragDropCompleted(object sender, RoutedEventArgs e)
		{
			if (this.isMousePressed && this.isDragStarted && this.listBoxItem != null)
			{
				Vector vector = this.currentPoint - this.startPoint;
				if (Math.Abs(vector.X) > Math.Abs(vector.Y) && this.startPoint.X > this.currentPoint.X && this.RemoveCommand != null && this.RemoveCommand.CanExecute(null))
				{
					this.RemoveCommand.Execute(this.listBoxItem.DataContext);
				}
			}
			this.isMousePressed = false;
			this.isDragStarted = false;
			this.listBoxItem = null;
		}

		private void AssociatedObject_DragOver(object sender, DragEventArgs e)
		{
			if (this.isMousePressed && this.listBoxItem != null && this.isDragStarted)
			{
				this.currentPoint = e.GetPosition(base.AssociatedObject);
			}
		}

		private void AssociatedObject_Drop(object sender, DragEventArgs e)
		{
			if (this.isMousePressed && this.isDragStarted && this.listBoxItem != null)
			{
				string[] formats = e.Data.GetFormats();
				if ((int)formats.Length != 0)
				{
					string str = formats.GetValue(0).ToString();
					object data = e.Data.GetData(str);
					ListBoxItem listBoxItem = ((DependencyObject)e.OriginalSource).FindAncestor<ListBoxItem>();
					if (listBoxItem != null && this.ReplaceCommand != null && this.ReplaceCommand.CanExecute(null))
					{
						Tuple<object, object> tuple = new Tuple<object, object>(listBoxItem.DataContext, data);
						this.ReplaceCommand.Execute(tuple);
					}
				}
			}
			this.isMousePressed = false;
			this.isDragStarted = false;
			this.listBoxItem = null;
		}

		private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.isMousePressed = false;
			this.isDragStarted = false;
			this.listBoxItem = null;
		}

		private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
		{
			if (this.isMousePressed && this.listBoxItem != null)
			{
				Point position = e.GetPosition(base.AssociatedObject);
				if ((this.startPoint - position).Length > 10)
				{
					this.StartDrag();
				}
			}
		}

		private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.isMousePressed = true;
			this.startPoint = e.GetPosition(base.AssociatedObject);
			this.listBoxItem = ((DependencyObject)e.OriginalSource).FindAncestor<ListBoxItem>();
		}

		protected override void OnAttached()
		{
			base.AssociatedObject.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.AssociatedObject_PreviewMouseLeftButtonDown);
			base.AssociatedObject.MouseMove += new MouseEventHandler(this.AssociatedObject_MouseMove);
			base.AssociatedObject.MouseLeftButtonUp += new MouseButtonEventHandler(this.AssociatedObject_MouseLeftButtonUp);
			base.AssociatedObject.Drop += new DragEventHandler(this.AssociatedObject_Drop);
			base.AssociatedObject.DragOver += new DragEventHandler(this.AssociatedObject_DragOver);
			FieldInfo field = typeof(DragDrop).GetField("DragDropCompletedEvent", BindingFlags.Static | BindingFlags.NonPublic);
			RoutedEvent value = (RoutedEvent)field.GetValue(null);
			base.AssociatedObject.AddHandler(value, new RoutedEventHandler(this.AssociatedObject_DragDropCompleted));
		}

		protected override void OnDetaching()
		{
			base.AssociatedObject.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.AssociatedObject_PreviewMouseLeftButtonDown);
			base.AssociatedObject.MouseMove -= new MouseEventHandler(this.AssociatedObject_MouseMove);
			base.AssociatedObject.MouseLeftButtonUp -= new MouseButtonEventHandler(this.AssociatedObject_MouseLeftButtonUp);
			base.AssociatedObject.Drop -= new DragEventHandler(this.AssociatedObject_Drop);
			base.AssociatedObject.DragOver -= new DragEventHandler(this.AssociatedObject_DragOver);
			FieldInfo field = typeof(DragDrop).GetField("DragDropCompletedEvent", BindingFlags.Static | BindingFlags.NonPublic);
			RoutedEvent value = (RoutedEvent)field.GetValue(null);
			base.AssociatedObject.RemoveHandler(value, new RoutedEventHandler(this.AssociatedObject_DragDropCompleted));
		}

		private void StartDrag()
		{
			if (!this.isDragStarted)
			{
				this.isDragStarted = true;
				DragDrop.DoDragDrop(base.AssociatedObject, this.listBoxItem.DataContext, DragDropEffects.All);
			}
		}
	}
}