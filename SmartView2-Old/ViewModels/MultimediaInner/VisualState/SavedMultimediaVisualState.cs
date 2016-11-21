using MediaLibrary.DataModels;
using SmartView2.Core;
using System;
using System.Runtime.CompilerServices;

namespace SmartView2.ViewModels.MultimediaInner.VisualState
{
	public class SavedMultimediaVisualState
	{
		public ContentType CurrentView
		{
			get;
			private set;
		}

		public IDataLibrary datalibrary
		{
			get;
			private set;
		}

		public IMultiScreen multiscreen
		{
			get;
			private set;
		}

		public SavedMusicVisualState MusicVS
		{
			get;
			private set;
		}

		public SavedVideoPhotoVisualstate PhotoVS
		{
			get;
			private set;
		}

		public SavedVideoPhotoVisualstate VideoVS
		{
			get;
			private set;
		}

		public SavedMultimediaVisualState(SavedMusicVisualState musicvs, SavedVideoPhotoVisualstate videovs, SavedVideoPhotoVisualstate photovs, ContentType currentview, IMultiScreen ms, IDataLibrary dl)
		{
			this.MusicVS = musicvs;
			this.VideoVS = videovs;
			this.PhotoVS = photovs;
			this.CurrentView = currentview;
			this.multiscreen = ms;
			this.datalibrary = dl;
		}
	}
}