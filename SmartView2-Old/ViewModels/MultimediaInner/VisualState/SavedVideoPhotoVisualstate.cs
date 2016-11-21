using MediaLibrary.DataModels;
using SmartView2.Core;
using System;
using System.Runtime.CompilerServices;

namespace SmartView2.ViewModels.MultimediaInner.VisualState
{
	public class SavedVideoPhotoVisualstate : BaseSavedVisualState
	{
		public MultimediaFolder CurrentItem
		{
			get;
			private set;
		}

		public VideoPhotoViewType ViewType
		{
			get;
			set;
		}

		public SavedVideoPhotoVisualstate(VideoPhotoViewType viewtype, MultimediaFolder currentitem, SavedPlayerState playerstate) : base(playerstate)
		{
			this.ViewType = viewtype;
			this.CurrentItem = currentitem;
		}
	}
}