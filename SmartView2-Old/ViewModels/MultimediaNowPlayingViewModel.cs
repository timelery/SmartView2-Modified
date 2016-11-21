using MediaLibrary.DataModels;
using SmartView2.Core;
using SmartView2.ViewModels.MultimediaInner.VisualState;
using SmartView2.Views;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using UIFoundation;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels
{
	[RelatedView(typeof(MultimediaNowPlayingPage))]
	public class MultimediaNowPlayingViewModel : PageViewModel
	{
		private IMultiScreen multiScreen;

		private ICommand goBack;

		private ICommand playPreviousFile;

		private ICommand play;

		private ICommand pause;

		private ICommand playNextFile;

		private ICommand changePosition;

		private ICommand playOrPause;

		private SavedMultimediaVisualState vs;

		private bool isContentNotEmpty;

		public ICommand ChangePosition
		{
			get
			{
				return this.changePosition;
			}
		}

		public MediaLibrary.DataModels.ContentType ContentType
		{
			get
			{
				if (this.multiScreen.CurrentMediaContent == null)
				{
					return MediaLibrary.DataModels.ContentType.Image;
				}
				return this.multiScreen.CurrentMediaContent.ContentType;
			}
		}

		public bool IsContentNotEmpty
		{
			get
			{
				return this.isContentNotEmpty;
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

		public IMultiScreen MultiScreen
		{
			get
			{
				return this.multiScreen;
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

		public ICommand PlayNextFile
		{
			get
			{
				return this.playNextFile;
			}
		}

		public ICommand PlayOrPause
		{
			get
			{
				return this.playOrPause;
			}
		}

		public ICommand PlayPreviousFile
		{
			get
			{
				return this.playPreviousFile;
			}
		}

		public MultimediaNowPlayingViewModel(SavedMultimediaVisualState vs, IMultiScreen multiScreen)
		{
			this.multiScreen = multiScreen;
			this.multiScreen.MultiscreenQueueUpdated += new EventHandler(this.multiScreen_MultiscreenQueueUpdated);
			this.multiScreen.MultiscreenCurrentMediaContentUpdated += new EventHandler(this.multiScreen_MultiscreenCurrentMediaContentUpdated);
			this.vs = vs;
			this.playPreviousFile = new UICommand(new Action<object>(this.OnPlayPreviousFile), new Predicate<object>(this.HasCurrentMediaContentDuration));
			this.play = new UICommand(new Action<object>(this.OnPlay), new Predicate<object>(this.IsCurrentMediaContentEmpty));
			this.pause = new UICommand(new Action<object>(this.OnPause), new Predicate<object>(this.IsCurrentMediaContentEmpty));
			this.playNextFile = new UICommand(new Action<object>(this.OnPlayNextFile), null);
			this.changePosition = new UICommand(new Action<object>(this.OnChangePosition), new Predicate<object>(this.HasCurrentMediaContentDuration));
			this.playOrPause = new UICommand(new Action<object>(this.OnPlayOrPause), new Predicate<object>(this.IsCurrentMediaContentEmpty));
		}

		private bool HasCurrentMediaContentDuration(object obj)
		{
			if (this.multiScreen.CurrentMediaContent == null)
			{
				return false;
			}
			return this.multiScreen.CurrentMediaContent.ContentType != MediaLibrary.DataModels.ContentType.Image;
		}

		private bool IsCurrentMediaContentEmpty(object obj)
		{
			return this.multiScreen.CurrentMediaContent != null;
		}

		private void multiScreen_MultiscreenCurrentMediaContentUpdated(object sender, EventArgs e)
		{
			base.OnPropertyChanged(this, "ContentType");
			CommandManager.InvalidateRequerySuggested();
		}

		private void multiScreen_MultiscreenQueueUpdated(object sender, EventArgs e)
		{
			base.OnPropertyChanged(this, "IsQueueReady");
			CommandManager.InvalidateRequerySuggested();
		}

		private void OnChangePosition(object obj)
		{
			if (obj is double)
			{
				this.multiScreen.SetCurrentMediaTimePosition((double)obj);
			}
		}

		public override async Task OnNavigateFromAsync()
		{
			this.multiScreen.MultiscreenQueueUpdated -= new EventHandler(this.multiScreen_MultiscreenQueueUpdated);
			//await this.<>n__FabricatedMethod6();
		}

		public override async Task OnNavigateToAsync()
		{
			//await this.<>n__FabricatedMethod2();
			this.isContentNotEmpty = this.IsCurrentMediaContentEmpty(null);
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

		private void OnPlayOrPause(object obj)
		{
			if (this.multiScreen.MediaState == MediaState.Play)
			{
				this.multiScreen.MediaPause();
				return;
			}
			this.multiScreen.MediaPlay();
		}

		private void OnPlayPreviousFile(object obj)
		{
			this.multiScreen.SetCurrentMediaTimePosition(0);
		}
	}
}