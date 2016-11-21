using SmartView2.Properties;
using SmartView2.Views.Popups;
using System;
using System.Configuration;
using System.Windows.Input;
using UIFoundation;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels.Popups
{
	[RelatedView(typeof(MultimediaIntroductionPopup))]
	public class MultimediaIntroductionPopupViewModel : PopupViewModel
	{
		private bool showAgain;

		private int page;

		private ICommand goToPreviousPage;

		private ICommand goToNextPage;

		public ICommand GoToNextPage
		{
			get
			{
				return this.goToNextPage;
			}
		}

		public ICommand GoToPreviousPage
		{
			get
			{
				return this.goToPreviousPage;
			}
		}

		public int Page
		{
			get
			{
				return this.page;
			}
			set
			{
				base.SetProperty<int>(ref this.page, value, "Page");
			}
		}

		public bool ShowAgain
		{
			get
			{
				return this.showAgain;
			}
			set
			{
				base.SetProperty<bool>(ref this.showAgain, value, "ShowAgain");
			}
		}

		public MultimediaIntroductionPopupViewModel()
		{
			this.Page = 0;
			this.ShowAgain = Settings.Default.MultimediaIntroductionDoNotShow;
			this.goToPreviousPage = new UICommand(new Action<object>(this.OnGoToPreviousPage), null);
			this.goToNextPage = new UICommand(new Action<object>(this.OnGoToNextPage), null);
		}

		private void OnGoToNextPage(object obj)
		{
			MultimediaIntroductionPopupViewModel page = this;
			page.Page = page.Page + 1;
		}

		private void OnGoToPreviousPage(object obj)
		{
			MultimediaIntroductionPopupViewModel page = this;
			page.Page = page.Page - 1;
		}

		public override void OnNavigateFrom()
		{
			Settings.Default.MultimediaIntroductionDoNotShow = this.ShowAgain;
			Settings.Default.Save();
			base.OnNavigateFrom();
		}
	}
}