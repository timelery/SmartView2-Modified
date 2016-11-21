using MediaLibrary.DataModels;
using SmartView2.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace SmartView2.Controls
{
	[TemplatePart(Name="Part_DurationTextBlock", Type=typeof(TextBlock))]
	[TemplatePart(Name="Part_Image", Type=typeof(Image))]
	[TemplatePart(Name="PART_PositionTextBlock", Type=typeof(TextBlock))]
	[TemplatePart(Name="PART_ProgressBar", Type=typeof(Slider))]
	public class MediaPlayerExt : MediaElementExt
	{
		public const string ProgressBarGridPart = "PART_ProgressBarGrid";

		public const string ProgressBarPart = "PART_ProgressBar";

		public const string PositionTextBlockPart = "PART_PositionTextBlock";

		public const string DurationTextBlockPart = "Part_DurationTextBlock";

		public const string ImagePart = "Part_Image";

		private DispatcherTimer dragTimer;

		private DispatcherTimer mediaFailedTimer;

		private Slider slider;

		private TextBlock positionTextBlock;

		private TextBlock durationTextBlock;

		private Image image;

		private bool isDragging;

		public readonly static DependencyProperty HasDurationProperty;

		public readonly static DependencyProperty PositionProperty;

		public readonly static DependencyProperty DefaultPositionProperty;

		public readonly static DependencyProperty DurationProperty;

		public readonly static DependencyProperty ImageLeftContentProperty;

		public readonly static DependencyProperty ImageRightContentProperty;

		public readonly static DependencyProperty BottomBarProperty;

		public readonly static DependencyProperty ContentTypeProperty;

		public readonly static DependencyProperty PreviewProperty;

		public readonly static RoutedEvent MediaEndedEvent;

		public Panel BottomBar
		{
			get
			{
				return (Panel)base.GetValue(MediaPlayerExt.BottomBarProperty);
			}
			set
			{
				base.SetValue(MediaPlayerExt.BottomBarProperty, value);
			}
		}

		public MediaLibrary.DataModels.ContentType ContentType
		{
			get
			{
				return (MediaLibrary.DataModels.ContentType)base.GetValue(MediaPlayerExt.ContentTypeProperty);
			}
			set
			{
				base.SetValue(MediaPlayerExt.ContentTypeProperty, value);
			}
		}

		public double? DefaultPosition
		{
			get
			{
				return new double?((double)base.GetValue(MediaPlayerExt.DefaultPositionProperty));
			}
			set
			{
				base.SetValue(MediaPlayerExt.DefaultPositionProperty, value);
			}
		}

		public double Duration
		{
			get
			{
				return (double)base.GetValue(MediaPlayerExt.DurationProperty);
			}
			set
			{
				base.SetValue(MediaPlayerExt.DurationProperty, value);
			}
		}

		public bool HasDuration
		{
			get
			{
				return (bool)base.GetValue(MediaPlayerExt.HasDurationProperty);
			}
			set
			{
				base.SetValue(MediaPlayerExt.HasDurationProperty, value);
			}
		}

		public Panel ImageLeftContent
		{
			get
			{
				return (Panel)base.GetValue(MediaPlayerExt.ImageLeftContentProperty);
			}
			set
			{
				base.SetValue(MediaPlayerExt.ImageLeftContentProperty, value);
			}
		}

		public Panel ImageRightContent
		{
			get
			{
				return (Panel)base.GetValue(MediaPlayerExt.ImageRightContentProperty);
			}
			set
			{
				base.SetValue(MediaPlayerExt.ImageRightContentProperty, value);
			}
		}

		public double Position
		{
			get
			{
				return (double)base.GetValue(MediaPlayerExt.PositionProperty);
			}
			set
			{
				base.SetValue(MediaPlayerExt.PositionProperty, value);
			}
		}

		public Uri Preview
		{
			get
			{
				return (Uri)base.GetValue(MediaPlayerExt.PreviewProperty);
			}
			set
			{
				base.SetValue(MediaPlayerExt.PreviewProperty, value);
			}
		}

		static MediaPlayerExt()
		{
			MediaPlayerExt.HasDurationProperty = DependencyProperty.Register("HasDuration", typeof(bool), typeof(MediaPlayerExt), new PropertyMetadata(true));
			MediaPlayerExt.PositionProperty = DependencyProperty.Register("Position", typeof(double), typeof(MediaPlayerExt));
			MediaPlayerExt.DefaultPositionProperty = DependencyProperty.Register("DefaultPosition", typeof(double?), typeof(MediaPlayerExt), new PropertyMetadata(null, new PropertyChangedCallback(MediaPlayerExt.OnDefaultPositionChanged)));
			MediaPlayerExt.DurationProperty = DependencyProperty.Register("Duration", typeof(double), typeof(MediaPlayerExt));
			MediaPlayerExt.ImageLeftContentProperty = DependencyProperty.Register("ImageLeftContent", typeof(Panel), typeof(MediaPlayerExt));
			MediaPlayerExt.ImageRightContentProperty = DependencyProperty.Register("ImageRightContent", typeof(Panel), typeof(MediaPlayerExt));
			MediaPlayerExt.BottomBarProperty = DependencyProperty.Register("BottomBar", typeof(Panel), typeof(MediaPlayerExt));
			MediaPlayerExt.ContentTypeProperty = DependencyProperty.Register("ContentType", typeof(MediaLibrary.DataModels.ContentType), typeof(MediaPlayerExt));
			MediaPlayerExt.PreviewProperty = DependencyProperty.Register("Preview", typeof(Uri), typeof(MediaPlayerExt));
			MediaPlayerExt.MediaEndedEvent = EventManager.RegisterRoutedEvent("MediaEnded", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(MediaPlayerExt));
		}

		public MediaPlayerExt()
		{
			base.Loaded += new RoutedEventHandler(this.OnLoaded);
		}

		private void ChangePosition(MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed && this.isDragging)
			{
				Point position = e.GetPosition(this.slider);
				double actualWidth = 1 / this.slider.ActualWidth * position.X;
				double maximum = this.slider.Maximum * actualWidth;
				this.slider.SetCurrentValue(RangeBase.ValueProperty, maximum);
				this.Position = maximum;
			}
		}

		private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
		{
			CommandManager.InvalidateRequerySuggested();
			if (!this.mediaElement.NaturalDuration.HasTimeSpan)
			{
				if (this.ContentType == MediaLibrary.DataModels.ContentType.Track)
				{
					this.RaiseMediaEndedEvent();
				}
				return;
			}
			this.Position = this.mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
			this.RaiseMediaEndedEvent();
		}

		private void mediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
		{
			Logger.Instance.LogMessageFormat("[SmartView2][MediaPlayerExt]mediaElement_MediaFailed started... ", new object[0]);
			Logger instance = Logger.Instance;
			object[] str = new object[] { e.ErrorException.GetType().ToString(), e.ErrorException.Message };
			instance.LogMessageFormat("[SmartView2][MediaPlayerExt]mediaElement_MediaFailed Error type: {0}, Error message: {1}", str);
			Exception errorException = e.ErrorException;
			if (this.ContentType == MediaLibrary.DataModels.ContentType.Track)
			{
				this.mediaFailedTimer.Start();
			}
		}

		private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
		{
			CommandManager.InvalidateRequerySuggested();
			if (!this.mediaElement.NaturalDuration.HasTimeSpan)
			{
				this.dragTimer.Stop();
				return;
			}
			this.Position = 0;
			this.Duration = this.mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
			this.dragTimer.Start();
		}

		private void mediaFailedTimer_Tick(object sender, EventArgs e)
		{
			if (base.MediaElementState == SmartView2.Controls.MediaElementState.Play)
			{
				RoutedEventArgs routedEventArg = new RoutedEventArgs(MediaElement.MediaEndedEvent);
				this.mediaElement.RaiseEvent(routedEventArg);
				this.mediaFailedTimer.Stop();
			}
		}

		private void MediaPlayerExt_MouseLeave(object sender, MouseEventArgs e)
		{
			this.PlayFromCurrentPosition();
		}

		private void MediaPlayerExt_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.PlayFromCurrentPosition();
		}

		private void MediaPlayerExt_MouseMove(object sender, MouseEventArgs e)
		{
			this.ChangePosition(e);
		}

		private void MediaPlayerExt_Unloaded(object sender, RoutedEventArgs e)
		{
			base.MediaElementState = SmartView2.Controls.MediaElementState.Close;
			this.mediaElement.MediaFailed -= new EventHandler<ExceptionRoutedEventArgs>(this.mediaElement_MediaFailed);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.slider = (Slider)base.GetTemplateChild("PART_ProgressBar");
			this.positionTextBlock = (TextBlock)base.GetTemplateChild("PART_PositionTextBlock");
			this.durationTextBlock = (TextBlock)base.GetTemplateChild("Part_DurationTextBlock");
			this.image = (Image)base.GetTemplateChild("Part_Image");
		}

		private static void OnDefaultPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			double? newValue = (double?)e.NewValue;
			if (newValue.HasValue)
			{
				MediaPlayerExt totalMilliseconds = d as MediaPlayerExt;
				totalMilliseconds.isDragging = true;
				totalMilliseconds.mediaElement.Position = TimeSpan.FromMilliseconds(newValue.Value);
				totalMilliseconds.Position = totalMilliseconds.mediaElement.Position.TotalMilliseconds;
				totalMilliseconds.isDragging = false;
			}
		}

		private static void OnHasVideoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			base.MouseLeave += new MouseEventHandler(this.MediaPlayerExt_MouseLeave);
			base.MouseLeftButtonUp += new MouseButtonEventHandler(this.MediaPlayerExt_MouseLeftButtonUp);
			base.MouseMove += new MouseEventHandler(this.MediaPlayerExt_MouseMove);
			this.mediaElement.MediaEnded += new RoutedEventHandler(this.mediaElement_MediaEnded);
			this.mediaElement.MediaOpened += new RoutedEventHandler(this.mediaElement_MediaOpened);
			this.mediaElement.MediaFailed += new EventHandler<ExceptionRoutedEventArgs>(this.mediaElement_MediaFailed);
			this.slider.AddHandler(UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(this.progressBar_MouseLeftButtonDown), true);
			this.slider.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.progressBar_MouseLeftButtonUp);
			this.slider.MouseMove += new MouseEventHandler(this.slider_MouseMove);
			this.slider.MouseLeave += new MouseEventHandler(this.slider_MouseLeave);
			this.dragTimer = new DispatcherTimer()
			{
				Interval = TimeSpan.FromSeconds(1)
			};
			this.dragTimer.Tick += new EventHandler(this.OnTimerTick);
			this.mediaFailedTimer = new DispatcherTimer()
			{
				Interval = TimeSpan.FromSeconds(1)
			};
			this.mediaFailedTimer.Tick += new EventHandler(this.mediaFailedTimer_Tick);
			base.Unloaded += new RoutedEventHandler(this.MediaPlayerExt_Unloaded);
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			if (!this.isDragging)
			{
				this.Position = this.mediaElement.Position.TotalMilliseconds;
			}
		}

		private void PlayFromCurrentPosition()
		{
			if (this.isDragging)
			{
				this.mediaElement.Position = TimeSpan.FromMilliseconds(this.slider.Value);
				this.dragTimer.Start();
				this.isDragging = false;
			}
		}

		private void progressBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.isDragging = true;
		}

		private void progressBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.PlayFromCurrentPosition();
		}

		private void RaiseMediaEndedEvent()
		{
			base.RaiseEvent(new RoutedEventArgs(MediaPlayerExt.MediaEndedEvent));
		}

		private void slider_MouseLeave(object sender, MouseEventArgs e)
		{
			if (this.isDragging)
			{
				this.mediaElement.Position = TimeSpan.FromMilliseconds(this.slider.Value);
			}
		}

		private void slider_MouseMove(object sender, MouseEventArgs e)
		{
			this.ChangePosition(e);
		}

		public event RoutedEventHandler MediaEnded
		{
			add
			{
				base.AddHandler(MediaPlayerExt.MediaEndedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MediaPlayerExt.MediaEndedEvent, value);
			}
		}
	}
}