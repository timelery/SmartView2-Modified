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
	[RelatedView(typeof(OkCancelPopupView))]
	public class OkCancelPopupViewModel : PopupViewModel
	{
		private string caption = string.Empty;

		private string message = string.Empty;

		private string checkBoxMessage = ResourcesModel.Instanse.COM_TV_DO_NOT_SHOW_AGAIN;

		private Action<AlternativePopupEventArgs> callback;

		private bool checkBoxState;

		public ICommand CancelCommand
		{
			get;
			private set;
		}

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

		public ICommand OkCommand
		{
			get;
			private set;
		}

		public OkCancelPopupViewModel(string caption, string message) : this(caption, message, false, null, null)
		{
		}

		public OkCancelPopupViewModel(string caption, string message, bool checkBoxVisible) : this(caption, message, checkBoxVisible, null, null)
		{
		}

		public OkCancelPopupViewModel(string caption, string message, string checkBoxMessage) : this(caption, message, true, checkBoxMessage, null)
		{
		}

		private OkCancelPopupViewModel(string caption, string message, bool checkBoxVisible, string checkBoxMessage, Action<AlternativePopupEventArgs> callback)
		{
			this.OkCommand = new Command(new Action<object>(this.OnOk));
			this.CancelCommand = new Command(new Action<object>(this.OnCancel));
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

		private void OnCancel(object obj)
		{
			base.Controller.ClosePopup(new AlternativePopupEventArgs(new bool?(false), this.CheckBoxState));
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

		private void OnOk(object obj)
		{
			base.Controller.ClosePopup(new AlternativePopupEventArgs(new bool?(true), this.CheckBoxState));
		}
	}
}