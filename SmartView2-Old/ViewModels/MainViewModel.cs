using SmartView2.Core;
using SmartView2.Core.Commands;
using SmartView2.Properties;
using SmartView2.ViewModels.MultimediaInner.VisualState;
using SmartView2.ViewModels.Popups;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using UIFoundation;
using UIFoundation.Navigation;
using SmartView2.Views;

namespace SmartView2.ViewModels
{
	[RelatedView(typeof(MainPage))]
	public class MainViewModel : PageViewModel
	{
		private const string searchMask = "Supported Files|*.MP3;*.FLAC;*.MP4;*.AVI;*.WMV;*.MPEG;*.BMP;*.JPG;*.JPEG;*.PNG";

		private IDataLibrary dataLibrary;

		private PopupWrapper pinPopup;

		private PopupWrapper messagePopup;

		private DeviceController deviceController;

		private readonly ICommand guideCommand;

		private readonly ICommand versionCommand;

		private readonly ICommand licenseCommand;

		private readonly ICommand slideshowSettingsCommand;

		private readonly ICommand ccSettingsCommand;

		private readonly ICommand changeTVCommand;

		private readonly ICommand remoteControlCommand;

		private readonly ICommand multimediaCommand;

		private readonly ICommand tvVideoCommand;

		private IPageController innerController;

		private ITargetDevice targetDevice;

		private SavedMultimediaVisualState multimediaVisualState;

		private PopupWrapper guidePopup;

		private PopupWrapper versionPopup;

		private PopupWrapper licensePopup;

		private PopupWrapper slideshowSettingsPopup;

		private PopupWrapper ccSettingsPopup;

		private PopupWrapper changeTVPopup;

		private TvVideoViewModel tvViewModel;

		public ICommand CCSettingsCommand
		{
			get
			{
				return this.ccSettingsCommand;
			}
		}

		public ICommand ChangeTVCommand
		{
			get
			{
				return this.changeTVCommand;
			}
		}

		public ICommand GuideCommand
		{
			get
			{
				return this.guideCommand;
			}
		}

		public IPageController InnerController
		{
			get
			{
				return this.innerController;
			}
			private set
			{
				base.SetProperty<IPageController>(ref this.innerController, value, "InnerController");
			}
		}

		public bool IsAvailableCCData
		{
			get
			{
				return this.targetDevice.IsUSABoard;
			}
		}

		public ICommand LicenseCommand
		{
			get
			{
				return this.licenseCommand;
			}
		}

		public ICommand MultimediaCommand
		{
			get
			{
				return this.multimediaCommand;
			}
		}

		public ICommand RemoteControlCommand
		{
			get
			{
				return this.remoteControlCommand;
			}
		}

		public ICommand SlideshowSettingsCommand
		{
			get
			{
				return this.slideshowSettingsCommand;
			}
		}

		public ITargetDevice TargetDevice
		{
			get
			{
				return this.targetDevice;
			}
			private set
			{
				base.SetProperty<ITargetDevice>(ref this.targetDevice, value, "TargetDevice");
			}
		}

		public ICommand TvVideoCommand
		{
			get
			{
				return this.tvVideoCommand;
			}
		}

		public ICommand VersionCommand
		{
			get
			{
				return this.versionCommand;
			}
		}

		public MainViewModel(DeviceController deviceController)
		{
			if (deviceController == null)
			{
				throw new ArgumentNullException("deviceController");
			}
			this.deviceController = deviceController;
			this.deviceController.CurrentDeviceDisconnected += new EventHandler<EventArgs>(this.deviceController_CurrentDeviceDisconnected);
			this.TargetDevice = this.deviceController.CurrentDevice;
			this.dataLibrary = this.TargetDevice.DataLibrary;
			this.remoteControlCommand = new Command(new Action<object>(this.OnRemoteControl));
			this.multimediaCommand = new Command(new Action<object>(this.OnMultimedia));
			this.tvVideoCommand = new Command(new Action<object>(this.OnTvVideo));
			this.guideCommand = new Command(new Action<object>(this.OnGuide));
			this.versionCommand = new Command(new Action<object>(this.OnVersion));
			this.licenseCommand = new Command(new Action<object>(this.OnLicense));
			this.slideshowSettingsCommand = new Command(new Action<object>(this.OnSlideshowSettings));
			this.ccSettingsCommand = new Command(new Action<object>(this.OnCCSettings));
			this.changeTVCommand = new Command(new Action<object>(this.OnChangeTV));
			this.dataLibrary.ContentAlreadyExist += new EventHandler(this.dataLibrary_ContentAlreadyExist);
		}

		private void ccSettingsPopup_PopupClosed(object sender, PopupClosedEventArgs e)
		{
			this.tvViewModel.CcDataChanged();
		}

		private void CloseCCSettings()
		{
			if (this.ccSettingsPopup != null)
			{
				this.ccSettingsPopup.Close();
			}
		}

		private void CloseChangeTV()
		{
			if (this.changeTVPopup != null)
			{
				this.changeTVPopup.Close();
			}
		}

		private void CloseGuide()
		{
			if (this.guidePopup != null)
			{
				this.guidePopup.Close();
			}
		}

		private void CloseLicense()
		{
			if (this.licensePopup != null)
			{
				this.licensePopup.Close();
			}
		}

		private void CloseMenuPopups()
		{
			this.CloseGuide();
			this.CloseVersion();
			this.CloseLicense();
			this.CloseSlideshowSettings();
			this.CloseCCSettings();
			this.CloseChangeTV();
		}

		private void CloseSlideshowSettings()
		{
			if (this.slideshowSettingsPopup != null)
			{
				this.slideshowSettingsPopup.Close();
			}
		}

		private void CloseVersion()
		{
			if (this.versionPopup != null)
			{
				this.versionPopup.Close();
			}
		}

		private void dataLibrary_ContentAlreadyExist(object sender, EventArgs e)
		{
			base.Controller.ShowMessage(ResourcesModel.Instanse.MAPP_SID_CONTENT_ALREADY_EXISTS_IN_LIBRARY);
		}

		private async void deviceController_CurrentDeviceDisconnected(object sender, EventArgs e)
		{
			Logger.Instance.LogMessageFormat("[SmartView2][MainViewModel]deviceController_CurrentDeviceDisconnected started... ", new object[0]);
			base.IsDataLoaded = false;
			if (base.Controller != null)
			{
				bool flag = this.deviceController == null;
				Logger.Instance.LogMessageFormat(string.Concat("[SmartView2][MainViewModel]deviceController_CurrentDeviceDisconnected this.deviceController isNull: ", flag.ToString()), new object[0]);
				await this.deviceController.DisconnectAsync(true);
				IDispatcher dispatcher = base.Controller.Dispatcher;
				await dispatcher.Invoke<Task>(async () => await this.messagePopup.ShowDialogAsync(new string[] { ResourcesModel.Instanse.MAPP_SID_DEVICE_CONNECTION_INFO, ResourcesModel.Instanse.MAPP_SID_CONNECTION_WITH_DEVICE_LOST }));
				await base.Controller.GoBack(typeof(WelcomeViewModel));
			}
			base.IsDataLoaded = true;
		}

		private void OnCCSettings(object obj)
		{
			this.ccSettingsPopup.Show();
		}

		private void OnChangeTV(object obj)
		{
			PopupWrapper popupWrapper = this.changeTVPopup;
			object[] controller = new object[] { this.deviceController, base.Controller, this.InnerController };
			popupWrapper.Show(controller);
		}

		public override async Task OnCreateAsync()
		{
			//await this.<>n__FabricatedMethod2();
			this.pinPopup = base.Controller.CreatePopup(new PinViewModel(this.deviceController), false);
			this.tvViewModel = new TvVideoViewModel(this.deviceController, this.TargetDevice);
			this.InnerController = new UIFoundation.Navigation.Controller(this.tvViewModel, base.Controller);
			this.messagePopup = base.Controller.CreatePopup(new MessagePopupViewModel(), false);
			this.slideshowSettingsPopup = base.Controller.CreatePopup(new QueueSettingsViewModel(this.TargetDevice.MultiScreen), false);
			this.slideshowSettingsPopup.PopupClosed += new EventHandler<PopupClosedEventArgs>(this.slideshowSettingsPopup_PopupClosed);
			this.guidePopup = base.Controller.CreatePopup(new GuideViewModel(), true);
			this.versionPopup = base.Controller.CreatePopup(new VersionViewModel(), true);
			this.licensePopup = base.Controller.CreatePopup(new LicenseViewModel(), true);
			this.ccSettingsPopup = base.Controller.CreatePopup(new CCSettingsViewModel(), true);
			this.ccSettingsPopup.PopupClosed += new EventHandler<PopupClosedEventArgs>(this.ccSettingsPopup_PopupClosed);
			this.changeTVPopup = base.Controller.CreatePopup(new ChangeTVViewModel(), true);
		}

		public override async Task OnDestroyAsync()
		{
			//await this.<>n__FabricatedMethode();
			this.deviceController.CurrentDeviceDisconnected -= new EventHandler<EventArgs>(this.deviceController_CurrentDeviceDisconnected);
			this.slideshowSettingsPopup.PopupClosed -= new EventHandler<PopupClosedEventArgs>(this.slideshowSettingsPopup_PopupClosed);
			this.ccSettingsPopup.PopupClosed -= new EventHandler<PopupClosedEventArgs>(this.ccSettingsPopup_PopupClosed);
			await this.InnerController.Dispose();
		}

		private void OnGuide(object obj)
		{
			this.guidePopup.Show();
		}

		private void OnLicense(object obj)
		{
			this.licensePopup.Show();
		}

		private async void OnMultimedia(object obj)
		{
			if (!(this.InnerController.CurrentVM is MultimediaViewModel))
			{
				await this.TargetDevice.InitializeMultiScreenAsync();
				await this.InnerController.Navigate(new MultimediaViewModel(this.multimediaVisualState, this.TargetDevice.MultiScreen, this.dataLibrary), true);
			}
		}

		public override async Task OnNavigateFromAsync()
		{
			if (this.dataLibrary != null)
			{
				this.dataLibrary.ContentAlreadyExist -= new EventHandler(this.dataLibrary_ContentAlreadyExist);
			}
			if (this.deviceController != null)
			{
				this.CloseMenuPopups();
				//await this.<>n__FabricatedMethoda();
			}
		}

		public override async Task OnNavigateToAsync()
		{
			//await this.<>n__FabricatedMethod6();
		}

		private void OnRemoteControl(object obj)
		{
			if (this.InnerController.CurrentVM is MultimediaViewModel)
			{
				this.multimediaVisualState = (this.InnerController.CurrentVM as MultimediaViewModel).GetVisualState();
			}
			if (!(this.InnerController.CurrentVM is RemoteControlViewModel))
			{
				this.InnerController.Navigate(new RemoteControlViewModel(this.TargetDevice, this.deviceController, base.Controller), true);
			}
		}

		private void OnSlideshowSettings(object obj)
		{
			PopupWrapper popupWrapper = this.slideshowSettingsPopup;
			SlideShowSettingsModel slideShowSettingsModel = new SlideShowSettingsModel()
			{
				Speed = Settings.Default.QueueSpeed,
				Effect = Settings.Default.QueueEffect
			};
			popupWrapper.Show(slideShowSettingsModel);
		}

		private void OnTvVideo(object obj)
		{
			if (this.InnerController.CurrentVM is MultimediaViewModel)
			{
				this.multimediaVisualState = (this.InnerController.CurrentVM as MultimediaViewModel).GetVisualState();
			}
			if (!(this.InnerController.CurrentVM is TvVideoViewModel))
			{
				this.InnerController.Navigate(new TvVideoViewModel(this.deviceController, this.TargetDevice), true);
			}
		}

		private void OnVersion(object obj)
		{
			this.versionPopup.Show();
		}

		public void ShowGuide()
		{
			this.guidePopup.Show();
		}

		private void slideshowSettingsPopup_PopupClosed(object sender, PopupClosedEventArgs e)
		{
			SlideShowSettingsModel result = e.Result as SlideShowSettingsModel;
			if (result != null)
			{
				Settings.Default.QueueSpeed = result.Speed;
				Settings.Default.QueueEffect = result.Effect;
				Settings.Default.Save();
			}
		}
	}
}