using SmartView2.Properties;
using SmartView2.Views.Popups;
using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels.Popups
{
	[RelatedView(typeof(LicensePopup))]
	internal class LicenseViewModel : PopupViewModel
	{
		public FlowDocument LicenseDocument
		{
			get
			{
				FlowDocument flowDocument = new FlowDocument();
				TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
				string str = "license.rtf";
				if (!File.Exists(str))
				{
					//File.WriteAllText(str, Resources.OpenSourceLicense);
				}
				using (FileStream fileStream = File.Open(str, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					textRange.Load(fileStream, DataFormats.Rtf);
				}
				return flowDocument;
			}
		}

		public LicenseViewModel()
		{
		}
	}
}