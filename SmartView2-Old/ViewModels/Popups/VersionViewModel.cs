using SmartView2;
using SmartView2.Core.Commands;
using SmartView2.Properties;
using SmartView2.Views.Popups;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels.Popups
{
	[RelatedView(typeof(VersionPopup))]
	internal class VersionViewModel : PopupViewModel
	{
		public const string AppVersion = "1.1.0";

		private string version;

		private PopupWrapper messagePopup;

		private ICommand checkForUpdatesCommand;

		public ICommand CheckForUpdatesCommand
		{
			get
			{
				return this.checkForUpdatesCommand;
			}
		}

		public string Version
		{
			get
			{
				return this.version;
			}
		}

		public VersionViewModel()
		{
			this.version = "1.1.0";
			this.checkForUpdatesCommand = new Command(new Action<object>(this.OnCheckForUpdatesAsync));
		}

		private async void OnCheckForUpdatesAsync(object obj)
		{
			UpdateController updateController = new UpdateController();
			await updateController.CheckForUpdateAsync();
			if (!updateController.UpdateFound)
			{
				PopupWrapper popupWrapper = this.messagePopup;
				string[] cOMSIDLATESTVERSION = new string[] { ResourcesModel.Instanse.COM_SID_LATEST_VERSION, ResourcesModel.Instanse.MAPP_SID_YOU_HAVE_THE_LATEST_VERION };
				popupWrapper.Show(cOMSIDLATESTVERSION);
			}
			else
			{
				UpdateMessagePopupViewModel updateMessagePopupViewModel = new UpdateMessagePopupViewModel();
				object obj1 = await base.Controller.CreatePopup(updateMessagePopupViewModel, false).ShowDialogAsync();
				bool? decision = (obj1 as AlternativePopupEventArgs).Decision;
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

		public override void OnNavigateTo(object p)
		{
			this.messagePopup = base.Controller.CreatePopup(new MessagePopupViewModel(), false);
		}
	}
}