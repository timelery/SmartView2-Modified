using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class GridDragAndDropBehavior : Behavior<Grid>
	{
		public readonly static DependencyProperty DragAndDropCommandProperty;

		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(GridDragAndDropBehavior.DragAndDropCommandProperty);
			}
			set
			{
				base.SetValue(GridDragAndDropBehavior.DragAndDropCommandProperty, value);
			}
		}

		static GridDragAndDropBehavior()
		{
			GridDragAndDropBehavior.DragAndDropCommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(GridDragAndDropBehavior), new PropertyMetadata(null));
		}

		public GridDragAndDropBehavior()
		{
		}

		private void AssociatedObject_Drop(object sender, DragEventArgs e)
		{
			if (this.Command != null && this.Command.CanExecute(null) && e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
				this.Command.Execute(data);
			}
		}

		protected override void OnAttached()
		{
			base.AssociatedObject.Drop += new DragEventHandler(this.AssociatedObject_Drop);
		}
	}
}