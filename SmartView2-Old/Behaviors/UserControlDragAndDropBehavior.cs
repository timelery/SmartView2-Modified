using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class UserControlDragAndDropBehavior : Behavior<UserControl>
	{
		public readonly static DependencyProperty DragAndDropCommandProperty;

		private Point? startPoint = null;

		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(UserControlDragAndDropBehavior.DragAndDropCommandProperty);
			}
			set
			{
				base.SetValue(UserControlDragAndDropBehavior.DragAndDropCommandProperty, value);
			}
		}

		static UserControlDragAndDropBehavior()
		{
			UserControlDragAndDropBehavior.DragAndDropCommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(UserControlDragAndDropBehavior), new PropertyMetadata(null));
		}

		public UserControlDragAndDropBehavior()
		{
		}

		private void AssociatedObject_Drop(object sender, DragEventArgs e)
		{
			if (this.Command != null && this.Command.CanExecute(null))
			{
				if (!this.startPoint.HasValue)
				{
					return;
				}
				Point position = e.GetPosition(base.AssociatedObject);
				if (Math.Abs(this.startPoint.Value.X - position.X) < Math.Abs(this.startPoint.Value.Y - position.Y))
				{
					return;
				}
				ListBoxItem data = (ListBoxItem)e.Data.GetData(typeof(ListBoxItem));
				if (data != null)
				{
					this.Command.Execute(data.DataContext);
				}
			}
		}

		private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.startPoint = new Point?(e.GetPosition(base.AssociatedObject));
		}

		protected override void OnAttached()
		{
			base.AssociatedObject.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.AssociatedObject_PreviewMouseLeftButtonDown);
			base.AssociatedObject.Drop += new DragEventHandler(this.AssociatedObject_Drop);
		}
	}
}