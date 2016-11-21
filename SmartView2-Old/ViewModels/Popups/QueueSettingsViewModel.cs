using SmartView2.Core;
using SmartView2.Views.Popups;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using UIFoundation;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels.Popups
{
	[RelatedView(typeof(QueueSettingsPopup))]
	public class QueueSettingsViewModel : PopupViewModel
	{
		private ICommand okCommand;

		private SlideShowSettingsModel settingsModel;

		private IMultiScreen multiScreen;

		public ICommand OkCommand
		{
			get
			{
				return this.okCommand;
			}
		}

		public SlideShowSettingsModel SettingsModel
		{
			get
			{
				return this.settingsModel;
			}
			private set
			{
				base.SetProperty<SlideShowSettingsModel>(ref this.settingsModel, value, "SettingsModel");
			}
		}

		public QueueSettingsViewModel(IMultiScreen multiScreen)
		{
			if (multiScreen == null)
			{
				throw new ArgumentNullException("multiScreen");
			}
			this.multiScreen = multiScreen;
			this.okCommand = new UICommand((object o) => {
				this.multiScreen.SetSlideShowSettings(this.settingsModel);
				base.Controller.ClosePopup(this.settingsModel);
			}, null);
		}

		public override void OnNavigateTo(object p)
		{
			base.OnNavigateTo(p);
			this.SettingsModel = p as SlideShowSettingsModel;
		}
	}
}