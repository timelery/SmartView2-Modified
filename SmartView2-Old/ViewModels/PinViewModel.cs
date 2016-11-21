using SmartView2;
using SmartView2.Core.Commands;
using SmartView2.Properties;
using SmartView2.ViewModels.Popups;
using SmartView2.Views;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using UIFoundation;
using UIFoundation.Navigation;
using UPnP.DataContracts;

namespace SmartView2.ViewModels
{
	[RelatedView(typeof(PinPage))]
	public class PinViewModel : PopupViewModel
	{
		private const int PinPageTimeOut = 120;

		private const int maxInvalidCount = 5;

		private string[] pinNumbers = new string[4];

		private int invalidPinCount;

		private string pinErrorMessage = string.Empty;

		private ICommand pairCommand;

		private DeviceInfo device;

		private PopupWrapper messagePopup;

		private DispatcherTimer pinTimeOutTimer;

		private readonly DeviceController deviceController;

		private bool isPinFocused;

		public int InvalidPinCount
		{
			get
			{
				return this.invalidPinCount;
			}
			set
			{
				if (this.invalidPinCount != value)
				{
					base.SetProperty<int>(ref this.invalidPinCount, value, "InvalidPinCount");
				}
			}
		}

		public bool IsPinFocused
		{
			get
			{
				return this.isPinFocused;
			}
			set
			{
				base.SetProperty<bool>(ref this.isPinFocused, value, "IsPinFocused");
			}
		}

		public int MaxInvalidCount
		{
			get
			{
				return 5;
			}
		}

		public ICommand PairCommand
		{
			get
			{
				return this.pairCommand;
			}
		}

		public string PinErrorMessage
		{
			get
			{
				return this.pinErrorMessage;
			}
			set
			{
				base.SetProperty<string>(ref this.pinErrorMessage, value, "PinErrorMessage");
				base.OnPropertyChanged(this, "PinErrorVisible");
			}
		}

		public bool PinErrorVisible
		{
			get
			{
				return !string.IsNullOrEmpty(this.pinErrorMessage);
			}
		}

		public string PinNumber1
		{
			get
			{
				return this.pinNumbers[0];
			}
			set
			{
				this.pinNumbers[0] = value;
			}
		}

		public string PinNumber2
		{
			get
			{
				return this.pinNumbers[1];
			}
			set
			{
				this.pinNumbers[1] = value;
			}
		}

		public string PinNumber3
		{
			get
			{
				return this.pinNumbers[2];
			}
			set
			{
				this.pinNumbers[2] = value;
			}
		}

		public string PinNumber4
		{
			get
			{
				return this.pinNumbers[3];
			}
			set
			{
				this.pinNumbers[3] = value;
			}
		}

		public PinViewModel(DeviceController deviceController)
		{
			this.deviceController = deviceController;
			this.pinTimeOutTimer = new DispatcherTimer();
			this.pinTimeOutTimer.Tick += new EventHandler(this.pinTimeOutTimer_Tick);
			this.pairCommand = new Command(new Action<object>(this.OnPair));
			this.InvalidPinCount = 0;
			this.PinErrorMessage = string.Empty;
		}

		public override async void OnNavigateFrom()
		{
            base.OnNavigateFrom();
            try
            {
                await this.deviceController.ClosePinPageOnTV();
            }
            catch
            {
            }
            this.StopTimer();
        }

		public override void OnNavigateTo(object p)
		{
			this.device = (DeviceInfo)p;
			base.IsDataLoaded = true;
			this.InvalidPinCount = 0;
			this.PinErrorMessage = string.Empty;
			this.messagePopup = base.Controller.CreatePopup(new MessagePopupViewModel(), true);
			this.ResetPin();
			this.StartTimer();
			base.OnNavigateTo(p);
		}

		private async void OnPair(object obj)
		{
			string[] strArrays = this.pinNumbers;
			if (!(
				from number in strArrays
				where string.IsNullOrEmpty(number)
				select number).Any<string>())
			{
				try
				{
					try
					{
						base.IsDataLoaded = false;
						string str = string.Concat(this.pinNumbers);
						if (await this.deviceController.SetPin(str))
						{
							base.Controller.ClosePopup(true);
						}
						PinViewModel invalidPinCount = this;
						invalidPinCount.InvalidPinCount = invalidPinCount.InvalidPinCount + 1;
						PinViewModel pinViewModel = this;
						object[] mAPPSIDENTEREDINCORRECTPIN = new object[] { ResourcesModel.Instanse.MAPP_SID_ENTERED_INCORRECT_PIN, this.InvalidPinCount, this.MaxInvalidCount, ResourcesModel.Instanse.COM_TV_SID_PLEASE_TRY_AGAIN };
						pinViewModel.PinErrorMessage = string.Format("{0}({1}/{2})\n{3}", mAPPSIDENTEREDINCORRECTPIN);
						this.ResetPin();
						if (this.InvalidPinCount >= this.MaxInvalidCount)
						{
							this.RestartTimer();
							PopupWrapper popupWrapper = this.messagePopup;
							string[] mAPPSIDPINERROR = new string[] { ResourcesModel.Instanse.MAPP_SID_PIN_ERROR, ResourcesModel.Instanse.MAPP_SID_ENTERED_INCORRECT_PIN_CHECK_PIN_AGAIN };
							await popupWrapper.ShowDialogAsync(mAPPSIDPINERROR);
							base.Controller.ClosePopup(false);
						}
					}
					catch (Exception exception)
					{
						base.Controller.ClosePopup(exception);
					}
				}
				finally
				{
					base.IsDataLoaded = true;
				}
			}
			else
			{
				this.PinErrorMessage = ResourcesModel.Instanse.MAPP_SID_PIN_ENTERD_SHORT_TRY;
			}
		}

		private async void pinTimeOutTimer_Tick(object sender, EventArgs e)
		{
			await this.deviceController.ClosePinPageOnTV();
			this.StopTimer();
			PopupWrapper popupWrapper = this.messagePopup;
			string[] cOMTVSIDTRYAGAIN = new string[] { ResourcesModel.Instanse.COM_TV_SID_TRY_AGAIN, ResourcesModel.Instanse.MAPP_SID_PIN_ENTRY_TIME_LIMIT_EXPIRED };
			await popupWrapper.ShowDialogAsync(cOMTVSIDTRYAGAIN);
			base.Controller.ClosePopup(false);
		}

		private void ResetPin()
		{
			this.pinNumbers = new string[4];
			base.OnPropertyChanged(this, "PinNumber1");
			base.OnPropertyChanged(this, "PinNumber2");
			base.OnPropertyChanged(this, "PinNumber3");
			base.OnPropertyChanged(this, "PinNumber4");
			this.IsPinFocused = false;
			this.IsPinFocused = true;
		}

		private void RestartTimer()
		{
			this.pinTimeOutTimer.Stop();
			this.pinTimeOutTimer.Start();
		}

		private void StartTimer()
		{
			this.pinTimeOutTimer.Interval = TimeSpan.FromSeconds(120);
			this.pinTimeOutTimer.Start();
		}

		private void StopTimer()
		{
			this.pinTimeOutTimer.Stop();
		}
	}
}