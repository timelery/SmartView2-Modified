using System;
using System.Runtime.CompilerServices;

namespace SmartView2.ViewModels.MultimediaInner.VisualState
{
	public class BaseSavedVisualState
	{
		public SavedPlayerState PlayerState
		{
			get;
			set;
		}

		public BaseSavedVisualState(SavedPlayerState playerstate)
		{
			this.PlayerState = playerstate;
		}
	}
}