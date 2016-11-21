using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class ComboBoxSelectionChangedBehavior : Behavior<ComboBox>
	{
		public readonly static DependencyProperty SelectionChangedCommandProperty;

		public readonly static DependencyProperty CommandParametrProperty;

		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(ComboBoxSelectionChangedBehavior.SelectionChangedCommandProperty);
			}
			set
			{
				base.SetValue(ComboBoxSelectionChangedBehavior.SelectionChangedCommandProperty, value);
			}
		}

		public object CommandParametr
		{
			get
			{
				return base.GetValue(ComboBoxSelectionChangedBehavior.CommandParametrProperty);
			}
			set
			{
				base.SetValue(ComboBoxSelectionChangedBehavior.CommandParametrProperty, value);
			}
		}

		static ComboBoxSelectionChangedBehavior()
		{
			ComboBoxSelectionChangedBehavior.SelectionChangedCommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ComboBoxSelectionChangedBehavior), new PropertyMetadata(null));
			ComboBoxSelectionChangedBehavior.CommandParametrProperty = DependencyProperty.Register("CommandParametr", typeof(object), typeof(ComboBoxSelectionChangedBehavior), new PropertyMetadata(null));
		}

		public ComboBoxSelectionChangedBehavior()
		{
		}

		private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.Command != null && this.Command.CanExecute(this.CommandParametr))
			{
				this.Command.Execute(base.AssociatedObject.SelectedIndex);
			}
		}

		protected override void OnAttached()
		{
			base.AssociatedObject.SelectionChanged += new SelectionChangedEventHandler(this.AssociatedObject_SelectionChanged);
		}
	}
}