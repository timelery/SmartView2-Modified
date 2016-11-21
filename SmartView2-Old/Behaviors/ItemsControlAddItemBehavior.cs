using System;
using System.Windows;
using System.Windows.Input;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class ItemsControlAddItemBehavior : Behavior<FrameworkElement>
	{
		public readonly static DependencyProperty CommandProperty;

		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(ItemsControlAddItemBehavior.CommandProperty);
			}
			set
			{
				base.SetValue(ItemsControlAddItemBehavior.CommandProperty, value);
			}
		}

		static ItemsControlAddItemBehavior()
		{
			ItemsControlAddItemBehavior.CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ItemsControlAddItemBehavior), new PropertyMetadata(null));
		}

		public ItemsControlAddItemBehavior()
		{
		}

		private void AssociatedObject_Drop(object sender, DragEventArgs e)
		{
			if (e.AllowedEffects == DragDropEffects.Copy && this.Command != null && this.Command.CanExecute(null))
			{
				string[] formats = e.Data.GetFormats();
				if ((int)formats.Length != 0)
				{
					string str = formats.GetValue(0).ToString();
					object data = e.Data.GetData(str);
					if (data != null)
					{
						this.Command.Execute(data);
					}
				}
			}
		}

		protected override void OnAttached()
		{
			base.AssociatedObject.Drop += new DragEventHandler(this.AssociatedObject_Drop);
		}
	}
}