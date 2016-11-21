using SmartView2.Core.Commands;
using System;
using System.Windows;
using System.Windows.Input;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class TouchPadBehavior : Behavior<UIElement>
	{
		private Point? startPoint = null;

		private Point? previousPoint = null;

		private double totalDistance;

		private readonly static DependencyProperty acceptCommand;

		private readonly static DependencyProperty moveUpCommand;

		private readonly static DependencyProperty moveRightCommand;

		private readonly static DependencyProperty moveDownCommand;

		private readonly static DependencyProperty moveLeftCommand;

		private readonly static DependencyProperty sensitivity;

		public ICommand AcceptCommand
		{
			get
			{
				return (ICommand)base.GetValue(TouchPadBehavior.acceptCommand);
			}
			set
			{
				base.SetValue(TouchPadBehavior.acceptCommand, value);
			}
		}

		public ICommand MoveDownCommand
		{
			get
			{
				return (ICommand)base.GetValue(TouchPadBehavior.moveDownCommand);
			}
			set
			{
				base.SetValue(TouchPadBehavior.moveDownCommand, value);
			}
		}

		public ICommand MoveLeftCommand
		{
			get
			{
				return (ICommand)base.GetValue(TouchPadBehavior.moveLeftCommand);
			}
			set
			{
				base.SetValue(TouchPadBehavior.moveLeftCommand, value);
			}
		}

		public ICommand MoveRightCommand
		{
			get
			{
				return (ICommand)base.GetValue(TouchPadBehavior.moveRightCommand);
			}
			set
			{
				base.SetValue(TouchPadBehavior.moveRightCommand, value);
			}
		}

		public ICommand MoveUpCommand
		{
			get
			{
				return (ICommand)base.GetValue(TouchPadBehavior.moveUpCommand);
			}
			set
			{
				base.SetValue(TouchPadBehavior.moveUpCommand, value);
			}
		}

		public double Sensitivity
		{
			get
			{
				return (double)base.GetValue(TouchPadBehavior.sensitivity);
			}
			set
			{
				base.SetValue(TouchPadBehavior.sensitivity, value);
			}
		}

		static TouchPadBehavior()
		{
			TouchPadBehavior.acceptCommand = DependencyProperty.Register("AcceptCommand", typeof(ICommand), typeof(TouchPadBehavior), new PropertyMetadata(null));
			TouchPadBehavior.moveUpCommand = DependencyProperty.Register("MoveUpCommand", typeof(ICommand), typeof(TouchPadBehavior), new PropertyMetadata(null));
			TouchPadBehavior.moveRightCommand = DependencyProperty.Register("MoveRightCommand", typeof(ICommand), typeof(TouchPadBehavior), new PropertyMetadata(null));
			TouchPadBehavior.moveDownCommand = DependencyProperty.Register("MoveDownCommand", typeof(ICommand), typeof(TouchPadBehavior), new PropertyMetadata(null));
			TouchPadBehavior.moveLeftCommand = DependencyProperty.Register("MoveLeftCommand", typeof(ICommand), typeof(TouchPadBehavior), new PropertyMetadata(null));
			TouchPadBehavior.sensitivity = DependencyProperty.Register("Sensitivity", typeof(double), typeof(TouchPadBehavior), new PropertyMetadata((double)15));
		}

		public TouchPadBehavior()
		{
		}

		private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.startPoint = new Point?(e.GetPosition(base.AssociatedObject));
			this.previousPoint = new Point?(e.GetPosition(base.AssociatedObject));
			base.AssociatedObject.CaptureMouse();
		}

		private void AssociatedObject_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (base.AssociatedObject.IsMouseCaptured)
			{
				if (this.totalDistance >= this.Sensitivity)
				{
					Point position = e.GetPosition(base.AssociatedObject);
					this.TryProcessGesture(this.startPoint.Value, position);
				}
				else
				{
					this.AcceptCommand.ExecuteIfYouCan(null);
				}
				base.AssociatedObject.ReleaseMouseCapture();
				this.startPoint = null;
				this.previousPoint = null;
				this.totalDistance = 0;
			}
		}

		private void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (base.AssociatedObject.IsMouseCaptured)
			{
				Point position = e.GetPosition(base.AssociatedObject);
				TouchPadBehavior length = this;
				double num = length.totalDistance;
				Vector vector = Point.Subtract(position, this.previousPoint.Value);
				length.totalDistance = num + vector.Length;
				this.previousPoint = new Point?(position);
				if (this.TryProcessGesture(this.startPoint.Value, position))
				{
					this.startPoint = new Point?(position);
				}
			}
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.AssociatedObject_PreviewMouseLeftButtonDown);
			base.AssociatedObject.PreviewMouseMove += new MouseEventHandler(this.AssociatedObject_PreviewMouseMove);
			base.AssociatedObject.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.AssociatedObject_PreviewMouseLeftButtonUp);
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			base.AssociatedObject.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.AssociatedObject_PreviewMouseLeftButtonDown);
			base.AssociatedObject.PreviewMouseMove -= new MouseEventHandler(this.AssociatedObject_PreviewMouseMove);
			base.AssociatedObject.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.AssociatedObject_PreviewMouseLeftButtonUp);
		}

		private bool TryProcessGesture(Point start, Point end)
		{
			Vector vector = Point.Subtract(end, start);
			if (vector.Length < this.Sensitivity)
			{
				return false;
			}
			if (Math.Abs(vector.X) > Math.Abs(vector.Y))
			{
				if (vector.X <= 0)
				{
					this.MoveLeftCommand.ExecuteIfYouCan(null);
				}
				else
				{
					this.MoveRightCommand.ExecuteIfYouCan(null);
				}
			}
			else if (vector.Y <= 0)
			{
				this.MoveUpCommand.ExecuteIfYouCan(null);
			}
			else
			{
				this.MoveDownCommand.ExecuteIfYouCan(null);
			}
			return true;
		}
	}
}