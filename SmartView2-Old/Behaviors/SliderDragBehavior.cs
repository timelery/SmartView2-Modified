using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class SliderDragBehavior : Behavior<Slider>
	{
		public readonly static DependencyProperty CommandProperty;

		public readonly static DependencyProperty ValueProperty;

		private double position;

		private Point? startPoint = null;

		private double? nextPosition = null;

		private DispatcherTimer timer;

		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(SliderDragBehavior.CommandProperty);
			}
			set
			{
				base.SetValue(SliderDragBehavior.CommandProperty, value);
			}
		}

		protected double? NextPosition
		{
			get
			{
				return this.nextPosition;
			}
		}

		public double? Value
		{
			get
			{
				return (double?)base.GetValue(SliderDragBehavior.ValueProperty);
			}
			set
			{
				base.SetValue(SliderDragBehavior.ValueProperty, value);
			}
		}

		static SliderDragBehavior()
		{
			SliderDragBehavior.CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(SliderDragBehavior), new PropertyMetadata(null));
			SliderDragBehavior.ValueProperty = DependencyProperty.Register("Value", typeof(double?), typeof(SliderDragBehavior), new PropertyMetadata(null, new PropertyChangedCallback(SliderDragBehavior.OnValueChanged)));
		}

		public SliderDragBehavior()
		{
		}

		private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
		{
			this.position = 0;
			this.startPoint = null;
			base.AssociatedObject.ReleaseMouseCapture();
		}

		private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (this.Command != null && this.Command.CanExecute(null))
			{
				this.CaptureNextPosition(this.position);
				if (this.position != 0)
				{
					this.Command.Execute(this.position);
				}
				else if (this.startPoint.HasValue)
				{
					double actualWidth = 1 / base.AssociatedObject.ActualWidth * this.startPoint.Value.X;
					this.position = base.AssociatedObject.Maximum * actualWidth;
					base.AssociatedObject.SetCurrentValue(RangeBase.ValueProperty, this.position);
					this.Command.Execute(this.position);
				}
			}
			this.position = 0;
			this.startPoint = null;
			base.AssociatedObject.ReleaseMouseCapture();
		}

		private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
		{
			if (base.AssociatedObject.IsMouseCaptured)
			{
				Point position = e.GetPosition(base.AssociatedObject);
				double actualWidth = 1 / base.AssociatedObject.ActualWidth * position.X;
				this.position = base.AssociatedObject.Maximum * actualWidth;
				base.AssociatedObject.SetCurrentValue(RangeBase.ValueProperty, this.position);
			}
		}

		private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.startPoint = new Point?(e.GetPosition(base.AssociatedObject));
			base.AssociatedObject.CaptureMouse();
		}

		private void CaptureNextPosition(double pos)
		{
			this.nextPosition = new double?(pos);
			this.timer.Start();
		}

		protected override void OnAttached()
		{
			base.AssociatedObject.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.AssociatedObject_PreviewMouseLeftButtonDown);
			base.AssociatedObject.MouseMove += new MouseEventHandler(this.AssociatedObject_MouseMove);
			base.AssociatedObject.MouseLeftButtonUp += new MouseButtonEventHandler(this.AssociatedObject_MouseLeftButtonUp);
			base.AssociatedObject.MouseLeave += new MouseEventHandler(this.AssociatedObject_MouseLeave);
			this.timer = new DispatcherTimer()
			{
				Interval = TimeSpan.FromSeconds(1)
			};
			this.timer.Tick += new EventHandler(this.timer_Tick);
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SliderDragBehavior value = d as SliderDragBehavior;
			double? newValue = (double?)e.NewValue;
			if (!value.AssociatedObject.IsMouseCaptured && newValue.HasValue)
			{
				if (!value.NextPosition.HasValue)
				{
					value.AssociatedObject.Value = newValue.Value;
					return;
				}
				if (value.NextPosition.Value == newValue.Value)
				{
					value.AssociatedObject.Value = newValue.Value;
				}
			}
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			this.nextPosition = null;
			this.timer.Stop();
		}
	}
}