using Microsoft.Win32;
using SmartView2.Core;
using SmartView2.Properties;
using SmartView2.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TagLib;
using TagLib.Id3v2;
using UIFoundation;
using UIFoundation.Navigation;

namespace SmartView2
{
	public partial class App : Application
	{
		private Mutex myMutex;

		public App()
		{
			base.Dispatcher.UnhandledException += new DispatcherUnhandledExceptionEventHandler(this.Dispatcher_UnhandledException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);
		}

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception exceptionObject = e.ExceptionObject as Exception;
			if (exceptionObject != null)
			{
				Logger.Instance.LogError(exceptionObject.Message);
				Logger.Instance.LogError(exceptionObject.StackTrace);
			}
			if (System.Diagnostics.Debugger.IsAttached)
			{
				System.Diagnostics.Debugger.Break();
				return;
			}
			System.Diagnostics.Debugger.Launch();
		}

		private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			Logger.Instance.LogError(e.Exception.Message);
			Logger.Instance.LogError(e.Exception.StackTrace);
			if (System.Diagnostics.Debugger.IsAttached)
			{
				System.Diagnostics.Debugger.Break();
			}
		}

		private void GenerateResources()
		{
			Size size = new Size(800, 600);
			Border border = new Border()
			{
				Width = size.Width,
				Height = size.Height
			};
			Border item = border;
			item.Background = (Brush)base.Resources["backgroundGradientBrush"];
			item.Measure(size);
			item.Arrange(new Rect(size));
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
			renderTargetBitmap.Render(item);
			ResourceDictionary resourceDictionaries = new ResourceDictionary()
			{
				{ "commonGradientBackground", new ImageBrush(renderTargetBitmap) }
			};
			base.Resources.MergedDictionaries.Add(resourceDictionaries);
			TagLib.Id3v2.Tag.ForceDefaultEncoding = true;
			ByteVector.UseBrokenLatin1Behavior = true;
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern bool IsIconic(IntPtr hWnd);

		private void OnFirstStart(Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if ((int)(Registry.GetValue("HKEY_CURRENT_USER\\Software\\Samsung\\Smart View 2.0", "CreateAppData", 1) ?? 1) == 1)
			{
				action();
				Registry.SetValue("HKEY_CURRENT_USER\\Software\\Samsung\\Smart View 2.0", "CreateAppData", 0);
			}
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			ResourcesModel.Instanse.Culture = Thread.CurrentThread.CurrentUICulture;
			bool flag = false;
			this.myMutex = new Mutex(true, "SingletonSmartView2", out flag);
			if (flag || System.Diagnostics.Debugger.IsAttached)
			{
				base.OnStartup(e);
				this.OnFirstStart(() => {
					string directoryName = (new FileInfo(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath)).DirectoryName;
					try
					{
						Directory.Delete(directoryName, true);
					}
					catch
					{
					}
				});
				this.GenerateResources();
				SmartView2.MainWindow mainWindow = new SmartView2.MainWindow();
				Controller controller = new Controller(new EntryViewModel(), new DispatcherWrapper(base.Dispatcher), DesignModeHelper.IsDesignMode);
				mainWindow.DataContext = controller;
				mainWindow.Show();
			}
			else
			{
				Process[] processesByName = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
				if ((int)processesByName.Length > 1)
				{
					Process process = ((IEnumerable<Process>)processesByName).FirstOrDefault<Process>((Process p) => p.MainWindowHandle != IntPtr.Zero);
					if (process != null)
					{
						if (!App.IsIconic(process.MainWindowHandle))
						{
							App.ShowWindow(process.MainWindowHandle, 5);
						}
						else
						{
							App.ShowWindow(process.MainWindowHandle, 9);
						}
						App.SetForegroundWindow(process.MainWindowHandle);
					}
				}
				Application.Current.Shutdown();
			}
			Logger.Instance = new Log4NetLogger();
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern IntPtr SetActiveWindow(IntPtr hWnd);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
	}
}