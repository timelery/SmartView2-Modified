using SmartView2;
using SmartView2.Core;
using SmartView2.Core.Commands;
using SmartView2.Native.Wlan;
using SmartView2.Properties;
using SmartView2.ViewModels;
using SmartView2.Views.Popups;
using System;
using System.Collections.Generic;
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

namespace SmartView2.ViewModels.Popups
{
	[RelatedView(typeof(ChangeTVPopup))]
	internal class ChangeTVViewModel : PopupViewModel
	{
		private readonly ICommand changeDeviceCommand;

		private readonly ICommand refreshCommand;

		private IEnumerable<DeviceInfo> devices;

		private DeviceController deviceController;

		private IPageController pageController;

		private IPageController innerPageController;

		private PopupWrapper pinPopup;

		private PopupWrapper messagePopup;

		private string networkNames;

		public ICommand ChangeDeviceCommand
		{
			get
			{
				return this.changeDeviceCommand;
			}
		}

		public IEnumerable<DeviceInfo> Devices
		{
			get
			{
				return this.devices;
			}
			set
			{
				base.SetProperty<IEnumerable<DeviceInfo>>(ref this.devices, value, "Devices");
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

		public ChangeTVViewModel()
		{
			this.refreshCommand = new Command(new Action<object>(this.OnRefresh));
			this.changeDeviceCommand = new Command(new Action<object>(this.OnChangeDeviceCommand));
			this.networkNames = this.GetNetworksNames();
		}

		private void Devices_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.Devices = (
				from p in this.deviceController.Devices
				where p != this.deviceController.CurrentDeviceInfo
				select p).ToArray<DeviceInfo>();
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

		private async void OnChangeDeviceCommand(object obj)
		{
			DeviceInfo deviceInfo = obj as DeviceInfo;
			if (deviceInfo != null)
			{
				if (TvDiscovery.IsTv2014(deviceInfo))
				{
					base.IsDataLoaded = false;
					await this.innerPageController.Dispose();
					if (await this.deviceController.TryToConnect(deviceInfo.DeviceAddress.Host))
					{
						await this.pageController.Navigate(new MainViewModel(this.deviceController), true);
					}
					else if (await this.deviceController.Reconnect(deviceInfo) == DeviceController.ConnectResult.Ok)
					{
						object obj1 = await this.pinPopup.ShowDialogAsync(deviceInfo);
						if (obj1 == null)
						{
							await this.pageController.GoBack(typeof(WelcomeViewModel));
						}
						else
						{
							if (obj1 is Exception)
							{
								PopupWrapper popupWrapper = this.messagePopup;
								string[] mAPPSIDSMARTVIEW20 = new string[] { ResourcesModel.Instanse.MAPP_SID_SMART_VIEW_2_0, ResourcesModel.Instanse.MAPP_SID_ERROR_OCCURED_WHILE_CONNECTING_TO_TV };
								await popupWrapper.ShowDialogAsync(mAPPSIDSMARTVIEW20);
								await this.pageController.GoBack(typeof(WelcomeViewModel));
							}
							if (obj1 is bool?)
							{
								bool? nullable = (bool?)(obj1 as bool?);
								if ((!nullable.GetValueOrDefault() ? true : !nullable.HasValue))
								{
									goto Label1;
								}
								await this.pageController.Navigate(new MainViewModel(this.deviceController), true);
								goto Label0;
							}
						Label1:
							await this.pageController.GoBack(typeof(WelcomeViewModel));
						}
					}
					else
					{
						PopupWrapper popupWrapper1 = this.messagePopup;
						string[] strArrays = new string[] { ResourcesModel.Instanse.MAPP_SID_SMART_VIEW_2_0, ResourcesModel.Instanse.MAPP_SID_ERROR_OCCURED_WHILE_CONNECTING_TO_TV };
						await popupWrapper1.ShowDialogAsync(strArrays);
						await this.pageController.GoBack(typeof(WelcomeViewModel));
					}
				Label0:
					base.IsDataLoaded = true;
					base.Controller.ClosePopup();
				}
				else
				{
					PopupWrapper popupWrapper2 = this.messagePopup;
					string[] mAPPSIDUNABLECONNECTTV = new string[] { ResourcesModel.Instanse.MAPP_SID_UNABLE_CONNECT_TV, ResourcesModel.Instanse.MAPP_SID_SMART_VIEW_NOT_SUPPORT_YOUR_TV };
					await popupWrapper2.ShowDialogAsync(mAPPSIDUNABLECONNECTTV);
				}
			}
		}

		public override void OnNavigateFrom()
		{
			this.deviceController.Devices.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.Devices_CollectionChanged);
			base.OnNavigateFrom();
		}

		public override void OnNavigateTo(object p)
		{
			this.deviceController = ((object[])p)[0] as DeviceController;
			this.pageController = ((object[])p)[1] as IPageController;
			this.innerPageController = ((object[])p)[2] as IPageController;
			this.messagePopup = base.Controller.CreatePopup(new MessagePopupViewModel(), false);
			this.pinPopup = base.Controller.CreatePopup(new PinViewModel(this.deviceController), false);
			this.deviceController.Devices.CollectionChanged += new NotifyCollectionChangedEventHandler(this.Devices_CollectionChanged);
			this.Devices = (
				from o in this.deviceController.Devices
				where o != this.deviceController.CurrentDeviceInfo
				select o).ToArray<DeviceInfo>();
			if (this.Devices.Count<DeviceInfo>() == 0)
			{
				this.OnRefresh(null);
			}
		}

		private void OnRefresh(object obj)
		{
			base.Controller.Dispatcher.Invoke(() => this.deviceController.RefreshDiscovery());
		}
	}
}