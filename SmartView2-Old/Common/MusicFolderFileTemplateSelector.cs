using SmartView2.ViewModels.MultimediaInner;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SmartView2.Common
{
	public class MusicFolderFileTemplateSelector : DataTemplateSelector
	{
		public DataTemplate FileTemplate
		{
			get;
			set;
		}

		public DataTemplate FolderTemplate
		{
			get;
			set;
		}

		public MusicFolderFileTemplateSelector()
		{
		}

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			MusicViewModel musicViewModel = item as MusicViewModel;
			if (musicViewModel == null)
			{
				return null;
			}
			if (musicViewModel.IsInsideFolder)
			{
				return this.FileTemplate;
			}
			return this.FolderTemplate;
		}
	}
}