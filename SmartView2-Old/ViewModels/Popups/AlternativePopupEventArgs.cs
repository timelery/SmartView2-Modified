using System;
using System.Runtime.CompilerServices;

namespace SmartView2.ViewModels.Popups
{
	public class AlternativePopupEventArgs : EventArgs
	{
		public bool CheckBoxState
		{
			get;
			private set;
		}

		public bool? Decision
		{
			get;
			private set;
		}

		public AlternativePopupEventArgs(bool? decision, bool checkBoxState)
		{
			this.CheckBoxState = checkBoxState;
			this.Decision = decision;
		}
	}
}