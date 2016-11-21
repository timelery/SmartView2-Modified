using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SmartView2.Common
{
	public class GuideSettingsTemplateSelector : DataTemplateSelector
	{
		public DataTemplate IntroductionTempate
		{
			get;
			set;
		}

		public DataTemplate MultimediaTemplate
		{
			get;
			set;
		}

		public GuideSettingsTemplateSelector()
		{
		}

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item == null || !(item is string))
			{
				return this.IntroductionTempate;
			}
			if ((string)item == "Introduction")
			{
				return this.IntroductionTempate;
			}
			return this.MultimediaTemplate;
		}
	}
}