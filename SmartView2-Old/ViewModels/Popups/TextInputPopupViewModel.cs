using SmartView2.Core;
using SmartView2.Core.Commands;
using SmartView2.Views.Popups;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using UIFoundation;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels.Popups
{
	[RelatedView(typeof(TextInputPopupView))]
	public class TextInputPopupViewModel : PopupViewModel
	{
		private ITargetDevice targetDevice;

		private string text = string.Empty;

		private int cursorPosition;

		public int CursorPosition
		{
			get
			{
				return this.cursorPosition;
			}
			set
			{
				base.SetProperty<int>(ref this.cursorPosition, value, "CursorPosition");
			}
		}

		public ICommand EndInputCommand
		{
			get;
			private set;
		}

		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.SetText(value);
			}
		}

		public TextInputPopupViewModel(ITargetDevice targetDevice)
		{
			if (targetDevice == null)
			{
				throw new ArgumentNullException("targetDevice");
			}
			this.targetDevice = targetDevice;
			this.EndInputCommand = new AsyncCommand((object arg) => this.targetDevice.EndInputAsync());
		}

		public override void OnNavigateFrom()
		{
			base.OnNavigateFrom();
			this.targetDevice.TextUpdated -= new EventHandler<UpdateTextEventArgs>(this.targetDevice_TextUpdated);
		}

		public override void OnNavigateTo(object p)
		{
			base.OnNavigateTo(p);
			this.targetDevice.TextUpdated += new EventHandler<UpdateTextEventArgs>(this.targetDevice_TextUpdated);
			this.text = string.Empty;
			base.OnPropertyChanged(this, "Text");
			this.CursorPosition = 0;
			if (p != null && p is string)
			{
				this.text = p.ToString();
				base.OnPropertyChanged(this, "Text");
				this.CursorPosition = this.text.Length;
			}
		}

		private async void SetText(string text)
		{
			this.text = text;
			await this.targetDevice.SetInputTextAsync(text);
		}

		private void targetDevice_TextUpdated(object sender, UpdateTextEventArgs e)
		{
			this.text = e.Text;
			base.OnPropertyChanged(this, "Text");
			this.CursorPosition = this.text.Length;
		}
	}
}