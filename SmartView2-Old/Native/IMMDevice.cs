using System;
using System.Runtime.InteropServices;

namespace SmartView2.Native
{
	[Guid("D666063F-1587-4E43-81F1-B948E807363F")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IMMDevice
	{
		int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, out object ppInterface);
	}
}