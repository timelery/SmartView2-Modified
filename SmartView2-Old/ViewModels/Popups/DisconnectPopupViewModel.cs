using SmartView2.Core.Commands;
using SmartView2.Properties;
using SmartView2.Views.Popups;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using UIFoundation;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels.Popups
{
	[RelatedView(typeof(DisconnectPopup))]
	internal class DisconnectPopupViewModel : PopupViewModel
	{
		private string deviceName;

		public string DeviceName
		{
			get
			{
				return this.deviceName;
			}
			set
			{
				base.SetProperty<string>(ref this.deviceName, value, "DeviceName");
			}
		}

		public bool IsChecked
		{
			get;
			set;
		}

		public ICommand OkCommand
		{
			get;
			set;
		}

		public DisconnectPopupViewModel()
		{
			this.OkCommand = new Command(new Action<object>(this.OnOkCommand));
		}

		public override void OnNavigateTo(object p)
		{
			base.OnNavigateTo(p);
			this.IsChecked = false;
			this.DeviceName = p as string;
		}

		private void OnOkCommand(object obj)
		{
			if (this.IsChecked)
			{
				Settings.Default.ShowDisconnectMessage = false;
			}
			base.Controller.ClosePopup(true);
		}
	}
}