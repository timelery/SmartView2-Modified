using SmartView2;
using SmartView2.Core;
using SmartView2.Core.Commands;
using SmartView2.Devices;
using SmartView2.Devices.DataContracts;
using SmartView2.Devices.SecondTv;
using SmartView2.Properties;
using SmartView2.ViewModels.Popups;
using SmartView2.Views;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using UIFoundation;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels
{
	[RelatedView(typeof(TvVideoPage))]
	public class TvVideoViewModel : PageViewModel
	{
		private bool isChannelListVisiable;

		private ICommand channelListCommand;

		private ICommand selectChannelCommand;

		private ICommand channelUpCommand;

		private ICommand channelDownCommand;

		private ICommand selectSourceCommand;

		private ICommand sendDeviceViewToTvCommand;

		private ICommand sendTvViewToDeviceCommand;

		private ICommand ccDataCommand;

		private ICommand restartCommand;

		private PopupWrapper messagePopup;

		private string errorMessage;

		private double volume;

		private bool isCcDataVisible;

		private DeviceController deviceController;

		private bool isCcDataButtonPressed;

		private bool isFirstChannelChange = true;

		public ICommand CcDataCommand
		{
			get
			{
				return this.ccDataCommand;
			}
		}

		public ICommand ChannelDownCommand
		{
			get
			{
				return this.channelDownCommand;
			}
		}

		public ICommand ChannelListCommand
		{
			get
			{
				return this.channelListCommand;
			}
		}

		public ICommand ChannelUpCommand
		{
			get
			{
				return this.channelUpCommand;
			}
		}

		public string ErrorMessage
		{
			get
			{
				return this.errorMessage;
			}
			private set
			{
				base.SetProperty<string>(ref this.errorMessage, value, "ErrorMessage");
			}
		}

		public bool IsCcDataButtonPressed
		{
			get
			{
				return this.isCcDataButtonPressed;
			}
			set
			{
				base.SetProperty<bool>(ref this.isCcDataButtonPressed, value, "IsCcDataButtonPressed");
				base.OnPropertyChanged(this, "IsCcDataVisible");
			}
		}

		public bool IsCcDataEnabled
		{
			get
			{
				return Settings.Default.ClosedCaption;
			}
		}

		public bool IsCcDataVisible
		{
			get
			{
				if (this.TargetDevice.IsCloneView || !this.IsCcDataButtonPressed)
				{
					return false;
				}
				return this.IsCcDataEnabled;
			}
		}

		public bool IsChannelListVisiable
		{
			get
			{
				return this.isChannelListVisiable;
			}
			set
			{
				base.SetProperty<bool>(ref this.isChannelListVisiable, value, "IsChannelListVisiable");
			}
		}

		public bool IsDebuggerAttached
		{
			get
			{
				return Debugger.IsAttached;
			}
		}

		public IPlayerNotificationProvider PlayerNotification
		{
			get
			{
				return this.deviceController.NotificationProvider;
			}
		}

		public ICommand RestartCommand
		{
			get
			{
				return this.restartCommand;
			}
		}

		public ICommand SelectChannelCommand
		{
			get
			{
				return this.selectChannelCommand;
			}
		}

		public ICommand SelectSourceCommand
		{
			get
			{
				return this.selectSourceCommand;
			}
		}

		public ICommand SendDeviceViewToTvCommand
		{
			get
			{
				return this.sendDeviceViewToTvCommand;
			}
		}

		public ICommand SendTvViewToDeviceCommand
		{
			get
			{
				return this.sendTvViewToDeviceCommand;
			}
		}

		public ITargetDevice TargetDevice
		{
			get;
			private set;
		}

		public double Volume
		{
			get
			{
				return this.volume;
			}
			set
			{
				base.SetProperty<double>(ref this.volume, value, "Volume");
			}
		}

		public TvVideoViewModel(DeviceController deviceController, ITargetDevice targetDevice)
		{
			this.TargetDevice = targetDevice;
			this.deviceController = deviceController;
			this.channelListCommand = new Command(new Action<object>(this.OnShowChannelList));
			this.selectChannelCommand = new Command(new Action<object>(this.OnSetChannel));
			this.channelUpCommand = new Command(new Action<object>(this.OnChannelUp));
			this.channelDownCommand = new Command(new Action<object>(this.OnChannelDown));
			this.sendDeviceViewToTvCommand = new Command(new Action<object>(this.OnSendDeviceViewToTv));
			this.sendTvViewToDeviceCommand = new Command(new Action<object>(this.OnSendTvViewToDevice));
			this.selectSourceCommand = new UICommand(new Action<object>(this.OnSetSource), new Predicate<object>(this.CanSetSource));
			this.ccDataCommand = new Command(new Action<object>(this.OnCcDataClick));
			this.restartCommand = new Command(new Action<object>(this.OnRestart));
		}

		private bool CanSetSource(object obj)
		{
			if (obj == null || !(obj is Source))
			{
				return false;
			}
			if (!((Source)obj).Type.ToString().Contains("HDMI"))
			{
				return true;
			}
			Source currentSource = this.TargetDevice.CurrentSource;
			return false;
		}

		public void CcDataChanged()
		{
			base.OnPropertyChanged(this, "IsCcDataEnabled");
			base.OnPropertyChanged(this, "IsCcDataVisible");
		}

		private void OnCcDataClick(object obj)
		{
			this.IsCcDataButtonPressed = !this.IsCcDataButtonPressed;
		}

		private async void OnChannelDown(object obj)
		{
			bool flag = false;
			if (!this.isFirstChannelChange)
			{
				try
				{
					await this.TargetDevice.ChannelDownAsync();
				}
				catch (SecondTvException secondTvException)
				{
					if (secondTvException.ErrorType == ErrorCode.NOTOK_Recording)
					{
						flag = true;
					}
				}
				if (flag)
				{
					if (await this.OnRecErrorAsync())
					{
						await this.TargetDevice.ChannelDownAsync();
					}
				}
			}
			else if (!await this.showTVChannelChangePopup())
			{
				this.isFirstChannelChange = false;
			}
			else
			{
				this.isFirstChannelChange = false;
				this.OnChannelDown(obj);
			}
		}

		private async void OnChannelUp(object obj)
		{
			bool flag = false;
			if (!this.isFirstChannelChange)
			{
				try
				{
					await this.TargetDevice.ChannelUpAsync();
				}
				catch (SecondTvException secondTvException)
				{
					if (secondTvException.ErrorType == ErrorCode.NOTOK_Recording)
					{
						flag = true;
					}
				}
				if (flag)
				{
					if (await this.OnRecErrorAsync())
					{
						await this.TargetDevice.ChannelUpAsync();
					}
				}
			}
			else if (await this.showTVChannelChangePopup())
			{
				this.isFirstChannelChange = false;
				this.OnChannelUp(obj);
			}
		}

		public override async Task OnNavigateFromAsync()
		{
			Settings.Default.VolumeTv = this.Volume;
			Settings.Default.CcDataVisible = this.IsCcDataButtonPressed;
			Settings.Default.Save();
			this.TargetDevice.ErrorMessageArised -= new EventHandler<ErrorMessageEventArgs>(this.TvVideoViewModel_ErrorMessageArised);
			this.TargetDevice.ViewStarted -= new EventHandler(this.TvVideoViewModel_ViewStarted);
			await this.TargetDevice.SetRemoteControlStateAsync(true);
			this.messagePopup.Dispose();
		}

		public override async Task OnNavigateToAsync()
		{
			this.Volume = Settings.Default.VolumeTv;
			this.IsCcDataButtonPressed = Settings.Default.CcDataVisible;
			this.messagePopup = base.Controller.CreatePopup(new MessagePopupViewModel(), false);
			this.TargetDevice.ErrorMessageArised += new EventHandler<ErrorMessageEventArgs>(this.TvVideoViewModel_ErrorMessageArised);
			this.TargetDevice.ViewStarted += new EventHandler(this.TvVideoViewModel_ViewStarted);
			await this.TargetDevice.SetRemoteControlStateAsync(false);
		}

		private async Task<bool> OnRecErrorAsync()
		{
			bool flag;
			OkCancelPopupViewModel okCancelPopupViewModel = new OkCancelPopupViewModel(ResourcesModel.Instanse.COM_SID_STOP_RECORDING, ResourcesModel.Instanse.MAPP_SID_TV_CURRENTLY_RECORDING_BEFORE_CHANGE_SOURCE, true);
			PopupWrapper popupWrapper = base.Controller.CreatePopup(okCancelPopupViewModel, false);
			AlternativePopupEventArgs alternativePopupEventArg = await popupWrapper.ShowDialogAsync() as AlternativePopupEventArgs;
			Settings.Default.ExitDoNotShowAgain = okCancelPopupViewModel.CheckBoxState;
			Settings.Default.Save();
			if (alternativePopupEventArg != null)
			{
				bool? decision = alternativePopupEventArg.Decision;
				if ((!decision.GetValueOrDefault() ? false : decision.HasValue))
				{
					bool flag1 = await this.TargetDevice.StopRecordAsync();
					if (!flag1)
					{
						flag = flag1;
						return flag;
					}
					else
					{
						base.Controller.ShowMessage("Recording stopped");
						flag = flag1;
						return flag;
					}
				}
			}
			await this.TargetDevice.RestartView();
			flag = false;
			return flag;
		}

		private async void OnRestart(object obj)
		{
			await this.TargetDevice.RestartView();
		}

		private async void OnSendDeviceViewToTv(object obj)
		{
			await this.TargetDevice.SendDeviceViewToTv();
		}

		private async void OnSendTvViewToDevice(object obj)
		{
			await this.TargetDevice.SendTvViewToDevice();
		}

		private async void OnSetChannel(object obj)
		{
			bool flag = false;
			if (!this.isFirstChannelChange)
			{
				try
				{
					if (obj != null && obj is Channel)
					{
						await this.TargetDevice.SetChannelAsync(obj as Channel);
					}
				}
				catch (SecondTvException secondTvException)
				{
					if (secondTvException.ErrorType == ErrorCode.NOTOK_Recording)
					{
						flag = true;
					}
				}
				if (flag && obj != null && obj is Channel)
				{
					Channel channel = obj as Channel;
					if (await this.OnRecErrorAsync())
					{
						await this.TargetDevice.SetChannelAsync(channel);
					}
				}
			}
			else if (await this.showTVChannelChangePopup())
			{
				this.isFirstChannelChange = false;
				this.OnSetChannel(obj);
			}
		}

		private async void OnSetSource(object obj)
		{
			Logger.Instance.LogMessage("[SmartView2][TvVideoViewModel]OnSetSource started...");
			if (obj != null && obj is Source)
			{
				Source source = obj as Source;
				Logger.Instance.LogMessage("[SmartView2][TvVideoViewModel]OnSetSource SetSourceAsync");
				try
				{
					await this.TargetDevice.SetSourceAsync(source);
				}
				catch (SecondTvException secondTvException)
				{
					if (secondTvException.ErrorType == ErrorCode.NOTOK_Recording)
					{
						this.TargetDevice.ShowErrorMessage(MessageType.Recording, false);
					}
				}
			}
			Logger.Instance.LogMessage("[SmartView2][TvVideoViewModel]OnSetSource ended...");
		}

		private void OnShowChannelList(object obj)
		{
			this.IsChannelListVisiable = !this.isChannelListVisiable;
		}

		private async Task<bool> showTVChannelChangePopup()
		{
			bool flag;
			if (!Settings.Default.TVChannelNoShow)
			{
				YesNoPopupViewModel yesNoPopupViewModel = new YesNoPopupViewModel(ResourcesModel.Instanse.MAPP_SID_TV_CHANNEL_CHANGE, ResourcesModel.Instanse.MAPP_SID_CHANGE_CHANNLE_PC_CONTINUTE, true);
				PopupWrapper popupWrapper = base.Controller.CreatePopup(yesNoPopupViewModel, false);
				AlternativePopupEventArgs alternativePopupEventArg = await popupWrapper.ShowDialogAsync() as AlternativePopupEventArgs;
				Settings.Default.TVChannelNoShow = yesNoPopupViewModel.CheckBoxState;
				Settings.Default.Save();
				if (alternativePopupEventArg != null)
				{
					bool? decision = alternativePopupEventArg.Decision;
					if ((!decision.GetValueOrDefault() ? false : decision.HasValue))
					{
						flag = true;
						return flag;
					}
				}
				flag = false;
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		private void TvVideoViewModel_ErrorMessageArised(object sender, ErrorMessageEventArgs e)
		{
			// 
			// Current member / type: System.Void SmartView2.ViewModels.TvVideoViewModel::TvVideoViewModel_ErrorMessageArised(System.Object,SmartView2.Core.ErrorMessageEventArgs)
			// File path: C:\Program Files (x86)\SmartView2\Smart View 2.0.exe
			// 
			// Product version: 2016.1.316.0
			// Exception in: System.Void TvVideoViewModel_ErrorMessageArised(System.Object,SmartView2.Core.ErrorMessageEventArgs)
			// 
			// The given key was not present in the dictionary.
			//    at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
			//    at ..() in c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\GotoElimination\GotoCancelation.cs:line 61
			//    at ..() in c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\GotoElimination\GotoCancelation.cs:line 35
			//    at ..( ,  ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\GotoElimination\GotoCancelation.cs:line 26
			//    at ..(MethodBody ,  , ILanguage ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:line 88
			//    at ..(MethodBody , ILanguage ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:line 70
			//    at ..( , ILanguage , MethodBody , & ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs:line 99
			//    at ..(MethodBody , ILanguage , & ,  ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs:line 62
			//    at ..(ILanguage , MethodDefinition ,  ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\WriterContextServices\BaseWriterContextService.cs:line 117
			// 
			// mailto: JustDecompilePublicFeedback@telerik.com

		}

		private async void TvVideoViewModel_ViewStarted(object sender, EventArgs e)
		{
			if (this.TargetDevice.CurrentSource.Type != SourceType.TV && !this.TargetDevice.CurrentSource.IsMbr && this.messagePopup != null && !Settings.Default.UniversalRemoteSetupInfoDoNotShow)
			{
				Logger.Instance.LogMessage("[SmartView2][TvVideoViewModel]TvVideoViewModel_ViewStarted");
				PopupWrapper popupWrapper = this.messagePopup;
				object[] mAPPSIDUNIVERSALREMOTENOTSETUP = new object[] { ResourcesModel.Instanse.MAPP_SID_UNIVERSAL_REMOTE_NOT_SET_UP, ResourcesModel.Instanse.MAPP_SID_NOT_SET_UP_UNIVERSAL_REMOTE_YET_CONTROL_TV, true };
				bool? nullable = (bool?)(await popupWrapper.ShowDialogAsync(mAPPSIDUNIVERSALREMOTENOTSETUP) as bool?);
				if (nullable.HasValue)
				{
					Settings.Default.UniversalRemoteSetupInfoDoNotShow = nullable.Value;
				}
			}
		}
	}
}