using SmartView2.Common.Enums;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SmartView2.Common
{
	public class SettingsTemplateSelector : DataTemplateSelector
	{
		public DataTemplate ChangeDeviceTemplate
		{
			get;
			set;
		}

		public DataTemplate ClosedCaptionTemplate
		{
			get;
			set;
		}

		public DataTemplate GuideTemplate
		{
			get;
			set;
		}

		public DataTemplate LanguageTemplate
		{
			get;
			set;
		}

		public DataTemplate LicenseTemplate
		{
			get;
			set;
		}

		public DataTemplate VersionTemplate
		{
			get;
			set;
		}

		public SettingsTemplateSelector()
		{
		}

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item == null)
			{
				return this.GuideTemplate;
			}
			switch ((SettingsMenu)item)
			{
				case SettingsMenu.Guide:
				{
					return this.GuideTemplate;
				}
				case SettingsMenu.Version:
				{
					return this.VersionTemplate;
				}
				case SettingsMenu.Language:
				{
					return this.LanguageTemplate;
				}
				case SettingsMenu.License:
				{
					return this.LicenseTemplate;
				}
				case SettingsMenu.ClosedCaption:
				{
					return this.ClosedCaptionTemplate;
				}
				case SettingsMenu.ChangeTV:
				{
					return this.ChangeDeviceTemplate;
				}
			}
			return this.GuideTemplate;
		}
	}
}