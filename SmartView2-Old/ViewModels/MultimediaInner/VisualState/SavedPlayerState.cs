using MediaLibrary.DataModels;
using SmartView2.Controls;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SmartView2.ViewModels.MultimediaInner.VisualState
{
	public class SavedPlayerState
	{
		public MediaLibrary.DataModels.ContentType ContentType
		{
			get;
			private set;
		}

		public ItemBase File
		{
			get;
			private set;
		}

		public IEnumerable<ItemBase> FileList
		{
			get;
			private set;
		}

		public bool IsRandom
		{
			get;
			private set;
		}

		public bool? IsRepeat
		{
			get;
			private set;
		}

		public SmartView2.Controls.MediaElementState MediaElementState
		{
			get;
			private set;
		}

		public double Position
		{
			get;
			private set;
		}

		public SavedPlayerState(MediaLibrary.DataModels.ContentType contentType, ItemBase file, SmartView2.Controls.MediaElementState mediaElementState, double position, IEnumerable<ItemBase> files, bool? isRepeat, bool isRandom)
		{
			this.ContentType = contentType;
			this.File = file;
			this.MediaElementState = mediaElementState;
			this.Position = position;
			this.FileList = files;
			this.IsRepeat = isRepeat;
			this.IsRandom = isRandom;
		}
	}
}