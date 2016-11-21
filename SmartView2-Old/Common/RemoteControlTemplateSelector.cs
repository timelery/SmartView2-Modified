using SmartView2.Devices.RemoteControls;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SmartView2.Common
{
	public class RemoteControlTemplateSelector : DataTemplateSelector
	{
		public DataTemplate BdControlTemplate
		{
			get;
			set;
		}

		public DataTemplate HdmiControlTemplate
		{
			get;
			set;
		}

		public DataTemplate HtsControlTemplate
		{
			get;
			set;
		}

		public DataTemplate StbControlTemplate
		{
			get;
			set;
		}

		public DataTemplate TvControlTemplate
		{
			get;
			set;
		}

		public DataTemplate UnknownControlTemplate
		{
			get;
			set;
		}

		public RemoteControlTemplateSelector()
		{
		}

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item is TvRemoteControl)
			{
				return this.TvControlTemplate;
			}
			if (item is BdRemoteControl)
			{
				return this.BdControlTemplate;
			}
			if (item is HtsRemoteControl)
			{
				return this.HtsControlTemplate;
			}
			if (item is StbRemoteControl)
			{
				return this.StbControlTemplate;
			}
			if (item is HdmiRemoteControl)
			{
				return this.HdmiControlTemplate;
			}
			if (!(item is UnknownRemoteControl))
			{
				return null;
			}
			return this.UnknownControlTemplate;
		}
	}
}