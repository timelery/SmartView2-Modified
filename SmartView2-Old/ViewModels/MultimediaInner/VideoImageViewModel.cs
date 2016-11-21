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
	[RelatedView(typeof(VideoImagePage))]
	public class VideoImageViewModel : PageViewModel
	{
		private ICommand addFilesCommand;

		private ICommand addFolderCommand;

		private ICommand deleteItemCommand;

		private ICommand itemDoubleClickCommand;

		private ICommand videoImageBackCommand;

		private ICommand sendToQueueCommand;

		private ICommand playLocalCommand;

		private ICommand mainViewChangedCommand;

		private ICommand playOnTvCommand;

		private ICommand dragItem;

		private ICommand contentDropCommand;

		private ICommand cancelAddingCommand;

		private ObservableCollection<ItemBase> itemsList = new ObservableCollection<ItemBase>();

		private IDataLibrary dataLibrary;

		private MultimediaFolder currentFolder;

		private MultimediaFolder rootFolder;

		private IMultiScreen multiScreen;

		private bool isBackVisible;

		private Visibility isCopyPasteVisible;

		private Visibility isMenuPlayVisible;

		private Visibility videoIconVisibility;

		private Visibility imageIconVisibility;

		private VideoPhotoViewType viewType;

		private int comboBoxCurrentState;

		private bool temporaryDataLoaded;

		private bool isPlayerResumed;

		private MediaLibrary.DataModels.ContentType contentType;

		private SavedVideoPhotoVisualstate savedVisualState;

		private string videoSearchMask = "Video files(*.MP4;*.AVI;*.WMV)|*.MP4;*.AVI;*.WMV|All files (*.*)|*.*";

		private string imageSearchMask = "Image files(*.BMP;*.JPG;*.JPEG;*.PNG*;*.TIF;*.TIFF;*.GIF)|*.BMP;*.JPG;*.JPEG;*.PNG;*.TIF;*.TIFF;*.GIF|All files (*.*)|*.*";

		private PopupWrapper multimediaLoadingPopup;

		private MediaPlayerViewModel localPlayerVM;

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

		public ICommand CancelAddingCommand
		{
			get
			{
				return this.cancelAddingCommand;
			}
		}

		public int ComboBoxCurrentState
		{
			get
			{
				return this.comboBoxCurrentState;
			}
			set
			{
				base.SetProperty<int>(ref this.comboBoxCurrentState, value, "ComboBoxCurrentState");
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
				return this.contentType;
			}
			set
			{
				base.SetProperty<MediaLibrary.DataModels.ContentType>(ref this.contentType, value, "ContentType");
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

		public Visibility ImageIconVisibility
		{
			get
			{
				return this.imageIconVisibility;
			}
			set
			{
				base.SetProperty<Visibility>(ref this.imageIconVisibility, value, "ImageIconVisibility");
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

		public Visibility IsCopyPasteVisible
		{
			get
			{
				return this.isCopyPasteVisible;
			}
			set
			{
				base.SetProperty<Visibility>(ref this.isCopyPasteVisible, value, "IsCopyPasteVisible");
			}
		}

		public bool IsInRootFolder
		{
			get
			{
				if (this.ContentType == MediaLibrary.DataModels.ContentType.Image)
				{
					if (this.CurrentFolder == null)
					{
						return true;
					}
					return this.CurrentFolder == this.dataLibrary.RootImageFolder;
				}
				if (this.CurrentFolder == null)
				{
					return true;
				}
				return this.CurrentFolder == this.dataLibrary.RootVideoFolder;
			}
		}

		public bool IsInsideFolder
		{
			get
			{
				if (this.ContentType == MediaLibrary.DataModels.ContentType.Image)
				{
					if (this.CurrentFolder == null)
					{
						return false;
					}
					return this.CurrentFolder != this.dataLibrary.RootImageFolder;
				}
				if (this.CurrentFolder == null)
				{
					return false;
				}
				return this.CurrentFolder != this.dataLibrary.RootVideoFolder;
			}
		}

		public Visibility IsMenuPlayVisible
		{
			get
			{
				return this.isMenuPlayVisible;
			}
			set
			{
				base.SetProperty<Visibility>(ref this.isMenuPlayVisible, value, "IsMenuPlayVisible");
			}
		}

		public ICommand ItemDoubleClickCommand
		{
			get
			{
				return this.itemDoubleClickCommand;
			}
		}

		public ObservableCollection<ItemBase> ItemsList
		{
			get
			{
				return this.itemsList;
			}
			set
			{
				base.SetProperty<ObservableCollection<ItemBase>>(ref this.itemsList, value, "ItemsList");
			}
		}

		public ICommand MainViewChangedCommand
		{
			get
			{
				return this.mainViewChangedCommand;
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

		public ICommand SendToQueueCommand
		{
			get
			{
				return this.sendToQueueCommand;
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

		public Visibility VideoIconVisibility
		{
			get
			{
				return this.videoIconVisibility;
			}
			set
			{
				base.SetProperty<Visibility>(ref this.videoIconVisibility, value, "VideoIconVisibility");
			}
		}

		public ICommand VideoImageBackCommand
		{
			get
			{
				return this.videoImageBackCommand;
			}
		}

		public VideoPhotoViewType ViewType
		{
			get
			{
				return this.viewType;
			}
			set
			{
				base.SetProperty<VideoPhotoViewType>(ref this.viewType, value, "ViewType");
				this.OnViewChanged();
			}
		}

		public VideoImageViewModel(SavedVideoPhotoVisualstate visualstate, IDataLibrary library, IMultiScreen multiScreen, MultimediaFolder rootfolder, MediaLibrary.DataModels.ContentType type) : this(library, multiScreen, rootfolder, type)
		{
			if (visualstate == null)
			{
				return;
			}
			this.CurrentFolder = visualstate.CurrentItem;
			this.ViewType = visualstate.ViewType;
			if (this.ViewType == VideoPhotoViewType.NoContent && rootfolder.GetAllFilesList(null).Count > 0)
			{
				this.ViewType = VideoPhotoViewType.Date;
				visualstate.ViewType = VideoPhotoViewType.Date;
				this.ReloadItemsCollection();
			}
			this.ComboBoxCurrentState = (int)this.ViewType;
			if (this.currentFolder == this.rootFolder || this.ViewType != VideoPhotoViewType.Folder)
			{
				this.ReloadItemsCollection();
			}
			else
			{
				this.OnItemDoubleClick(this.currentFolder);
			}
			if (visualstate.PlayerState != null)
			{
				this.savedVisualState = visualstate;
			}
		}

		public VideoImageViewModel(IDataLibrary library, IMultiScreen multiScreen, MultimediaFolder rootfolder, MediaLibrary.DataModels.ContentType type)
		{
			if (library == null)
			{
				throw new Exception("multiScreen cant work with null library");
			}
			if (multiScreen == null)
			{
				throw new ArgumentNullException("multiScreen is null");
			}
			this.dataLibrary = library;
			this.multiScreen = multiScreen;
			this.rootFolder = rootfolder;
			this.ContentType = type;
			this.IsBackVisible = false;
			this.IsCopyPasteVisible = Visibility.Collapsed;
			this.CurrentFolder = this.rootFolder;
			this.ViewType = VideoPhotoViewType.Date;
			this.ReloadItemsCollection();
			this.TemporaryDataLoaded = this.dataLibrary.IsDataLoaded;
			if (type != MediaLibrary.DataModels.ContentType.Image)
			{
				this.IsMenuPlayVisible = Visibility.Visible;
				this.ImageIconVisibility = Visibility.Collapsed;
				this.VideoIconVisibility = Visibility.Visible;
			}
			else
			{
				this.IsMenuPlayVisible = Visibility.Collapsed;
				this.ImageIconVisibility = Visibility.Visible;
				this.VideoIconVisibility = Visibility.Collapsed;
			}
			this.addFilesCommand = new Command(new Action<object>(this.OnAddFiles));
			this.addFolderCommand = new Command(new Action<object>(this.OnAddFolder));
			this.RemoveFolderCommand = new Command(new Action<object>(this.OnRemoveFolder));
			this.deleteItemCommand = new Command(new Action<object>(this.OnDeleteItem));
			this.itemDoubleClickCommand = new Command(new Action<object>(this.OnItemDoubleClick));
			this.videoImageBackCommand = new Command(new Action<object>(this.OnVideoImageBack));
			this.sendToQueueCommand = new Command(new Action<object>(this.OnSendToQueue));
			this.playLocalCommand = new Command(new Action<object>(this.OnPlayLocal));
			this.mainViewChangedCommand = new Command(new Action<object>(this.OnMainViewChanged));
			this.playOnTvCommand = new Command(new Action<object>(this.OnPlayOnTv));
			this.dragItem = new Command(new Action<object>(this.OnDragItem));
			this.contentDropCommand = new Command(new Action<object>(this.OnContentDrop));
			this.cancelAddingCommand = new Command(new Action<object>(this.OnCancelAdding));
		}

		private void dataLibrary_AddingContentCanceled(object sender, EventArgs e)
		{
			this.ReloadItemsCollection();
		}

		private void dataLibrary_DataUpdated(object sender, EventArgs e)
		{
			this.ReloadItemsCollection();
		}

		private bool IsFileExist(Content file)
		{
			bool flag;
			try
			{
				flag = (file == null ? false : (new FileInfo(file.Path)).Exists);
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		private async void OnAddFiles(object obj)
		{
			switch (this.ContentType)
			{
				case MediaLibrary.DataModels.ContentType.Image:
				{
					string[] filePath = MultimediaViewModel.GetFilePath("", this.imageSearchMask, "Select files to add");
					if (filePath != null)
					{
						await this.dataLibrary.AddContentFromFiles(filePath, this.contentType);
						goto case MediaLibrary.DataModels.ContentType.Track;
					}
					else
					{
						break;
					}
				}
				case MediaLibrary.DataModels.ContentType.Track:
				{
					break;
				}
				case MediaLibrary.DataModels.ContentType.Video:
				{
					string[] strArrays = MultimediaViewModel.GetFilePath("", this.videoSearchMask, "Select files to add");
					if (strArrays != null)
					{
						await this.dataLibrary.AddContentFromFiles(strArrays, this.contentType);
						goto case MediaLibrary.DataModels.ContentType.Track;
					}
					else
					{
						break;
					}
				}
				default:
				{
					goto case MediaLibrary.DataModels.ContentType.Track;
				}
			}
		}

		private async void OnAddFolder(object obj)
		{
			string folderPath = MultimediaViewModel.GetFolderPath("");
			if (!string.IsNullOrEmpty(folderPath))
			{
				await this.dataLibrary.AddContentFromFolder(folderPath, this.contentType);
			}
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
					await this.dataLibrary.AddContentFromFolder(strArrays[i], this.contentType);
				}
			}
			if (strs.Count > 0)
			{
				await this.dataLibrary.AddContentFromFiles(strs.ToArray(), this.contentType);
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
			this.dataLibrary.DeleteFileOrFolder(this.rootFolder, (ItemBase)obj);
			this.ReloadItemsCollection();
		}

		public override async Task OnDestroyAsync()
		{
			//await this.<>n__FabricatedMethodc();
			this.dataLibrary.DataUpdated -= new EventHandler(this.dataLibrary_DataUpdated);
		}

		private void OnDragItem(object obj)
		{
			if (obj is ItemBase)
			{
				ItemBase itemBase = obj as ItemBase;
				if (itemBase == null)
				{
					return;
				}
				if (itemBase.Type != ItemType.Content)
				{
					MultimediaFolder multimediaFolder = obj as MultimediaFolder;
					DragDrop.DoDragDrop(new UIElement(), multimediaFolder.GetAllFilesList(null), DragDropEffects.Copy);
					return;
				}
				List<Content> contents = new List<Content>()
				{
					obj as Content
				};
				DragDrop.DoDragDrop(new UIElement(), contents, DragDropEffects.Copy);
				return;
			}
			IEnumerable<object> objs = obj as IEnumerable<object>;
			if (objs != null)
			{
				Content[] array = objs.OfType<Content>().ToArray<Content>();
				if (array.Count<Content>() > 0)
				{
					DragDrop.DoDragDrop(new UIElement(), array, DragDropEffects.Copy);
					return;
				}
				MultimediaFolder[] multimediaFolderArray = objs.OfType<MultimediaFolder>().ToArray<MultimediaFolder>();
				if (multimediaFolderArray.Count<MultimediaFolder>() > 0)
				{
					IEnumerable<Content> contents1 = ((IEnumerable<MultimediaFolder>)multimediaFolderArray).SelectMany<MultimediaFolder, Content>((MultimediaFolder p) => p.GetFilesList());
					DragDrop.DoDragDrop(new UIElement(), contents1, DragDropEffects.Copy);
				}
			}
		}

		private void OnItemDoubleClick(object obj)
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
					this.ViewType = VideoPhotoViewType.Folder;
					this.ReloadItemsCollection();
					this.OnViewChanged();
					return;
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
					this.ViewType = VideoPhotoViewType.Date;
					this.ReloadItemsCollection();
					return;
				}
				case 1:
				{
					this.ViewType = VideoPhotoViewType.Title;
					this.ReloadItemsCollection();
					return;
				}
				case 2:
				{
					this.ViewType = VideoPhotoViewType.Folder;
					this.ReloadItemsCollection();
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
				case VideoPhotoViewType.Date:
				{
					Content content = (Content)obj;
					if (!this.IsFileExist(content))
					{
						return;
					}
					this.localPlayerVM = new MediaPlayerViewModel(this.SaveVisualState(null), this.multiScreen, this.dataLibrary, this.ItemsList, content, this.ContentType);
					base.Controller.Navigate(this.localPlayerVM, false);
					return;
				}
				case VideoPhotoViewType.Title:
				{
					Content content1 = (Content)obj;
					if (!this.IsFileExist(content1))
					{
						return;
					}
					this.localPlayerVM = new MediaPlayerViewModel(this.SaveVisualState(null), this.multiScreen, this.dataLibrary, this.ItemsList, content1, this.ContentType);
					base.Controller.Navigate(this.localPlayerVM, false);
					return;
				}
				case VideoPhotoViewType.Folder:
				{
					ItemBase itemBase = (ItemBase)obj;
					switch (itemBase.Type)
					{
						case ItemType.Content:
						{
							Content content2 = (Content)itemBase;
							if (!this.IsFileExist(content2))
							{
								return;
							}
							this.localPlayerVM = new MediaPlayerViewModel(this.SaveVisualState(null), this.multiScreen, this.dataLibrary, this.ItemsList, content2, MediaLibrary.DataModels.ContentType.Image);
							base.Controller.Navigate(this.localPlayerVM, false);
							break;
						}
						case ItemType.Folder:
						{
							if (this.ItemsList.Count <= 0)
							{
								break;
							}
							this.localPlayerVM = new MediaPlayerViewModel(this.SaveVisualState(null), this.multiScreen, this.dataLibrary, this.ItemsList, (Content)this.ItemsList[0], MediaLibrary.DataModels.ContentType.Video);
							base.Controller.Navigate(this.localPlayerVM, false);
							return;
						}
						default:
						{
							return;
						}
					}
					return;
				}
				default:
				{
					return;
				}
			}
		}

		private void OnPlayOnTv(object obj)
		{
			if (!(obj is Content))
			{
				return;
			}
			this.multiScreen.PushMediaToTv((Content)obj);
		}

		private async void OnRemoveFolder(object obj)
		{
			bool flag;
			MultimediaFolder multimediaFolder = obj as MultimediaFolder;
			if (multimediaFolder != null)
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
				switch (this.ContentType)
				{
					case MediaLibrary.DataModels.ContentType.Image:
					{
						this.dataLibrary.DeleteFileOrFolder(this.dataLibrary.RootImageFolder, multimediaFolder);
						goto case MediaLibrary.DataModels.ContentType.Track;
					}
					case MediaLibrary.DataModels.ContentType.Track:
					{
						break;
					}
					case MediaLibrary.DataModels.ContentType.Video:
					{
						this.dataLibrary.DeleteFileOrFolder(this.dataLibrary.RootVideoFolder, multimediaFolder);
						goto case MediaLibrary.DataModels.ContentType.Track;
					}
					default:
					{
						goto case MediaLibrary.DataModels.ContentType.Track;
					}
				}
			}
		}

		private void OnSendToQueue(object obj)
		{
			ItemBase itemBase = obj as ItemBase;
			if (itemBase != null)
			{
				switch (itemBase.Type)
				{
					case ItemType.Content:
					{
						this.multiScreen.PushMediaToTvQueue((Content)itemBase);
						break;
					}
					case ItemType.Folder:
					{
						MultimediaFolder multimediaFolder = (MultimediaFolder)itemBase;
						this.multiScreen.PushMediaToTvQueue(multimediaFolder.GetFilesList());
						return;
					}
					default:
					{
						return;
					}
				}
			}
		}

		private void OnVideoImageBack(object obj)
		{
			Guid parent = this.currentFolder.Parent;
			MultimediaFolder multimediaFolder = this.rootFolder.FindFolderByID(this.currentFolder.Parent);
			if (multimediaFolder != null)
			{
				this.CurrentFolder = multimediaFolder;
				IEnumerable<ItemBase> itemsList = this.currentFolder.ItemsList;
				if (this.ViewType == VideoPhotoViewType.Folder)
				{
					itemsList = 
						from item in itemsList
						orderby item.Date descending
						select item;
				}
				this.ItemsList = new ObservableCollection<ItemBase>(itemsList);
				if (this.currentFolder == this.rootFolder)
				{
					this.IsBackVisible = false;
				}
			}
			this.OnViewChanged();
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
				case VideoPhotoViewType.Date:
				{
					List<Content> allFilesList = this.rootFolder.GetAllFilesList(null);
					if (allFilesList.Count < 1)
					{
						this.ViewType = VideoPhotoViewType.NoContent;
						return;
					}
					List<MultimediaFile> list = (
						from x in (
							from i in allFilesList
							select (MultimediaFile)i).ToList<MultimediaFile>()
						orderby x.Date descending
						select x).ToList<MultimediaFile>();
					this.ItemsList = new ObservableCollection<ItemBase>(list);
					if (this.localPlayerVM != null)
					{
						this.localPlayerVM.UpdateContent(this.ItemsList);
					}
					this.IsCopyPasteVisible = Visibility.Collapsed;
					return;
				}
				case VideoPhotoViewType.Title:
				{
					List<Content> contents = this.rootFolder.GetAllFilesList(null);
					if (contents.Count < 1)
					{
						this.ViewType = VideoPhotoViewType.NoContent;
						return;
					}
					List<MultimediaFile> multimediaFiles = (
						from x in (
							from i in contents
							select (MultimediaFile)i).ToList<MultimediaFile>()
						orderby x.Name
						select x).ToList<MultimediaFile>();
					this.ItemsList = new ObservableCollection<ItemBase>(multimediaFiles);
					if (this.localPlayerVM != null)
					{
						this.localPlayerVM.UpdateContent(this.ItemsList);
					}
					this.IsCopyPasteVisible = Visibility.Collapsed;
					return;
				}
				case VideoPhotoViewType.Folder:
				{
					if (this.rootFolder.GetAllFilesList(null).Count < 1)
					{
						this.CurrentFolder = this.rootFolder;
						this.ViewType = VideoPhotoViewType.NoContent;
						this.IsBackVisible = false;
						this.OnViewChanged();
						return;
					}
					IEnumerable<ItemBase> itemsList = 
						from arg in this.CurrentFolder.ItemsList
						orderby arg.Date descending
						select arg;
					this.ItemsList = new ObservableCollection<ItemBase>(itemsList);
					if (this.localPlayerVM != null)
					{
						this.localPlayerVM.UpdateContent(this.ItemsList);
					}
					this.IsCopyPasteVisible = Visibility.Visible;
					if (this.ItemsList.Count == 0)
					{
						this.CurrentFolder = this.rootFolder;
						this.ReloadItemsCollection();
						this.OnViewChanged();
						return;
					}
					if (this.CurrentFolder != this.rootFolder)
					{
						break;
					}
					this.IsBackVisible = false;
					return;
				}
				case VideoPhotoViewType.NoContent:
				{
					if (this.rootFolder.GetAllFilesList(null).Count <= 0)
					{
						break;
					}
					this.ViewType = VideoPhotoViewType.Date;
					this.ReloadItemsCollection();
					break;
				}
				default:
				{
					return;
				}
			}
		}

		public SavedVideoPhotoVisualstate SaveVisualState(SavedPlayerState playerstate = null)
		{
			return new SavedVideoPhotoVisualstate(this.ViewType, this.currentFolder, playerstate);
		}

		public event VideoImageViewModel.ViewChanged ViewChangedEvent;

		public delegate void ViewChanged();
	}
}