using SmartView2;
using SmartView2.Core;
using SmartView2.Core.Commands;
using SmartView2.Properties;
using SmartView2.ViewModels.Popups;
using SmartView2.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UIFoundation;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels
{
	[RelatedView(typeof(WelcomePage))]
	public class WelcomeViewModel : PageViewModel
	{
		private bool IsGameAvaliable;

		private bool connectEnabled = true;

		private DeviceController deviceController;

		private IEnumerable<object> guidItems;

		private ICommand connectCommand;

		private ICommand settingCommand;

		private readonly ICommand guideCommand;

		private readonly ICommand versionCommand;

		private readonly ICommand licenseCommand;

		private PopupWrapper messagePopup;

		private PopupWrapper guidePopup;

		private PopupWrapper versionPopup;

		private PopupWrapper licensePopup;

		public ICommand ConnectCommand
		{
			get
			{
				return this.connectCommand;
			}
		}

		public bool ConnectEnabled
		{
			get
			{
				return this.connectEnabled;
			}
			set
			{
				base.SetProperty<bool>(ref this.connectEnabled, value, "ConnectEnabled");
			}
		}

		public ICommand GuideCommand
		{
			get
			{
				return this.guideCommand;
			}
		}

		public IEnumerable<object> GuidItems
		{
			get
			{
				return this.guidItems;
			}
			set
			{
				base.SetProperty<IEnumerable<object>>(ref this.guidItems, value, "GuidItems");
			}
		}

		public ICommand LicenseCommand
		{
			get
			{
				return this.licenseCommand;
			}
		}

		public ICommand SettingCommand
		{
			get
			{
				return this.settingCommand;
			}
		}

		public ICommand VersionCommand
		{
			get
			{
				return this.versionCommand;
			}
		}

		public WelcomeViewModel(DeviceController deviceController)
		{
			this.deviceController = deviceController;
			this.settingCommand = new Command(new Action<object>(this.OnSettingCommand));
			this.connectCommand = new Command(new Action<object>(this.OnConnect));
			this.guideCommand = new Command(new Action<object>(this.OnGuide));
			this.versionCommand = new Command(new Action<object>(this.OnVersion));
			this.licenseCommand = new Command(new Action<object>(this.OnLicense));
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
		}

		private void CloseVersion()
		{
			if (this.versionPopup != null)
			{
				this.versionPopup.Close();
			}
		}

		private async void OnConnect(object obj)
		{
			if (NetworkInterface.GetIsNetworkAvailable())
			{
				base.IsDataLoaded = false;
				if (!await this.deviceController.TryToConnect(""))
				{
					await base.Controller.Navigate(new DeviceListViewModel(this.deviceController), false);
				}
				else
				{
					await base.Controller.Navigate(new MainViewModel(this.deviceController), false);
					await base.Controller.Push(new DeviceListViewModel(this.deviceController));
				}
				base.IsDataLoaded = true;
			}
			else
			{
				PopupWrapper popupWrapper = this.messagePopup;
				string[] mAPPSIDNOWIFICONNECTION = new string[] { ResourcesModel.Instanse.MAPP_SID_NO_WIFI_CONNECTION, ResourcesModel.Instanse.MAPP_SID_APP_REQUIRES_ACTIVE_WIFI_CONNTECTION_NOW };
				popupWrapper.Show(mAPPSIDNOWIFICONNECTION);
			}
		}

		public override async Task OnCreateAsync()
		{
			this.messagePopup = base.Controller.CreatePopup(new MessagePopupViewModel(), false);
			this.guidePopup = base.Controller.CreatePopup(new GuideViewModel(), true);
			this.versionPopup = base.Controller.CreatePopup(new VersionViewModel(), true);
			this.licensePopup = base.Controller.CreatePopup(new LicenseViewModel(), true);
		}

		public override async Task OnDestroyAsync()
		{
			Logger.Instance.LogMessageFormat("[SmartView2][OnDestroyAsync]OnDestroyAsync started... ", new object[0]);
			if (this.deviceController != null)
			{
				await this.deviceController.DisconnectAsync(false);
			}
		}

		private void OnGuide(object obj)
		{
			this.guidePopup.Show();
		}

		private void OnLicense(object obj)
		{
			this.licensePopup.Show();
		}

		public override async Task OnNavigateFromAsync()
		{
			this.CloseMenuPopups();
		}

		public override async Task OnNavigateToAsync()
		{
			List<object> objs = new List<object>()
			{
				new { Description = ResourcesModel.Instanse.MAPP_SID_ENJOY_MULTIMEDIA_MUSIC_PC_TV, Image = new BitmapImage(new Uri("/Resources/Images/guide_01.png", UriKind.Relative)) }
			};
			if (this.IsGameAvaliable)
			{
				objs.Add(new { Description = "You can enjoy the game that installed on the Smart Hub", Image = new BitmapImage(new Uri("/Resources/Images/guide_02_l.png", UriKind.Relative)) });
			}
			objs.Add(new { Description = ResourcesModel.Instanse.MAPP_SID_USE_PC_REMOTE_SECOND_TV, Image = new BitmapImage(new Uri("/Resources/Images/guide_02.png", UriKind.Relative)) });
			this.GuidItems = objs;
		}

		private void OnSettingCommand(object obj)
		{
		}

		private void OnVersion(object obj)
		{
			this.versionPopup.Show();
		}
	}
}