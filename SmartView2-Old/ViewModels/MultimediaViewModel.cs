using MediaLibrary.DataModels;
using SmartView2.Core;
using SmartView2.Core.Commands;
using SmartView2.Native.MultiScreen;
using SmartView2.Properties;
using SmartView2.ViewModels.MultimediaInner;
using SmartView2.ViewModels.MultimediaInner.VisualState;
using SmartView2.ViewModels.Popups;
using SmartView2.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using UIFoundation;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels
{
	[RelatedView(typeof(MultimediaPage))]
	public class MultimediaViewModel : PageViewModel
	{
		private ICommand goToVideoCommand;

		private ICommand goToMusicCommand;

		private ICommand goToImagesCommand;

		private ICommand playPreviousFile;

		private ICommand playOnTv;

		private ICommand play;

		private ICommand pause;

		private ICommand playNextFile;

		private ICommand goToNowPlaying;

		private ICommand innerViewChangedCommand;

		private ICommand replaceQueueItem;

		private ICommand removeQueueItem;

		private ICommand addToQueue;

		private ICommand nowPlayingGoBack;

		private ICommand addFilesCommand;

		private ICommand addFolderCommand;

		private IDataLibrary dataLibrary;

		private readonly IMultiScreen multiScreen;

		private Content selectedQueueFile;

		private SavedMusicVisualState musicVisualState;

		private SavedVideoPhotoVisualstate videoVisualState;

		private SavedVideoPhotoVisualstate imageVisualState;

		private ContentType currentContent;

		private int comboBoxMusicCurrentState;

		private int comboBoxVICurrentState;

		private CurrentMultimediaHeader currentHeader;

		private bool comboBoxMusicEnabled;

		private bool comboBoxVIEnabled;

		private bool photoChecked;

		private bool videoChecked;

		private bool musicChecked;

		private ContentType previousContentType;

		private bool isSelectionMode;

		private IEnumerable<object> selectedItems;

		private bool isFirstOpen;

		private PopupWrapper messagePopup;

		private IPageController innerController;

		private string videoSearchMask = "Video files(*.MP4;*.AVI;*.WMV)|*.MP4;*.AVI;*.WMV|All files (*.*)|*.*";

		private string imageSearchMask = "Image files(*.BMP;*.JPG;*.JPEG;*.PNG*;*.TIF;*.GIF)|*.BMP;*.JPG;*.JPEG;*.PNG;*.TIF;*.GIF|All files (*.*)|*.*";

		public string searchMask = "Music files(*.mp3;*.flac;*.mid)|*.mp3;*.flac;*.mid|All files (*.*)|*.*";

		public ICommand AddFilesCommand
		{
			get
			{
				return this.addFilesCommand;
			}
		}

		public ICommand AddFolderCommand
		{
			get
			{
				return this.addFolderCommand;
			}
		}

		public ICommand AddToQueue
		{
			get
			{
				return this.addToQueue;
			}
		}

		public int ComboBoxMusicCurrentState
		{
			get
			{
				return this.comboBoxMusicCurrentState;
			}
			set
			{
				base.SetProperty<int>(ref this.comboBoxMusicCurrentState, value, "ComboBoxMusicCurrentState");
			}
		}

		public bool ComboBoxMusicEnabled
		{
			get
			{
				return this.comboBoxMusicEnabled;
			}
			set
			{
				base.SetProperty<bool>(ref this.comboBoxMusicEnabled, value, "ComboBoxMusicEnabled");
			}
		}

		public int ComboBoxVICurrentState
		{
			get
			{
				return this.comboBoxVICurrentState;
			}
			set
			{
				if (value == 3)
				{
					value = 0;
				}
				base.SetProperty<int>(ref this.comboBoxVICurrentState, value, "ComboBoxVICurrentState");
			}
		}

		public bool ComboBoxVIEnabled
		{
			get
			{
				return this.comboBoxVIEnabled;
			}
			set
			{
				base.SetProperty<bool>(ref this.comboBoxVIEnabled, value, "ComboBoxVIEnabled");
			}
		}

		public CurrentMultimediaHeader CurrentHeader
		{
			get
			{
				return this.currentHeader;
			}
			set
			{
				base.SetProperty<CurrentMultimediaHeader>(ref this.currentHeader, value, "CurrentHeader");
			}
		}

		public ICommand GoToImagesCommand
		{
			get
			{
				return this.goToImagesCommand;
			}
		}

		public ICommand GoToMusicCommand
		{
			get
			{
				return this.goToMusicCommand;
			}
		}

		public ICommand GoToNowPlaying
		{
			get
			{
				return this.goToNowPlaying;
			}
		}

		public ICommand GoToVideoCommand
		{
			get
			{
				return this.goToVideoCommand;
			}
		}

		public IPageController InnerController
		{
			get
			{
				return this.innerController;
			}
			private set
			{
				base.SetProperty<IPageController>(ref this.innerController, value, "InnerController");
			}
		}

		public ICommand InnerViewChangedCommand
		{
			get
			{
				return this.innerViewChangedCommand;
			}
		}

		public bool IsQueueReady
		{
			get
			{
				if (this.multiScreen.MediaQueue == null)
				{
					return false;
				}
				return this.multiScreen.MediaQueue.Count<Content>() > 0;
			}
		}

		public bool IsSelectionMode
		{
			get
			{
				return this.isSelectionMode;
			}
			set
			{
				base.SetProperty<bool>(ref this.isSelectionMode, value, "IsSelectionMode");
			}
		}

		public ICommand MoveItemDown
		{
			get;
			private set;
		}

		public ICommand MoveItemUp
		{
			get;
			private set;
		}

		public IMultiScreen MultiScreen
		{
			get
			{
				return this.multiScreen;
			}
		}

		public bool MusicChecked
		{
			get
			{
				return this.musicChecked;
			}
			set
			{
				base.SetProperty<bool>(ref this.musicChecked, value, "MusicChecked");
			}
		}

		public ICommand NowPlayingGoBack
		{
			get
			{
				return this.nowPlayingGoBack;
			}
		}

		public ICommand Pause
		{
			get
			{
				return this.pause;
			}
		}

		public bool PhotoChecked
		{
			get
			{
				return this.photoChecked;
			}
			set
			{
				base.SetProperty<bool>(ref this.photoChecked, value, "PhotoChecked");
			}
		}

		public ICommand Play
		{
			get
			{
				return this.play;
			}
		}

		public ICommand PlayNextFile
		{
			get
			{
				return this.playNextFile;
			}
		}

		public ICommand PlayOnTv
		{
			get
			{
				return this.playOnTv;
			}
		}

		public ICommand PlayPreviousFile
		{
			get
			{
				return this.playPreviousFile;
			}
		}

		public ICommand RemoveFilesCommand
		{
			get;
			private set;
		}

		public ICommand RemoveFoldersCommand
		{
			get;
			private set;
		}

		public ICommand RemoveQueueItem
		{
			get
			{
				return this.removeQueueItem;
			}
		}

		public ICommand ReplaceQueueItem
		{
			get
			{
				return this.replaceQueueItem;
			}
		}

		public IEnumerable<object> SelectedItems
		{
			get
			{
				return this.selectedItems;
			}
			set
			{
				base.SetProperty<IEnumerable<object>>(ref this.selectedItems, value, "SelectedItems");
				IEnumerable<object> objs = this.selectedItems;
			}
		}

		public Content SelectedQueueFile
		{
			get
			{
				return this.selectedQueueFile;
			}
			set
			{
				if (this.selectedQueueFile != value)
				{
					base.SetProperty<Content>(ref this.selectedQueueFile, value, "SelectedQueueFile");
				}
			}
		}

		public bool VideoChecked
		{
			get
			{
				return this.videoChecked;
			}
			set
			{
				base.SetProperty<bool>(ref this.videoChecked, value, "VideoChecked");
			}
		}

		public MultimediaViewModel(SavedMultimediaVisualState savedvisualstate, IMultiScreen multiScreen, IDataLibrary dataLibrary) : this(multiScreen, dataLibrary)
		{
			if (savedvisualstate == null)
			{
				this.isFirstOpen = true;
				return;
			}
			this.musicVisualState = savedvisualstate.MusicVS;
			this.videoVisualState = savedvisualstate.VideoVS;
			this.imageVisualState = savedvisualstate.PhotoVS;
			this.currentContent = savedvisualstate.CurrentView;
		}

		public MultimediaViewModel(IMultiScreen multiScreen, IDataLibrary dataLibrary) : this(dataLibrary)
		{
			if (multiScreen == null)
			{
				throw new ArgumentNullException("multiScreen is null");
			}
			this.multiScreen = multiScreen;
			this.multiScreen.MultiscreenQueueUpdated += new EventHandler(this.multiScreen_MultiscreenQueueUpdated);
			this.multiScreen.MultiscreenStartFailed += new EventHandler(this.multiScreen_MultiscreenStartFailed);
			this.multiScreen.MultiscreenQueueEnded += new EventHandler(this.multiScreen_MultiscreenQueueEnded);
			this.multiScreen.PushToTvEnded += new EventHandler(this.multiScreen_PushToTvEnded);
			this.multiScreen.PushToTvQueueEnded += new EventHandler<MediaQueueEventArgs>(this.multiScreen_PushToTvQueueEnded);
			this.multiScreen.MultiScreenContentBroken += new EventHandler(this.multiScreen_MultiScreenContentBroken);
			this.multiScreen.MultiScreenContentNotSupported += new EventHandler(this.multiScreen_MultiScreenContentNotSupported);
			this.multiScreen.MultiScreenContentFailed += new EventHandler(this.multiScreen_MultiScreenContentFailed);
			this.multiScreen.MultiscreenCurrentMediaContentUpdated += new EventHandler(this.multiScreen_MultiscreenCurrentMediaContentUpdated);
		}

		public MultimediaViewModel(IDataLibrary dataLibrary)
		{
			this.addFilesCommand = new UICommand(new Action<object>(this.OnAddFiles), null);
			this.addFolderCommand = new UICommand(new Action<object>(this.OnAddFolder), null);
			this.RemoveFilesCommand = new UICommand(new Action<object>(this.OnRemoveFiles), null);
			this.goToImagesCommand = new UICommand(new Action<object>(this.GoToImages), null);
			this.goToMusicCommand = new UICommand(new Action<object>(this.GoToMusic), null);
			this.goToVideoCommand = new UICommand(new Action<object>(this.GoToVideo), null);
			this.playOnTv = new UICommand(new Action<object>(this.OnPlayOnTv), null);
			this.playPreviousFile = new UICommand(new Action<object>(this.OnPlayPreviousFile), new Predicate<object>(this.HasCurrentMediaContentDuration));
			this.play = new UICommand(new Action<object>(this.OnPlay), null);
			this.pause = new UICommand(new Action<object>(this.OnPause), null);
			this.playNextFile = new UICommand(new Action<object>(this.OnPlayNextFile), null);
			this.goToNowPlaying = new UICommand(new Action<object>(this.OnGoToNowPlaying), null);
			this.innerViewChangedCommand = new SmartView2.Core.Commands.Command(new Action<object>(this.OnInnerViewChanged));
			this.replaceQueueItem = new UICommand(new Action<object>(this.OnReplaceQueueItem), null);
			this.removeQueueItem = new UICommand(new Action<object>(this.OnRemoveQueueItem), null);
			this.addToQueue = new UICommand(new Action<object>(this.OnAddToQueue), null);
			this.nowPlayingGoBack = new UICommand(new Action<object>(this.OnNowPlayingGoBack), null);
			this.MoveItemUp = new UICommand(new Action<object>(this.OnMoveItemUp), new Predicate<object>(this.CanMoveItemUp));
			this.MoveItemDown = new UICommand(new Action<object>(this.OnMoveItemDown), new Predicate<object>(this.CanMoveItemDown));
			this.ComboBoxMusicCurrentState = 0;
			this.ComboBoxVICurrentState = 0;
			this.CurrentHeader = CurrentMultimediaHeader.VideoImage;
			this.dataLibrary = dataLibrary;
			this.currentContent = ContentType.Image;
			this.ComboBoxMusicEnabled = true;
			this.RemoveFoldersCommand = new UICommand(new Action<object>(this.OnRemoveFolders), null);
			dataLibrary.RootImageFolder.GetAllFilesList(null);
			this.ComboBoxVIEnabled = true;
		}

		private bool CanMoveItemDown(object obj)
		{
			Content content = obj as Content;
			if (content == null)
			{
				return false;
			}
			int num = this.multiScreen.MediaQueue.ToList<Content>().IndexOf(content);
			return num < this.multiScreen.MediaQueue.Count<Content>() - 1;
		}

		private bool CanMoveItemUp(object obj)
		{
			Content content = obj as Content;
			if (content == null)
			{
				return false;
			}
			int num = this.multiScreen.MediaQueue.ToList<Content>().IndexOf(content);
			return num > 0;
		}

		private Content ConvertMediaContent(IMediaContent media)
		{
			if (media == null)
			{
				return null;
			}
			if (media.Type == "audio")
			{
				return new Track(media.Name, "", Guid.Empty, new Artist(media.ArtistName, null), new Album(media.AlbumName, null), new Genre(string.Empty), null, media.Id);
			}
			if (media.Type == "video")
			{
				return new MultimediaFile(media.Name, "", Guid.Empty, ContentType.Video, DateTime.Now, media.Id);
			}
			if (media.Type != "image")
			{
				return null;
			}
			return new MultimediaFile(media.Name, "", Guid.Empty, ContentType.Image, DateTime.Now, media.Id);
		}

		public static string[] GetFilePath(string defaultpath = "", string mask = "", string dialogname = "Select files to add")
		{
			string str = defaultpath;
			try
			{
				using (OpenFileDialog openFileDialog = new OpenFileDialog())
				{
					openFileDialog.Multiselect = true;
					openFileDialog.Title = dialogname;
					openFileDialog.InitialDirectory = (str.Equals("") ? Environment.SpecialFolder.MyComputer.ToString() : str);
					openFileDialog.Filter = mask;
					if (openFileDialog.ShowDialog() == DialogResult.OK)
					{
						return openFileDialog.FileNames;
					}
				}
			}
			catch (Exception exception)
			{
			}
			return null;
		}

		public static string GetFolderPath(string default_path = "")
		{
			string defaultPath = default_path;
			try
			{
				using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
				{
					if (defaultPath.Equals(""))
					{
						folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
					}
					folderBrowserDialog.SelectedPath = defaultPath;
					if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
					{
						return folderBrowserDialog.SelectedPath;
					}
				}
			}
			catch (Exception exception)
			{
			}
			return "";
		}

		public SavedMultimediaVisualState GetVisualState()
		{
			this.SaveInnerVisualState();
			return new SavedMultimediaVisualState(this.musicVisualState, this.videoVisualState, this.imageVisualState, this.currentContent, this.multiScreen, this.dataLibrary);
		}

		private async void GoToImages(object obj)
		{
			if (!(this.InnerController.CurrentVM is VideoImageViewModel) || this.currentContent != ContentType.Image)
			{
				if (!(this.InnerController.CurrentVM is MediaPlayerViewModel) || (this.InnerController.CurrentVM as MediaPlayerViewModel).ContentType != ContentType.Image)
				{
					this.SaveInnerVisualState();
					if (this.imageVisualState != null)
					{
						this.SetVICBState(this.imageVisualState.ViewType);
					}
					this.previousContentType = ContentType.Image;
					IPageController innerController = this.InnerController;
					VideoImageViewModel videoImageViewModel = new VideoImageViewModel(this.imageVisualState, this.dataLibrary, this.multiScreen, this.dataLibrary.RootImageFolder, ContentType.Image)
					{
						RemoveFilesCommand = new SmartView2.Core.Commands.Command(new Action<object>(this.OnRemoveFiles))
					};
					await innerController.Navigate(videoImageViewModel, false);
					this.currentContent = ContentType.Image;
					VideoImageViewModel currentVM = this.InnerController.CurrentVM as VideoImageViewModel;
					if (currentVM != null)
					{
						if (this.imageVisualState == null)
						{
							this.SetVICBState(currentVM.ViewType);
						}
						int viewType = (int)currentVM.ViewType;
						if (viewType == 3)
						{
							this.ComboBoxVICurrentState = 0;
							this.ComboBoxVIEnabled = false;
						}
						else if (viewType != 2 || currentVM.CurrentFolder == this.dataLibrary.RootImageFolder)
						{
							this.ComboBoxVIEnabled = true;
						}
						else
						{
							this.ComboBoxVIEnabled = false;
						}
						this.CurrentHeader = CurrentMultimediaHeader.VideoImage;
					}
				}
			}
		}

		private async void GoToMusic(object obj)
		{
			if (!(this.InnerController.CurrentVM is MusicViewModel))
			{
				if (!(this.InnerController.CurrentVM is MediaPlayerViewModel) || (this.InnerController.CurrentVM as MediaPlayerViewModel).ContentType != ContentType.Track)
				{
					this.SaveInnerVisualState();
					if (this.musicVisualState != null && this.musicVisualState.ViewType == MusicViewType.NoContent)
					{
						this.ComboBoxVICurrentState = 0;
						this.ComboBoxVIEnabled = false;
					}
					this.previousContentType = ContentType.Track;
					IPageController innerController = this.InnerController;
					MusicViewModel musicViewModel = new MusicViewModel(this.musicVisualState, this.dataLibrary, this.multiScreen)
					{
						RemoveFilesCommand = new SmartView2.Core.Commands.Command(new Action<object>(this.OnRemoveFiles))
					};
					await innerController.Navigate(musicViewModel, false);
					this.currentContent = ContentType.Track;
					MusicViewModel currentVM = this.InnerController.CurrentVM as MusicViewModel;
					if (currentVM != null)
					{
						if (this.musicVisualState == null)
						{
							this.SetMusicCBState((int)currentVM.ViewType);
						}
						int viewType = (int)currentVM.ViewType;
						if (viewType == 8)
						{
							this.ComboBoxMusicCurrentState = 0;
							this.ComboBoxMusicEnabled = false;
						}
						else if (viewType == 2 || viewType == 4 || viewType == 6 || viewType == 7 && currentVM.CurrentFolder.ID != this.dataLibrary.RootMusicFolder.ID)
						{
							this.ComboBoxMusicEnabled = false;
						}
						else
						{
							this.ComboBoxMusicEnabled = true;
						}
						if (this.musicVisualState != null)
						{
							this.SetMusicCBState((int)this.musicVisualState.ViewType);
						}
						this.CurrentHeader = CurrentMultimediaHeader.Music;
					}
				}
			}
		}

		private async void GoToVideo(object obj)
		{
			if (!(this.InnerController.CurrentVM is VideoImageViewModel) || this.currentContent != ContentType.Video)
			{
				if (!(this.InnerController.CurrentVM is MediaPlayerViewModel) || (this.InnerController.CurrentVM as MediaPlayerViewModel).ContentType != ContentType.Video)
				{
					this.SaveInnerVisualState();
					if (this.videoVisualState != null)
					{
						this.SetVICBState(this.videoVisualState.ViewType);
					}
					this.previousContentType = ContentType.Video;
					IPageController innerController = this.InnerController;
					VideoImageViewModel videoImageViewModel = new VideoImageViewModel(this.videoVisualState, this.dataLibrary, this.multiScreen, this.dataLibrary.RootVideoFolder, ContentType.Video)
					{
						RemoveFilesCommand = new SmartView2.Core.Commands.Command(new Action<object>(this.OnRemoveFiles))
					};
					await innerController.Navigate(videoImageViewModel, false);
					VideoImageViewModel currentVM = this.InnerController.CurrentVM as VideoImageViewModel;
					this.currentContent = ContentType.Video;
					if (currentVM != null)
					{
						if (this.videoVisualState == null)
						{
							this.SetVICBState(currentVM.ViewType);
						}
						int viewType = (int)currentVM.ViewType;
						if (viewType == 3)
						{
							this.ComboBoxVICurrentState = 0;
							this.ComboBoxVIEnabled = false;
						}
						else if (viewType != 2 || currentVM.CurrentFolder == this.dataLibrary.RootVideoFolder)
						{
							this.ComboBoxVIEnabled = true;
						}
						else
						{
							this.ComboBoxVIEnabled = false;
						}
						this.currentContent = ContentType.Video;
						this.CurrentHeader = CurrentMultimediaHeader.VideoImage;
					}
				}
			}
		}

		private bool HasCurrentMediaContentDuration(object obj)
		{
			if (this.multiScreen.CurrentMediaContent == null)
			{
				return false;
			}
			return this.multiScreen.CurrentMediaContent.ContentType != ContentType.Image;
		}

		private bool IsQueueNotEmpty(object obj)
		{
			if (this.multiScreen.MediaQueue == null)
			{
				return false;
			}
			return this.multiScreen.MediaQueue.Count<Content>() > 0;
		}

		private void multiScreen_MultiScreenContentBroken(object sender, EventArgs e)
		{
			base.Controller.ShowMessage(ResourcesModel.Instanse.MAPP_SID_THERE_IS_NO_CONTENT_TO_PLAY);
		}

		private void multiScreen_MultiScreenContentFailed(object sender, EventArgs e)
		{
			base.Controller.Dispatcher.Invoke(() => this.messagePopup.Show(new string[] { ResourcesModel.Instanse.COM_SID_ERROR, ResourcesModel.Instanse.MAPP_SID_UNABLE_SEND_TV_TRY }));
		}

		private void multiScreen_MultiScreenContentNotSupported(object sender, EventArgs e)
		{
			base.Controller.Dispatcher.Invoke(() => this.messagePopup.Show(new string[] { ResourcesModel.Instanse.COM_SID_ERROR, ResourcesModel.Instanse.MAPP_SID_CONTENT_SENT_BECAUSE_FORMAT_NOT }));
		}

		private void multiScreen_MultiscreenCurrentMediaContentUpdated(object sender, EventArgs e)
		{
			if (this.multiScreen.CurrentMediaContent != null && this.multiScreen.CurrentMediaContent.ContentType != ContentType.Image)
			{
				List<Content> allFilesList = null;
				if (this.multiScreen.CurrentMediaContent.ContentType != ContentType.Track)
				{
					if (this.multiScreen.CurrentMediaTimeInfo != null && this.multiScreen.CurrentMediaTimeInfo.Duration == ((MultimediaFile)this.multiScreen.CurrentMediaContent).Duration.TotalSeconds)
					{
						return;
					}
					allFilesList = this.dataLibrary.RootVideoFolder.GetAllFilesList(null);
				}
				else
				{
					if (this.multiScreen.CurrentMediaTimeInfo != null && this.multiScreen.CurrentMediaTimeInfo.Duration == ((Track)this.multiScreen.CurrentMediaContent).Duration.TotalSeconds)
					{
						return;
					}
					allFilesList = this.dataLibrary.RootMusicFolder.GetAllFilesList(null);
				}
				Content content = allFilesList.FirstOrDefault<Content>((Content f) => f.ID == this.multiScreen.CurrentMediaContent.ID);
				if (content != null)
				{
					if (content.ContentType == ContentType.Track)
					{
						if (this.multiScreen.CurrentMediaTimeInfo == null)
						{
							IMultiScreen multiScreen = this.multiScreen;
							MediaTimeInfo mediaTimeInfo = new MediaTimeInfo()
							{
								Duration = ((Track)content).Duration.TotalSeconds,
								CurrentTime = 0,
								Media = null
							};
							multiScreen.CurrentMediaTimeInfo = mediaTimeInfo;
							base.OnPropertyChanged(this, "CurrentMediaTimeInfo");
							CommandManager.InvalidateRequerySuggested();
							return;
						}
						IMultiScreen multiScreen1 = this.multiScreen;
						MediaTimeInfo mediaTimeInfo1 = new MediaTimeInfo()
						{
							Duration = ((Track)content).Duration.TotalSeconds,
							CurrentTime = this.multiScreen.CurrentMediaTimeInfo.CurrentTime,
							Media = this.multiScreen.CurrentMediaTimeInfo.Media
						};
						multiScreen1.CurrentMediaTimeInfo = mediaTimeInfo1;
						base.OnPropertyChanged(this, "CurrentMediaTimeInfo");
						CommandManager.InvalidateRequerySuggested();
						return;
					}
					if (this.multiScreen.CurrentMediaTimeInfo == null)
					{
						IMultiScreen multiScreen2 = this.multiScreen;
						MediaTimeInfo mediaTimeInfo2 = new MediaTimeInfo()
						{
							Duration = ((MultimediaFile)content).Duration.TotalSeconds,
							CurrentTime = 0,
							Media = null
						};
						multiScreen2.CurrentMediaTimeInfo = mediaTimeInfo2;
						base.OnPropertyChanged(this, "CurrentMediaTimeInfo");
						CommandManager.InvalidateRequerySuggested();
						return;
					}
					IMultiScreen multiScreen3 = this.multiScreen;
					MediaTimeInfo mediaTimeInfo3 = new MediaTimeInfo()
					{
						Duration = ((MultimediaFile)content).Duration.TotalSeconds,
						CurrentTime = this.multiScreen.CurrentMediaTimeInfo.CurrentTime,
						Media = this.multiScreen.CurrentMediaTimeInfo.Media
					};
					multiScreen3.CurrentMediaTimeInfo = mediaTimeInfo3;
					base.OnPropertyChanged(this, "CurrentMediaTimeInfo");
					CommandManager.InvalidateRequerySuggested();
				}
			}
		}

		private void multiScreen_MultiscreenQueueEnded(object sender, EventArgs e)
		{
			if (this.currentHeader == CurrentMultimediaHeader.NowPlaying)
			{
				this.OnNowPlayingGoBack(null);
			}
		}

		private void multiScreen_MultiscreenQueueUpdated(object sender, EventArgs e)
		{
			base.OnPropertyChanged(this, "IsQueueReady");
			CommandManager.InvalidateRequerySuggested();
		}

		private void multiScreen_MultiscreenStartFailed(object sender, EventArgs e)
		{
			base.Controller.Dispatcher.Invoke(() => this.messagePopup.Show(new string[] { ResourcesModel.Instanse.COM_SID_ERROR, ResourcesModel.Instanse.MAPP_SID_UNABLE_SEND_TV_TRY }));
		}

		private void multiScreen_PushToTvEnded(object sender, EventArgs e)
		{
			base.Controller.ShowMessage(ResourcesModel.Instanse.MAPP_SID_PLAY_ON_TV);
		}

		private void multiScreen_PushToTvQueueEnded(object sender, MediaQueueEventArgs e)
		{
			if (e.AddedFiles > 0)
			{
				base.Controller.ShowMessage(ResourcesModel.Instanse.MAPP_SID_ADDED_TO_QUEUE);
				return;
			}
			if (e.RepeatedFiles > 0)
			{
				base.Controller.ShowMessage(ResourcesModel.Instanse.MAPP_SID_THIS_CONTENT_IS_ALREADY_SELECTED);
			}
		}

		private async void OnAddFiles(object obj)
		{
			switch (this.currentContent)
			{
				case ContentType.Image:
				{
					string[] filePath = MultimediaViewModel.GetFilePath("", this.imageSearchMask, "Select files to add");
					if (filePath != null)
					{
						await this.dataLibrary.AddContentFromFiles(filePath, this.currentContent);
						break;
					}
					else
					{
						return;
					}
				}
				case ContentType.Track:
				{
					string[] strArrays = MultimediaViewModel.GetFilePath("", this.searchMask, "Select files to add");
					await this.dataLibrary.AddContentFromFiles(strArrays, this.currentContent);
					break;
				}
				case ContentType.Video:
				{
					string[] filePath1 = MultimediaViewModel.GetFilePath("", this.videoSearchMask, "Select files to add");
					if (filePath1 != null)
					{
						await this.dataLibrary.AddContentFromFiles(filePath1, this.currentContent);
						break;
					}
					else
					{
						return;
					}
				}
			}
		}

		private async void OnAddFolder(object obj)
		{
			string folderPath = MultimediaViewModel.GetFolderPath("");
			if (!string.IsNullOrEmpty(folderPath))
			{
				await this.dataLibrary.AddContentFromFolder(folderPath, this.currentContent);
			}
		}

		private void OnAddToQueue(object obj)
		{
			IEnumerable<object> objs = obj as IEnumerable<object>;
			if (objs != null)
			{
				this.multiScreen.PushMediaToTvQueue(objs.Cast<Content>());
			}
			else if (obj is Content)
			{
				this.multiScreen.PushMediaToTvQueue(obj as Content);
			}
			this.IsSelectionMode = false;
		}

		public override async Task OnCreateAsync()
		{
			//await this.<>n__FabricatedMethod2();
			this.messagePopup = base.Controller.CreatePopup(new MessagePopupViewModel(), false);
		}

		private void OnDeleteQueueFile(object obj)
		{
			if (obj != null && obj is Content)
			{
				Content content = obj as Content;
				if (!string.IsNullOrEmpty(content.QueueId))
				{
					this.multiScreen.DeleteQueueItem(content.QueueId);
				}
			}
		}

		public override async Task OnDestroyAsync()
		{
			//await this.<>n__FabricatedMethod10();
			UIFoundation.Navigation.Controller innerController = this.InnerController as UIFoundation.Navigation.Controller;
			innerController.ViewModelChangedEvent -= new Action<ViewModelChangedEventArgs>(this.OnInnerVewModelChanged);
			this.messagePopup.Dispose();
			this.multiScreen.MultiscreenQueueUpdated -= new EventHandler(this.multiScreen_MultiscreenQueueUpdated);
			this.multiScreen.MultiscreenStartFailed -= new EventHandler(this.multiScreen_MultiscreenStartFailed);
			this.multiScreen.MultiscreenQueueEnded -= new EventHandler(this.multiScreen_MultiscreenQueueEnded);
			this.multiScreen.PushToTvQueueEnded -= new EventHandler<MediaQueueEventArgs>(this.multiScreen_PushToTvQueueEnded);
			this.multiScreen.PushToTvEnded -= new EventHandler(this.multiScreen_PushToTvEnded);
			await this.InnerController.Dispose();
		}

		private async void OnGoToNowPlaying(object obj)
		{
			if (!(this.InnerController.CurrentVM is MultimediaNowPlayingViewModel))
			{
				this.previousContentType = this.currentContent;
				this.SaveInnerVisualState();
				await this.InnerController.Navigate(new MultimediaNowPlayingViewModel(this.GetVisualState(), this.multiScreen), true);
			}
		}

		private void OnInnerVewModelChanged(ViewModelChangedEventArgs e)
		{
			if (e.NewViewModel is MediaPlayerViewModel)
			{
				this.CurrentHeader = CurrentMultimediaHeader.None;
				this.IsSelectionMode = false;
			}
			else if (e.NewViewModel is MusicViewModel)
			{
				this.CurrentHeader = CurrentMultimediaHeader.Music;
				(e.NewViewModel as MusicViewModel).ViewChangedEvent += new MusicViewModel.ViewChanged(this.OnInnerVievChanged);
			}
			else if (!(e.NewViewModel is VideoImageViewModel))
			{
				this.CurrentHeader = CurrentMultimediaHeader.NowPlaying;
				this.IsSelectionMode = false;
			}
			else
			{
				this.CurrentHeader = CurrentMultimediaHeader.VideoImage;
				(e.NewViewModel as VideoImageViewModel).ViewChangedEvent += new VideoImageViewModel.ViewChanged(this.OnInnerVievChanged);
			}
			if (e.OldViewModel is MusicViewModel)
			{
				(e.OldViewModel as MusicViewModel).ViewChangedEvent -= new MusicViewModel.ViewChanged(this.OnInnerVievChanged);
				return;
			}
			if (e.OldViewModel is VideoImageViewModel)
			{
				(e.OldViewModel as VideoImageViewModel).ViewChangedEvent -= new VideoImageViewModel.ViewChanged(this.OnInnerVievChanged);
			}
		}

		private void OnInnerVievChanged()
		{
			if (this.InnerController.CurrentVM is MusicViewModel)
			{
				MusicViewModel currentVM = this.InnerController.CurrentVM as MusicViewModel;
				int viewType = (int)currentVM.ViewType;
				if (viewType == 8)
				{
					this.ComboBoxMusicCurrentState = 0;
					this.ComboBoxMusicEnabled = false;
				}
				else if (viewType == 2 || viewType == 4 || viewType == 6 || viewType == 7 && currentVM.CurrentFolder.ID != this.dataLibrary.RootMusicFolder.ID)
				{
					this.ComboBoxMusicEnabled = false;
				}
				else
				{
					this.ComboBoxMusicEnabled = true;
				}
			}
			if (this.InnerController.CurrentVM is VideoImageViewModel)
			{
				VideoImageViewModel videoImageViewModel = this.InnerController.CurrentVM as VideoImageViewModel;
				MultimediaFolder multimediaFolder = new MultimediaFolder();
				if (videoImageViewModel.ContentType == ContentType.Video)
				{
					multimediaFolder = this.dataLibrary.RootVideoFolder;
				}
				else if (videoImageViewModel.ContentType == ContentType.Image)
				{
					multimediaFolder = this.dataLibrary.RootImageFolder;
				}
				int num = (int)videoImageViewModel.ViewType;
				if (num == 3)
				{
					this.ComboBoxVICurrentState = 0;
					this.ComboBoxVIEnabled = false;
					return;
				}
				if (num == 2 && videoImageViewModel.CurrentFolder != multimediaFolder)
				{
					this.ComboBoxVIEnabled = false;
					return;
				}
				this.ComboBoxVIEnabled = true;
			}
		}

		private void OnInnerViewChanged(object obj)
		{
			if (this.InnerController.CurrentVM is MusicViewModel)
			{
				(this.InnerController.CurrentVM as MusicViewModel).OnMainViewChanged(obj);
			}
			if (this.InnerController.CurrentVM is VideoImageViewModel)
			{
				(this.InnerController.CurrentVM as VideoImageViewModel).OnMainViewChanged(obj);
			}
		}

		private void OnMoveItemDown(object obj)
		{
			if (obj is Content)
			{
				Content content = obj as Content;
				int num = this.multiScreen.MediaQueue.ToList<Content>().IndexOf(content);
				this.multiScreen.MoveQueueItem(content.QueueId, num + 1);
			}
		}

		private void OnMoveItemUp(object obj)
		{
			if (obj is Content)
			{
				Content content = obj as Content;
				int num = this.multiScreen.MediaQueue.ToList<Content>().IndexOf(content);
				this.multiScreen.MoveQueueItem(content.QueueId, num - 1);
			}
		}

		public override async Task OnNavigateFromAsync()
		{
			//await this.<>n__FabricatedMethod14();
			this.multiScreen.MultiscreenQueueUpdated -= new EventHandler(this.multiScreen_MultiscreenQueueUpdated);
			this.multiScreen.MultiscreenStartFailed -= new EventHandler(this.multiScreen_MultiscreenStartFailed);
			this.multiScreen.MultiscreenQueueEnded -= new EventHandler(this.multiScreen_MultiscreenQueueEnded);
			this.multiScreen.PushToTvEnded -= new EventHandler(this.multiScreen_PushToTvEnded);
			this.multiScreen.PushToTvQueueEnded -= new EventHandler<MediaQueueEventArgs>(this.multiScreen_PushToTvQueueEnded);
			this.multiScreen.MultiScreenContentBroken -= new EventHandler(this.multiScreen_MultiScreenContentBroken);
			this.multiScreen.MultiScreenContentNotSupported -= new EventHandler(this.multiScreen_MultiScreenContentNotSupported);
			this.multiScreen.MultiScreenContentFailed -= new EventHandler(this.multiScreen_MultiScreenContentFailed);
			this.multiScreen.Close();
		}

		public override async Task OnNavigateToAsync()
		{
			//await this.<>n__FabricatedMethodb();
			if (this.isFirstOpen && !Settings.Default.MultimediaIntroductionDoNotShow)
			{
				base.Controller.CreatePopup(new MultimediaIntroductionPopupViewModel(), false).Show();
			}
			if (this.currentContent != ContentType.Image || this.imageVisualState != null)
			{
				switch (this.currentContent)
				{
					case ContentType.Image:
					{
						MultimediaViewModel controller = this;
						VideoImageViewModel videoImageViewModel = new VideoImageViewModel(this.imageVisualState, this.dataLibrary, this.multiScreen, this.dataLibrary.RootImageFolder, ContentType.Image)
						{
							RemoveFilesCommand = new SmartView2.Core.Commands.Command(new Action<object>(this.OnRemoveFiles))
						};
						controller.InnerController = new UIFoundation.Navigation.Controller(videoImageViewModel, base.Controller);
						VideoImageViewModel currentVM = this.InnerController.CurrentVM as VideoImageViewModel;
						int viewType = (int)currentVM.ViewType;
						if (viewType == 3)
						{
							this.ComboBoxVICurrentState = 0;
							this.ComboBoxVIEnabled = false;
						}
						else if (viewType != 2 || currentVM.CurrentFolder == this.dataLibrary.RootImageFolder)
						{
							this.ComboBoxVIEnabled = true;
						}
						else
						{
							this.ComboBoxVIEnabled = false;
						}
						this.CurrentHeader = CurrentMultimediaHeader.VideoImage;
						this.SetVICBState(this.imageVisualState.ViewType);
						this.PhotoChecked = true;
						break;
					}
					case ContentType.Track:
					{
						MultimediaViewModel multimediaViewModel = this;
						MusicViewModel musicViewModel = new MusicViewModel(this.musicVisualState, this.dataLibrary, this.multiScreen)
						{
							RemoveFilesCommand = new SmartView2.Core.Commands.Command(new Action<object>(this.OnRemoveFiles))
						};
						multimediaViewModel.InnerController = new UIFoundation.Navigation.Controller(musicViewModel, base.Controller);
						MusicViewModel currentVM1 = this.InnerController.CurrentVM as MusicViewModel;
						int num = (int)currentVM1.ViewType;
						if (num == 8)
						{
							this.ComboBoxMusicCurrentState = 0;
							this.ComboBoxMusicEnabled = false;
						}
						else if (num == 2 || num == 4 || num == 6 || num == 7 && currentVM1.CurrentFolder.ID != this.dataLibrary.RootMusicFolder.ID)
						{
							this.ComboBoxMusicEnabled = false;
						}
						else
						{
							this.ComboBoxMusicEnabled = true;
						}
						this.CurrentHeader = CurrentMultimediaHeader.Music;
						this.SetMusicCBState((int)this.musicVisualState.ViewType);
						this.MusicChecked = true;
						break;
					}
					case ContentType.Video:
					{
						MultimediaViewModel controller1 = this;
						VideoImageViewModel videoImageViewModel1 = new VideoImageViewModel(this.videoVisualState, this.dataLibrary, this.multiScreen, this.dataLibrary.RootVideoFolder, ContentType.Video)
						{
							RemoveFilesCommand = new SmartView2.Core.Commands.Command(new Action<object>(this.OnRemoveFiles))
						};
						controller1.InnerController = new UIFoundation.Navigation.Controller(videoImageViewModel1, base.Controller);
						VideoImageViewModel currentVM2 = this.InnerController.CurrentVM as VideoImageViewModel;
						int viewType1 = (int)currentVM2.ViewType;
						if (viewType1 == 3)
						{
							this.ComboBoxVICurrentState = 0;
							this.ComboBoxVIEnabled = false;
						}
						else if (viewType1 != 2 || currentVM2.CurrentFolder == this.dataLibrary.RootVideoFolder)
						{
							this.ComboBoxVIEnabled = true;
						}
						else
						{
							this.ComboBoxVIEnabled = false;
						}
						this.CurrentHeader = CurrentMultimediaHeader.VideoImage;
						this.SetVICBState(this.videoVisualState.ViewType);
						this.VideoChecked = true;
						break;
					}
				}
			}
			else
			{
				MultimediaViewModel multimediaViewModel1 = this;
				VideoImageViewModel videoImageViewModel2 = new VideoImageViewModel(this.dataLibrary, this.multiScreen, this.dataLibrary.RootImageFolder, ContentType.Image)
				{
					RemoveFilesCommand = new SmartView2.Core.Commands.Command(new Action<object>(this.OnRemoveFiles))
				};
				multimediaViewModel1.InnerController = new UIFoundation.Navigation.Controller(videoImageViewModel2, base.Controller);
				if (this.dataLibrary.RootImageFolder.GetAllFilesList(null).Count < 1)
				{
					this.ComboBoxVIEnabled = false;
				}
				this.PhotoChecked = true;
			}
			UIFoundation.Navigation.Controller innerController = this.InnerController as UIFoundation.Navigation.Controller;
			innerController.ViewModelChangedEvent += new Action<ViewModelChangedEventArgs>(this.OnInnerVewModelChanged);
		}

		private async void OnNowPlayingGoBack(object obj)
		{
			if (this.previousContentType == ContentType.Track)
			{
				IPageController innerController = this.InnerController;
				MusicViewModel musicViewModel = new MusicViewModel(this.musicVisualState, this.dataLibrary, this.multiScreen)
				{
					RemoveFilesCommand = new SmartView2.Core.Commands.Command(new Action<object>(this.OnRemoveFiles))
				};
				await innerController.Navigate(musicViewModel, true);
			}
			else if (this.previousContentType != ContentType.Video)
			{
				IPageController pageController = this.InnerController;
				VideoImageViewModel videoImageViewModel = new VideoImageViewModel(this.imageVisualState, this.dataLibrary, this.multiScreen, this.dataLibrary.RootImageFolder, ContentType.Image)
				{
					RemoveFilesCommand = new SmartView2.Core.Commands.Command(new Action<object>(this.OnRemoveFiles))
				};
				await pageController.Navigate(videoImageViewModel, true);
			}
			else
			{
				IPageController innerController1 = this.InnerController;
				VideoImageViewModel videoImageViewModel1 = new VideoImageViewModel(this.videoVisualState, this.dataLibrary, this.multiScreen, this.dataLibrary.RootVideoFolder, ContentType.Video)
				{
					RemoveFilesCommand = new SmartView2.Core.Commands.Command(new Action<object>(this.OnRemoveFiles))
				};
				await innerController1.Navigate(videoImageViewModel1, true);
			}
			this.OnInnerVievChanged();
		}

		private void OnPause(object obj)
		{
			this.multiScreen.MediaPause();
		}

		private void OnPlay(object obj)
		{
			this.multiScreen.MediaPlay();
		}

		private void OnPlayNextFile(object obj)
		{
			this.multiScreen.QueueNext();
		}

		private void OnPlayOnTv(object obj)
		{
			this.multiScreen.QueuePlay();
			CommandManager.InvalidateRequerySuggested();
		}

		private void OnPlayPreviousFile(object obj)
		{
			this.multiScreen.SetCurrentMediaTimePosition(0);
		}

		private async void OnRemoveFiles(object obj)
		{
			bool flag;
			if (this.selectedItems != null && this.selectedItems.Any<object>())
			{
				if (!Settings.Default.RemoveFileNotificationDoNotShowAgain)
				{
					using (PopupWrapper popupWrapper = base.Controller.CreatePopup(new YesNoPopupViewModel(ResourcesModel.Instanse.MAPP_SID_REMOVE_FILES, ResourcesModel.Instanse.MAPP_SID_REMOVE_FILE_MESSAGE, true), false))
					{
						AlternativePopupEventArgs alternativePopupEventArg = await popupWrapper.ShowDialogAsync() as AlternativePopupEventArgs;
						if (alternativePopupEventArg != null)
						{
							Settings.Default.RemoveFileNotificationDoNotShowAgain = alternativePopupEventArg.CheckBoxState;
							Settings.Default.Save();
							bool? decision = alternativePopupEventArg.Decision;
							flag = (!decision.GetValueOrDefault() ? true : !decision.HasValue);
							if (flag)
							{
								return;
							}
						}
						else
						{
							return;
						}
					}
				}
				IEnumerable<object> objs = this.selectedItems;
				IEnumerable<Content> contents = (
					from item in objs
					where item is Content
					select item).Cast<Content>();
				IDataLibrary dataLibrary = this.dataLibrary;
				MultimediaFolder rootImageFolder = this.dataLibrary.RootImageFolder;
				IEnumerable<Content> contents1 = contents;
				dataLibrary.DeleteItems(rootImageFolder, (
					from file in contents1
					where file.ContentType == ContentType.Image
					select file).ToList<Content>());
				IDataLibrary dataLibrary1 = this.dataLibrary;
				MultimediaFolder rootVideoFolder = this.dataLibrary.RootVideoFolder;
				IEnumerable<Content> contents2 = contents;
				dataLibrary1.DeleteItems(rootVideoFolder, (
					from file in contents2
					where file.ContentType == ContentType.Video
					select file).ToList<Content>());
				IDataLibrary dataLibrary2 = this.dataLibrary;
				MultimediaFolder rootMusicFolder = this.dataLibrary.RootMusicFolder;
				IEnumerable<Content> contents3 = contents;
				dataLibrary2.DeleteItems(rootMusicFolder, (
					from file in contents3
					where file.ContentType == ContentType.Track
					select file).ToList<Content>());
				this.IsSelectionMode = false;
			}
		}

		private async void OnRemoveFolders(object obj)
		{
			bool flag;
			if (this.selectedItems != null && this.selectedItems.Any<object>())
			{
				if (!Settings.Default.RemoveFolderNotificationDoNotShowAgain)
				{
					using (PopupWrapper popupWrapper = base.Controller.CreatePopup(new YesNoPopupViewModel(ResourcesModel.Instanse.MAPP_SID_REMOVE_FOLDER, ResourcesModel.Instanse.MAPP_SID_SURE_WANT_TO_REMOVE_SELECTED_FOLDERS, true), false))
					{
						AlternativePopupEventArgs alternativePopupEventArg = await popupWrapper.ShowDialogAsync() as AlternativePopupEventArgs;
						if (alternativePopupEventArg != null)
						{
							Settings.Default.RemoveFolderNotificationDoNotShowAgain = alternativePopupEventArg.CheckBoxState;
							Settings.Default.Save();
							bool? decision = alternativePopupEventArg.Decision;
							flag = (!decision.GetValueOrDefault() ? true : !decision.HasValue);
							if (flag)
							{
								return;
							}
						}
						else
						{
							return;
						}
					}
				}
				IEnumerable<object> objs = this.selectedItems;
				IEnumerable<MultimediaFolder> multimediaFolders = (
					from item in objs
					where item is MultimediaFolder
					select item).Cast<MultimediaFolder>();
				if (this.InnerController.CurrentVM is VideoImageViewModel)
				{
					switch (((VideoImageViewModel)this.InnerController.CurrentVM).ContentType)
					{
						case ContentType.Image:
						{
							using (IEnumerator<MultimediaFolder> enumerator = multimediaFolders.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									MultimediaFolder current = enumerator.Current;
									this.dataLibrary.DeleteFileOrFolder(this.dataLibrary.RootImageFolder, current);
								}
								break;
							}
							break;
						}
						case ContentType.Video:
						{
							using (IEnumerator<MultimediaFolder> enumerator1 = multimediaFolders.GetEnumerator())
							{
								while (enumerator1.MoveNext())
								{
									MultimediaFolder multimediaFolder = enumerator1.Current;
									this.dataLibrary.DeleteFileOrFolder(this.dataLibrary.RootVideoFolder, multimediaFolder);
								}
								break;
							}
							break;
						}
					}
				}
				if (this.InnerController.CurrentVM is MusicViewModel)
				{
					MusicViewModel currentVM = (MusicViewModel)this.InnerController.CurrentVM;
					foreach (MultimediaFolder multimediaFolder1 in multimediaFolders)
					{
						this.dataLibrary.DeleteFileOrFolder(this.dataLibrary.RootMusicFolder, multimediaFolder1);
					}
				}
				this.IsSelectionMode = false;
			}
		}

		private void OnRemoveQueueItem(object obj)
		{
			if (obj is Content)
			{
				Content content = (Content)obj;
				this.multiScreen.DeleteQueueItem(content.QueueId);
			}
		}

		private void OnReplaceQueueItem(object obj)
		{
			if (obj is Tuple<object, object>)
			{
				Tuple<object, object> tuple = (Tuple<object, object>)obj;
				Content item1 = tuple.Item1 as Content;
				Content item2 = tuple.Item2 as Content;
				if (item1 != null && item2 != null)
				{
					List<Content> list = this.multiScreen.MediaQueue.ToList<Content>();
					list.IndexOf(item2);
					int num = list.IndexOf(item1);
					this.multiScreen.MoveQueueItem(item2.QueueId, num);
				}
			}
		}

		private void SaveInnerVisualState()
		{
			if (this.InnerController == null || this.InnerController.CurrentVM is MultimediaNowPlayingViewModel)
			{
				return;
			}
			object currentVM = this.InnerController.CurrentVM;
			MediaPlayerViewModel mediaPlayerViewModel = currentVM as MediaPlayerViewModel;
			if (mediaPlayerViewModel == null)
			{
				MusicViewModel musicViewModel = currentVM as MusicViewModel;
				if (musicViewModel != null)
				{
					this.musicVisualState = musicViewModel.SaveVisualState(null);
				}
				VideoImageViewModel videoImageViewModel = currentVM as VideoImageViewModel;
				if (videoImageViewModel != null)
				{
					if (this.currentContent == ContentType.Image)
					{
						this.imageVisualState = videoImageViewModel.SaveVisualState(null);
						return;
					}
					this.videoVisualState = videoImageViewModel.SaveVisualState(null);
				}
			}
			else
			{
				BaseSavedVisualState baseSavedVisualState = mediaPlayerViewModel.SavePlayerState();
				if (baseSavedVisualState is SavedMusicVisualState)
				{
					this.musicVisualState = (SavedMusicVisualState)baseSavedVisualState;
					return;
				}
				if (baseSavedVisualState is SavedVideoPhotoVisualstate)
				{
					if (this.currentContent == ContentType.Image)
					{
						this.imageVisualState = (SavedVideoPhotoVisualstate)baseSavedVisualState;
						return;
					}
					this.videoVisualState = (SavedVideoPhotoVisualstate)baseSavedVisualState;
					return;
				}
			}
		}

		private void SetMusicCBState(int state)
		{
			switch (state)
			{
				case 0:
				{
					this.ComboBoxMusicCurrentState = state;
					return;
				}
				case 1:
				case 2:
				{
					this.ComboBoxMusicCurrentState = 1;
					return;
				}
				case 3:
				case 4:
				{
					this.ComboBoxMusicCurrentState = 2;
					return;
				}
				case 5:
				case 6:
				{
					this.ComboBoxMusicCurrentState = 3;
					return;
				}
				case 7:
				{
					this.ComboBoxMusicCurrentState = 4;
					return;
				}
				default:
				{
					return;
				}
			}
		}

		private void SetVICBState(VideoPhotoViewType state)
		{
			if (state != VideoPhotoViewType.NoContent)
			{
				this.ComboBoxVICurrentState = (int)state;
				return;
			}
			this.ComboBoxVICurrentState = 0;
			this.ComboBoxVIEnabled = false;
		}
	}
}