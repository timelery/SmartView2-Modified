using SmartView2.Core;
using SmartView2.Views.Popups;
using System;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels.Popups
{
	[RelatedView(typeof(MultimediaAddingContentPopupView))]
	public class MultimediaAddingContentPopupViewModel : PopupViewModel
	{
		private IDataLibrary dataLibrary;

		public MultimediaAddingContentPopupViewModel(IDataLibrary dataLibrary)
		{
			this.dataLibrary = dataLibrary;
		}

		protected override void OnCloseCommand(object obj)
		{
			this.dataLibrary.CancelAdding();
			base.OnCloseCommand(obj);
		}
	}
}