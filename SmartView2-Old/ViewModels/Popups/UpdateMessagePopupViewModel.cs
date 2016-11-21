using SmartView2.Views.Popups;
using System;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels.Popups
{
	[RelatedView(typeof(UpdateMessagePopupView))]
	internal class UpdateMessagePopupViewModel : OkCancelPopupViewModel
	{
		public UpdateMessagePopupViewModel() : base(string.Empty, string.Empty)
		{
		}
	}
}