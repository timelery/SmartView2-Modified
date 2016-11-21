using SmartView2.Core;
using SmartView2.Devices;
using SmartView2.Native;
using SmartView2.Native.Player;
using SmartView2.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using UIFoundation.Navigation;

namespace SmartView2.Controls
{
	[TemplatePart(Name="PART_CCDataLayout", Type=typeof(Panel))]
	[TemplatePart(Name="PART_LayoutHost", Type=typeof(Border))]
	[TemplatePart(Name="PART_LayoutRoot", Type=typeof(Panel))]
	[TemplatePart(Name="PART_LoadingAnimation", Type=typeof(UIElement))]
	[TemplatePart(Name="PART_MediaElement", Type=typeof(MediaElement))]
	[TemplatePart(Name="PART_NotificationContent", Type=typeof(ContentPresenter))]
	[TemplatePart(Name="PART_OnVideoContentControl", Type=typeof(ContentPresenter))]
	[TemplateVisualState(Name="STATE_Hidden", GroupName="GROUP_OnVideoContentVisibility")]
	[TemplateVisualState(Name="STATE_Loading", GroupName="GROUP_VideoLoading")]
	[TemplateVisualState(Name="STATE_NonLoading", GroupName="GROUP_VideoLoading")]
	[TemplateVisualState(Name="STATE_NotificationNonVisible", GroupName="GROUP_NotificationVisibility")]
	[TemplateVisualState(Name="STATE_NotificationVisible", GroupName="GROUP_NotificationVisibility")]
	[TemplateVisualState(Name="STATE_Visible", GroupName="GROUP_OnVideoContentVisibility")]
	public class MediaElementExt : ContentControl
	{
		public const string LayoutRootPart = "PART_LayoutRoot";

		public const string MediaElementPart = "PART_MediaElement";

		public const string CCDataLayoutPart = "PART_CCDataLayout";

		public const string OnVideoContentControlPart = "PART_OnVideoContentControl";

		public const string LayoutHostPart = "PART_LayoutHost";

		public const string LoadingPart = "PART_LoadingAnimation";

		public const string ContentVisibilityVisualStateGroup = "GROUP_OnVideoContentVisibility";

		public const string ContentVisibilityVisibleState = "STATE_Visible";

		public const string ContentVisibilityHiddenState = "STATE_Hidden";

		public const string VideoLoadingGroup = "GROUP_VideoLoading";

		public const string LoadingState = "STATE_Loading";

		public const string NonLoadingState = "STATE_NonLoading";

		public const string NotificationVisibilityGroup = "GROUP_NotificationVisibility";

		public const string NotificationVisibleState = "STATE_NotificationVisible";

		public const string NotificationNonVisibleState = "STATE_NotificationNonVisible";

		public const string ClickListenerPart = "PART_ClickListener";

		public const string NotificationContentPresenter = "PART_NotificationContent";

		private const string VideoStreamStarted = "VIDEO_PLAYBACK_STARTED";

		protected MediaElement mediaElement;

		private bool fadeOutAnimationEnded = true;

		private Panel layoutRoot;

		private MediaElementExtFullscreenWindow fullscreenWindow;

		private Grid ccDataLayout;

		private CCDataDecoder ccDataDecoder;

		private Border layoutHostPart;

		private ContentPresenter onVideoContent;

		private DispatcherTimer timer;

		private DispatcherTimer loadingTimer;

		public readonly static DependencyProperty SourceUriProperty;

		public readonly static DependencyProperty PlayerNotificationProviderProperty;

		public readonly static DependencyProperty CanFullscreenProperty;

		public readonly static DependencyProperty IsContentFadesProperty;

		public readonly static DependencyProperty MediaElementStateProperty;

		public readonly static DependencyProperty IsNotificationShownProperty;

		public readonly static DependencyProperty NotificationContentProperty;

		public readonly static DependencyProperty NotificationTypeProperty;

		public readonly static DependencyProperty IsCcDataVisibleProperty;

		public readonly static DependencyProperty NotificationSourceProperty;

		public readonly static DependencyProperty IsFullScreenProperty;

		public readonly static DependencyProperty VolumeProperty;

		public readonly static DependencyProperty IsVideoStartedProperty;

		public readonly static DependencyProperty IsLoadingVisibleProperty;

		public readonly static DependencyProperty PopupSourceProperty;

		public bool CanFullscreen
		{
			get
			{
				return (bool)base.GetValue(MediaElementExt.CanFullscreenProperty);
			}
			set
			{
				base.SetValue(MediaElementExt.CanFullscreenProperty, value);
			}
		}

		public bool IsCcDataVisible
		{
			get
			{
				return (bool)base.GetValue(MediaElementExt.IsCcDataVisibleProperty);
			}
			set
			{
				base.SetValue(MediaElementExt.IsCcDataVisibleProperty, value);
			}
		}

		public bool IsContentFades
		{
			get
			{
				return (bool)base.GetValue(MediaElementExt.IsContentFadesProperty);
			}
			set
			{
				base.SetValue(MediaElementExt.IsContentFadesProperty, value);
			}
		}

		public bool IsFullScreen
		{
			get
			{
				return (bool)base.GetValue(MediaElementExt.IsFullScreenProperty);
			}
			private set
			{
				base.SetValue(MediaElementExt.IsFullScreenProperty, value);
			}
		}

		public bool IsLoadingVisible
		{
			get
			{
				return (bool)base.GetValue(MediaElementExt.IsLoadingVisibleProperty);
			}
			set
			{
				base.SetValue(MediaElementExt.IsLoadingVisibleProperty, value);
			}
		}

		public bool IsNotificationShown
		{
			get
			{
				return (bool)base.GetValue(MediaElementExt.IsNotificationShownProperty);
			}
			set
			{
				base.SetValue(MediaElementExt.IsNotificationShownProperty, value);
			}
		}

		public bool IsVideoStarted
		{
			get
			{
				return (bool)base.GetValue(MediaElementExt.IsVideoStartedProperty);
			}
			set
			{
				base.SetValue(MediaElementExt.IsVideoStartedProperty, value);
			}
		}

		public SmartView2.Controls.MediaElementState MediaElementState
		{
			get
			{
				return (SmartView2.Controls.MediaElementState)base.GetValue(MediaElementExt.MediaElementStateProperty);
			}
			set
			{
				base.SetValue(MediaElementExt.MediaElementStateProperty, value);
			}
		}

		public object NotificationContent
		{
			get
			{
				return base.GetValue(MediaElementExt.NotificationContentProperty);
			}
			set
			{
				base.SetValue(MediaElementExt.NotificationContentProperty, value);
			}
		}

		public IEnumerable<TostMessage> NotificationSource
		{
			get
			{
				return (IEnumerable<TostMessage>)base.GetValue(MediaElementExt.NotificationSourceProperty);
			}
			set
			{
				base.SetValue(MediaElementExt.NotificationSourceProperty, value);
			}
		}

		public MessageType? NotificationType
		{
			get
			{
				return (MessageType?)base.GetValue(MediaElementExt.NotificationTypeProperty);
			}
			set
			{
				base.SetValue(MediaElementExt.NotificationTypeProperty, value);
			}
		}

		public IPlayerNotificationProvider PlayerNotificationProvider
		{
			get
			{
				return (IPlayerNotificationProvider)base.GetValue(MediaElementExt.PlayerNotificationProviderProperty);
			}
			set
			{
				base.SetValue(MediaElementExt.PlayerNotificationProviderProperty, value);
			}
		}

		public IEnumerable<PopupViewModel> PopupSource
		{
			get
			{
				return (IEnumerable<PopupViewModel>)base.GetValue(MediaElementExt.PopupSourceProperty);
			}
			set
			{
				base.SetValue(MediaElementExt.PopupSourceProperty, value);
			}
		}

		public Uri SourceUri
		{
			get
			{
				return (Uri)base.GetValue(MediaElementExt.SourceUriProperty);
			}
			set
			{
				base.SetValue(MediaElementExt.SourceUriProperty, value);
			}
		}

		public double Volume
		{
			get
			{
				return (double)base.GetValue(MediaElementExt.VolumeProperty);
			}
			set
			{
				base.SetValue(MediaElementExt.VolumeProperty, value);
			}
		}

		static MediaElementExt()
		{
			MediaElementExt.SourceUriProperty = DependencyProperty.Register("SourceUri", typeof(Uri), typeof(MediaElementExt), new PropertyMetadata(null, new PropertyChangedCallback(MediaElementExt.OnSourceUri)));
			MediaElementExt.PlayerNotificationProviderProperty = DependencyProperty.Register("PlayerNotificationProvider", typeof(IPlayerNotificationProvider), typeof(MediaElementExt), new PropertyMetadata(null, new PropertyChangedCallback(MediaElementExt.OnPlayerNotificationProvider)));
			MediaElementExt.CanFullscreenProperty = DependencyProperty.Register("CanFullscreen", typeof(bool), typeof(MediaElementExt), new PropertyMetadata(true));
			MediaElementExt.IsContentFadesProperty = DependencyProperty.Register("IsContentFades", typeof(bool), typeof(MediaElementExt), new PropertyMetadata(true, new PropertyChangedCallback(MediaElementExt.OnIsContentFadesChanget)));
			MediaElementExt.MediaElementStateProperty = DependencyProperty.Register("MediaElementState", typeof(SmartView2.Controls.MediaElementState), typeof(MediaElementExt), new PropertyMetadata(SmartView2.Controls.MediaElementState.Default, new PropertyChangedCallback(MediaElementExt.MediaElementStateChanged)));
			MediaElementExt.IsNotificationShownProperty = DependencyProperty.Register("IsNotificationShown", typeof(bool), typeof(MediaElementExt), new PropertyMetadata(false, new PropertyChangedCallback(MediaElementExt.OnIsNotificationShownChanged)));
			MediaElementExt.NotificationContentProperty = DependencyProperty.Register("NotificationContent", typeof(object), typeof(MediaElementExt), new PropertyMetadata(null, new PropertyChangedCallback(MediaElementExt.OnNotificationContentChanged)));
			MediaElementExt.NotificationTypeProperty = DependencyProperty.Register("NotificationType", typeof(MessageType?), typeof(MediaElementExt), new PropertyMetadata(null, new PropertyChangedCallback(MediaElementExt.OnNotificationTypeChanged)));
			MediaElementExt.IsCcDataVisibleProperty = DependencyProperty.Register("IsCcDataVisible", typeof(bool), typeof(MediaElementExt), new PropertyMetadata(false, new PropertyChangedCallback(MediaElementExt.OnCcDataVisibleChanged)));
			MediaElementExt.NotificationSourceProperty = DependencyProperty.Register("NotificationSource", typeof(IEnumerable<TostMessage>), typeof(MediaElementExt), new PropertyMetadata(null));
			MediaElementExt.IsFullScreenProperty = DependencyProperty.Register("IsFullScreen", typeof(bool), typeof(MediaElementExt), new PropertyMetadata(false));
			MediaElementExt.VolumeProperty = DependencyProperty.Register("Volume", typeof(double), typeof(MediaElementExt), new PropertyMetadata(0.5));
			MediaElementExt.IsVideoStartedProperty = DependencyProperty.Register("IsVideoStarted", typeof(bool), typeof(MediaElementExt), new PropertyMetadata(false, new PropertyChangedCallback(MediaElementExt.OnIsVideoStartedChanged)));
			MediaElementExt.IsLoadingVisibleProperty = DependencyProperty.Register("IsLoadingVisible", typeof(bool), typeof(MediaElementExt), new PropertyMetadata(true));
			MediaElementExt.PopupSourceProperty = DependencyProperty.Register("PopupSource", typeof(IEnumerable<PopupViewModel>), typeof(MediaElementExt), new PropertyMetadata(null));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(MediaElementExt), new FrameworkPropertyMetadata(typeof(MediaElementExt)));
		}

		public MediaElementExt()
		{
			this.timer = new DispatcherTimer()
			{
				Interval = TimeSpan.FromSeconds(5)
			};
			this.timer.Tick += new EventHandler(this.timer_Tick);
			this.loadingTimer = new DispatcherTimer()
			{
				Interval = TimeSpan.FromSeconds(30)
			};
			this.loadingTimer.Tick += new EventHandler(this.loadingTimer_Tick);
			this.ccDataDecoder = new CCDataDecoder();
			this.ccDataDecoder.ShowCaption += new EventHandler<CaptionEventArgs>(this.ccDataDecoder_ShowCaption);
			this.ccDataDecoder.HideCaption += new EventHandler<CaptionEventArgs>(this.ccDataDecoder_HideCaption);
			Grid grid = new Grid();
			MediaElementExt.SwitchStateToLoading(this);
			base.Unloaded += new RoutedEventHandler(this.MediaElementExt_Unloaded);
		}

		private void AdjustMixerVolume()
		{
            IMMDeviceEnumerator deviceEnumerator = (IMMDeviceEnumerator)new MMDeviceEnumerator();
            IMMDevice ppDevice;
            deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eConsole, out ppDevice);
            Guid guid = typeof(IAudioSessionManager2).GUID;
            object ppInterface;
            ppDevice.Activate(ref guid, 0, IntPtr.Zero, out ppInterface);
            IAudioSessionManager2 audioSessionManager2 = (IAudioSessionManager2)ppInterface;
            IAudioSessionEnumerator SessionEnum;
            audioSessionManager2.GetSessionEnumerator(out SessionEnum);
            int SessionCount1;
            SessionEnum.GetCount(out SessionCount1);
            List<ISimpleAudioVolume> list = new List<ISimpleAudioVolume>();
            for (int SessionCount2 = 0; SessionCount2 < SessionCount1; ++SessionCount2)
            {
                IAudioSessionControl2 Session;
                SessionEnum.GetSession(SessionCount2, out Session);
                ulong pRetVal;
                Session.GetProcessId(out pRetVal);
                if ((long)pRetVal == (long)Process.GetCurrentProcess().Id)
                    list.Add(Session as ISimpleAudioVolume);
            }
            float fLevel = 1f;
            foreach (ISimpleAudioVolume simpleAudioVolume in list)
            {
                float pfLevel;
                simpleAudioVolume.GetMasterVolume(out pfLevel);
                if ((double)fLevel > (double)pfLevel)
                    fLevel = pfLevel;
            }
            foreach (ISimpleAudioVolume simpleAudioVolume in list)
            {
                Guid EventContext = Guid.Empty;
                simpleAudioVolume.SetMasterVolume(fLevel, ref EventContext);
                Marshal.ReleaseComObject(simpleAudioVolume);
            }
            Marshal.ReleaseComObject(SessionEnum);
            Marshal.ReleaseComObject(audioSessionManager2);
            Marshal.ReleaseComObject(ppDevice);
            Marshal.ReleaseComObject(deviceEnumerator);
        }

		private void ccDataDecoder_HideCaption(object sender, CaptionEventArgs e)
		{
			this.ccDataLayout.Children.Remove(e.WindowRef);
		}

		private void ccDataDecoder_ShowCaption(object sender, CaptionEventArgs e)
		{
			this.ccDataLayout.Children.Add(e.WindowRef);
		}

		private void clickListener_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount >= 2 && e.ChangedButton == MouseButton.Left)
			{
				this.SwitchFullScreenMode();
			}
		}

		private void ContentExchange()
		{
			base.Dispatcher.Invoke(() => {
				if (this.fullscreenWindow != null)
				{
					this.IsFullScreen = false;
					object content = this.fullscreenWindow.Content;
					System.Windows.Controls.ContextMenu contextMenu = this.fullscreenWindow.ContextMenu;
					this.fullscreenWindow.Content = null;
					this.fullscreenWindow.ContextMenu = null;
					this.fullscreenWindow = null;
					base.ContextMenu = contextMenu;
					this.layoutHostPart.Child = (UIElement)content;
				}
				else if (this.CanFullscreen)
				{
					this.IsFullScreen = true;
					this.fullscreenWindow = this.CreateWindow();
					this.fullscreenWindow.NotificationSource = this.NotificationSource;
					this.fullscreenWindow.PopupSource = this.PopupSource;
					this.fullscreenWindow.DataContext = base.DataContext;
					this.fullscreenWindow.Closing += new CancelEventHandler((object s, CancelEventArgs e) => {
						this.fullscreenWindow.KeyDown -= new KeyEventHandler(this.fullscreenWindow_KeyDown);
						this.ContentExchange();
					});
					this.fullscreenWindow.KeyDown += new KeyEventHandler(this.fullscreenWindow_KeyDown);
					UIElement child = this.layoutHostPart.Child;
					System.Windows.Controls.ContextMenu contextMenu1 = base.ContextMenu;
					this.mediaElement.ContextMenu = null;
					this.layoutHostPart.Child = null;
					this.fullscreenWindow.Content = child;
					this.fullscreenWindow.ContextMenu = contextMenu1;
					this.fullscreenWindow.ShowDialog();
					return;
				}
			});
		}

		private MediaElementExtFullscreenWindow CreateWindow()
		{
			return new MediaElementExtFullscreenWindow()
			{
				Owner = Application.Current.MainWindow
			};
		}

		private void fullscreenWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if (!this.IsContentFades)
			{
				return;
			}
			this.timer.Stop();
			this.timer.Start();
			VisualStateManager.GoToState(this, "STATE_Visible", true);
		}

		private void layoutRoot_MouseMove(object sender, MouseEventArgs e)
		{
			if (!this.IsContentFades)
			{
				return;
			}
			this.timer.Stop();
			this.timer.Start();
			if (this.fadeOutAnimationEnded)
			{
				VisualStateManager.GoToState(this, "STATE_Visible", true);
			}
		}

		private void loadingTimer_Tick(object sender, EventArgs e)
		{
			base.SetCurrentValue(MediaElementExt.NotificationTypeProperty, MessageType.NeedRestart);
			this.loadingTimer.Stop();
		}

		private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
		{
			this.AdjustMixerVolume();
		}

		private void MediaElementExt_Unloaded(object sender, RoutedEventArgs e)
		{
			this.mediaElement.Source = null;
			if (this.PlayerNotificationProvider != null)
			{
				this.PlayerNotificationProvider.CCDataReceived -= new EventHandler<CCDataEventArgs>(this.OnCCDataReceived);
			}
			if (this.fullscreenWindow != null)
			{
				this.fullscreenWindow.Close();
			}
		}

		private static void MediaElementStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is MediaElementExt)
			{
				MediaElementExt mediaElementExt = (MediaElementExt)d;
				switch ((SmartView2.Controls.MediaElementState)e.NewValue)
				{
					case SmartView2.Controls.MediaElementState.Play:
					{
						mediaElementExt.mediaElement.Play();
						return;
					}
					case SmartView2.Controls.MediaElementState.Pause:
					{
						if (mediaElementExt.MediaElementState != SmartView2.Controls.MediaElementState.Play)
						{
							mediaElementExt.mediaElement.Play();
						}
						mediaElementExt.mediaElement.Pause();
						return;
					}
					case SmartView2.Controls.MediaElementState.Stop:
					{
						mediaElementExt.mediaElement.Pause();
						if (!mediaElementExt.mediaElement.NaturalDuration.HasTimeSpan)
						{
							break;
						}
						mediaElementExt.mediaElement.Position = TimeSpan.FromSeconds(0);
						return;
					}
					case SmartView2.Controls.MediaElementState.Close:
					{
						mediaElementExt.mediaElement.Close();
						break;
					}
					default:
					{
						return;
					}
				}
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this.layoutRoot != null)
			{
				this.layoutRoot.MouseMove -= new MouseEventHandler(this.layoutRoot_MouseMove);
			}
			this.layoutRoot = (Panel)base.GetTemplateChild("PART_LayoutRoot");
			if (this.layoutRoot != null)
			{
				this.layoutRoot.MouseMove += new MouseEventHandler(this.layoutRoot_MouseMove);
			}
			this.mediaElement = (MediaElement)base.GetTemplateChild("PART_MediaElement");
			this.mediaElement.MediaOpened += new RoutedEventHandler(this.mediaElement_MediaOpened);
			this.mediaElement.Source = this.SourceUri;
			this.ccDataLayout = (Grid)base.GetTemplateChild("PART_CCDataLayout");
			this.layoutHostPart = (Border)base.GetTemplateChild("PART_LayoutHost");
			this.onVideoContent = (ContentPresenter)base.GetTemplateChild("PART_OnVideoContentControl");
			Panel templateChild = (Panel)base.GetTemplateChild("PART_ClickListener");
			if (templateChild != null)
			{
				templateChild.MouseDown += new MouseButtonEventHandler(this.clickListener_MouseDown);
			}
			if (base.ContextMenu != null)
			{
				base.ContextMenu.Tag = this;
			}
			if (this.layoutRoot != null)
			{
				if (!this.IsContentFades)
				{
					VisualStateManager.GoToState(this, "STATE_Visible", true);
					return;
				}
				this.timer.Start();
			}
		}

		private void OnCCDataReceived(object sender, CCDataEventArgs e)
		{
			this.ccDataDecoder.ParseData(e.Data);
		}

		private static void OnCcDataVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MediaElementExt mediaElementExt = (MediaElementExt)d;
			if (mediaElementExt != null)
			{
				bool? newValue = (bool?)(e.NewValue as bool?);
				if ((!newValue.GetValueOrDefault() ? false : newValue.HasValue))
				{
					mediaElementExt.ccDataLayout.Visibility = System.Windows.Visibility.Visible;
					return;
				}
				mediaElementExt.ccDataLayout.Visibility = System.Windows.Visibility.Collapsed;
			}
		}

		private static void OnIsContentFadesChanget(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MediaElementExt mediaElementExt = d as MediaElementExt;
			bool newValue = (bool)e.NewValue;
			if (mediaElementExt != null && mediaElementExt.layoutRoot != null)
			{
				if (!newValue)
				{
					mediaElementExt.timer.Stop();
					VisualStateManager.GoToState(mediaElementExt, "STATE_Visible", true);
					return;
				}
				mediaElementExt.timer.Start();
			}
		}

		private static void OnIsNotificationShownChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MediaElementExt mediaElementExt = d as MediaElementExt;
			if (mediaElementExt != null)
			{
				bool? newValue = (bool?)(e.NewValue as bool?);
				if ((!newValue.GetValueOrDefault() ? false : newValue.HasValue))
				{
					VisualStateManager.GoToState(mediaElementExt, "STATE_NotificationVisible", true);
					return;
				}
				VisualStateManager.GoToState(mediaElementExt, "STATE_NotificationNonVisible", true);
			}
		}

		private static void OnIsVideoStartedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MediaElementExt mediaElementExt = (MediaElementExt)d;
			if (mediaElementExt != null)
			{
				bool? newValue = (bool?)(e.NewValue as bool?);
				if ((!newValue.GetValueOrDefault() ? false : newValue.HasValue))
				{
					MediaElementExt.SwitchStateToNonloading(mediaElementExt);
					return;
				}
				if (mediaElementExt.NotificationContent == null)
				{
					MediaElementExt.SwitchStateToLoading(mediaElementExt);
				}
			}
		}

		private static void OnNotificationContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((MediaElementExt)d).OnContentChanged(e.OldValue, e.NewValue);
		}

		private static void OnNotificationTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MediaElementExt mediaElementExt = (MediaElementExt)d;
			mediaElementExt.OnContentChanged(e.OldValue, e.NewValue);
			if (e.NewValue == null)
			{
				MediaElementExt.SwitchStateToLoading(mediaElementExt);
				VisualStateManager.GoToState(mediaElementExt, "STATE_NotificationNonVisible", true);
				mediaElementExt.NotificationContent = null;
				return;
			}
			try
			{
				MessageType messageType = (MessageType)Enum.Parse(typeof(MessageType), e.NewValue.ToString());
				if (messageType != MessageType.AudioVideo && messageType != MessageType.HdmiConflict && messageType != MessageType.NeedRestart)
				{
					VisualStateManager.GoToState(mediaElementExt, "STATE_NotificationVisible", true);
					StackPanel stackPanel = new StackPanel();
					TextBlock textBlock = new TextBlock();
					Image image = new Image();
					if (messageType != MessageType.AudioOnly && messageType != MessageType.HdmiConflict && messageType != MessageType.NeedRestart)
					{
						textBlock.FontSize = 26;
						textBlock.Foreground = new SolidColorBrush(Colors.White);
						textBlock.FontWeight = FontWeights.Bold;
						textBlock.TextWrapping = TextWrapping.WrapWithOverflow;
						textBlock.TextAlignment = TextAlignment.Center;
						stackPanel.Children.Add(textBlock);
						if (messageType == MessageType.HdmiConflict)
						{
							textBlock.Text = ResourcesModel.Instanse.MAPP_SID_TV_CURRENT_STATUS_PREVENTS_SCREEN_SHARING_WINDOW;
						}
						else if (messageType == MessageType.AndroidPriority)
						{
							textBlock.Text = ResourcesModel.Instanse.MAPP_SID_TV_NO_LONGER_AVAILABLE_ANDROID_DEVICE;
						}
						else if (messageType == MessageType.LowPriority)
						{
							textBlock.Text = ResourcesModel.Instanse.MAPP_SID_YOU_CAN_NOT_WATCH_2ND_TV_ANYMORE;
						}
						else if (messageType == MessageType.SourceConflict || messageType == MessageType.OtherMode)
						{
							textBlock.Text = ResourcesModel.Instanse.MAPP_SID_CANNOT_WATCH_TV_MOBILE_DEVICE_CURRENT_TV_MODE;
						}
						else if (messageType == MessageType.NoSignal)
						{
							textBlock.Text = ResourcesModel.Instanse.COM_SID_NO_SIGNAL;
						}
						else if (messageType == MessageType.CheckCable)
						{
							textBlock.Text = ResourcesModel.Instanse.MAPP_SID_CHECK_CABLE;
						}
						else if (messageType == MessageType.DataService)
						{
							textBlock.Text = ResourcesModel.Instanse.COM_TV_SID_DATA_SERVICE_CHANNEL;
						}
						else if (messageType == MessageType.Locked)
						{
							textBlock.Text = ResourcesModel.Instanse.COM_SID_THIS_CHANNEL_LOCKED_MSG;
						}
						else if (messageType == MessageType.NotAvailable)
						{
							textBlock.Text = ResourcesModel.Instanse.COM_SID_NOT_AVAILABLE;
						}
						else if (messageType == MessageType.NotAvailable3D)
						{
							textBlock.Text = ResourcesModel.Instanse.COM_TV_SID_3D_IS_NOT_SUPPORTED;
						}
						else if (messageType == MessageType.NotConnected)
						{
							textBlock.Text = ResourcesModel.Instanse.MAPP_SID_NOT_CONNECTED;
						}
						else if (messageType == MessageType.ParentalLock)
						{
							textBlock.Text = ResourcesModel.Instanse.MAPP_SID_PARENTAL_LOCK;
						}
						else if (messageType == MessageType.ScrambledChannel)
						{
							textBlock.Text = ResourcesModel.Instanse.MAPP_SID_SCRAMLED_CHANNEL;
						}
						else if (messageType == MessageType.ServiceNotAvailable)
						{
							textBlock.Text = ResourcesModel.Instanse.COM_TV_SID_THIS_SERVICE_IS_NOT_AVAILABLE;
						}
						else if (messageType == MessageType.Recording)
						{
							textBlock.Text = ResourcesModel.Instanse.MAPP_SID_NOT_AVAILABLE_TV_RECORIDNG;
						}
						else if (messageType != MessageType.NeedRestart)
						{
							textBlock.Text = "Some error.:'(";
						}
						else
						{
							textBlock.Text = ResourcesModel.Instanse.COM_TV_SID_CLOSING_MULTI_DISPLAY_TV_MSG_KR_NETWORK;
							Button button = new Button()
							{
								Content = ResourcesModel.Instanse.COM_TV_SID_TRY_AGAIN,
								Margin = new Thickness(10),
								HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
								Style = (System.Windows.Style)Application.Current.Resources["TheMostCommonButtonStyle"],
								FontSize = 26,
								Padding = new Thickness(2)
							};
							stackPanel.Children.Add(button);
							Binding binding = new Binding("RestartCommand")
							{
								Mode = BindingMode.OneWay,
								Source = mediaElementExt.DataContext
							};
							button.SetBinding(ButtonBase.CommandProperty, binding);
						}
					}
					else
					{
						BitmapImage bitmapImage = new BitmapImage();
						stackPanel.Children.Add(image);
						bitmapImage.BeginInit();
						bitmapImage.UriSource = new Uri("pack://application:,,,/Resources/Images/2nd_exception_icon.png");
						bitmapImage.EndInit();
						image.Stretch = Stretch.None;
						image.Source = bitmapImage;
					}
					mediaElementExt.NotificationContent = stackPanel;
					MediaElementExt.SwitchStateToNonloading(mediaElementExt);
				}
				else
				{
					VisualStateManager.GoToState(mediaElementExt, "STATE_NotificationNonVisible", true);
				}
			}
			catch
			{
				MediaElementExt.SwitchStateToLoading(mediaElementExt);
			}
		}

		private static void OnPlayerNotificationProvider(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MediaElementExt mediaElementExt = d as MediaElementExt;
			IPlayerNotificationProvider newValue = e.NewValue as IPlayerNotificationProvider;
			if (newValue != null && mediaElementExt != null)
			{
				newValue.CCDataReceived += new EventHandler<CCDataEventArgs>(mediaElementExt.OnCCDataReceived);
			}
			if (e.OldValue != null && mediaElementExt != null)
			{
				((IPlayerNotificationProvider)e.OldValue).CCDataReceived -= new EventHandler<CCDataEventArgs>(mediaElementExt.OnCCDataReceived);
			}
		}

		private static void OnSourceUri(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as MediaElementExt).mediaElement.Source = e.NewValue as Uri;
		}

		public void SwitchFullScreenMode()
		{
			if (this.fullscreenWindow == null)
			{
				this.ContentExchange();
				return;
			}
			this.fullscreenWindow.Close();
		}

		private static async void SwitchStateToLoading(MediaElementExt mediaElementExt)
		{
			mediaElementExt.fadeOutAnimationEnded = false;
			VisualStateManager.GoToState(mediaElementExt, "STATE_Loading", false);
			mediaElementExt.loadingTimer.Start();
			await Task.Delay(20);
			mediaElementExt.fadeOutAnimationEnded = true;
		}

		private static async void SwitchStateToNonloading(MediaElementExt mediaElementExt)
		{
			mediaElementExt.fadeOutAnimationEnded = false;
			VisualStateManager.GoToState(mediaElementExt, "STATE_NonLoading", false);
			mediaElementExt.loadingTimer.Stop();
			await Task.Delay(20);
			mediaElementExt.fadeOutAnimationEnded = true;
		}

		private async void timer_Tick(object sender, EventArgs e)
		{
			this.fadeOutAnimationEnded = false;
			VisualStateManager.GoToState(this, "STATE_Hidden", false);
			this.timer.Stop();
			await Task.Delay(350);
			this.fadeOutAnimationEnded = true;
		}
	}
}