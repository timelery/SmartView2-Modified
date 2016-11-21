using SmartView2.Core.Commands;
using SmartView2.Views.Popups;
using System;
using System.Runtime.CompilerServices;
using UIFoundation;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels.Popups
{
	[RelatedView(typeof(MessagePopupView))]
	public class MessagePopupViewModel : PopupViewModel
	{
		private string message;

		private string caption;

		private bool checkBoxState;

		public string Caption
		{
			get
			{
				return this.caption;
			}
			set
			{
				base.SetProperty<string>(ref this.caption, value, "Caption");
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

		public MessagePopupViewModel()
		{
			base.CloseCommand = new Command(new Action<object>(this.OnClose));
		}

		public MessagePopupViewModel(string message)
		{
			this.Message = message;
		}

		public MessagePopupViewModel(string message, string caption)
		{
			this.Message = message;
			this.Caption = caption;
		}

		public MessagePopupViewModel(string message, string caption, bool checkbox)
		{
			this.Message = message;
			this.Caption = caption;
			this.IsCheckBoxVisible = checkbox;
			base.CloseCommand = new Command(new Action<object>(this.OnClose));
		}

		private void OnClose(object obj)
		{
			base.Controller.ClosePopup(this.CheckBoxState);
		}

		public override void OnNavigateTo(object p)
		{
			base.OnNavigateTo(p);
			if (p is string)
			{
				this.Message = (string)p;
			}
			object[] objArray = p as object[];
			if (objArray != null)
			{
				if ((int)objArray.Length == 2)
				{
					this.Message = (string)objArray[1];
					this.Caption = (string)objArray[0];
					return;
				}
				this.Message = (string)objArray[1];
				this.Caption = (string)objArray[0];
				this.IsCheckBoxVisible = (!(objArray[2] is bool) ? false : (bool)objArray[2]);
			}
		}
	}
}