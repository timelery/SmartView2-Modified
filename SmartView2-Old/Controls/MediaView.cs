using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SmartView2.Controls
{
	[TemplatePart(Name="PART_BitmapCache", Type=typeof(Image))]
	internal class MediaView : Control, INotifyPropertyChanged
	{
		private const string PART_OutputImage = "PART_BitmapCache";

		private Image outputImage;

		private BitmapImage bitmap = new BitmapImage();

		public BitmapImage Bitmap
		{
			get
			{
				return this.bitmap;
			}
			private set
			{
				this.bitmap = value;
				this.RisePropertyChanged("Bitmap");
			}
		}

		static MediaView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(MediaView), new FrameworkPropertyMetadata(typeof(MediaView)));
		}

		public MediaView()
		{
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this.outputImage == null)
			{
				this.outputImage = base.GetTemplateChild("PART_BitmapCache") as Image;
				Binding binding = new Binding("Bitmap")
				{
					Source = this
				};
				this.outputImage.SetBinding(Image.SourceProperty, binding);
			}
		}

		private void RisePropertyChanged(string prop)
		{
			PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
			if (propertyChangedEventHandler != null)
			{
				propertyChangedEventHandler(this, new PropertyChangedEventArgs(prop));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}