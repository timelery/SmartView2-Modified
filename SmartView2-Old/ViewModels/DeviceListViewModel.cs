using SmartView2;
using SmartView2.Core;
using SmartView2.Core.Commands;
using SmartView2.Native.Wlan;
using SmartView2.Properties;
using SmartView2.ViewModels.Popups;
using SmartView2.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using UIFoundation;
using UIFoundation.Navigation;
using UPnP.DataContracts;

namespace SmartView2.ViewModels
{
	[RelatedView(typeof(DeviceListPage))]
	public class DeviceListViewModel : PageViewModel
	{
		private PopupWrapper pinPopup;

		private PopupWrapper messagePopup;

		private PopupWrapper guidePopup;

		private PopupWrapper versionPopup;

		private PopupWrapper licensePopup;

		private readonly ICommand connectCommand;

		private readonly ICommand backCommand;

		private readonly ICommand refreshCommand;

		private readonly ICommand guideCommand;

		private readonly ICommand versionCommand;

		private readonly ICommand licenseCommand;

		private readonly DeviceController deviceController;

		private string networkNames;

		private bool isDeviceAvailable;

		public ICommand BackCommand
		{
			get
			{
				return this.backCommand;
			}
		}

		public ICommand ConnectCommand
		{
			get
			{
				return this.connectCommand;
			}
		}

		public ObservableCollection<DeviceInfo> Devices
		{
			get;
			private set;
		}

		public ICommand GuideCommand
		{
			get
			{
				return this.guideCommand;
			}
		}

		public bool IsDeviceAvailable
		{
			get
			{
				return this.isDeviceAvailable;
			}
			private set
			{
				base.SetProperty<bool>(ref this.isDeviceAvailable, value, "IsDeviceAvailable");
			}
		}

		public ICommand LicenseCommand
		{
			get
			{
				return this.licenseCommand;
			}
		}

		public string NetworkNames
		{
			get
			{
				return this.networkNames;
			}
			private set
			{
				base.SetProperty<string>(ref this.networkNames, value, "NetworkNames");
			}
		}

		public ICommand RefreshCommand
		{
			get
			{
				return this.refreshCommand;
			}
		}

		public ICommand VersionCommand
		{
			get
			{
				return this.versionCommand;
			}
		}

		public DeviceListViewModel(DeviceController deviceController)
		{
			NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(this.NetworkChange_NetworkAvailabilityChanged);
			NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(this.NetworkChange_NetworkAddressChanged);
			this.deviceController = deviceController;
			this.Devices = deviceController.Devices;
			this.Devices.CollectionChanged += new NotifyCollectionChangedEventHandler(this.Devices_CollectionChanged);
			this.connectCommand = new AsyncCommand(new Func<object, Task>(this.ConnectToDevice));
			this.backCommand = new Command(new Action<object>(this.OnBack));
			this.refreshCommand = new Command(new Action<object>(this.OnRefresh));
			this.guideCommand = new Command(new Action<object>(this.OnGuide));
			this.versionCommand = new Command(new Action<object>(this.OnVersion));
			this.licenseCommand = new Command(new Action<object>(this.OnLicense));
			this.networkNames = this.GetNetworksNames();
			this.CheckDeviceAvailable();
		}

		private void CheckDeviceAvailable()
		{
			this.IsDeviceAvailable = this.Devices.Count > 0;
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

		private async Task ConnectToDevice(object param)
		{
			Logger.Instance.LogMessageFormat("[SmartView2][DeviceListViewModel]ConnectToDevice started... ", new object[0]);
			base.IsDataLoaded = false;
			try
			{
				try
				{
					DeviceInfo deviceInfo = param as DeviceInfo;
					if (deviceInfo == null)
					{
						return;
					}
					else if (!TvDiscovery.IsTv2014(deviceInfo))
					{
						Logger.Instance.LogMessageFormat("[SmartView2][DeviceListViewModel]TV is not 14 year. ", new object[0]);
						PopupWrapper popupWrapper = this.messagePopup;
						string[] mAPPSIDUNABLECONNECTTV = new string[] { ResourcesModel.Instanse.MAPP_SID_UNABLE_CONNECT_TV, ResourcesModel.Instanse.MAPP_SID_SMART_VIEW_NOT_SUPPORT_YOUR_TV };
						await popupWrapper.ShowDialogAsync(mAPPSIDUNABLECONNECTTV);
						return;
					}
					else if (await this.deviceController.TryToConnect(deviceInfo.DeviceAddress.Host))
					{
						await base.Controller.Navigate(new MainViewModel(this.deviceController), false);
					}
					else
					{
						Logger.Instance.LogMessageFormat("[SmartView2][DeviceListViewModel]Call deviceController Connect . ", new object[0]);
						switch (await this.deviceController.Connect(deviceInfo))
						{
							case DeviceController.ConnectResult.PinPageAlreadyShown:
							{
								Logger.Instance.LogMessageFormat("[SmartView2][DeviceListViewModel]Another device is connecting. ", new object[0]);
								PopupWrapper popupWrapper1 = this.messagePopup;
								string[] cOMTVSIDTRYAGAIN = new string[] { ResourcesModel.Instanse.COM_TV_SID_TRY_AGAIN, ResourcesModel.Instanse.MAPP_SID_ANOTHER_DEVICE_CURRENTLY_PAIR_TV };
								await popupWrapper1.ShowDialogAsync(cOMTVSIDTRYAGAIN);
								await base.Controller.GoBack();
								return;
							}
							case DeviceController.ConnectResult.SocketException:
							{
								this.deviceController.RefreshDiscovery();
								Logger.Instance.LogMessageFormat("[SmartView2][DeviceListViewModel]TV is not ready. ", new object[0]);
								PopupWrapper popupWrapper2 = this.messagePopup;
								string[] strArrays = new string[] { ResourcesModel.Instanse.COM_TV_SID_TRY_AGAIN, ResourcesModel.Instanse.MAPP_SID_TV_NOT_READY_CONNECTION_TRY_AGAIN };
								await popupWrapper2.ShowDialogAsync(strArrays);
								await base.Controller.GoBack();
								return;
							}
							case DeviceController.ConnectResult.OtherException:
							{
								Logger.Instance.LogMessageFormat("[SmartView2][DeviceListViewModel]something went wrong. ", new object[0]);
								PopupWrapper popupWrapper3 = this.messagePopup;
								string[] cOMTVSIDTRYAGAIN1 = new string[] { ResourcesModel.Instanse.COM_TV_SID_TRY_AGAIN, ResourcesModel.Instanse.MAPP_SID_SOMETHING_WENT_WRONG };
								await popupWrapper3.ShowDialogAsync(cOMTVSIDTRYAGAIN1);
								await base.Controller.GoBack();
								return;
							}
							default:
							{
								object obj = await this.pinPopup.ShowDialogAsync(deviceInfo);
								if (obj == null)
								{
									break;
								}
								if (!(obj is Exception))
								{
									if (obj is bool?)
									{
										bool? nullable = (bool?)(obj as bool?);
										if ((!nullable.GetValueOrDefault() ? false : nullable.HasValue))
										{
											await base.Controller.Navigate(new MainViewModel(this.deviceController), false);
											break;
										}
									}
									await base.Controller.GoBack();
									break;
								}
								else
								{
									Exception exception = obj as Exception;
									Logger.Instance.LogMessageFormat("[SmartView2][DeviceListViewModel]Error occured while connectiong to tv. ", new object[0]);
									Logger instance = Logger.Instance;
									instance.LogMessageFormat("[SmartView2][DeviceListViewModel]Error message: {0} ", new object[] { exception.Message });
									Logger logger = Logger.Instance;
									logger.LogMessageFormat("[SmartView2][DeviceListViewModel]Error message stack trace: {0} ", new object[] { exception.StackTrace });
									PopupWrapper popupWrapper4 = this.messagePopup;
									string[] mAPPSIDSMARTVIEW20 = new string[] { ResourcesModel.Instanse.MAPP_SID_SMART_VIEW_2_0, ResourcesModel.Instanse.MAPP_SID_ERROR_OCCURED_WHILE_CONNECTING_TO_TV };
									popupWrapper4.Show(mAPPSIDSMARTVIEW20);
									return;
								}
							}
						}
					}
				}
				catch (Exception exception2)
				{
					Exception exception1 = exception2;
					Logger.Instance.LogMessageFormat("[SmartView2][DeviceListViewModel]Catch unknown error.", new object[0]);
					Logger instance1 = Logger.Instance;
					instance1.LogMessageFormat("[SmartView2][DeviceListViewModel]Error message: {0} ", new object[] { exception1.Message });
					Logger logger1 = Logger.Instance;
					logger1.LogMessageFormat("[SmartView2][DeviceListViewModel]Error message stack trace: {0} ", new object[] { exception1.StackTrace });
					PopupWrapper popupWrapper5 = this.messagePopup;
					string[] mAPPSIDSMARTVIEW201 = new string[] { ResourcesModel.Instanse.MAPP_SID_SMART_VIEW_2_0, ResourcesModel.Instanse.MAPP_SID_SOMETHING_WENT_WRONG };
					popupWrapper5.Show(mAPPSIDSMARTVIEW201);
				}
			}
			finally
			{
				base.IsDataLoaded = true;
			}
		}

		private void Devices_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.CheckDeviceAvailable();
		}

		private string GetNetworksNames()
		{
			List<string> strs = new List<string>();
			IEnumerable<NetworkInterface> networkInterfaces = ((IEnumerable<NetworkInterface>)NetworkInterface.GetAllNetworkInterfaces()).Where<NetworkInterface>((NetworkInterface arg) => {
				if (arg.OperationalStatus != OperationalStatus.Up)
				{
					return false;
				}
				return arg.NetworkInterfaceType == NetworkInterfaceType.Ethernet;
			});
			if (networkInterfaces.Count<NetworkInterface>() != 0)
			{
				strs.Add(SmartView2.Properties.Resources.COM_SID_WIRED);
			}
			foreach (WlanInterfaceInfo wlanInterfaceInfo in WlanApiWrapper.EnumInterfaces())
			{
				WlanConnectionInfo currentConnection = WlanApiWrapper.GetCurrentConnection(wlanInterfaceInfo.Guid);
				if (currentConnection == null)
				{
					continue;
				}
				strs.Add(string.Format("<<{0}>>", currentConnection.ProfileName));
			}
			return string.Join(", ", strs);
		}

		private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
		{
			this.NetworkNames = this.GetNetworksNames();
		}

		private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
		{
			this.NetworkNames = this.GetNetworksNames();
		}

		private void OnBack(object obj)
		{
			base.Controller.GoBack();
		}

		public override async Task OnCreateAsync()
		{
			//await this.<>n__FabricatedMethod2();
			this.messagePopup = base.Controller.CreatePopup(new MessagePopupViewModel(), false);
			this.guidePopup = base.Controller.CreatePopup(new GuideViewModel(), true);
			this.versionPopup = base.Controller.CreatePopup(new VersionViewModel(), true);
			this.licensePopup = base.Controller.CreatePopup(new LicenseViewModel(), true);
		}

		public override async Task OnDestroyAsync()
		{
			//await this.<>n__FabricatedMethod6();
			Logger.Instance.LogMessageFormat("[SmartView2][DeviceListViewModel]OnDestroyAsync started... ", new object[0]);
			if (this.deviceController != null)
			{
				await this.deviceController.DisconnectAsync(false);
			}
			this.messagePopup.Dispose();
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
			//await this.<>n__FabricatedMethod17();
		}

		public override async Task OnNavigateToAsync()
		{
			//await this.<>n__FabricatedMethod13();
			this.pinPopup = base.Controller.CreatePopup(new PinViewModel(this.deviceController), false);
		}

		private void OnRefresh(object obj)
		{
			base.Controller.Dispatcher.Invoke(() => this.deviceController.RefreshDiscovery());
		}

		private void OnVersion(object obj)
		{
			this.versionPopup.Show();
		}
	}
}