using MediaLibrary.DataModels;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using SmartView2.Core;
using SmartView2.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using TagLib;

namespace SmartView2.Native.MediaLibrary
{
	public class DataLibrary : IDataLibrary
	{
		public readonly static string[] imageSearchPattern;

		public readonly static string[] musicSearchPattern;

		public readonly static string[] videoSearchPattern;

		private bool isDataLoaded;

		private CancellationTokenSource cancelToken;

		private Task addingTask;

		private string localDataDir;

		private Uri defaultTrackIcon = new Uri("pack://application:,,,/Resources/Images/Music_track_default.png");

		private Queue<Tuple<string[], ContentType, bool>> contentAddingQueue;

		private FileSystemWatcher watcher;

		private IBaseDispatcher dispatcher;

		public Guid ID
		{
			get;
			set;
		}

		public bool IsDataLoaded
		{
			get
			{
				return JustDecompileGenerated_get_IsDataLoaded();
			}
			set
			{
				JustDecompileGenerated_set_IsDataLoaded(value);
			}
		}

		public bool JustDecompileGenerated_get_IsDataLoaded()
		{
			return this.isDataLoaded;
		}

		private void JustDecompileGenerated_set_IsDataLoaded(bool value)
		{
			this.isDataLoaded = value;
			this.OnDataLoaded();
		}

		public MultimediaFolder RootFolder
		{
			get
			{
				return JustDecompileGenerated_get_RootFolder();
			}
			set
			{
				JustDecompileGenerated_set_RootFolder(value);
			}
		}

		private MultimediaFolder JustDecompileGenerated_RootFolder_k__BackingField;

		public MultimediaFolder JustDecompileGenerated_get_RootFolder()
		{
			return this.JustDecompileGenerated_RootFolder_k__BackingField;
		}

		private void JustDecompileGenerated_set_RootFolder(MultimediaFolder value)
		{
			this.JustDecompileGenerated_RootFolder_k__BackingField = value;
		}

		public MultimediaFolder RootImageFolder
		{
			get
			{
				return JustDecompileGenerated_get_RootImageFolder();
			}
			set
			{
				JustDecompileGenerated_set_RootImageFolder(value);
			}
		}

		private MultimediaFolder JustDecompileGenerated_RootImageFolder_k__BackingField;

		public MultimediaFolder JustDecompileGenerated_get_RootImageFolder()
		{
			return this.JustDecompileGenerated_RootImageFolder_k__BackingField;
		}

		private void JustDecompileGenerated_set_RootImageFolder(MultimediaFolder value)
		{
			this.JustDecompileGenerated_RootImageFolder_k__BackingField = value;
		}

		public MultimediaFolder RootMusicFolder
		{
			get
			{
				return JustDecompileGenerated_get_RootMusicFolder();
			}
			set
			{
				JustDecompileGenerated_set_RootMusicFolder(value);
			}
		}

		private MultimediaFolder JustDecompileGenerated_RootMusicFolder_k__BackingField;

		public MultimediaFolder JustDecompileGenerated_get_RootMusicFolder()
		{
			return this.JustDecompileGenerated_RootMusicFolder_k__BackingField;
		}

		private void JustDecompileGenerated_set_RootMusicFolder(MultimediaFolder value)
		{
			this.JustDecompileGenerated_RootMusicFolder_k__BackingField = value;
		}

		public MultimediaFolder RootVideoFolder
		{
			get
			{
				return JustDecompileGenerated_get_RootVideoFolder();
			}
			set
			{
				JustDecompileGenerated_set_RootVideoFolder(value);
			}
		}

		private MultimediaFolder JustDecompileGenerated_RootVideoFolder_k__BackingField;

		public MultimediaFolder JustDecompileGenerated_get_RootVideoFolder()
		{
			return this.JustDecompileGenerated_RootVideoFolder_k__BackingField;
		}

		private void JustDecompileGenerated_set_RootVideoFolder(MultimediaFolder value)
		{
			this.JustDecompileGenerated_RootVideoFolder_k__BackingField = value;
		}

		public List<Track> TrackList
		{
			get
			{
				return JustDecompileGenerated_get_TrackList();
			}
			set
			{
				JustDecompileGenerated_set_TrackList(value);
			}
		}

		private List<Track> JustDecompileGenerated_TrackList_k__BackingField;

		public List<Track> JustDecompileGenerated_get_TrackList()
		{
			return this.JustDecompileGenerated_TrackList_k__BackingField;
		}

		private void JustDecompileGenerated_set_TrackList(List<Track> value)
		{
			this.JustDecompileGenerated_TrackList_k__BackingField = value;
		}

		static DataLibrary()
		{
			string[] strArrays = new string[] { "*.jpeg", "*.jpg", "*.png", "*.bmp", "*.tif", "*.tiff", "*.gif" };
			DataLibrary.imageSearchPattern = strArrays;
			string[] strArrays1 = new string[] { "*.mp3", "*.flac", "*.mid", "*.midi" };
			DataLibrary.musicSearchPattern = strArrays1;
			string[] strArrays2 = new string[] { "*.mp4", "*.avi", "*.wmv", "*.mpeg" };
			DataLibrary.videoSearchPattern = strArrays2;
		}

		public DataLibrary(IBaseDispatcher dispatcher)
		{
			this.dispatcher = dispatcher;
			this.localDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SmartView2");
			this.RootMusicFolder = new MultimediaFolder("RootMusicFolder", this.ID, DateTime.Now);
			this.RootImageFolder = new MultimediaFolder("RootImageFolder", this.ID, DateTime.Now);
			this.RootVideoFolder = new MultimediaFolder("RootVideoFolder", this.ID, DateTime.Now);
			this.RootFolder = new MultimediaFolder("RootFolder", this.ID, DateTime.Now);
			this.TrackList = new List<Track>();
			this.contentAddingQueue = new Queue<Tuple<string[], ContentType, bool>>();
			this.watcher = new FileSystemWatcher()
			{
				Path = this.localDataDir,
				NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite | NotifyFilters.LastAccess,
				Filter = "*.png"
			};
			this.watcher.Changed += new FileSystemEventHandler(this.OnCacheDataFolderChanged);
			this.watcher.Deleted += new FileSystemEventHandler(this.OnCacheDataFolderChanged);
			this.watcher.EnableRaisingEvents = true;
			TagLib.File.AddFileTypeResolver(new TagLib.File.FileTypeResolver(this.MidiResolver));
		}

		public async Task AddContentFromFiles(string[] files, ContentType contentType)
		{
			Func<Content, Track> func = null;
			if (files != null)
			{
				this.IsDataLoaded = false;
				this.cancelToken = new CancellationTokenSource();
				if (this.addingTask == null || this.addingTask.IsCompleted)
				{
					this.addingTask = new Task(() => {
						int rootFolder = 0;
						switch (contentType)
						{
							case ContentType.Image:
							{
								string[] filesFromArray = this.GetFilesFromArray(files, DataLibrary.imageSearchPattern);
								rootFolder = this.AddFilesToRootFolder(filesFromArray, ContentType.Image, this.cancelToken.Token);
								break;
							}
							case ContentType.Track:
							{
								string[] strArrays = this.GetFilesFromArray(files, DataLibrary.musicSearchPattern);
								rootFolder = this.AddMusicsToRootFolder(strArrays, this.cancelToken.Token);
								DataLibrary u003cu003e4_this = this;
								List<Content> allFilesList = this.RootMusicFolder.GetAllFilesList(null);
								if (func == null)
								{
									func = (Content i) => (Track)i;
								}
								u003cu003e4_this.TrackList = allFilesList.Select<Content, Track>(func).ToList<Track>();
								this.ReloadAlbumsTracksCount();
								break;
							}
							case ContentType.Video:
							{
								string[] filesFromArray1 = this.GetFilesFromArray(files, DataLibrary.videoSearchPattern);
								rootFolder = this.AddFilesToRootFolder(filesFromArray1, ContentType.Video, this.cancelToken.Token);
								break;
							}
						}
						if (rootFolder == 0)
						{
							this.OnContentAlreadyExist();
						}
						this.OnDataUpdated();
					}, this.cancelToken.Token);
					try
					{
						this.addingTask.Start();
						await this.addingTask;
						await this.SaveLibrary();
					}
					catch (TaskCanceledException taskCanceledException)
					{
					}
					if (this.contentAddingQueue.Count != 0)
					{
						await this.AddContentFromQueue();
					}
					else
					{
						this.IsDataLoaded = true;
					}
				}
				else
				{
					this.contentAddingQueue.Enqueue(new Tuple<string[], ContentType, bool>(files, contentType, true));
				}
			}
		}

		public async Task AddContentFromFolder(string pathtofolder, ContentType contentType)
		{
			Func<Content, Track> func = null;
			this.IsDataLoaded = false;
			this.cancelToken = new CancellationTokenSource();
			if (this.addingTask == null || this.addingTask.IsCompleted)
			{
				this.addingTask = new Task(() => {
					int rootFolder = 0;
					switch (contentType)
					{
						case ContentType.Image:
						{
							string[] strArrays = DataLibrary.FindFiles(pathtofolder, DataLibrary.imageSearchPattern, SearchOption.AllDirectories);
							rootFolder = this.AddFilesToRootFolder(strArrays, ContentType.Image, this.cancelToken.Token);
							break;
						}
						case ContentType.Track:
						{
							string[] strArrays1 = DataLibrary.FindFiles(pathtofolder, DataLibrary.musicSearchPattern, SearchOption.AllDirectories);
							rootFolder = this.AddMusicsToRootFolder(strArrays1, this.cancelToken.Token);
							DataLibrary u003cu003e4_this = this;
							List<Content> allFilesList = this.RootMusicFolder.GetAllFilesList(null);
							if (func == null)
							{
								func = (Content i) => (Track)i;
							}
							u003cu003e4_this.TrackList = allFilesList.Select<Content, Track>(func).ToList<Track>();
							this.ReloadAlbumsTracksCount();
							break;
						}
						case ContentType.Video:
						{
							string[] strArrays2 = DataLibrary.FindFiles(pathtofolder, DataLibrary.videoSearchPattern, SearchOption.AllDirectories);
							rootFolder = this.AddFilesToRootFolder(strArrays2, ContentType.Video, this.cancelToken.Token);
							break;
						}
					}
					if (rootFolder == 0)
					{
						this.OnContentAlreadyExist();
					}
					this.OnDataUpdated();
				}, this.cancelToken.Token);
				try
				{
					this.addingTask.Start();
					await this.addingTask;
					await this.SaveLibrary();
				}
				catch (TaskCanceledException taskCanceledException)
				{
				}
				if (this.contentAddingQueue.Count != 0)
				{
					await this.AddContentFromQueue();
				}
				else
				{
					this.IsDataLoaded = true;
				}
			}
			else
			{
				Queue<Tuple<string[], ContentType, bool>> tuples = this.contentAddingQueue;
				string[] strArrays3 = new string[] { pathtofolder };
				tuples.Enqueue(new Tuple<string[], ContentType, bool>(strArrays3, contentType, false));
			}
		}

		private async Task AddContentFromQueue()
		{
			Tuple<string[], ContentType, bool> tuple = this.contentAddingQueue.Dequeue();
			if (!tuple.Item3)
			{
				await this.AddContentFromFolder(tuple.Item1[0], tuple.Item2);
			}
			else
			{
				await this.AddContentFromFiles(tuple.Item1, tuple.Item2);
			}
		}

		private bool AddFile(MultimediaFolder targetfolder, string filepath, ContentType type)
		{
			if (string.IsNullOrEmpty(filepath) || targetfolder == null)
			{
				return false;
			}
			FileInfo fileInfo = new FileInfo(filepath);
			fileInfo.Refresh();
			string name = fileInfo.Name;
			ImageConverter imageConverter = new ImageConverter();
			MultimediaFile multimediaFile = new MultimediaFile(name, filepath, targetfolder.ID, type, fileInfo.LastWriteTime, null)
			{
				LastUpdate = System.IO.File.GetLastWriteTimeUtc(filepath)
			};
			MultimediaFile preview = multimediaFile;
			targetfolder.AddItem(preview);
			if (type != ContentType.Track)
			{
				preview.Preview = this.GetPreview(filepath, type, preview.ID);
			}
			if (type == ContentType.Video)
			{
				preview.Duration = this.GetDuration(filepath);
			}
			return true;
		}

		private int AddFilesToRootFolder(string[] filepaths, ContentType type, CancellationToken cancellationToken)
		{
			List<Content> allFilesList;
			int num;
			string str;
			string[] strArrays;
			int i;
			if (cancellationToken.IsCancellationRequested)
			{
				return 0;
			}
			if (filepaths == null || (int)filepaths.Length < 1)
			{
				return 0;
			}
			MultimediaFolder rootImageFolder = null;
			switch (type)
			{
				case ContentType.Image:
				{
					rootImageFolder = this.RootImageFolder;
					allFilesList = rootImageFolder.GetAllFilesList(null);
					num = 0;
					strArrays = filepaths;
					for (i = 0; i < (int)strArrays.Length; i++)
					{
						str = strArrays[i];
						if (cancellationToken.IsCancellationRequested)
						{
							break;
						}
						if (this.CheckFile(allFilesList, str) && this.AddFile(this.GetTargetFolder(rootImageFolder, str), str, type))
						{
							num++;
						}
					}
					return num;
				}
				case ContentType.Track:
				{
					allFilesList = rootImageFolder.GetAllFilesList(null);
					num = 0;
					strArrays = filepaths;
					for (i = 0; i < (int)strArrays.Length; i++)
					{
						str = strArrays[i];
						if (cancellationToken.IsCancellationRequested)
						{
							break;
						}
						if (this.CheckFile(allFilesList, str) && this.AddFile(this.GetTargetFolder(rootImageFolder, str), str, type))
						{
							num++;
						}
					}
					return num;
				}
				case ContentType.Video:
				{
					rootImageFolder = this.RootVideoFolder;
					allFilesList = rootImageFolder.GetAllFilesList(null);
					num = 0;
					strArrays = filepaths;
					for (i = 0; i < (int)strArrays.Length; i++)
					{
						str = strArrays[i];
						if (cancellationToken.IsCancellationRequested)
						{
							break;
						}
						if (this.CheckFile(allFilesList, str) && this.AddFile(this.GetTargetFolder(rootImageFolder, str), str, type))
						{
							num++;
						}
					}
					return num;
				}
				default:
				{
					allFilesList = rootImageFolder.GetAllFilesList(null);
					num = 0;
					strArrays = filepaths;
					for (i = 0; i < (int)strArrays.Length; i++)
					{
						str = strArrays[i];
						if (cancellationToken.IsCancellationRequested)
						{
							break;
						}
						if (this.CheckFile(allFilesList, str) && this.AddFile(this.GetTargetFolder(rootImageFolder, str), str, type))
						{
							num++;
						}
					}
					return num;
				}
			}
		}

		private async Task AddMusics(MultimediaFolder folder, string[] filesPath)
		{
			bool flag;
			if (filesPath != null && (int)filesPath.Length >= 1)
			{
				string[] strArrays = filesPath;
				for (int i = 0; i < (int)strArrays.Length; i++)
				{
					string str = strArrays[i];
					if (this.CheckMusicFile(str))
					{
						bool flag1 = false;
						foreach (Content allFilesList in this.RootMusicFolder.GetAllFilesList(null))
						{
							if (!allFilesList.Path.Equals(str))
							{
								continue;
							}
							flag1 = true;
							break;
						}
						if (!flag1)
						{
							await Task.Run<bool>(() => this.AddTrack(folder, str));
						}
					}
				}
			}
		}

		private int AddMusicsToRootFolder(string[] filesPath, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return 0;
			}
			if (filesPath == null || (int)filesPath.Length < 1)
			{
				return 0;
			}
			int num = 0;
			string[] strArrays = filesPath;
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str = strArrays[i];
				if (cancellationToken.IsCancellationRequested)
				{
					break;
				}
				if (this.CheckMusicFile(str) && this.AddTrack(this.RootMusicFolder, str))
				{
					num++;
				}
			}
			return num;
		}

		private bool AddTrack(MultimediaFolder rootfolder, string musictrack)
		{
			bool flag;
			if (this.CheckTrack(rootfolder, musictrack))
			{
				return false;
			}
			MultimediaFolder targetFolder = this.GetTargetFolder(rootfolder, musictrack);
			try
			{
				using (TagLib.File file = TagLib.File.Create(musictrack))
				{
					if (file != null)
					{
						string str = (file.Tag.FirstPerformer != null ? file.Tag.FirstPerformer : "Unknown artist");
						string str1 = (file.Tag.Album != null ? file.Tag.Album : "Unknown album");
						string fileName = (file.Tag.Title != null ? file.Tag.Title : "Unknown");
						string str2 = string.Join("/", file.Tag.Genres);
						if (string.IsNullOrWhiteSpace(str2))
						{
							str2 = "Unknown";
						}
						int track = (int)file.Tag.Track;
						TimeSpan duration = file.Properties.Duration;
						if (string.IsNullOrEmpty(fileName))
						{
							fileName = Path.GetFileName(musictrack);
						}
						Artist artist = this.CheckArtist(str);
						if (artist != null)
						{
							Album album = this.CheckAlbum(str1);
							if (album == null)
							{
								album = new Album(str1, null)
								{
									Preview = this.GetAlbumPreview(musictrack, album.ID, file.Tag.Pictures, false)
								};
								Genre genre = this.CheckGenre(str2, album.Preview) ?? new Genre(str2)
								{
									Preview = album.Preview
								};
								Track track1 = new Track(fileName, musictrack, targetFolder.ID, artist, album, genre, this.defaultTrackIcon, null)
								{
									TrackNumber = track,
									Duration = duration,
									LastUpdate = System.IO.File.GetLastWriteTimeUtc(musictrack)
								};
								Track track2 = track1;
								track2.Preview = (track2.Album.Preview == null ? this.defaultTrackIcon : track2.Album.Preview);
								this.TrackList.Add(track2);
								targetFolder.AddItem(track2);
							}
							else
							{
								if (album.Preview == null)
								{
									album.Preview = this.GetAlbumPreview(musictrack, album.ID, file.Tag.Pictures, true);
								}
								Genre genre1 = this.CheckGenre(str2, album.Preview) ?? new Genre(str2)
								{
									Preview = album.Preview
								};
								Track track3 = new Track(fileName, musictrack, targetFolder.ID, artist, album, genre1, this.defaultTrackIcon, null)
								{
									TrackNumber = track,
									Duration = duration,
									LastUpdate = System.IO.File.GetLastWriteTimeUtc(musictrack)
								};
								Track track4 = track3;
								track4.Preview = (track4.Album.Preview == null ? this.defaultTrackIcon : track4.Album.Preview);
								this.TrackList.Add(track4);
								targetFolder.AddItem(track4);
							}
						}
						else
						{
							artist = new Artist(str, this.defaultTrackIcon);
							Uri albumPreview = this.GetAlbumPreview(musictrack, artist.ID, file.Tag.Pictures, false);
							Album album1 = this.CheckAlbum(str1);
							if (album1 == null)
							{
								album1 = new Album(str1, albumPreview);
							}
							else if (albumPreview != null)
							{
								album1.Preview = albumPreview;
							}
							Genre genre2 = this.CheckGenre(str2, albumPreview) ?? new Genre(str2)
							{
								Preview = albumPreview
							};
							Track track5 = new Track(fileName, musictrack, targetFolder.ID, artist, album1, genre2, this.defaultTrackIcon, null)
							{
								TrackNumber = track,
								Duration = duration,
								LastUpdate = System.IO.File.GetLastWriteTimeUtc(musictrack)
							};
							Track track6 = track5;
							track6.Preview = (track6.Album.Preview == null ? this.defaultTrackIcon : track6.Album.Preview);
							targetFolder.AddItem(track6);
							this.TrackList.Add(track6);
						}
					}
					else
					{
						flag = false;
						return flag;
					}
				}
				return true;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		public void CancelAdding()
		{
			if (this.cancelToken != null)
			{
				this.cancelToken.Cancel();
				this.OnDataUpdated();
				this.contentAddingQueue.Clear();
				this.IsDataLoaded = true;
			}
		}

		private Album CheckAlbum(string albumname)
		{
			Album album;
			List<Track>.Enumerator enumerator = this.TrackList.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Track current = enumerator.Current;
					if (!current.Album.Name.Equals(albumname))
					{
						continue;
					}
					album = current.Album;
					return album;
				}
				return null;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return album;
		}

		private Artist CheckArtist(string artistname)
		{
			Artist artist;
			List<Track>.Enumerator enumerator = this.TrackList.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Track current = enumerator.Current;
					if (!current.Artist.Name.ToLower().Equals(artistname.ToLower()))
					{
						continue;
					}
					artist = current.Artist;
					return artist;
				}
				return null;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return artist;
		}

		private bool CheckFile(List<Content> files, string newfile)
		{
			bool flag;
			List<Content>.Enumerator enumerator = files.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Path.Equals(newfile))
					{
						continue;
					}
					flag = false;
					return flag;
				}
				return true;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return flag;
		}

		private MultimediaFolder CheckFolder(MultimediaFolder parentfolder, string name)
		{
			MultimediaFolder multimediaFolder;
			List<ItemBase>.Enumerator enumerator = parentfolder.ItemsList.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MultimediaFolder current = (MultimediaFolder)enumerator.Current;
					if (!(new FileInfo(((Content)current.ItemsList.First<ItemBase>()).Path)).DirectoryName.Equals(name))
					{
						continue;
					}
					multimediaFolder = current;
					return multimediaFolder;
				}
				return null;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return multimediaFolder;
		}

		private bool CheckFolderByName(MultimediaFolder item, string name)
		{
			bool flag;
			List<ItemBase>.Enumerator enumerator = item.ItemsList.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ItemBase current = enumerator.Current;
					if (current.Type != ItemType.Folder || !((MultimediaFolder)current).Name.Equals(name))
					{
						continue;
					}
					flag = true;
					return flag;
				}
				return false;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return flag;
		}

		private Genre CheckGenre(string genrename, Uri albumPreview)
		{
			Genre genre;
			List<Track>.Enumerator enumerator = this.TrackList.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Track current = enumerator.Current;
					if (current.Genre == null || !(current.Genre.Name == genrename))
					{
						continue;
					}
					Genre genre1 = current.Genre;
					if (genre1.Preview == null)
					{
						genre1.Preview = albumPreview;
					}
					genre = genre1;
					return genre;
				}
				return null;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return genre;
		}

		private bool CheckMusicFile(string filepath)
		{
			bool flag;
			List<Content>.Enumerator enumerator = this.RootMusicFolder.GetAllFilesList(null).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Path.Equals(filepath))
					{
						continue;
					}
					flag = false;
					return flag;
				}
				return true;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return flag;
		}

		private bool CheckTrack(MultimediaFolder folder, string pathtotrack)
		{
			bool flag;
			if (folder == null || pathtotrack.Length < 1)
			{
				return false;
			}
			List<Content>.Enumerator enumerator = folder.GetFilesList().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (((Track)enumerator.Current).Path != pathtotrack)
					{
						continue;
					}
					flag = true;
					return flag;
				}
				return false;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return flag;
		}

		private static BitmapFrame CreateResizedImage(ImageSource source, int width, int height, int margin)
		{
			Rect rect = new Rect((double)margin, (double)margin, (double)(width - margin * 2), (double)(height - margin * 2));
			DrawingGroup drawingGroup = new DrawingGroup();
			RenderOptions.SetBitmapScalingMode(drawingGroup, BitmapScalingMode.HighQuality);
			drawingGroup.Children.Add(new ImageDrawing(source, rect));
			DrawingVisual drawingVisual = new DrawingVisual();
			using (DrawingContext drawingContext = drawingVisual.RenderOpen())
			{
				drawingContext.DrawDrawing(drawingGroup);
			}
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
			renderTargetBitmap.Render(drawingVisual);
			return BitmapFrame.Create(renderTargetBitmap);
		}

		public void DeleteFileOrFolder(MultimediaFolder rootfolder, ItemBase itemtodelete)
		{
			this.IsDataLoaded = false;
			MultimediaFolder multimediaFolder = rootfolder.FindFolderByID(itemtodelete.Parent);
			multimediaFolder.RemoveItem(itemtodelete);
			if (multimediaFolder.GetAllFilesList(null).Count < 1 && multimediaFolder.ID != rootfolder.ID)
			{
				rootfolder.FindFolderByID(multimediaFolder.Parent).RemoveItem(multimediaFolder);
			}
			if (rootfolder.ID == this.RootMusicFolder.ID)
			{
				this.TrackList = (
					from i in this.RootMusicFolder.GetAllFilesList(null)
					select (Track)i).ToList<Track>();
			}
			this.ReloadAlbumsTracksCount();
			this.IsDataLoaded = true;
		}

		public void DeleteItems(MultimediaFolder rootfolder, List<Content> itemstodelete)
		{
			if (itemstodelete.Count == 0)
			{
				return;
			}
			this.IsDataLoaded = false;
			foreach (Content content in itemstodelete)
			{
				MultimediaFolder multimediaFolder = rootfolder.FindFolderByID(content.Parent);
				multimediaFolder.RemoveItem(content);
				if (content.ContentType == ContentType.Track)
				{
					this.TrackList.Remove((Track)content);
				}
				if (multimediaFolder.GetAllFilesList(null).Count >= 1)
				{
					continue;
				}
				rootfolder.RemoveItem(multimediaFolder);
			}
			this.ReloadAlbumsTracksCount();
			this.IsDataLoaded = true;
		}

		private static IEnumerable<string> FindAccessableFiles(string path, string file_pattern, bool recurse)
		{
			IEnumerator<FileInfo> enumerator;
			IEnumerator<DirectoryInfo> enumerator1;
			List<string> strs = new List<string>();
			string str = "mp4";
			if (System.IO.File.Exists(path))
			{
				yield return path;
			}
			else if (Directory.Exists(path))
			{
				if (file_pattern == null)
				{
					file_pattern = string.Concat("*.", str);
				}
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				try
				{
					enumerator = directoryInfo.EnumerateFiles(file_pattern).GetEnumerator();
				}
				catch (Exception exception)
				{
					enumerator = null;
				}
				while (true)
				{
					FileInfo current = null;
					try
					{
						if (enumerator == null || !enumerator.MoveNext())
						{
							break;
						}
						else
						{
							current = enumerator.Current;
						}
					}
					catch (UnauthorizedAccessException unauthorizedAccessException)
					{
						continue;
					}
					catch (PathTooLongException pathTooLongException)
					{
						continue;
					}
					yield return current.FullName;
				}
				if (recurse)
				{
					try
					{
						enumerator1 = directoryInfo.EnumerateDirectories("*").GetEnumerator();
					}
					catch (Exception exception1)
					{
						enumerator1 = null;
					}
					while (true)
					{
						DirectoryInfo current1 = null;
						try
						{
							if (enumerator1 == null || !enumerator1.MoveNext())
							{
								break;
							}
							else
							{
								current1 = enumerator1.Current;
							}
						}
						catch (UnauthorizedAccessException unauthorizedAccessException1)
						{
							continue;
						}
						catch (PathTooLongException pathTooLongException1)
						{
							continue;
						}
						foreach (string str1 in DataLibrary.FindAccessableFiles(current1.FullName, file_pattern, recurse))
						{
							yield return str1;
						}
					}
				}
			}
		}

		public static string[] FindFiles(string dir, string[] searchPatterns, SearchOption searchOption = SearchOption.AllDirectories)
		{
			//IEnumerable<string> strs = 
			//	from searchpattern in searchPatterns
			//	from f in DataLibrary.SearchFiles(dir, searchpattern).ToArray()
			//	select f;
			//return strs.ToArray<string>();

            return Enumerable.ToArray<string>(Enumerable.SelectMany<string, string, string>((IEnumerable<string>)searchPatterns, (Func<string, IEnumerable<string>>)(searchpattern => (IEnumerable<string>)DataLibrary.SearchFiles(dir, searchpattern).ToArray()), (Func<string, string, string>)((searchpattern, f) => f)));
        }

		private Uri GetAlbumPreview(string filepath, Guid fileGuid, IPicture[] pictures, bool onlytag = false)
		{
			Uri uri;
			if (pictures == null)
			{
				return null;
			}
			IPicture[] pictureArray = pictures;
			int num = 0;
			if (num >= (int)pictureArray.Length)
			{
				return null;
			}
			IPicture picture = pictureArray[num];
			try
			{
				using (MemoryStream memoryStream = new MemoryStream(picture.Data.Data))
				{
					try
					{
						PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
						BitmapDecoder bitmapDecoder = BitmapDecoder.Create(memoryStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
						pngBitmapEncoder.Interlace = PngInterlaceOption.On;
						pngBitmapEncoder.Frames.Add(DataLibrary.CreateResizedImage(bitmapDecoder.Frames[0], 100, 100, 0));
						using (FileStream fileStream = new FileStream(string.Format("{0}/{1}.{2}", this.localDataDir, fileGuid.ToString(), "png"), FileMode.Create))
						{
							pngBitmapEncoder.Save(fileStream);
							uri = new Uri(fileStream.Name);
						}
					}
					catch (ArgumentException argumentException)
					{
						memoryStream.Position = (long)0;
						Bitmap bitmap = new Bitmap(memoryStream);
						string str = Path.Combine(this.localDataDir, string.Format("{0}.{1}", fileGuid, "bmp"));
						bitmap.Save(str);
						uri = new Uri(str);
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				Logger instance = Logger.Instance;
				object[] message = new object[] { exception.Message, exception.StackTrace };
				instance.LogErrorFormat("[DataLibrary.GetAlbumPreview] Exception: {0}\n{1}", message);
				uri = null;
			}
			return uri;
		}

		public List<ItemBase> GetAlbums()
		{
			List<Guid> guids = new List<Guid>();
			List<ItemBase> itemBases = new List<ItemBase>();
			foreach (Track trackList in this.TrackList)
			{
				if (guids.Contains(trackList.Album.ID))
				{
					continue;
				}
				guids.Add(trackList.Album.ID);
				itemBases.Add(trackList);
			}
			return itemBases;
		}

		public List<Content> GetAlbumTracks(Guid id)
		{
			return (
				from track in this.TrackList
				where track.Album.ID == id
				orderby track.TrackNumber
				select track).Cast<Content>().ToList<Content>();
		}

		public List<ItemBase> GetArtists()
		{
			List<Guid> guids = new List<Guid>();
			List<ItemBase> itemBases = new List<ItemBase>();
			foreach (Track trackList in this.TrackList)
			{
				if (guids.Contains(trackList.Artist.ID))
				{
					continue;
				}
				guids.Add(trackList.Artist.ID);
				itemBases.Add(trackList);
			}
			List<ItemBase> albums = this.GetAlbums();
			for (int i = 0; i < itemBases.Count; i++)
			{
				Track item = (Track)itemBases[i];
				foreach (Track album in albums)
				{
					if (!(album.Artist.ID == item.Artist.ID) || !(album.Album.Preview != null))
					{
						continue;
					}
					itemBases[i] = album;
					break;
				}
			}
			return itemBases;
		}

		public List<Content> GetArtistsTracks(Guid id)
		{
			return (
				from track in this.TrackList
				where track.Artist.ID == id
				orderby track.TrackNumber
				select track).Cast<Content>().ToList<Content>();
		}

		private TimeSpan GetDuration(string filepath)
		{
			TimeSpan zero;
			try
			{
				ShellFile shellFile = ShellFile.FromFilePath(filepath);
				if (shellFile.Properties.System.Media.Duration.Value.HasValue)
				{
					ulong? value = shellFile.Properties.System.Media.Duration.Value;
					ulong num = value.Value / (long)10000;
					zero = TimeSpan.FromMilliseconds((double)((float)num));
				}
				else
				{
					zero = TimeSpan.Zero;
				}
			}
			catch
			{
				zero = TimeSpan.Zero;
			}
			return zero;
		}

		private string[] GetFilesFromArray(string[] files, string[] searchpatterns)
		{
			List<string> strs = new List<string>();
			string[] strArrays = files;
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str = strArrays[i];
				string str1 = string.Concat("*", Path.GetExtension(str));
				string[] strArrays1 = searchpatterns;
				int num = 0;
				while (num < (int)strArrays1.Length)
				{
					if (!strArrays1[num].ToLower().Equals(str1.ToLower()))
					{
						num++;
					}
					else
					{
						strs.Add(str);
						break;
					}
				}
			}
			return strs.ToArray();
		}

		public void GetFolderById(ref MultimediaFolder searchresult, Guid id, MultimediaFolder folderwheretosearch)
		{
			if (folderwheretosearch.ID == id)
			{
				searchresult = folderwheretosearch;
				return;
			}
			foreach (ItemBase itemsList in folderwheretosearch.ItemsList)
			{
				if (itemsList.Type != ItemType.Folder)
				{
					continue;
				}
				if (itemsList.ID != id)
				{
					this.GetFolderById(ref searchresult, id, (MultimediaFolder)itemsList);
				}
				else
				{
					searchresult = (MultimediaFolder)itemsList;
				}
				if (searchresult == null)
				{
					continue;
				}
				return;
			}
		}

		public List<ItemBase> GetGenres()
		{
			List<string> strs = new List<string>();
			List<ItemBase> itemBases = new List<ItemBase>();
			foreach (Track trackList in this.TrackList)
			{
				if (strs.Contains(trackList.Genre.Name))
				{
					continue;
				}
				strs.Add(trackList.Genre.Name);
				itemBases.Add(trackList);
			}
			for (int i = 0; i < itemBases.Count; i++)
			{
				Track item = (Track)itemBases[i];
				foreach (Track track in this.TrackList)
				{
					if (!track.Genre.Name.Equals(item.Genre.Name) || !(track.Artist.ID == item.Artist.ID) || !(track.Album.Preview != null))
					{
						continue;
					}
					itemBases[i] = track;
					break;
				}
			}
			return itemBases;
		}

		public List<Content> GetGenreTracks(string genre)
		{
			List<Content> contents = new List<Content>();
			foreach (Track trackList in this.TrackList)
			{
				if (trackList.Genre == null || !trackList.Genre.Name.Equals(genre))
				{
					continue;
				}
				contents.Add(trackList);
			}
			return contents;
		}

		public ItemBase GetItemById(Guid id, MultimediaFolder root)
		{
			ItemBase itemBase;
			if (root.ID == id)
			{
				return root;
			}
			List<ItemBase>.Enumerator enumerator = root.ItemsList.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ItemBase current = enumerator.Current;
					if (current.ID != id)
					{
						if (current.Type != ItemType.Folder)
						{
							continue;
						}
						ItemBase itemById = this.GetItemById(id, current as MultimediaFolder);
						if (itemById == null)
						{
							continue;
						}
						itemBase = itemById;
						return itemBase;
					}
					else
					{
						itemBase = current;
						return itemBase;
					}
				}
				return null;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return itemBase;
		}

		private Uri GetPreview(string file, ContentType type, Guid guid)
		{
			Uri uri;
			if (type != ContentType.Image)
			{
				Bitmap mediumBitmap = ShellFile.FromFilePath(file).Thumbnail.MediumBitmap;
				string str = string.Format("{0}/{1}.{2}", this.localDataDir, guid.ToString(), "png");
				mediumBitmap.Save(str, ImageFormat.Png);
				return new Uri(str);
			}
			using (FileStream fileStream = (new FileInfo(file)).OpenRead())
			{
				try
				{
					Image thumbnailImage = Image.FromStream(fileStream);
					SizeF sizeF = new SizeF((float)thumbnailImage.Width, (float)thumbnailImage.Height);
					float width = sizeF.Width / sizeF.Height;
					SizeF sizeF1 = new SizeF(200f, 200f);
					SizeF sizeF2 = (sizeF1.Width / sizeF1.Height > width ? new SizeF(sizeF.Width * sizeF1.Height / sizeF.Height, sizeF1.Height) : new SizeF(sizeF1.Width, sizeF.Height * sizeF1.Width / sizeF.Width));
					int num = (int)Math.Max(sizeF2.Width, 1f);
					int num1 = (int)Math.Max(sizeF2.Height, 1f);
					thumbnailImage = thumbnailImage.GetThumbnailImage(num, num1, null, IntPtr.Zero);
					string str1 = string.Format("{0}/{1}.{2}", this.localDataDir, guid.ToString(), "png");
					thumbnailImage.Save(str1, ImageFormat.Png);
					uri = new Uri(str1);
				}
				catch
				{
					uri = null;
				}
			}
			return uri;
		}

		private MultimediaFolder GetTargetFolder(MultimediaFolder rootfolder, string file)
		{
			DirectoryInfo directory = (new FileInfo(file)).Directory;
			MultimediaFolder multimediaFolder = this.CheckFolder(rootfolder, directory.FullName);
			if (multimediaFolder != null)
			{
				return multimediaFolder;
			}
			multimediaFolder = new MultimediaFolder(directory.Name, rootfolder.ID, directory.CreationTime);
			rootfolder.AddItem(multimediaFolder);
			return multimediaFolder;
		}

		private byte[] ImageToByteArray(BitmapImage image)
		{
			byte[] array;
			JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
			jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(image));
			using (MemoryStream memoryStream = new MemoryStream())
			{
				jpegBitmapEncoder.Save(memoryStream);
				array = memoryStream.ToArray();
			}
			return array;
		}

		public async Task LoadLibrary()
		{
			this.IsDataLoaded = false;
			await Task.Run(() => {
				if (Settings.Default.ID == Guid.Empty)
				{
					this.ID = Guid.NewGuid();
				}
				else
				{
					this.ID = Settings.Default.ID;
				}
				if (!string.IsNullOrWhiteSpace(Settings.Default.RootMusicFolder))
				{
					try
					{
						XmlSerializer xmlSerializer = new XmlSerializer(typeof(MultimediaFolder));
						using (StringReader stringReader = new StringReader(Settings.Default.RootMusicFolder))
						{
							this.RootMusicFolder = (MultimediaFolder)xmlSerializer.Deserialize(stringReader);
						}
					}
					catch
					{
					}
				}
				if (this.RootMusicFolder == null)
				{
					this.RootMusicFolder = new MultimediaFolder("RootMusicFolder", this.ID, DateTime.Now);
				}
				if (!string.IsNullOrWhiteSpace(Settings.Default.RootImageFolder))
				{
					try
					{
						XmlSerializer xmlSerializer1 = new XmlSerializer(typeof(MultimediaFolder));
						using (StringReader stringReader1 = new StringReader(Settings.Default.RootImageFolder))
						{
							this.RootImageFolder = (MultimediaFolder)xmlSerializer1.Deserialize(stringReader1);
						}
					}
					catch
					{
					}
				}
				if (this.RootImageFolder == null)
				{
					this.RootImageFolder = new MultimediaFolder("RootImageFolder", this.ID, DateTime.Now);
				}
				if (!string.IsNullOrWhiteSpace(Settings.Default.RootVideoFolder))
				{
					try
					{
						XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(MultimediaFolder));
						using (StringReader stringReader2 = new StringReader(Settings.Default.RootVideoFolder))
						{
							this.RootVideoFolder = (MultimediaFolder)xmlSerializer2.Deserialize(stringReader2);
						}
					}
					catch
					{
					}
				}
				if (this.RootVideoFolder == null)
				{
					this.RootVideoFolder = new MultimediaFolder("RootVideoFolder", this.ID, DateTime.Now);
				}
				foreach (Content allFilesList in this.RootMusicFolder.GetAllFilesList(null))
				{
					if (!System.IO.File.Exists(allFilesList.Path))
					{
						this.DeleteFileOrFolder(this.RootMusicFolder, allFilesList);
						try
						{
							if (System.IO.File.Exists(allFilesList.PreviewString))
							{
								System.IO.File.Delete(allFilesList.PreviewString);
							}
						}
						catch
						{
						}
					}
					else if (System.IO.File.Exists(allFilesList.PreviewString) || allFilesList.PreviewString.Contains("/Resources/"))
					{
						bool lastUpdate = allFilesList.LastUpdate != System.IO.File.GetLastWriteTimeUtc(allFilesList.Path);
					}
					else
					{
						this.DeleteFileOrFolder(this.RootMusicFolder, allFilesList);
					}
				}
				foreach (Content preview in this.RootVideoFolder.GetAllFilesList(null))
				{
					if (!System.IO.File.Exists(preview.Path))
					{
						this.DeleteFileOrFolder(this.RootVideoFolder, preview);
						try
						{
							if (System.IO.File.Exists(preview.PreviewString))
							{
								System.IO.File.Delete(preview.PreviewString);
							}
						}
						catch
						{
						}
					}
					else if (System.IO.File.Exists(preview.PreviewString))
					{
						if (preview.LastUpdate == System.IO.File.GetLastWriteTimeUtc(preview.Path))
						{
							continue;
						}
						preview.Preview = this.GetPreview(preview.Path, ContentType.Video, preview.ID);
					}
					else
					{
						this.DeleteFileOrFolder(this.RootVideoFolder, preview);
					}
				}
				foreach (Content content in this.RootImageFolder.GetAllFilesList(null))
				{
					if (!System.IO.File.Exists(content.Path))
					{
						this.DeleteFileOrFolder(this.RootImageFolder, content);
						try
						{
							if (System.IO.File.Exists(content.PreviewString))
							{
								System.IO.File.Delete(content.PreviewString);
							}
						}
						catch
						{
						}
					}
					else if (System.IO.File.Exists(content.PreviewString))
					{
						if (content.LastUpdate == System.IO.File.GetLastWriteTimeUtc(content.Path))
						{
							continue;
						}
						content.Preview = this.GetPreview(content.Path, ContentType.Image, content.ID);
					}
					else
					{
						this.DeleteFileOrFolder(this.RootImageFolder, content);
					}
				}
				this.TrackList = new List<Track>();
				foreach (ItemBase itemBase in this.RootMusicFolder.GetAllFilesList(null))
				{
					this.TrackList.Add((Track)itemBase);
				}
			});
			await this.RemoveDuplicatedObjects();
			this.RootFolder.AddItem(this.RootImageFolder);
			this.RootFolder.AddItem(this.RootMusicFolder);
			this.RootFolder.AddItem(this.RootVideoFolder);
			this.IsDataLoaded = true;
		}

		private TagLib.File MidiResolver(TagLib.File.IFileAbstraction abstraction, string mimetype, ReadStyle style)
		{
			if (mimetype != "taglib/mid")
			{
				return null;
			}
			return new MidiFile(abstraction.Name);
		}

		private void OnCacheDataFolderChanged(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType == WatcherChangeTypes.Deleted)
			{
				bool flag = false;
				FileInfo fileInfo = new FileInfo(e.Name);
				Guid empty = Guid.Empty;
				if (Guid.TryParse(fileInfo.Name.Replace(fileInfo.Extension, ""), out empty))
				{
					this.dispatcher.Invoke(() => {
						this.IsDataLoaded = false;
						flag = this.RemoveItemFromFolderById(this.RootImageFolder, empty);
						if (!flag)
						{
							flag = this.RemoveItemFromFolderById(this.RootVideoFolder, empty);
							if (!flag)
							{
								flag = this.RemoveItemFromFolderById(this.RootMusicFolder, empty);
							}
						}
						if (flag)
						{
							this.OnDataUpdated();
						}
						this.IsDataLoaded = true;
					});
				}
			}
		}

		private void OnContentAlreadyExist()
		{
			EventHandler eventHandler = this.ContentAlreadyExist;
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		private void OnDataLoaded()
		{
			EventHandler eventHandler = this.DataLoaded;
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		private void OnDataUpdated()
		{
			EventHandler eventHandler = this.DataUpdated;
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		private void ReloadAlbumsTracksCount()
		{
			foreach (Track artist in this.GetArtists())
			{
				List<Content> artistsTracks = this.GetArtistsTracks(artist.Artist.ID);
				artist.Artist.SongsCount = artistsTracks.Count;
				List<Album> albums = new List<Album>();
				foreach (Track artistsTrack in artistsTracks)
				{
					if (albums.Contains(artistsTrack.Album))
					{
						continue;
					}
					albums.Add(artistsTrack.Album);
				}
				artist.Artist.AlbumsCount = albums.Count;
			}
			foreach (Track album in this.GetAlbums())
			{
				List<Content> albumTracks = this.GetAlbumTracks(album.Album.ID);
				album.Album.SongsCount = albumTracks.Count;
			}
			foreach (Track genre in this.GetGenres())
			{
				List<Content> genreTracks = this.GetGenreTracks(genre.Genre.Name);
				genre.Genre.SongsCount = genreTracks.Count;
			}
		}

		private async Task RemoveDuplicatedObjects()
		{
			if (this.TrackList.Count >= 1)
			{
				await Task.Run(() => {
					List<Artist> artists = new List<Artist>();
					List<Guid> guids = new List<Guid>();
					artists.Add(this.TrackList[0].Artist);
					guids.Add(this.TrackList[0].Artist.ID);
					foreach (Track trackList in this.TrackList)
					{
						if (guids.Contains(trackList.Artist.ID))
						{
							continue;
						}
						guids.Add(trackList.Artist.ID);
						artists.Add(trackList.Artist);
					}
					foreach (Artist artist in artists)
					{
						foreach (Track track in this.TrackList)
						{
							if (track.Artist.ID != artist.ID)
							{
								continue;
							}
							track.Artist = artist;
						}
					}
					List<Album> albums = new List<Album>();
					List<Guid> guids1 = new List<Guid>();
					albums.Add(this.TrackList[0].Album);
					guids1.Add(this.TrackList[0].Album.ID);
					foreach (Track trackList1 in this.TrackList)
					{
						if (guids1.Contains(trackList1.Album.ID))
						{
							continue;
						}
						guids1.Add(trackList1.Album.ID);
						albums.Add(trackList1.Album);
					}
					foreach (Album album in albums)
					{
						foreach (Track track1 in this.TrackList)
						{
							if (track1.Album.ID != album.ID)
							{
								continue;
							}
							track1.Album = album;
						}
					}
				});
			}
		}

		private bool RemoveItemFromFolderById(MultimediaFolder folder, Guid guid)
		{
			ItemBase itemBase = folder.FindItemById(guid);
			if (itemBase == null || !(itemBase is Content))
			{
				return false;
			}
			this.DeleteItems(folder, new List<Content>()
			{
				(Content)itemBase
			});
			return true;
		}

		public async Task SaveLibrary()
		{
			await Task.Run(() => {
				try
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(MultimediaFolder));
					using (StringWriter stringWriter = new StringWriter())
					{
						xmlSerializer.Serialize(stringWriter, this.RootMusicFolder);
						Settings.Default.RootMusicFolder = stringWriter.ToString();
					}
				}
				catch (Exception exception)
				{
				}
				try
				{
					XmlSerializer xmlSerializer1 = new XmlSerializer(typeof(MultimediaFolder));
					using (StringWriter stringWriter1 = new StringWriter())
					{
						xmlSerializer1.Serialize(stringWriter1, this.RootVideoFolder);
						Settings.Default.RootVideoFolder = stringWriter1.ToString();
					}
				}
				catch (Exception exception1)
				{
				}
				try
				{
					XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(MultimediaFolder));
					using (StringWriter stringWriter2 = new StringWriter())
					{
						xmlSerializer2.Serialize(stringWriter2, this.RootImageFolder);
						Settings.Default.RootImageFolder = stringWriter2.ToString();
					}
				}
				catch (Exception exception2)
				{
				}
				Settings.Default.ID = this.ID;
				Settings.Default.Save();
			});
		}

		private static List<string> SearchFiles(string folder, string pattern)
		{
			List<string> strs = new List<string>();
			strs.AddRange(DataLibrary.FindAccessableFiles(folder, pattern, true));
			return strs;
		}

		private void StartPreviewLoad()
		{
			Task.Run(() => {
				Parallel.ForEach<Content>(this.RootImageFolder.GetAllFilesList(null), (Content item) => {
					if (item.IsPreviewLoaded)
					{
						return;
					}
					item.Preview = this.GetPreview(item.Path, ContentType.Image, item.ID);
					item.IsPreviewLoaded = true;
				});
				Parallel.ForEach<Content>(this.RootVideoFolder.GetAllFilesList(null), (Content item) => {
					if (item.IsPreviewLoaded)
					{
						return;
					}
					item.Preview = this.GetPreview(item.Path, ContentType.Video, item.ID);
					item.IsPreviewLoaded = true;
				});
			});
		}

		public event EventHandler ContentAlreadyExist;

		public event EventHandler DataLoaded;

		public event EventHandler DataUpdated;
	}
}