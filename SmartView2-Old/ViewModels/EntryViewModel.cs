using Networking;
using Networking.Native;
using SmartView2;
using SmartView2.Devices;
using SmartView2.Native;
using SmartView2.Native.Player;
using SmartView2.Properties;
using SmartView2.ViewModels.Popups;
using SmartView2.Views;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using UIFoundation.Navigation;
using UPnP;

namespace SmartView2.ViewModels
{
	[RelatedView(typeof(EntryPage))]
	public class EntryViewModel : PageViewModel
	{
		private CMAuthServer server;

		private DeviceController deviceController;

		private INetworkTransportFactory transportFactory;

		private IDevicePairing devicePairing;

		public EntryViewModel()
		{
			//this.server = (CMAuthServer)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("D27678AB-DDCC-4470-A637-2DE030023E0E")));
            //string test = string.Concat(Directory.GetCurrentDirectory(), "\\data\\");
			//this.server.StartServer(test);
			Guid deviceId = Settings.Default.DeviceId;
			if (deviceId == Guid.Empty)
			{
				Settings @default = Settings.Default;
				Guid guid = Guid.NewGuid();
				deviceId = guid;
				@default.DeviceId = guid;
				Settings.Default.Save();
			}
			this.transportFactory = new TransportFactory();
			this.devicePairing = new DevicePairing(deviceId, this.transportFactory, new SpcApiWrapper());
		}

		private async Task CheckForUpdates()
		{
			UpdateController updateController = new UpdateController();
			await updateController.CheckForUpdateAsync();
			if (updateController.UpdateFound)
			{
				UpdateMessagePopupViewModel updateMessagePopupViewModel = new UpdateMessagePopupViewModel();
				object obj = await base.Controller.CreatePopup(updateMessagePopupViewModel, false).ShowDialogAsync();
				bool? decision = (obj as AlternativePopupEventArgs).Decision;
				if ((!decision.GetValueOrDefault() ? false : decision.HasValue))
				{
					Process.Start(updateController.UpdateUrl);
				}
				if (updateController.ForceUpdate)
				{
					Application.Current.Shutdown();
				}
			}
		}

		public override async Task OnDestroyAsync()
		{
			//await this.<>n__FabricatedMethodd();
		}

		public override async Task OnNavigateToAsync()
		{
			HwndSource hwndSource = (HwndSource)PresentationSource.FromVisual(Application.Current.MainWindow);
			IPlayerNotificationProvider playerNotificationProvider = new PlayerNotificationProvider(hwndSource);
			IDeviceListener uPnPDeviceListener = new UPnPDeviceListener(new NetworkInfoProvider(), this.transportFactory);
			IDeviceDiscovery uPnPDeviceDiscovery = new UPnPDeviceDiscovery(this.transportFactory, uPnPDeviceListener);
			IDeviceDiscovery tvDiscovery = new TvDiscovery(uPnPDeviceDiscovery, new TcpWebTransport(TimeSpan.FromSeconds(5)));
			this.deviceController = new DeviceController(tvDiscovery, this.devicePairing, playerNotificationProvider, new DeviceSettingProvider(), base.Controller);
			this.deviceController.StartDiscovery();
            base.IsDataLoaded = false;
			await this.CheckForUpdates();
			bool previousDeviceAsync = await this.deviceController.ConnectToPreviousDeviceAsync();
			base.IsDataLoaded = true;
			if (!previousDeviceAsync)
			{
				base.Controller.Navigate(new WelcomeViewModel(this.deviceController), true);
			}
			else
			{
				base.Controller.Push(new WelcomeViewModel(this.deviceController));
				base.Controller.Navigate(new MainViewModel(this.deviceController), true);
			}
		}
	}
}