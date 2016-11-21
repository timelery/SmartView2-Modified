using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SmartView2.Controls
{
	public class BindableRichTextBox : RichTextBox
	{
		public readonly static DependencyProperty DocumentProperty;

		public new FlowDocument Document
		{
			get
			{
				return (FlowDocument)base.GetValue(BindableRichTextBox.DocumentProperty);
			}
			set
			{
				base.SetValue(BindableRichTextBox.DocumentProperty, value);
			}
		}

		static BindableRichTextBox()
		{
			BindableRichTextBox.DocumentProperty = DependencyProperty.Register("Document", typeof(FlowDocument), typeof(BindableRichTextBox), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(BindableRichTextBox.OnDocumentChanged)));
		}

		public BindableRichTextBox()
		{
		}

		public static void OnDocumentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if (obj == null)
			{
				return;
			}
			RichTextBox flowDocument = obj as RichTextBox;
			if (flowDocument != null)
			{
				if (args.NewValue == null)
				{
					flowDocument.Document = new FlowDocument();
					return;
				}
				flowDocument.Document = args.NewValue as FlowDocument;
			}
		}
	}
}