using SmartView2.Views.Popups;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UIFoundation;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels.Popups
{
	[RelatedView(typeof(PreviewPopupView))]
	public class PreviewPopupViewModel : MessagePopupViewModel
	{
		private Style textStyle;

		public Style TextStyle
		{
			get
			{
				return this.textStyle;
			}
			set
			{
				base.SetProperty<Style>(ref this.textStyle, value, "TextStyle");
			}
		}

		public PreviewPopupViewModel(string name, string caption, Style textStyle = null) : base(name, caption)
		{
			if (textStyle == null)
			{
				textStyle = new Style();
				textStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, new SolidColorBrush(Colors.White)));
			}
			this.TextStyle = textStyle;
		}
	}
}