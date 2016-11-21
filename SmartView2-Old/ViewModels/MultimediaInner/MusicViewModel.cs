using MediaLibrary.DataModels;
using SmartView2.Core;
using SmartView2.Core.Commands;
using SmartView2.Properties;
using SmartView2.ViewModels;
using SmartView2.ViewModels.MultimediaInner.VisualState;
using SmartView2.ViewModels.Popups;
using SmartView2.Views.MultimediaInner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UIFoundation;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels.MultimediaInner
{
	[RelatedView(typeof(MusicPage))]
	internal class MusicViewModel : PageViewModel
	{
		private ICommand addMusicFilesCommand;

		private ICommand addFolderCommand;

		private ICommand itemDoubleClickCommand;

		private ICommand musicBackCommand;

		private ICommand sendToQueueCommand;

		private ICommand playLocalCommand;

		private ICommand mainViewChangedCommand;

		private ICommand playOnTvCommand;

		private ICommand deleteItemCommand;

		private ICommand dragItem;

		private ICommand contentDropCommand;

		private ICommand cancelAddingCommand;

		private IDataLibrary dataLibrary;

		private ObservableCollection<ItemBase> artistList;

		private ObservableCollection<ItemBase> albumsList;

		private ObservableCollection<ItemBase> genresList;

		private ObservableCollection<ItemBase> tracksList;

		private Track selectedTrack;

		private List<ItemBase> itemsList;

		private Album currentAlbum;

		private Artist currentArtist;

		private string currentGenre;

		public string searchMask = "Music files(*.mp3;*.flac;*.mid)|*.mp3;*.flac;*.mid|All files (*.*)|*.*";

		private MultimediaFolder currentFolder;

		private MusicViewType viewType;

		private readonly IMultiScreen multiScreen;

		private bool isBackVisible;

		private bool temporaryDataLoaded;

		private bool isPlayerResumed;

		private bool sortByTitle;

		private SavedMusicVisualState savedVisualState;

		private MediaPlayerViewModel localPlayerVM;

		private PopupWrapper multimediaLoadingPopup;

		public ICommand AddFolderCommand
		{
			get
			{
				return this.addFolderCommand;
			}
		}

		public ICommand AddMusicFilesCommand
		{
			get
			{
				return this.addMusicFilesCommand;
			}
		}

		public ObservableCollection<ItemBase> AlbumsList
		{
			get
			{
				return this.albumsList;
			}
			set
			{
				base.SetProperty<ObservableCollection<ItemBase>>(ref this.albumsList, value, "AlbumsList");
			}
		}

		public ObservableCollection<ItemBase> ArtistList
		{
			get
			{
				return this.artistList;
			}
			set
			{
				base.SetProperty<ObservableCollection<ItemBase>>(ref this.artistList, value, "ArtistList");
			}
		}

		public ICommand CancelAddingCommand
		{
			get
			{
				return this.cancelAddingCommand;
			}
		}

		public ICommand ContentDropCommand
		{
			get
			{
				return this.contentDropCommand;
			}
		}

		public MediaLibrary.DataModels.ContentType ContentType
		{
			get
			{
				return MediaLibrary.DataModels.ContentType.Track;
			}
		}

		public Album CurrentAlbum
		{
			get
			{
				return this.currentAlbum;
			}
			set
			{
				base.SetProperty<Album>(ref this.currentAlbum, value, "CurrentAlbum");
			}
		}

		public Artist CurrentArtist
		{
			get
			{
				return this.currentArtist;
			}
			set
			{
				base.SetProperty<Artist>(ref this.currentArtist, value, "CurrentArtist");
			}
		}

		public MultimediaFolder CurrentFolder
		{
			get
			{
				return this.currentFolder;
			}
			set
			{
				base.SetProperty<MultimediaFolder>(ref this.currentFolder, value, "CurrentFolder");
				base.OnPropertyChanged(this, "IsInsideFolder");
				base.OnPropertyChanged(this, "IsInRootFolder");
			}
		}

		public string CurrentGenre
		{
			get
			{
				return this.currentGenre;
			}
			set
			{
				base.SetProperty<string>(ref this.currentGenre, value, "CurrentGenre");
			}
		}

		public ICommand DeleteItemCommand
		{
			get
			{
				return this.deleteItemCommand;
			}
		}

		public ICommand DragItem
		{
			get
			{
				return this.dragItem;
			}
		}

		public ObservableCollection<ItemBase> GenresList
		{
			get
			{
				return this.genresList;
			}
			set
			{
				base.SetProperty<ObservableCollection<ItemBase>>(ref this.genresList, value, "GenresList");
			}
		}

		public bool IsBackVisible
		{
			get
			{
				return this.isBackVisible;
			}
			set
			{
				base.SetProperty<bool>(ref this.isBackVisible, value, "IsBackVisible");
			}
		}

		public bool IsInRootFolder
		{
			get
			{
				if (this.CurrentFolder == null)
				{
					return true;
				}
				return this.CurrentFolder == this.dataLibrary.RootMusicFolder;
			}
		}

		public bool IsInsideFolder
		{
			get
			{
				if (this.CurrentFolder == null)
				{
					return false;
				}
				return this.CurrentFolder != this.dataLibrary.RootMusicFolder;
			}
		}

		public ICommand ItemDoubleClickCommand
		{
			get
			{
				return this.itemDoubleClickCommand;
			}
		}

		public List<ItemBase> ItemsList
		{
			get
			{
				return this.itemsList;
			}
			set
			{
				base.SetProperty<List<ItemBase>>(ref this.itemsList, value, "ItemsList");
			}
		}

		public ICommand MainViewChangedCommand
		{
			get
			{
				return this.mainViewChangedCommand;
			}
		}

		public ICommand MusicBackCommand
		{
			get
			{
				return this.musicBackCommand;
			}
		}

		public ICommand PlayLocalCommand
		{
			get
			{
				return this.playLocalCommand;
			}
		}

		public ICommand PlayOnTvCommand
		{
			get
			{
				return this.playOnTvCommand;
			}
		}

		public ICommand RemoveAlbumCommand
		{
			get;
			private set;
		}

		public ICommand RemoveArtistCommand
		{
			get;
			private set;
		}

		public ICommand RemoveFilesCommand
		{
			get;
			set;
		}

		public ICommand RemoveFolderCommand
		{
			get;
			private set;
		}

		public ICommand RemoveGenreCommand
		{
			get;
			private set;
		}

		public Track SelectedTrack
		{
			get
			{
				return this.selectedTrack;
			}
			set
			{
				base.SetProperty<Track>(ref this.selectedTrack, value, "SelectedTrack");
			}
		}

		public ICommand SendToQueueCommand
		{
			get
			{
				return this.sendToQueueCommand;
			}
		}

		public bool SortByTitle
		{
			get
			{
				return this.sortByTitle;
			}
			set
			{
				base.SetProperty<bool>(ref this.sortByTitle, value, "SortByTitle");
				this.ReloadItemsCollection();
			}
		}

		public bool TemporaryDataLoaded
		{
			get
			{
				return this.temporaryDataLoaded;
			}
			set
			{
				if (value != this.temporaryDataLoaded && this.multimediaLoadingPopup != null)
				{
					if (value)
					{
						this.multimediaLoadingPopup.Close();
					}
					else
					{
						this.multimediaLoadingPopup.Show();
					}
				}
				base.SetProperty<bool>(ref this.temporaryDataLoaded, value, "TemporaryDataLoaded");
			}
		}

		public ObservableCollection<ItemBase> TracksList
		{
			get
			{
				return this.tracksList;
			}
			set
			{
				base.SetProperty<ObservableCollection<ItemBase>>(ref this.tracksList, value, "TracksList");
			}
		}

		public MusicViewType ViewType
		{
			get
			{
				return this.viewType;
			}
			set
			{
				base.SetProperty<MusicViewType>(ref this.viewType, value, "ViewType");
				this.OnViewChanged();
			}
		}

		public MusicViewModel(SavedMusicVisualState visualstate, IDataLibrary library, IMultiScreen multiScreen) : this(library, multiScreen)
		{
			if (visualstate == null)
			{
				return;
			}
			this.CurrentFolder = visualstate.CurrentFolder;
			this.ViewType = visualstate.ViewType;
			switch (this.ViewType)
			{
				case MusicViewType.Track:
				{
					this.ReloadItemsCollection();
					break;
				}
				case MusicViewType.ArtistGeneral:
				{
					this.ReloadItemsCollection();
					break;
				}
				case MusicViewType.ArtistDetailed:
				{
					this.ViewType = MusicViewType.ArtistGeneral;
					this.OnItemDoubleClick(visualstate.CurrentItem);
					break;
				}
				case MusicViewType.AlbumGeneral:
				{
					this.ReloadItemsCollection();
					break;
				}
				case MusicViewType.AlbumDetailed:
				{
					this.ViewType = MusicViewType.AlbumGeneral;
					this.OnItemDoubleClick(visualstate.CurrentItem);
					break;
				}
				case MusicViewType.GenreGeneral:
				{
					this.ReloadItemsCollection();
					break;
				}
				case MusicViewType.GenreDetailed:
				{
					this.ViewType = MusicViewType.GenreGeneral;
					this.OnItemDoubleClick(visualstate.CurrentItem);
					break;
				}
				case MusicViewType.Folder:
				{
					if (this.CurrentFolder == this.dataLibrary.RootMusicFolder)
					{
						this.ReloadItemsCollection();
						break;
					}
					else
					{
						this.OnItemDoubleClick(this.CurrentFolder);
						break;
					}
				}
				case MusicViewType.NoContent:
				{
					if (this.dataLibrary.TrackList.Count <= 0)
					{
						break;
					}
					this.ViewType = MusicViewType.Track;
					this.ReloadItemsCollection();
					break;
				}
			}
			if (visualstate.PlayerState != null)
			{
				this.savedVisualState = visualstate;
			}
		}

		public MusicViewModel(IDataLibrary library, IMultiScreen multiScreen)
		{
			if (library == null)
			{
				throw new Exception("VideoViewModel cant work with null library");
			}
			this.dataLibrary = library;
			this.TracksList = new ObservableCollection<ItemBase>();
			this.ArtistList = new ObservableCollection<ItemBase>();
			this.AlbumsList = new ObservableCollection<ItemBase>();
			this.GenresList = new ObservableCollection<ItemBase>();
			this.ItemsList = new List<ItemBase>();
			this.CurrentFolder = this.dataLibrary.RootMusicFolder;
			this.multiScreen = multiScreen;
			this.isBackVisible = false;
			this.ViewType = MusicViewType.Track;
			this.ReloadItemsCollection();
			this.TemporaryDataLoaded = this.dataLibrary.IsDataLoaded;
			this.addMusicFilesCommand = new Command(new Action<object>(this.OnAddMusicFiles));
			this.addFolderCommand = new Command(new Action<object>(this.OnAddFolderMusic));
			this.RemoveFolderCommand = new Command(new Action<object>(this.OnRemoveFolderAsync));
			this.RemoveAlbumCommand = new Command(new Action<object>(this.OnRemoveAlbumAsync));
			this.RemoveArtistCommand = new Command(new Action<object>(this.OnRemoveArtistAsync));
			this.RemoveGenreCommand = new Command(new Action<object>(this.OnRemoveGenreAsync));
			this.itemDoubleClickCommand = new Command(new Action<object>(this.OnItemDoubleClick));
			this.musicBackCommand = new Command(new Action<object>(this.OnMusicBack));
			this.sendToQueueCommand = new Command(new Action<object>(this.OnSendToQueue));
			this.playLocalCommand = new Command(new Action<object>(this.OnPlayLocal));
			this.mainViewChangedCommand = new Command(new Action<object>(this.OnMainViewChanged));
			this.playOnTvCommand = new Command(new Action<object>(this.OnPlayOnTv));
			this.deleteItemCommand = new Command(new Action<object>(this.OnDeleteItem));
			this.dragItem = new Command(new Action<object>(this.OnDragItem));
			this.contentDropCommand = new Command(new Action<object>(this.OnContentDrop));
			this.cancelAddingCommand = new Command(new Action<object>(this.OnCancelAdding));
		}

		private async Task<bool> ConfirmRemoveFilesAsync()
		{
			bool flag;
			bool flag1;
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
						flag1 = (!decision.GetValueOrDefault() ? true : !decision.HasValue);
						if (flag1)
						{
							flag = false;
							return flag;
						}
					}
					else
					{
						flag = false;
						return flag;
					}
				}
			}
			flag = true;
			return flag;
		}

		private async Task<bool> ConfirmRemoveFolderAsync()
		{
			bool flag;
			bool flag1;
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
						flag1 = (!decision.GetValueOrDefault() ? true : !decision.HasValue);
						if (flag1)
						{
							flag = false;
							return flag;
						}
					}
					else
					{
						flag = false;
						return flag;
					}
				}
			}
			flag = true;
			return flag;
		}

		private void dataLibrary_DataUpdated(object sender, EventArgs e)
		{
			this.ReloadItemsCollection();
		}

		private bool IsTrackExist(Content track)
		{
			bool flag;
			try
			{
				flag = (track == null ? false : (new FileInfo(track.Path)).Exists);
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		private async void OnAddFolderMusic(object obj)
		{
			string folderPath = MultimediaViewModel.GetFolderPath("");
			if (!folderPath.Equals(""))
			{
				this.TemporaryDataLoaded = false;
				await this.dataLibrary.AddContentFromFolder(folderPath, MediaLibrary.DataModels.ContentType.Track);
				this.TemporaryDataLoaded = true;
			}
		}

		private async void OnAddMusicFiles(object obj)
		{
			string[] filePath = MultimediaViewModel.GetFilePath("", this.searchMask, "Select files to add");
			this.TemporaryDataLoaded = false;
			await this.dataLibrary.AddContentFromFiles(filePath, MediaLibrary.DataModels.ContentType.Track);
			this.TemporaryDataLoaded = true;
		}

		private void OnCancelAdding(object obj)
		{
			this.dataLibrary.CancelAdding();
		}

		private async void OnContentDrop(object obj)
		{
			string[] strArrays = obj as string[];
			List<string> strs = new List<string>();
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				FileAttributes attributes = File.GetAttributes(strArrays[i]);
				if ((int)(attributes & FileAttributes.Directory) == 0)
				{
					strs.Add(strArrays[i]);
				}
				else
				{
					await this.dataLibrary.AddContentFromFolder(strArrays[i], MediaLibrary.DataModels.ContentType.Track);
				}
			}
			if (strs.Count > 0)
			{
				await this.dataLibrary.AddContentFromFiles(strs.ToArray(), MediaLibrary.DataModels.ContentType.Track);
			}
		}

		public override async Task OnCreateAsync()
		{
			//await this.<>n__FabricatedMethod8();
			this.dataLibrary.DataUpdated += new EventHandler(this.dataLibrary_DataUpdated);
		}

		private void OnDataLoadedChanged(object sender, EventArgs e)
		{
			this.TemporaryDataLoaded = this.dataLibrary.IsDataLoaded;
			if (this.TemporaryDataLoaded)
			{
				this.ReloadItemsCollection();
			}
		}

		private void OnDeleteItem(object obj)
		{
			switch (this.ViewType)
			{
				case MusicViewType.Track:
				{
					this.dataLibrary.DeleteFileOrFolder(this.dataLibrary.RootMusicFolder, (ItemBase)obj);
					this.ReloadItemsCollection();
					return;
				}
				case MusicViewType.ArtistGeneral:
				{
					Track track = (Track)obj;
					List<Content> contents = new List<Content>(this.dataLibrary.GetArtistsTracks(track.Artist.ID));
					this.dataLibrary.DeleteItems(this.dataLibrary.RootMusicFolder, contents);
					this.ReloadItemsCollection();
					return;
				}
				case MusicViewType.ArtistDetailed:
				{
					this.dataLibrary.DeleteFileOrFolder(this.dataLibrary.RootMusicFolder, (ItemBase)obj);
					this.ReloadItemsCollection();
					return;
				}
				case MusicViewType.AlbumGeneral:
				{
					Track track1 = (Track)obj;
					List<Content> contents1 = new List<Content>(this.dataLibrary.GetAlbumTracks(track1.Album.ID));
					this.dataLibrary.DeleteItems(this.dataLibrary.RootMusicFolder, contents1);
					this.ReloadItemsCollection();
					return;
				}
				case MusicViewType.AlbumDetailed:
				{
					this.dataLibrary.DeleteFileOrFolder(this.dataLibrary.RootMusicFolder, (ItemBase)obj);
					this.ReloadItemsCollection();
					return;
				}
				case MusicViewType.GenreGeneral:
				{
					Track track2 = (Track)obj;
					List<Content> contents2 = new List<Content>(this.dataLibrary.GetGenreTracks(track2.Genre.Name));
					this.dataLibrary.DeleteItems(this.dataLibrary.RootMusicFolder, contents2);
					this.ReloadItemsCollection();
					return;
				}
				case MusicViewType.GenreDetailed:
				{
					this.dataLibrary.DeleteFileOrFolder(this.dataLibrary.RootMusicFolder, (ItemBase)obj);
					this.ReloadItemsCollection();
					return;
				}
				case MusicViewType.Folder:
				{
					this.dataLibrary.DeleteFileOrFolder(this.dataLibrary.RootMusicFolder, (ItemBase)obj);
					this.ReloadItemsCollection();
					return;
				}
				default:
				{
					return;
				}
			}
		}

		public override async Task OnDestroyAsync()
		{
			//await this.<>n__FabricatedMethodc();
			this.dataLibrary.DataUpdated -= new EventHandler(this.dataLibrary_DataUpdated);
		}

		private void OnDragItem(object obj)
		{
			switch (this.ViewType)
			{
				case MusicViewType.Track:
				case MusicViewType.ArtistDetailed:
				case MusicViewType.AlbumDetailed:
				case MusicViewType.GenreDetailed:
				case MusicViewType.Folder:
				{
					if (obj is Content)
					{
						List<Content> contents = new List<Content>()
						{
							obj as Content
						};
						DragDrop.DoDragDrop(new UIElement(), contents, DragDropEffects.Copy);
						return;
					}
					if (obj is MultimediaFolder)
					{
						MultimediaFolder multimediaFolder = obj as MultimediaFolder;
						if (multimediaFolder == null)
						{
							return;
						}
						DragDrop.DoDragDrop(new UIElement(), multimediaFolder.GetAllFilesList(null), DragDropEffects.Copy);
						return;
					}
					IEnumerable<object> objs = obj as IEnumerable<object>;
					if (objs == null)
					{
						break;
					}
					Content[] array = objs.OfType<Content>().ToArray<Content>();
					if (array.Count<Content>() > 0)
					{
						DragDrop.DoDragDrop(new UIElement(), array, DragDropEffects.Copy);
						return;
					}
					MultimediaFolder[] multimediaFolderArray = objs.OfType<MultimediaFolder>().ToArray<MultimediaFolder>();
					if (multimediaFolderArray.Count<MultimediaFolder>() <= 0)
					{
						break;
					}
					IEnumerable<Content> contents1 = ((IEnumerable<MultimediaFolder>)multimediaFolderArray).SelectMany<MultimediaFolder, Content>((MultimediaFolder p) => p.GetFilesList());
					DragDrop.DoDragDrop(new UIElement(), contents1, DragDropEffects.Copy);
					return;
				}
				case MusicViewType.ArtistGeneral:
				{
					if (obj is Track)
					{
						Track track = obj as Track;
						if (track == null)
						{
							return;
						}
						DragDrop.DoDragDrop(new UIElement(), this.dataLibrary.GetArtistsTracks(track.Artist.ID), DragDropEffects.Copy);
						return;
					}
					if (!(obj is IEnumerable<object>))
					{
						break;
					}
					DragDrop.DoDragDrop(new UIElement(), obj as IEnumerable<object>, DragDropEffects.Copy);
					return;
				}
				case MusicViewType.AlbumGeneral:
				{
					if (obj is Track)
					{
						Track track1 = obj as Track;
						if (track1 == null)
						{
							return;
						}
						DragDrop.DoDragDrop(new UIElement(), this.dataLibrary.GetAlbumTracks(track1.Album.ID), DragDropEffects.Copy);
						return;
					}
					if (!(obj is IEnumerable<object>))
					{
						break;
					}
					DragDrop.DoDragDrop(new UIElement(), obj as IEnumerable<object>, DragDropEffects.Copy);
					return;
				}
				case MusicViewType.GenreGeneral:
				{
					if (obj is Track)
					{
						Track track2 = obj as Track;
						if (track2 == null)
						{
							return;
						}
						DragDrop.DoDragDrop(new UIElement(), this.dataLibrary.GetGenreTracks(track2.Genre.Name), DragDropEffects.Copy);
						return;
					}
					if (!(obj is IEnumerable<object>))
					{
						break;
					}
					DragDrop.DoDragDrop(new UIElement(), obj as IEnumerable<object>, DragDropEffects.Copy);
					break;
				}
				default:
				{
					return;
				}
			}
		}

		private void OnItemDoubleClick(object obj)
		{
			switch (this.ViewType)
			{
				case MusicViewType.Track:
				{
					this.OnPlayLocal(obj);
					return;
				}
				case MusicViewType.ArtistGeneral:
				{
					this.CurrentArtist = ((Track)obj).Artist;
					this.ViewType = MusicViewType.ArtistDetailed;
					this.IsBackVisible = true;
					this.ReloadItemsCollection();
					return;
				}
				case MusicViewType.ArtistDetailed:
				{
					this.OnPlayLocal(obj);
					return;
				}
				case MusicViewType.AlbumGeneral:
				{
					Track track = (Track)obj;
					this.CurrentAlbum = track.Album;
					this.CurrentArtist = track.Artist;
					this.ViewType = MusicViewType.AlbumDetailed;
					this.IsBackVisible = true;
					this.ReloadItemsCollection();
					return;
				}
				case MusicViewType.AlbumDetailed:
				{
					this.OnPlayLocal(obj);
					return;
				}
				case MusicViewType.GenreGeneral:
				{
					Track track1 = (Track)obj;
					this.ViewType = MusicViewType.GenreDetailed;
					this.IsBackVisible = true;
					this.CurrentGenre = track1.Genre.Name;
					this.ReloadItemsCollection();
					return;
				}
				case MusicViewType.GenreDetailed:
				{
					this.OnPlayLocal(obj);
					return;
				}
				case MusicViewType.Folder:
				{
					ItemBase itemBase = (ItemBase)obj;
					switch (itemBase.Type)
					{
						case ItemType.Content:
						{
							this.OnPlayLocal(obj);
							return;
						}
						case ItemType.Folder:
						{
							this.CurrentFolder = (MultimediaFolder)itemBase;
							this.IsBackVisible = true;
							this.ViewType = MusicViewType.Folder;
							this.ReloadItemsCollection();
							return;
						}
						default:
						{
							return;
						}
					}
					break;
				}
				default:
				{
					return;
				}
			}
		}

		public void OnMainViewChanged(object obj)
		{
			switch ((int)obj)
			{
				case 0:
				{
					this.ViewType = MusicViewType.Track;
					this.ReloadItemsCollection();
					return;
				}
				case 1:
				{
					this.ViewType = MusicViewType.ArtistGeneral;
					this.ReloadItemsCollection();
					return;
				}
				case 2:
				{
					this.ViewType = MusicViewType.AlbumGeneral;
					this.ReloadItemsCollection();
					return;
				}
				case 3:
				{
					this.ViewType = MusicViewType.GenreGeneral;
					this.ReloadItemsCollection();
					return;
				}
				case 4:
				{
					this.ViewType = MusicViewType.Folder;
					this.ReloadItemsCollection();
					return;
				}
				default:
				{
					return;
				}
			}
		}

		private void OnMusicBack(object obj)
		{
			switch (this.ViewType)
			{
				case MusicViewType.Track:
				case MusicViewType.ArtistGeneral:
				case MusicViewType.AlbumGeneral:
				case MusicViewType.GenreGeneral:
				{
					return;
				}
				case MusicViewType.ArtistDetailed:
				{
					this.ViewType = MusicViewType.ArtistGeneral;
					this.IsBackVisible = false;
					this.ReloadItemsCollection();
					return;
				}
				case MusicViewType.AlbumDetailed:
				{
					this.ViewType = MusicViewType.AlbumGeneral;
					this.IsBackVisible = false;
					this.ReloadItemsCollection();
					return;
				}
				case MusicViewType.GenreDetailed:
				{
					this.ViewType = MusicViewType.GenreGeneral;
					this.IsBackVisible = false;
					this.ReloadItemsCollection();
					return;
				}
				case MusicViewType.Folder:
				{
					MultimediaFolder multimediaFolder = null;
					this.dataLibrary.GetFolderById(ref multimediaFolder, this.CurrentFolder.Parent, this.dataLibrary.RootMusicFolder);
					this.CurrentFolder = multimediaFolder;
					this.ReloadItemsCollection();
					if (this.CurrentFolder.ID != this.dataLibrary.RootMusicFolder.ID)
					{
						return;
					}
					this.IsBackVisible = false;
					this.ViewType = MusicViewType.Folder;
					return;
				}
				default:
				{
					return;
				}
			}
		}

		public override async Task OnNavigateFromAsync()
		{
			if (this.dataLibrary != null)
			{
				this.dataLibrary.DataLoaded -= new EventHandler(this.OnDataLoadedChanged);
			}
		}

		public override async Task OnNavigateToAsync()
		{
			//await this.<>n__FabricatedMethod2();
			if (this.dataLibrary != null)
			{
				this.multimediaLoadingPopup = base.Controller.CreatePopup(new MultimediaAddingContentPopupViewModel(this.dataLibrary), false);
				this.dataLibrary.DataLoaded += new EventHandler(this.OnDataLoadedChanged);
			}
			if (!this.isPlayerResumed && this.savedVisualState != null && this.savedVisualState.PlayerState != null)
			{
				BaseSavedVisualState baseSavedVisualState = this.SaveVisualState(this.savedVisualState.PlayerState);
				base.Controller.Navigate(new MediaPlayerViewModel(baseSavedVisualState, this.multiScreen, this.dataLibrary, baseSavedVisualState.PlayerState), false);
				this.isPlayerResumed = true;
			}
		}

		private void OnPlayLocal(object obj)
		{
			switch (this.ViewType)
			{
				case MusicViewType.Track:
				{
					Track track = (Track)obj;
					if (!this.IsTrackExist(track))
					{
						return;
					}
					MediaPlayerViewModel mediaPlayerViewModel = new MediaPlayerViewModel(this.SaveVisualState(null), this.multiScreen, this.dataLibrary, this.TracksList, track, MediaLibrary.DataModels.ContentType.Track)
					{
						SortByTitle = new bool?(this.SortByTitle)
					};
					this.localPlayerVM = mediaPlayerViewModel;
					base.Controller.Navigate(this.localPlayerVM, false);
					return;
				}
				case MusicViewType.ArtistGeneral:
				{
					MediaPlayerViewModel mediaPlayerViewModel1 = new MediaPlayerViewModel(this.SaveVisualState(null), this.multiScreen, this.dataLibrary, this.TracksList, (Content)this.TracksList[0], MediaLibrary.DataModels.ContentType.Track)
					{
						SortByTitle = null
					};
					this.localPlayerVM = mediaPlayerViewModel1;
					base.Controller.Navigate(this.localPlayerVM, false);
					return;
				}
				case MusicViewType.ArtistDetailed:
				{
					Track track1 = (Track)obj;
					if (!this.IsTrackExist(track1))
					{
						return;
					}
					MediaPlayerViewModel mediaPlayerViewModel2 = new MediaPlayerViewModel(this.SaveVisualState(null), this.multiScreen, this.dataLibrary, this.TracksList, track1, MediaLibrary.DataModels.ContentType.Track)
					{
						SortByTitle = null
					};
					this.localPlayerVM = mediaPlayerViewModel2;
					base.Controller.Navigate(this.localPlayerVM, false);
					return;
				}
				case MusicViewType.AlbumGeneral:
				{
					MediaPlayerViewModel mediaPlayerViewModel3 = new MediaPlayerViewModel(this.SaveVisualState(null), this.multiScreen, this.dataLibrary, this.TracksList, (Content)this.TracksList[0], MediaLibrary.DataModels.ContentType.Track)
					{
						SortByTitle = null
					};
					this.localPlayerVM = mediaPlayerViewModel3;
					base.Controller.Navigate(this.localPlayerVM, false);
					return;
				}
				case MusicViewType.AlbumDetailed:
				{
					Track track2 = (Track)obj;
					if (!this.IsTrackExist(track2))
					{
						return;
					}
					MediaPlayerViewModel mediaPlayerViewModel4 = new MediaPlayerViewModel(this.SaveVisualState(null), this.multiScreen, this.dataLibrary, this.TracksList, track2, MediaLibrary.DataModels.ContentType.Track)
					{
						SortByTitle = null
					};
					this.localPlayerVM = mediaPlayerViewModel4;
					base.Controller.Navigate(this.localPlayerVM, false);
					return;
				}
				case MusicViewType.GenreGeneral:
				{
					MediaPlayerViewModel mediaPlayerViewModel5 = new MediaPlayerViewModel(this.SaveVisualState(null), this.multiScreen, this.dataLibrary, this.TracksList, (Content)this.TracksList[0], MediaLibrary.DataModels.ContentType.Track)
					{
						SortByTitle = new bool?(this.SortByTitle)
					};
					this.localPlayerVM = mediaPlayerViewModel5;
					base.Controller.Navigate(this.localPlayerVM, false);
					return;
				}
				case MusicViewType.GenreDetailed:
				{
					Track track3 = (Track)obj;
					if (!this.IsTrackExist(track3))
					{
						return;
					}
					MediaPlayerViewModel mediaPlayerViewModel6 = new MediaPlayerViewModel(this.SaveVisualState(null), this.multiScreen, this.dataLibrary, this.TracksList, track3, MediaLibrary.DataModels.ContentType.Track)
					{
						SortByTitle = new bool?(this.SortByTitle)
					};
					this.localPlayerVM = mediaPlayerViewModel6;
					base.Controller.Navigate(this.localPlayerVM, false);
					return;
				}
				case MusicViewType.Folder:
				{
					ItemBase itemBase = (ItemBase)obj;
					switch (itemBase.Type)
					{
						case ItemType.Content:
						{
							Track track4 = (Track)itemBase;
							if (!this.IsTrackExist(track4))
							{
								return;
							}
							MediaPlayerViewModel mediaPlayerViewModel7 = new MediaPlayerViewModel(this.SaveVisualState(null), this.multiScreen, this.dataLibrary, this.TracksList, track4, MediaLibrary.DataModels.ContentType.Track)
							{
								SortByTitle = new bool?(this.SortByTitle)
							};
							this.localPlayerVM = mediaPlayerViewModel7;
							base.Controller.Navigate(this.localPlayerVM, false);
							return;
						}
						case ItemType.Folder:
						{
							base.Controller.Navigate(new MediaPlayerViewModel(this.SaveVisualState(null), this.multiScreen, this.dataLibrary, this.TracksList, (Content)this.TracksList[0], MediaLibrary.DataModels.ContentType.Track), false);
							this.localPlayerVM = new MediaPlayerViewModel(this.SaveVisualState(null), this.multiScreen, this.dataLibrary, this.TracksList, (Content)this.TracksList[0], MediaLibrary.DataModels.ContentType.Track);
							base.Controller.Navigate(this.localPlayerVM, false);
							return;
						}
						default:
						{
							return;
						}
					}
					break;
				}
				default:
				{
					return;
				}
			}
		}

		private void OnPlayOnTv(object obj)
		{
			this.multiScreen.PushMediaToTv((Content)obj);
		}

		private async void OnRemoveAlbumAsync(object obj)
		{
			Track track = (Track)obj;
			List<Content> albumTracks = this.dataLibrary.GetAlbumTracks(track.Album.ID);
			if (await this.ConfirmRemoveFolderAsync())
			{
				this.dataLibrary.DeleteItems(this.dataLibrary.RootMusicFolder, albumTracks);
			}
		}

		private async void OnRemoveArtistAsync(object obj)
		{
			Track track = (Track)obj;
			List<Content> artistsTracks = this.dataLibrary.GetArtistsTracks(track.Artist.ID);
			if (await this.ConfirmRemoveFolderAsync())
			{
				this.dataLibrary.DeleteItems(this.dataLibrary.RootMusicFolder, artistsTracks);
			}
		}

		private async void OnRemoveFolderAsync(object obj)
		{
			MultimediaFolder multimediaFolder = (MultimediaFolder)obj;
			if (await this.ConfirmRemoveFolderAsync())
			{
				this.dataLibrary.DeleteFileOrFolder(this.dataLibrary.RootMusicFolder, multimediaFolder);
			}
		}

		private async void OnRemoveGenreAsync(object obj)
		{
			Track track = (Track)obj;
			List<Content> genreTracks = this.dataLibrary.GetGenreTracks(track.Genre.Name);
			if (await this.ConfirmRemoveFolderAsync())
			{
				this.dataLibrary.DeleteItems(this.dataLibrary.RootMusicFolder, genreTracks);
			}
		}

		private void OnSendToQueue(object obj)
		{
			switch (this.ViewType)
			{
				case MusicViewType.Track:
				case MusicViewType.ArtistDetailed:
				case MusicViewType.AlbumDetailed:
				case MusicViewType.GenreDetailed:
				{
					this.multiScreen.PushMediaToTvQueue((Content)obj);
					return;
				}
				case MusicViewType.ArtistGeneral:
				{
					Track track = (Track)obj;
					List<Content> artistsTracks = this.dataLibrary.GetArtistsTracks(track.Artist.ID);
					this.multiScreen.PushMediaToTvQueue(artistsTracks);
					return;
				}
				case MusicViewType.AlbumGeneral:
				{
					Track track1 = (Track)obj;
					List<Content> albumTracks = this.dataLibrary.GetAlbumTracks(track1.Album.ID);
					this.multiScreen.PushMediaToTvQueue(albumTracks);
					return;
				}
				case MusicViewType.GenreGeneral:
				{
					Track track2 = (Track)obj;
					List<Content> genreTracks = this.dataLibrary.GetGenreTracks(track2.Genre.Name);
					this.multiScreen.PushMediaToTvQueue(genreTracks);
					return;
				}
				case MusicViewType.Folder:
				{
					switch (((ItemBase)obj).Type)
					{
						case ItemType.Content:
						{
							this.multiScreen.PushMediaToTvQueue((Content)obj);
							return;
						}
						case ItemType.Folder:
						{
							List<Content> filesList = ((MultimediaFolder)obj).GetFilesList();
							this.multiScreen.PushMediaToTvQueue(filesList);
							return;
						}
						default:
						{
							return;
						}
					}
					break;
				}
				default:
				{
					return;
				}
			}
		}

		private void OnViewChanged()
		{
			if (this.ViewChangedEvent != null)
			{
				this.ViewChangedEvent();
			}
		}

		private void ReloadItemsCollection()
		{
			switch (this.ViewType)
			{
				case MusicViewType.Track:
				{
					List<Track> tracks = null;
					tracks = (!this.SortByTitle ? (
						from x in (
							from i in this.dataLibrary.TrackList
							select i).ToList<Track>()
						orderby x.Name
						select x).ToList<Track>() : (
						from x in (
							from i in this.dataLibrary.TrackList
							select i).ToList<Track>()
						orderby x.Name descending
						select x).ToList<Track>());
					if (tracks.Count < 1)
					{
						this.ViewType = MusicViewType.NoContent;
						return;
					}
					this.TracksList = new ObservableCollection<ItemBase>(tracks);
					if (this.localPlayerVM == null)
					{
						break;
					}
					this.localPlayerVM.UpdateContent(this.TracksList);
					return;
				}
				case MusicViewType.ArtistGeneral:
				{
					List<Track> list = (
						from x in (
							from i in this.dataLibrary.GetArtists()
							select (Track)i).ToList<Track>()
						orderby x.Artist.Name
						select x).ToList<Track>();
					if (list.Count < 1)
					{
						this.ViewType = MusicViewType.NoContent;
						return;
					}
					this.ArtistList = new ObservableCollection<ItemBase>(list);
					return;
				}
				case MusicViewType.ArtistDetailed:
				{
					List<Content> contents = (
						from Track x in this.dataLibrary.GetArtistsTracks(this.CurrentArtist.ID)
						orderby x.Album.Name
						select x).Cast<Content>().ToList<Content>();
					if (contents.Count < 1)
					{
						this.ViewType = MusicViewType.ArtistGeneral;
						this.ReloadItemsCollection();
						return;
					}
					this.TracksList = new ObservableCollection<ItemBase>(contents);
					if (this.localPlayerVM == null)
					{
						break;
					}
					this.localPlayerVM.UpdateContent(this.TracksList);
					return;
				}
				case MusicViewType.AlbumGeneral:
				{
					List<Track> list1 = (
						from x in (
							from i in this.dataLibrary.GetAlbums()
							select (Track)i).ToList<Track>()
						orderby x.Album.Name
						select x).ToList<Track>();
					if (list1.Count < 1)
					{
						this.ViewType = MusicViewType.NoContent;
						return;
					}
					this.AlbumsList = new ObservableCollection<ItemBase>(list1);
					return;
				}
				case MusicViewType.AlbumDetailed:
				{
					List<Content> albumTracks = this.dataLibrary.GetAlbumTracks(this.CurrentAlbum.ID);
					if (albumTracks.Count<Content>() < 1)
					{
						this.ViewType = MusicViewType.AlbumGeneral;
						this.ReloadItemsCollection();
						return;
					}
					this.TracksList = new ObservableCollection<ItemBase>(albumTracks);
					if (this.localPlayerVM == null)
					{
						break;
					}
					this.localPlayerVM.UpdateContent(this.TracksList);
					return;
				}
				case MusicViewType.GenreGeneral:
				{
					List<ItemBase> genres = this.dataLibrary.GetGenres();
					List<Track> tracks1 = null;
					tracks1 = (!this.SortByTitle ? (
						from x in (
							from i in genres
							select (Track)i).ToList<Track>()
						orderby x.Genre.Name
						select x).ToList<Track>() : (
						from x in (
							from i in genres
							select (Track)i).ToList<Track>()
						orderby x.Genre.Name descending
						select x).ToList<Track>());
					if (tracks1 == null || tracks1.Count < 1)
					{
						this.ViewType = MusicViewType.NoContent;
						return;
					}
					this.GenresList = new ObservableCollection<ItemBase>(tracks1);
					return;
				}
				case MusicViewType.GenreDetailed:
				{
					List<Content> genreTracks = this.dataLibrary.GetGenreTracks(this.CurrentGenre);
					List<Track> tracks2 = null;
					tracks2 = (!this.SortByTitle ? (
						from x in (
							from i in genreTracks
							select (Track)i).ToList<Track>()
						orderby x.Name
						select x).ToList<Track>() : (
						from x in (
							from i in genreTracks
							select (Track)i).ToList<Track>()
						orderby x.Name descending
						select x).ToList<Track>());
					if (tracks2.Count < 1)
					{
						this.ViewType = MusicViewType.GenreGeneral;
						this.ReloadItemsCollection();
						return;
					}
					this.TracksList = new ObservableCollection<ItemBase>(tracks2);
					if (this.localPlayerVM == null)
					{
						break;
					}
					this.localPlayerVM.UpdateContent(this.TracksList);
					return;
				}
				case MusicViewType.Folder:
				{
					if (this.CurrentFolder == this.dataLibrary.RootMusicFolder)
					{
						this.IsBackVisible = false;
						this.OnViewChanged();
					}
					if (this.dataLibrary.TrackList.Count < 1)
					{
						this.ViewType = MusicViewType.NoContent;
						this.CurrentFolder = this.dataLibrary.RootMusicFolder;
						this.ReloadItemsCollection();
						return;
					}
					IEnumerable<ItemBase> itemsList = this.CurrentFolder.ItemsList;
					itemsList = (!this.SortByTitle ? 
						from arg in itemsList
						orderby arg.Name
						select arg : 
						from arg in itemsList
						orderby arg.Name descending
						select arg);
					this.ItemsList = new List<ItemBase>(itemsList);
					this.TracksList = new ObservableCollection<ItemBase>(itemsList);
					if (this.localPlayerVM != null)
					{
						this.localPlayerVM.UpdateContent(this.TracksList);
					}
					if (this.ItemsList.Count != 0)
					{
						break;
					}
					this.CurrentFolder = this.dataLibrary.RootMusicFolder;
					this.ReloadItemsCollection();
					this.OnViewChanged();
					return;
				}
				case MusicViewType.NoContent:
				{
					if (this.dataLibrary.TrackList.Count <= 0)
					{
						break;
					}
					this.ViewType = MusicViewType.Track;
					this.ReloadItemsCollection();
					break;
				}
				default:
				{
					return;
				}
			}
		}

		public SavedMusicVisualState SaveVisualState(SavedPlayerState playerstate = null)
		{
			ItemBase item;
			ItemBase itemBase;
			ItemBase item1;
			ItemBase itemBase1;
			ItemBase item2;
			ItemBase itemBase2;
			ItemBase item3;
			ItemBase itemBase3;
			ItemBase itemBase4 = null;
			switch (this.ViewType)
			{
				case MusicViewType.Track:
				{
					if (this.TracksList.Count > 0)
					{
						item = this.TracksList[0];
					}
					else
					{
						item = null;
					}
					itemBase4 = item;
					break;
				}
				case MusicViewType.ArtistGeneral:
				{
					if (this.ArtistList.Count > 0)
					{
						itemBase = this.ArtistList[0];
					}
					else
					{
						itemBase = null;
					}
					itemBase4 = itemBase;
					break;
				}
				case MusicViewType.ArtistDetailed:
				{
					if (this.TracksList.Count > 0)
					{
						item1 = this.TracksList[0];
					}
					else
					{
						item1 = null;
					}
					itemBase4 = item1;
					break;
				}
				case MusicViewType.AlbumGeneral:
				{
					if (this.AlbumsList.Count > 0)
					{
						itemBase1 = this.AlbumsList[0];
					}
					else
					{
						itemBase1 = null;
					}
					itemBase4 = itemBase1;
					break;
				}
				case MusicViewType.AlbumDetailed:
				{
					if (this.TracksList.Count > 0)
					{
						item2 = this.TracksList[0];
					}
					else
					{
						item2 = null;
					}
					itemBase4 = item2;
					break;
				}
				case MusicViewType.GenreGeneral:
				{
					if (this.GenresList.Count > 0)
					{
						itemBase2 = this.GenresList[0];
					}
					else
					{
						itemBase2 = null;
					}
					itemBase4 = itemBase2;
					break;
				}
				case MusicViewType.GenreDetailed:
				{
					if (this.TracksList.Count > 0)
					{
						item3 = this.TracksList[0];
					}
					else
					{
						item3 = null;
					}
					itemBase4 = item3;
					break;
				}
				case MusicViewType.Folder:
				{
					if (this.ItemsList.Count > 0)
					{
						itemBase3 = this.ItemsList[0];
					}
					else
					{
						itemBase3 = null;
					}
					itemBase4 = itemBase3;
					break;
				}
			}
			return new SavedMusicVisualState(this.ViewType, this.CurrentFolder, itemBase4, playerstate);
		}

		public event MusicViewModel.ViewChanged ViewChangedEvent;

		public delegate void ViewChanged();
	}
}