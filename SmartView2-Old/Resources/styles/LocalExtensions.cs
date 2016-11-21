using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Interop;

namespace SmartView2.Resources.Styles
{
	internal static class LocalExtensions
	{
		public static void ForWindowFromTemplate(this object templateFrameworkElement, Action<Window> action)
		{
			Window templatedParent = ((FrameworkElement)templateFrameworkElement).TemplatedParent as Window;
			if (templatedParent != null)
			{
				action(templatedParent);
			}
		}

		public static IntPtr GetWindowHandle(this Window window)
		{
			return (new WindowInteropHelper(window)).Handle;
		}
	}
}