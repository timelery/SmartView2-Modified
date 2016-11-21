using SmartView2.Core.Commands;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class DoubleClickBehavior : Behavior<UIElement>
	{
		private Point? lastClickPoint = null;

		private Stopwatch timer = new Stopwatch();

		private static DependencyProperty commandProperty;

		private static DependencyProperty commandParameterProperty;

		private static DependencyProperty maxDistanceProperty;

		private static DependencyProperty maxElapsedTimeProperty;

		private readonly static DependencyProperty IsEnabledProperty;

		public ICommand Command
		{
			get
			{
				return base.GetValue(DoubleClickBehavior.commandProperty) as ICommand;
			}
			set
			{
				base.SetValue(DoubleClickBehavior.commandProperty, value);
			}
		}

		public object CommandParameter
		{
			get
			{
				return base.GetValue(DoubleClickBehavior.commandParameterProperty);
			}
			set
			{
				base.SetValue(DoubleClickBehavior.commandParameterProperty, value);
			}
		}

		public bool IsEnabled
		{
			get
			{
				return (bool)base.GetValue(DoubleClickBehavior.IsEnabledProperty);
			}
			set
			{
				base.SetValue(DoubleClickBehavior.IsEnabledProperty, value);
			}
		}

		public double MaxDistance
		{
			get
			{
				return (double)base.GetValue(DoubleClickBehavior.maxDistanceProperty);
			}
			set
			{
				base.SetValue(DoubleClickBehavior.maxDistanceProperty, value);
			}
		}

		public TimeSpan MaxElapsedTime
		{
			get
			{
				return (TimeSpan)base.GetValue(DoubleClickBehavior.maxElapsedTimeProperty);
			}
			set
			{
				base.SetValue(DoubleClickBehavior.maxElapsedTimeProperty, value);
			}
		}

		static DoubleClickBehavior()
		{
			DoubleClickBehavior.commandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(DoubleClickBehavior));
			DoubleClickBehavior.commandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(DoubleClickBehavior));
			DoubleClickBehavior.maxDistanceProperty = DependencyProperty.Register("MaxDistance", typeof(double), typeof(DoubleClickBehavior), new PropertyMetadata((double)50));
			DoubleClickBehavior.maxElapsedTimeProperty = DependencyProperty.Register("MaxElapsedTime", typeof(TimeSpan), typeof(DoubleClickBehavior), new PropertyMetadata(TimeSpan.FromSeconds(0.4)));
			DoubleClickBehavior.IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(DoubleClickBehavior), new PropertyMetadata(true, new PropertyChangedCallback(DoubleClickBehavior.OnIsEnabledChanged)));
		}

		public DoubleClickBehavior()
		{
		}

		private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				Point position = e.GetPosition(base.AssociatedObject);
				if (this.lastClickPoint.HasValue && this.IsDoubleClick(position, this.lastClickPoint.Value, this.timer.Elapsed))
				{
					if (this.Command != null)
					{
						this.Command.ExecuteIfYouCan(this.CommandParameter);
					}
					this.Invalidate();
				}
				this.lastClickPoint = new Point?(position);
				this.timer.Restart();
			}
			e.Handled = false;
		}

		private void AssociatedObject_StylusDown(object sender, StylusDownEventArgs e)
		{
			e.Handled = true;
		}

		private void AssociatedObject_TouchDown(object sender, TouchEventArgs e)
		{
			e.Handled = true;
		}

		private void Execute()
		{
			this.Command.ExecuteIfYouCan(null);
		}

		private void Invalidate()
		{
			this.lastClickPoint = null;
		}

		private bool IsDoubleClick(Point currentPoint, Point lastPoint, TimeSpan elapsedTime)
		{
			Vector vector = Point.Subtract(currentPoint, lastPoint);
			bool length = vector.Length < this.MaxDistance;
			bool flag = (elapsedTime == TimeSpan.Zero ? false : elapsedTime < this.MaxElapsedTime);
			if (length)
			{
				return flag;
			}
			return false;
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.AssociatedObject_PreviewMouseLeftButtonDown);
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			base.AssociatedObject.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.AssociatedObject_PreviewMouseLeftButtonDown);
		}

		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DoubleClickBehavior doubleClickBehavior = d as DoubleClickBehavior;
			bool? newValue = (bool?)(e.NewValue as bool?);
			if (newValue.HasValue && doubleClickBehavior != null)
			{
				if (newValue.Value)
				{
					doubleClickBehavior.OnAttached();
					return;
				}
				doubleClickBehavior.OnDetaching();
				doubleClickBehavior.Invalidate();
			}
		}
	}
}