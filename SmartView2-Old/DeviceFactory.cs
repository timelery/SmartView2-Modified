using Networking.Native;
using SmartView2.Core;
using SmartView2.Devices;
using SmartView2.Devices.RemoteControls;
using SmartView2.Devices.SecondTv;
using SmartView2.Native.DLNA;
using SmartView2.Native.HTTP;
using SmartView2.Native.MediaLibrary;
using SmartView2.Native.MultiScreen;
using System;
using UPnP.DataContracts;

namespace SmartView2
{
	public static class DeviceFactory
	{
		public static ITargetDevice CreateTvDevice(DeviceInfo deviceInfo, IPlayerNotificationProvider playerNotification, IBaseDispatcher dispatcher)
		{
			return DeviceFactory.CreateTvDevice(deviceInfo, playerNotification, dispatcher, new NoSecurityProvider());
		}

		internal static ITargetDevice CreateTvDevice(DeviceInfo deviceInfo, IPlayerNotificationProvider playerNotification, IBaseDispatcher dispatcher, ISecondTvSecurityProvider securityProvider)
		{
			if (deviceInfo == null)
			{
				throw new ArgumentNullException("deviceInfo");
			}
			if (playerNotification == null)
			{
				throw new ArgumentNullException("playerNotification");
			}
			if (dispatcher == null)
			{
				throw new ArgumentNullException("dispatcher");
			}
			SecondTvSyncTransport secondTvSyncTransport = new SecondTvSyncTransport(securityProvider);
			SecondTvAsyncTransport secondTvAsyncTransport = new SecondTvAsyncTransport(securityProvider);
			secondTvSyncTransport.Connect(deviceInfo.DeviceAddress);
			secondTvAsyncTransport.Connect(deviceInfo.DeviceAddress);
			SecondTv secondTv = new SecondTv(secondTvSyncTransport, deviceInfo.LocalAddress);
			SecondTvRemoteInput secondTvRemoteInput = new SecondTvRemoteInput(secondTvAsyncTransport, deviceInfo.LocalAddress);
			secondTvRemoteInput.Connect(secondTvSyncTransport);
			MbrKeySender mbrKeySender = new MbrKeySender(secondTvSyncTransport, deviceInfo.LocalAddress);
			RemoteControlFactory remoteControlFactory = new RemoteControlFactory(secondTvRemoteInput, mbrKeySender);
			return new TvDevice(deviceInfo, secondTv, playerNotification, secondTvRemoteInput, remoteControlFactory, dispatcher, new HttpServer(), new MultiScreenController(), new DlnaServer(), new DataLibrary(dispatcher));
		}
	}
}