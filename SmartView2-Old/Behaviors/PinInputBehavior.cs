using SmartView2.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class PinInputBehavior : Behavior<TextBox>
	{
		private readonly static DependencyProperty previousControlProperty;

		private readonly static DependencyProperty nextControlProperty;

		private readonly static DependencyProperty pairCommandProperty;

		private readonly IDictionary<Key, string> digitKeys = new Dictionary<Key, string>()
		{
			{ Key.D0, "0" },
			{ Key.NumPad0, "0" },
			{ Key.D1, "1" },
			{ Key.NumPad1, "1" },
			{ Key.D2, "2" },
			{ Key.NumPad2, "2" },
			{ Key.D3, "3" },
			{ Key.NumPad3, "3" },
			{ Key.D4, "4" },
			{ Key.NumPad4, "4" },
			{ Key.D5, "5" },
			{ Key.NumPad5, "5" },
			{ Key.D6, "6" },
			{ Key.NumPad6, "6" },
			{ Key.D7, "7" },
			{ Key.NumPad7, "7" },
			{ Key.D8, "8" },
			{ Key.NumPad8, "8" },
			{ Key.D9, "9" },
			{ Key.NumPad9, "9" }
		};

		public TextBox NextControl
		{
			get
			{
				return base.GetValue(PinInputBehavior.nextControlProperty) as TextBox;
			}
			set
			{
				base.SetValue(PinInputBehavior.nextControlProperty, value);
			}
		}

		public ICommand PairCommand
		{
			get
			{
				return base.GetValue(PinInputBehavior.pairCommandProperty) as ICommand;
			}
			set
			{
				base.SetValue(PinInputBehavior.pairCommandProperty, value);
			}
		}

		public TextBox PreviousControl
		{
			get
			{
				return base.GetValue(PinInputBehavior.previousControlProperty) as TextBox;
			}
			set
			{
				base.SetValue(PinInputBehavior.previousControlProperty, value);
			}
		}

		static PinInputBehavior()
		{
			PinInputBehavior.previousControlProperty = DependencyProperty.Register("PreviousControl", typeof(TextBox), typeof(PinInputBehavior));
			PinInputBehavior.nextControlProperty = DependencyProperty.Register("NextControl", typeof(TextBox), typeof(PinInputBehavior));
			PinInputBehavior.pairCommandProperty = DependencyProperty.Register("PairCommand", typeof(ICommand), typeof(PinInputBehavior));
		}

		public PinInputBehavior()
		{
		}

		private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Back)
			{
				this.Back();
				e.Handled = true;
				return;
			}
			if (e.Key == Key.Return)
			{
				this.Enter();
				e.Handled = true;
			}
		}

		private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
		{
			e.Handled = true;
			TextChange textChange = e.Changes.FirstOrDefault<TextChange>();
			if ((textChange == null ? true : textChange.AddedLength <= 0))
			{
				return;
			}
			string str = base.AssociatedObject.Text.Substring(textChange.Offset, textChange.AddedLength);
			string text = base.AssociatedObject.Text;
			text = (!this.digitKeys.Values.Contains(str) ? text.Remove(textChange.Offset, textChange.AddedLength) : str);
			base.AssociatedObject.Dispatcher.BeginInvoke(new Action(() => {
				this.AssociatedObject.Text = text;
				this.AssociatedObject.SelectionStart = this.AssociatedObject.Text.Length;
			}), new object[0]);
			if (!string.IsNullOrEmpty(text))
			{
				this.ToNext();
			}
		}

		private void Back()
		{
			if (!string.IsNullOrEmpty(base.AssociatedObject.Text))
			{
				base.AssociatedObject.Text = string.Empty;
			}
			else
			{
				this.ToPrevious();
				if (this.PreviousControl != null)
				{
					this.PreviousControl.Text = string.Empty;
					return;
				}
			}
		}

		private void Enter()
		{
			BindingExpression bindingExpression = base.AssociatedObject.GetBindingExpression(TextBox.TextProperty);
			if (bindingExpression != null)
			{
				bindingExpression.UpdateSource();
			}
			this.PairCommand.ExecuteIfYouCan(null);
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.PreviewKeyDown += new KeyEventHandler(this.AssociatedObject_PreviewKeyDown);
			base.AssociatedObject.TextChanged += new TextChangedEventHandler(this.AssociatedObject_TextChanged);
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			base.AssociatedObject.PreviewKeyDown -= new KeyEventHandler(this.AssociatedObject_PreviewKeyDown);
			base.AssociatedObject.TextChanged -= new TextChangedEventHandler(this.AssociatedObject_TextChanged);
		}

		private void ToNext()
		{
			if (this.NextControl != null)
			{
				this.NextControl.Focus();
				this.NextControl.SelectAll();
			}
		}

		private void ToPrevious()
		{
			if (this.PreviousControl != null)
			{
				this.PreviousControl.Focus();
				this.PreviousControl.SelectAll();
			}
		}
	}
}