using System;
using System.Windows;
using System.Windows.Controls;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class TextInputBehavior : Behavior<TextBox>
	{
		private readonly static DependencyProperty cursorPosition;

		public int CursorPosition
		{
			get
			{
				return (int)base.GetValue(TextInputBehavior.cursorPosition);
			}
			set
			{
				base.SetValue(TextInputBehavior.cursorPosition, value);
			}
		}

		static TextInputBehavior()
		{
			TextInputBehavior.cursorPosition = DependencyProperty.Register("CursorPosition", typeof(int), typeof(TextInputBehavior), new PropertyMetadata(0, new PropertyChangedCallback(TextInputBehavior.OnCursorPositionChange)));
		}

		public TextInputBehavior()
		{
		}

		private static void OnCursorPositionChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBox associatedObject = (d as TextInputBehavior).AssociatedObject;
			associatedObject.SelectionStart = (int)e.NewValue;
			associatedObject.SelectionLength = 0;
		}
	}
}