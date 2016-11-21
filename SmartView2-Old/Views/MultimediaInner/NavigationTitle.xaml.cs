using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace SmartView2.Views.MultimediaInner
{
	public partial class NavigationTitle : UserControl
	{
		private readonly static DependencyProperty titleTextProperty;

		private readonly static DependencyProperty backCommandProperty;

		public ICommand BackCommand
		{
			get
			{
				return (ICommand)base.GetValue(NavigationTitle.backCommandProperty);
			}
			set
			{
				base.SetValue(NavigationTitle.backCommandProperty, value);
			}
		}

		public string TitleText
		{
			get
			{
				return (string)base.GetValue(NavigationTitle.titleTextProperty);
			}
			set
			{
				base.SetValue(NavigationTitle.titleTextProperty, value);
			}
		}

		static NavigationTitle()
		{
			NavigationTitle.titleTextProperty = DependencyProperty.Register("TitleText", typeof(string), typeof(NavigationTitle), new PropertyMetadata(string.Empty));
			NavigationTitle.backCommandProperty = DependencyProperty.Register("BackCommand", typeof(ICommand), typeof(NavigationTitle), new PropertyMetadata(null));
		}

		public NavigationTitle()
		{
			this.InitializeComponent();
		}
	}
}