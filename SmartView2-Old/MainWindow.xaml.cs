using SmartView2.Properties;
using SmartView2.ViewModels.Popups;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Threading;
using UIFoundation.Navigation;
using UIFoundation.Navigation.Controls;

namespace SmartView2
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();
		}

		private async void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Controller dataContext = (Controller)base.DataContext;
			if (Settings.Default.ExitDoNotShowAgain)
			{
				base.Close();
			}
			else
			{
				YesNoPopupViewModel yesNoPopupViewModel = new YesNoPopupViewModel(ResourcesModel.Instanse.COM_EXIT_SMART_VIEW, ResourcesModel.Instanse.COM_LFD_DO_YOU_WANT_TO_EXIT_THE_APPLICATION, true);
				PopupWrapper popupWrapper = dataContext.CreatePopup(yesNoPopupViewModel, false);
				AlternativePopupEventArgs alternativePopupEventArg = await popupWrapper.ShowDialogAsync() as AlternativePopupEventArgs;
				Settings.Default.ExitDoNotShowAgain = yesNoPopupViewModel.CheckBoxState;
				Settings.Default.Save();
				if (alternativePopupEventArg != null)
				{
					bool? decision = alternativePopupEventArg.Decision;
					if ((!decision.GetValueOrDefault() ? false : decision.HasValue))
					{
						base.Close();
					}
				}
			}
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern bool GetCursorPos(ref MainWindow.POINT pt);

		[DllImport("user32", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern bool GetMonitorInfo(IntPtr hMonitor, MainWindow.MONITORINFO lpmi);

		public async void InvokeAction(Action action)
		{
			await base.Dispatcher.InvokeAsync(action);
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern IntPtr MonitorFromPoint(MainWindow.POINT pt, MainWindow.MonitorOptions dwFlags);

		[DllImport("User32", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

		private void OnSourceInitialized(object sender, EventArgs e)
		{
			IntPtr handle = (new WindowInteropHelper(this)).Handle;
			HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(this.WindowProc));
		}

		private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == 36)
			{
				MainWindow.WmGetMinMaxInfo(hwnd, lParam);
			}
			return (IntPtr)0;
		}

		private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
		{
			MainWindow.MINMAXINFO structure = (MainWindow.MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MainWindow.MINMAXINFO));
			MainWindow.POINT pOINT = new MainWindow.POINT();
			MainWindow.GetCursorPos(ref pOINT);
			IntPtr intPtr = MainWindow.MonitorFromPoint(pOINT, MainWindow.MonitorOptions.MONITOR_DEFAULTTONEAREST);
			if (intPtr != IntPtr.Zero)
			{
				MainWindow.MONITORINFO mONITORINFO = new MainWindow.MONITORINFO();
				MainWindow.GetMonitorInfo(intPtr, mONITORINFO);
				MainWindow.RECT rECT = mONITORINFO.rcWork;
				MainWindow.RECT rECT1 = mONITORINFO.rcMonitor;
				structure.ptMaxPosition.x = Math.Abs(rECT.left - rECT1.left) - 7;
				structure.ptMaxPosition.y = Math.Abs(rECT.top - rECT1.top);
				structure.ptMaxSize.x = Math.Abs(rECT.right - rECT.left) + 14;
				structure.ptMaxSize.y = Math.Abs(rECT.bottom - rECT.top) + 7;
			}
			Marshal.StructureToPtr(structure, lParam, true);
		}

		private struct MINMAXINFO
		{
			public MainWindow.POINT ptReserved;

			public MainWindow.POINT ptMaxSize;

			public MainWindow.POINT ptMaxPosition;

			public MainWindow.POINT ptMinTrackSize;

			public MainWindow.POINT ptMaxTrackSize;
		}

		private class MONITORINFO
		{
			public int cbSize;

			public MainWindow.RECT rcMonitor;

			public MainWindow.RECT rcWork;

			public int dwFlags;

			public MONITORINFO()
			{
			}
		}

		private enum MonitorOptions : uint
		{
			MONITOR_DEFAULTTONULL,
			MONITOR_DEFAULTTOPRIMARY,
			MONITOR_DEFAULTTONEAREST
		}

		private struct POINT
		{
			public int x;

			public int y;

			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}

		private struct RECT
		{
			public int left;

			public int top;

			public int right;

			public int bottom;
		}
	}
}