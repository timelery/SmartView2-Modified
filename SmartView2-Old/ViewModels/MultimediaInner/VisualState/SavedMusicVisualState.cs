using MediaLibrary.DataModels;
using SmartView2.Core;
using System;
using System.Runtime.CompilerServices;

namespace SmartView2.ViewModels.MultimediaInner.VisualState
{
	public class SavedMusicVisualState : BaseSavedVisualState
	{
		public MultimediaFolder CurrentFolder
		{
			get;
			private set;
		}

		public ItemBase CurrentItem
		{
			get;
			private set;
		}

		public MusicViewType ViewType
		{
			get;
			private set;
		}

		public SavedMusicVisualState(MusicViewType viewtype, MultimediaFolder currentfolder, ItemBase currentitem, SavedPlayerState playerstate) : base(playerstate)
		{
			this.ViewType = viewtype;
			this.CurrentFolder = currentfolder;
			this.CurrentItem = currentitem;
		}
	}
}