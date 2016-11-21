using MediaLibrary.DataModels;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SmartView2.Native.MediaLibrary
{
	public class FolderFileTemplateSelector : DataTemplateSelector
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

		public FolderFileTemplateSelector()
		{
		}

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			ItemBase itemBase = (ItemBase)item;
			switch (itemBase.Type)
			{
				case ItemType.Content:
				{
					Content content = (Content)itemBase;
					return this.FileTemplate;
				}
				case ItemType.Folder:
				{
					return this.FolderTemplate;
				}
			}
			return null;
		}
	}
}