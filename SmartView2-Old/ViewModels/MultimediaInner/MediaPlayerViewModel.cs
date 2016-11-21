using MediaLibrary.DataModels;
using SmartView2.Controls;
using SmartView2.Core;
using SmartView2.Native.MediaLibrary;
using SmartView2.Properties;
using SmartView2.ViewModels.MultimediaInner.VisualState;
using SmartView2.ViewModels.Popups;
using SmartView2.Views.MultimediaInner;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using UIFoundation;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels.MultimediaInner
{
	[RelatedView(typeof(MediaPlayerPage))]
	public class MediaPlayerViewModel : PageViewModel
	{
		private ICommand goBack;

		private ICommand getPreviousFile;

		private ICommand getNextFile;

		private ICommand play;

		private ICommand pause;

		private ICommand addToQl;

		private ICommand playOnTv;

		private ICommand randomFile;

		private ICommand playOrPause;

		private FileList files;

		private ItemBase selectedFile;

		private System.Uri uri;

		private SmartView2.Controls.MediaElementState mediaElementState;

		private IMultiScreen multiScreen;

		private MediaLibrary.DataModels.ContentType contentType;

		private bool? isRepeat;

		private bool isRandom;

		private double position;

		private double? defaultPosition;

		private bool? sortByTitle = new bool?(false);

		private BaseSavedVisualState savedViewVisualState;

		private double volume;

		private IDataLibrary dataLibrary;

		private PopupWrapper multimediaLoadingPopup;

		public ICommand AddToQl
		{
			get
			{
				return this.addToQl;
			}
		}

		public MediaLibrary.DataModels.ContentType ContentType
		{
			get
			{
				return this.contentType;
			}
			set
			{
				if (this.contentType != value)
				{
					base.SetProperty<MediaLibrary.DataModels.ContentType>(ref this.contentType, value, "ContentType");
					base.OnPropertyChanged(this, "HasDuration");
				}
			}
		}

		public double? DefaultPosition
		{
			get
			{
				return this.defaultPosition;
			}
			set
			{
				double? nullable = this.defaultPosition;
				double? nullable1 = value;
				if (((double)nullable.GetValueOrDefault() != (double)nullable1.GetValueOrDefault() ? true : nullable.HasValue != nullable1.HasValue))
				{
					base.SetProperty<double?>(ref this.defaultPosition, value, "DefaultPosition");
				}
			}
		}

		public List<ItemBase> Files
		{
			get
			{
				if (this.contentType != MediaLibrary.DataModels.ContentType.Track)
				{
					return this.files.Files.ToList<ItemBase>();
				}
				if (!this.sortByTitle.HasValue)
				{
					return this.files.Files.ToList<ItemBase>();
				}
				if (this.sortByTitle.Value)
				{
					return this.files.GetFilesOrderByDescending().ToList<ItemBase>();
				}
				return this.files.GetFilesOrderByAscending().ToList<ItemBase>();
			}
		}

		public ICommand GetNextFile
		{
			get
			{
				return this.getNextFile;
			}
		}

		public ICommand GetPreviousFile
		{
			get
			{
				return this.getPreviousFile;
			}
		}

		public ICommand GoBack
		{
			get
			{
				return this.goBack;
			}
		}

		public bool HasDuration
		{
			get
			{
				if (this.contentType == MediaLibrary.DataModels.ContentType.Track)
				{
					return true;
				}
				return this.contentType == MediaLibrary.DataModels.ContentType.Video;
			}
		}

		public bool IsRandom
		{
			get
			{
				return this.isRandom;
			}
			set
			{
				if (this.isRandom != value)
				{
					base.SetProperty<bool>(ref this.isRandom, value, "IsRandom");
				}
			}
		}

		public bool? IsRepeat
		{
			get
			{
				return this.isRepeat;
			}
			set
			{
				bool? nullable = this.isRepeat;
				bool? nullable1 = value;
				if ((nullable.GetValueOrDefault() != nullable1.GetValueOrDefault() ? true : nullable.HasValue != nullable1.HasValue))
				{
					base.SetProperty<bool?>(ref this.isRepeat, value, "IsRepeat");
				}
			}
		}

		public bool IsVideoOrImage
		{
			get
			{
				if (this.contentType == MediaLibrary.DataModels.ContentType.Video)
				{
					return true;
				}
				return this.contentType == MediaLibrary.DataModels.ContentType.Image;
			}
		}

		public SmartView2.Controls.MediaElementState MediaElementState
		{
			get
			{
				return this.mediaElementState;
			}
			set
			{
				if (this.mediaElementState != value)
				{
					base.SetProperty<SmartView2.Controls.MediaElementState>(ref this.mediaElementState, value, "MediaElementState");
				}
			}
		}

		public ICommand Pause
		{
			get
			{
				return this.pause;
			}
		}

		public ICommand Play
		{
			get
			{
				return this.play;
			}
		}

		public ICommand PlayOnTv
		{
			get
			{
				return this.playOnTv;
			}
		}

		public ICommand PlayOrPause
		{
			get
			{
				return this.playOrPause;
			}
		}

		public double Position
		{
			get
			{
				return this.position;
			}
			set
			{
				if (this.position != value)
				{
					base.SetProperty<double>(ref this.position, value, "Position");
				}
			}
		}

		public ICommand RandomFile
		{
			get
			{
				return this.randomFile;
			}
		}

		public ItemBase SelectedFile
		{
			get
			{
				return this.selectedFile;
			}
			set
			{
				base.SetProperty<ItemBase>(ref this.selectedFile, value, "SelectedFile");
				if (value != null)
				{
					this.UpdateUri(((Content)this.selectedFile).Path);
					this.UpdateFileState();
				}
			}
		}

		public bool? SortByTitle
		{
			get
			{
				return this.sortByTitle;
			}
			set
			{
				base.SetProperty<bool?>(ref this.sortByTitle, value, "SortByTitle");
				base.OnPropertyChanged(this, "Files");
			}
		}

		public System.Uri Uri
		{
			get
			{
				return this.uri;
			}
			set
			{
				base.SetProperty<System.Uri>(ref this.uri, value, "Uri");
			}
		}

		public double Volume
		{
			get
			{
				return this.volume;
			}
			set
			{
				base.SetProperty<double>(ref this.volume, value, "Volume");
			}
		}

		public MediaPlayerViewModel(BaseSavedVisualState savedState, IMultiScreen multiScreen, IDataLibrary dataLibrary)
		{
			if (savedState == null)
			{
				throw new ArgumentNullException("savedmusicstate");
			}
			if (multiScreen == null)
			{
				throw new ArgumentNullException("multiScreen");
			}
			if (dataLibrary == null)
			{
				throw new ArgumentNullException("dataLibrary");
			}
			this.savedViewVisualState = savedState;
			this.multiScreen = multiScreen;
			this.dataLibrary = dataLibrary;
			this.InitializeCommands();
		}

		public MediaPlayerViewModel(BaseSavedVisualState savedState, IMultiScreen multiScreen, IDataLibrary dataLibrary, SavedPlayerState playerState) : this(savedState, multiScreen, dataLibrary)
		{
			if (playerState == null)
			{
				throw new ArgumentNullException("PlayerState");
			}
			if (playerState.FileList == null)
			{
				throw new ArgumentNullException("files");
			}
			if (playerState.File == null)
			{
				throw new ArgumentNullException("file");
			}
			this.ContentType = playerState.ContentType;
			if (playerState.ContentType != MediaLibrary.DataModels.ContentType.Track)
			{
				this.files = new FileList(playerState.FileList);
			}
			else
			{
				this.files = new TrackList(playerState.FileList);
			}
			this.SelectedFile = playerState.File;
			this.DefaultPosition = new double?(playerState.Position);
			this.MediaElementState = playerState.MediaElementState;
			this.IsRepeat = playerState.IsRepeat;
			this.IsRandom = playerState.IsRandom;
		}

		public MediaPlayerViewModel(BaseSavedVisualState savedState, IMultiScreen multiScreen, IDataLibrary dataLibrary, IEnumerable<ItemBase> files, Content selectedFile, MediaLibrary.DataModels.ContentType contentType) : this(savedState, multiScreen, dataLibrary)
		{
			if (files == null)
			{
				throw new ArgumentNullException("files");
			}
			this.ContentType = contentType;
			this.Position = 0;
			if (contentType != MediaLibrary.DataModels.ContentType.Track)
			{
				this.files = new FileList(files);
			}
			else
			{
				this.files = new TrackList(files);
			}
			if (selectedFile != null)
			{
				this.SelectedFile = selectedFile;
				return;
			}
			this.SelectedFile = files.FirstOrDefault<ItemBase>();
		}

		private bool CanAddToQl(object obj)
		{
			if (this.selectedFile == null)
			{
				return false;
			}
			return true;
		}

		public bool CanGetNextFile(object obj)
		{
			if (this.files == null || this.files.Count == 0 || this.selectedFile == null)
			{
				return false;
			}
			if (this.contentType == MediaLibrary.DataModels.ContentType.Track)
			{
				if (this.isRandom)
				{
					return true;
				}
				if (!this.isRepeat.HasValue && this.files.IndexOf(this.selectedFile) >= this.files.Count - 1)
				{
					return false;
				}
			}
			else if (this.files.IndexOf(this.selectedFile) >= this.files.Count - 1)
			{
				return false;
			}
			return true;
		}

		public bool CanGetPreviousFile(object obj)
		{
			if (this.files == null || this.files.Count == 0 || this.selectedFile == null)
			{
				return false;
			}
			if (this.contentType == MediaLibrary.DataModels.ContentType.Track)
			{
				if (this.isRandom)
				{
					return true;
				}
				if (!this.isRepeat.HasValue && this.files.IndexOf(this.selectedFile) <= 0)
				{
					return false;
				}
			}
			else if (this.files.IndexOf(this.selectedFile) <= 0)
			{
				return false;
			}
			return true;
		}

		private bool CanPlayOnTv(object obj)
		{
			if (this.selectedFile == null)
			{
				return false;
			}
			return true;
		}

		private void InitializeCommands()
		{
			this.goBack = new UICommand(new Action<object>(this.OnGoBack), null);
			this.getPreviousFile = new UICommand(new Action<object>(this.OnGetPreviousFile), null);
			this.getNextFile = new UICommand(new Action<object>(this.OnGetNextFile), null);
			this.play = new UICommand(new Action<object>(this.OnPlay), null);
			this.pause = new UICommand(new Action<object>(this.OnPause), null);
			this.playOrPause = new UICommand(new Action<object>(this.OnPlayOrPause), null);
			this.addToQl = new UICommand(new Action<object>(this.OnAddToQl), new Predicate<object>(this.CanAddToQl));
			this.playOnTv = new UICommand(new Action<object>(this.OnPlayOnTv), new Predicate<object>(this.CanPlayOnTv));
			this.randomFile = new UICommand(new Action<object>(this.OnRandomFile), null);
		}

		private void OnAddToQl(object obj)
		{
			if (obj == null)
			{
				return;
			}
			Content content = obj as Content;
			if (content != null)
			{
				this.multiScreen.PushMediaToTvQueue(content);
			}
		}

		private void OnDataLoadedChanged(object sender, EventArgs e)
		{
			if (!this.dataLibrary.IsDataLoaded && this.multimediaLoadingPopup != null)
			{
				this.multimediaLoadingPopup.Show();
				return;
			}
			this.multimediaLoadingPopup.Close();
		}

		private void OnGetNextFile(object obj)
		{
			ItemBase itemBase = null;
			itemBase = (this.isRepeat.HasValue ? this.files.GetNextFile(this.selectedFile, this.isRepeat, this.isRandom) : this.files.GetNextFile(this.selectedFile, new bool?(true), this.isRandom));
			if (itemBase != this.selectedFile)
			{
				this.SelectedFile = itemBase;
				return;
			}
			this.MediaElementState = SmartView2.Controls.MediaElementState.Stop;
			this.MediaElementState = SmartView2.Controls.MediaElementState.Play;
		}

		private void OnGetPreviousFile(object obj)
		{
			ItemBase itemBase = null;
			itemBase = (this.isRepeat.HasValue ? this.files.GetPreviousFile(this.selectedFile, this.isRepeat, this.isRandom) : this.files.GetPreviousFile(this.selectedFile, new bool?(true), this.isRandom));
			if (itemBase != this.selectedFile)
			{
				this.SelectedFile = itemBase;
				return;
			}
			this.MediaElementState = SmartView2.Controls.MediaElementState.Stop;
			this.MediaElementState = SmartView2.Controls.MediaElementState.Play;
		}

		private async void OnGoBack(object obj)
		{
			await base.Controller.GoBack();
		}

		public override async Task OnNavigateFromAsync()
		{
			string str;
			//await this.<>n__FabricatedMethod7();
			Settings @default = Settings.Default;
			str = (this.IsRepeat.HasValue ? this.IsRepeat.ToString() : "Null");
			@default.RepeatTrackFlag = str;
			Settings.Default.ShuffleTrackFlag = this.IsRandom;
			if (this.ContentType == MediaLibrary.DataModels.ContentType.Video)
			{
				Settings.Default.VolumePlayerForVideo = this.Volume;
			}
			if (this.ContentType == MediaLibrary.DataModels.ContentType.Track)
			{
				Settings.Default.VolumePlayerForAudio = this.Volume;
			}
			Settings.Default.Save();
			if (this.dataLibrary != null)
			{
				this.dataLibrary.DataLoaded -= new EventHandler(this.OnDataLoadedChanged);
			}
		}

		public override async Task OnNavigateToAsync()
		{
			//await this.<>n__FabricatedMethod3();
			if (this.dataLibrary != null)
			{
				this.multimediaLoadingPopup = base.Controller.CreatePopup(new MultimediaAddingContentPopupViewModel(this.dataLibrary), false);
				this.dataLibrary.DataLoaded += new EventHandler(this.OnDataLoadedChanged);
			}
			string repeatTrackFlag = Settings.Default.RepeatTrackFlag;
			try
			{
				this.IsRepeat = new bool?(bool.Parse(repeatTrackFlag));
			}
			catch
			{
				this.IsRepeat = null;
			}
			this.IsRandom = Settings.Default.ShuffleTrackFlag;
			if (this.ContentType == MediaLibrary.DataModels.ContentType.Video)
			{
				this.Volume = Settings.Default.VolumePlayerForVideo;
			}
			if (this.ContentType == MediaLibrary.DataModels.ContentType.Track)
			{
				this.Volume = Settings.Default.VolumePlayerForAudio;
			}
		}

		private void OnPause(object obj)
		{
			this.MediaElementState = SmartView2.Controls.MediaElementState.Pause;
		}

		private void OnPlay(object obj)
		{
			this.MediaElementState = SmartView2.Controls.MediaElementState.Play;
		}

		private void OnPlayOnTv(object obj)
		{
			if (obj == null)
			{
				return;
			}
			Content content = obj as Content;
			if (content != null)
			{
				this.multiScreen.PushMediaToTv(content);
			}
		}

		private void OnPlayOrPause(object obj)
		{
			if (obj == null)
			{
				return;
			}
			Content content = obj as Content;
			if (content == null)
			{
				return;
			}
			if (this.selectedFile != content)
			{
				this.SelectedFile = content;
				this.MediaElementState = SmartView2.Controls.MediaElementState.Play;
				return;
			}
			if (this.mediaElementState == SmartView2.Controls.MediaElementState.Stop || this.mediaElementState == SmartView2.Controls.MediaElementState.Pause)
			{
				this.MediaElementState = SmartView2.Controls.MediaElementState.Play;
				return;
			}
			if (this.mediaElementState == SmartView2.Controls.MediaElementState.Play)
			{
				this.MediaElementState = SmartView2.Controls.MediaElementState.Pause;
			}
		}

		private void OnRandomFile(object obj)
		{
			if (this.isRandom)
			{
				this.files.FillRandomPriority();
			}
		}

		public BaseSavedVisualState SavePlayerState()
		{
			this.savedViewVisualState.PlayerState = new SavedPlayerState(this.contentType, this.selectedFile, this.mediaElementState, this.position, this.files.Files, this.isRepeat, this.isRandom);
			return this.savedViewVisualState;
		}

		internal void UpdateContent(IEnumerable<ItemBase> ItemsList)
		{
			if (this.ContentType != MediaLibrary.DataModels.ContentType.Track)
			{
				this.files = new FileList(ItemsList);
			}
			else
			{
				this.files = new TrackList(ItemsList);
			}
			base.OnPropertyChanged(this, "Files");
			if (this.SelectedFile != null)
			{
				this.SelectedFile = this.files.Files.FirstOrDefault<ItemBase>((ItemBase f) => f.ID == this.selectedFile.ID);
			}
		}

		private void UpdateFileState()
		{
			this.MediaElementState = SmartView2.Controls.MediaElementState.Play;
		}

		private void UpdateUri(string path)
		{
			try
			{
				this.Uri = new System.Uri(path);
			}
			catch
			{
				this.Uri = null;
			}
		}
	}
}