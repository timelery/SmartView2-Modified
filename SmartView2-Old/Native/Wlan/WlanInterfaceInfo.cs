using System;
using System.Runtime.CompilerServices;

namespace SmartView2.Native.Wlan
{
	public sealed class WlanInterfaceInfo
	{
		public string Description
		{
			get;
			internal set;
		}

		public System.Guid Guid
		{
			get;
			internal set;
		}

		public WlanInterfaceState State
		{
			get;
			internal set;
		}

		public WlanInterfaceInfo()
		{
		}
	}
}