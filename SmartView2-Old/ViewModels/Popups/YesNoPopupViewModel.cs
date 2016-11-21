using SmartView2.Core.Commands;
using SmartView2.Properties;
using SmartView2.Views.Popups;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using UIFoundation;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels.Popups
{
	[RelatedView(typeof(YesNoPopupView))]
	public class YesNoPopupViewModel : PopupViewModel
	{
		private string caption = string.Empty;

		private string message = string.Empty;

		private string checkBoxMessage = ResourcesModel.Instanse.COM_TV_DO_NOT_SHOW_AGAIN;

		private Action<AlternativePopupEventArgs> callback;

		private bool checkBoxState;

		public string Caption
		{
			get
			{
				return this.caption;
			}
			set
			{
				this.caption = value;
			}
		}

		public string CheckBoxMessage
		{
			get
			{
				return this.checkBoxMessage;
			}
			set
			{
				this.checkBoxMessage = value;
			}
		}

		public bool CheckBoxState
		{
			get
			{
				return this.checkBoxState;
			}
			set
			{
				base.SetProperty<bool>(ref this.checkBoxState, value, "CheckBoxState");
			}
		}

		public bool IsCheckBoxVisible
		{
			get;
			private set;
		}

		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				base.SetProperty<string>(ref this.message, value, "Message");
			}
		}

		public ICommand NoCommand
		{
			get;
			private set;
		}

		public ICommand YesCommand
		{
			get;
			private set;
		}

		public YesNoPopupViewModel(string caption, string message) : this(caption, message, false, null, null)
		{
		}

		public YesNoPopupViewModel(string caption, string message, bool checkBoxVisible) : this(caption, message, checkBoxVisible, null, null)
		{
		}

		public YesNoPopupViewModel(string caption, string message, string checkBoxMessage) : this(caption, message, true, checkBoxMessage, null)
		{
		}

		private YesNoPopupViewModel(string caption, string message, bool checkBoxVisible, string checkBoxMessage, Action<AlternativePopupEventArgs> callback)
		{
			this.YesCommand = new Command(new Action<object>(this.OnYes));
			this.NoCommand = new Command(new Action<object>(this.OnNo));
			base.CloseCommand = new Command(new Action<object>(this.OnClose));
			this.Caption = caption;
			this.Message = message;
			this.IsCheckBoxVisible = checkBoxVisible;
			if (!string.IsNullOrEmpty(checkBoxMessage))
			{
				this.CheckBoxMessage = checkBoxMessage;
			}
			this.callback = callback;
		}

		private void OnClose(object obj)
		{
			base.Controller.ClosePopup(new AlternativePopupEventArgs(null, this.CheckBoxState));
		}

		public override void OnNavigateTo(object p)
		{
			string str = p as string;
			if (str != null)
			{
				this.Message = str;
			}
			this.CheckBoxState = false;
		}

		private void OnNo(object obj)
		{
			base.Controller.ClosePopup(new AlternativePopupEventArgs(new bool?(false), this.CheckBoxState));
		}

		private void OnYes(object obj)
		{
			base.Controller.ClosePopup(new AlternativePopupEventArgs(new bool?(true), this.CheckBoxState));
		}
	}
}