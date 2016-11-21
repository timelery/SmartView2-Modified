using SmartView2;
using SmartView2.Core;
using SmartView2.Core.Commands;
using SmartView2.Properties;
using SmartView2.ViewModels.Popups;
using SmartView2.Views;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels
{
	[RelatedView(typeof(RemoteControlPage))]
	public class RemoteControlViewModel : PageViewModel
	{
		private PopupWrapper messagePopup;

		private PopupWrapper textInputPopup;

		private string lastText;

		private DeviceController deviceController;

		private IPageController parentController;

		public ICommand SetSourceCommand
		{
			get;
			private set;
		}

		public ICommand ShowKeypadCommand
		{
			get;
			private set;
		}

		public ITargetDevice TargetDevice
		{
			get;
			private set;
		}

		public ICommand TvPowerCommand
		{
			get;
			private set;
		}

		public RemoteControlViewModel(ITargetDevice targetDevice, DeviceController deviceController, IPageController parentController)
		{
			this.TargetDevice = targetDevice;
			this.deviceController = deviceController;
			this.parentController = parentController;
			this.SetSourceCommand = new Command(new Action<object>(this.OnSetSource));
			this.TvPowerCommand = new Command(new Action<object>(this.OnTvPower));
			this.ShowKeypadCommand = new Command((object arg) => {
				this.lastText = string.Empty;
				this.OnShowTextInput();
			});
		}

		public override async Task OnNavigateFromAsync()
		{
			this.TargetDevice.ErrorMessageArised -= new EventHandler<ErrorMessageEventArgs>(this.TargetDevice_ErrorMessageArised);
			this.TargetDevice.ShowInputKeyboard -= new EventHandler<EventArgs>(this.TargetDevice_ShowInputKeyboard);
			this.TargetDevice.ShowPasswordKeyboard -= new EventHandler<EventArgs>(this.TargetDevice_ShowPasswordKeyboard);
			this.TargetDevice.TextUpdated -= new EventHandler<UpdateTextEventArgs>(this.TargetDevice_TextUpdated);
			this.TargetDevice.HideKeyboard -= new EventHandler<EventArgs>(this.TargetDevice_HideKeyboard);
			this.messagePopup.Dispose();
			this.textInputPopup.Dispose();
		}

		public override async Task OnNavigateToAsync()
		{
			//await this.<>n__FabricatedMethod4();
			this.messagePopup = base.Controller.CreatePopup(new VideoErrorPopupViewModel(), false);
			this.textInputPopup = base.Controller.CreatePopup(new TextInputPopupViewModel(this.TargetDevice), false);
			this.TargetDevice.ShowInputKeyboard += new EventHandler<EventArgs>(this.TargetDevice_ShowInputKeyboard);
			this.TargetDevice.ShowPasswordKeyboard += new EventHandler<EventArgs>(this.TargetDevice_ShowPasswordKeyboard);
			this.TargetDevice.HideKeyboard += new EventHandler<EventArgs>(this.TargetDevice_HideKeyboard);
			this.TargetDevice.TextUpdated += new EventHandler<UpdateTextEventArgs>(this.TargetDevice_TextUpdated);
			this.TargetDevice.ErrorMessageArised += new EventHandler<ErrorMessageEventArgs>(this.TargetDevice_ErrorMessageArised);
		}

		private void OnSetSource(object obj)
		{
			if (obj != null && obj is Source)
			{
				this.TargetDevice.SetSourceAsync(obj as Source);
			}
		}

		private void OnShowTextInput()
		{
			this.textInputPopup.Show(this.lastText);
		}

		private async void OnTvPower(object obj)
		{
			using (PopupWrapper popupWrapper = base.Controller.CreatePopup(new YesNoPopupViewModel(ResourcesModel.Instanse.COM_TV_SID_CMD_POWER_OFF, ResourcesModel.Instanse.MAPP_SID_TURN_OFF_TV_APP_ALSO_CLOSE), false))
			{
				AlternativePopupEventArgs alternativePopupEventArg = await popupWrapper.ShowDialogAsync() as AlternativePopupEventArgs;
				if (alternativePopupEventArg != null)
				{
					bool? decision = alternativePopupEventArg.Decision;
					if ((!decision.GetValueOrDefault() ? false : decision.HasValue))
					{
						this.TargetDevice.CurrentSource.RemoteControl.Power.ExecuteIfYouCan(obj);
						await this.deviceController.DisconnectAsync(true);
						this.deviceController.RefreshDiscovery();
						await this.parentController.GoBack(typeof(WelcomeViewModel));
					}
				}
			}
		}

		private void TargetDevice_ErrorMessageArised(object sender, ErrorMessageEventArgs e)
		{
			this.messagePopup.Show(e.MessageType);
		}

		private void TargetDevice_HideKeyboard(object sender, EventArgs e)
		{
			this.textInputPopup.Close();
		}

		private void TargetDevice_ShowInputKeyboard(object sender, EventArgs e)
		{
			this.OnShowTextInput();
		}

		private void TargetDevice_ShowPasswordKeyboard(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private void TargetDevice_TextUpdated(object sender, UpdateTextEventArgs e)
		{
			this.lastText = e.Text;
		}
	}
}