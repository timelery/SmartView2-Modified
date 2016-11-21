using SmartView2.Properties;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Threading;
using System.Windows.Media;

namespace SmartView2.Models.Settings
{
	public class SettingsModel : INotifyPropertyChanged
	{
		private bool closedCaption;

		private Color characterColor = Colors.White;

		private SettingsModel.Opacity characterOpacity;

		private float characterSize;

		private SettingsModel.EdgeTypes edgeType;

		private Color edgeColor = Colors.White;

		private SettingsModel.FontStyles fontStyle;

		private Color captionBackgroundColor = Colors.White;

		private SettingsModel.Opacity captionBackgroundOpacity;

		private Color captionWindowColor = Colors.White;

		private SettingsModel.Opacity captionWindowOpacity;

		public Color CaptionBackgroundColor
		{
			get
			{
				return this.captionBackgroundColor;
			}
			set
			{
				this.captionBackgroundColor = value;
				this.OnPropertyChanged("CaptionBackgroundColor");
			}
		}

		public SettingsModel.Opacity CaptionBackgroundOpacity
		{
			get
			{
				return this.captionBackgroundOpacity;
			}
			set
			{
				this.captionBackgroundOpacity = value;
				this.OnPropertyChanged("CaptionBackgroundOpacity");
			}
		}

		public Color CaptionWindowColor
		{
			get
			{
				return this.captionWindowColor;
			}
			set
			{
				this.captionWindowColor = value;
				this.OnPropertyChanged("CaptionWindowColor");
			}
		}

		public SettingsModel.Opacity CaptionWindowOpacity
		{
			get
			{
				return this.captionWindowOpacity;
			}
			set
			{
				this.captionWindowOpacity = value;
				this.OnPropertyChanged("CaptionWindowOpacity");
			}
		}

		public Color CharacterColor
		{
			get
			{
				return this.characterColor;
			}
			set
			{
				this.characterColor = value;
				this.OnPropertyChanged("CharacterColor");
			}
		}

		public SettingsModel.Opacity CharacterOpacity
		{
			get
			{
				return this.characterOpacity;
			}
			set
			{
				this.characterOpacity = value;
				this.OnPropertyChanged("CharacterOpacity");
			}
		}

		public float CharacterSize
		{
			get
			{
				return this.characterSize;
			}
			set
			{
				this.characterSize = value;
				this.OnPropertyChanged("CharacterSize");
			}
		}

		public bool ClosedCaption
		{
			get
			{
				return this.closedCaption;
			}
			set
			{
				this.closedCaption = value;
				this.OnPropertyChanged("ClosedCaption");
			}
		}

		public static SettingsModel Defaults
		{
			get
			{
				return SettingsModel.GetDefaultValues();
			}
		}

		public Color EdgeColor
		{
			get
			{
				return this.edgeColor;
			}
			set
			{
				this.edgeColor = value;
				this.OnPropertyChanged("EdgeColor");
			}
		}

		public SettingsModel.EdgeTypes EdgeType
		{
			get
			{
				return this.edgeType;
			}
			set
			{
				this.edgeType = value;
				this.OnPropertyChanged("EdgeType");
			}
		}

		public SettingsModel.FontStyles FontStyle
		{
			get
			{
				return this.fontStyle;
			}
			set
			{
				this.fontStyle = value;
				this.OnPropertyChanged("FontStyle");
			}
		}

		public SettingsModel()
		{
		}

		private static SettingsModel GetDefaultValues()
		{
			SettingsModel settingsModel = new SettingsModel()
			{
				closedCaption = SmartView2.Properties.Settings.Default.ClosedCaption,
				characterColor = SmartView2.Properties.Settings.Default.DefaultCharacterColor,
				characterOpacity = (SettingsModel.Opacity)SmartView2.Properties.Settings.Default.DefaultCharacterOpacity,
				characterSize = SmartView2.Properties.Settings.Default.DefaultCharacterSize,
				edgeType = (SettingsModel.EdgeTypes)SmartView2.Properties.Settings.Default.DefaultEdgeType,
				edgeColor = SmartView2.Properties.Settings.Default.DefaultEdgeColor,
				fontStyle = (SettingsModel.FontStyles)SmartView2.Properties.Settings.Default.DefaultFontStyle,
				captionBackgroundColor = SmartView2.Properties.Settings.Default.DefaultCaptionBackgroundColor,
				captionBackgroundOpacity = (SettingsModel.Opacity)SmartView2.Properties.Settings.Default.DefaultCaptionBackgroundOpacity,
				captionWindowColor = SmartView2.Properties.Settings.Default.DefaultCaptionWindowColor,
				captionWindowOpacity = (SettingsModel.Opacity)SmartView2.Properties.Settings.Default.DefaultCaptionWindowOpacity
			};
			return settingsModel;
		}

		public static SettingsModel Load()
		{
			SettingsModel settingsModel = new SettingsModel()
			{
				closedCaption = SmartView2.Properties.Settings.Default.ClosedCaption,
				characterColor = SmartView2.Properties.Settings.Default.CharacterColor,
				characterOpacity = (SettingsModel.Opacity)SmartView2.Properties.Settings.Default.CharacterOpacity,
				characterSize = SmartView2.Properties.Settings.Default.CharacterSize,
				edgeType = (SettingsModel.EdgeTypes)SmartView2.Properties.Settings.Default.EdgeType,
				edgeColor = SmartView2.Properties.Settings.Default.EdgeColor,
				fontStyle = (SettingsModel.FontStyles)SmartView2.Properties.Settings.Default.FontStyle,
				captionBackgroundColor = SmartView2.Properties.Settings.Default.CaptionBackgroundColor,
				captionBackgroundOpacity = (SettingsModel.Opacity)SmartView2.Properties.Settings.Default.CaptionBackgroundOpacity,
				captionWindowColor = SmartView2.Properties.Settings.Default.CaptionWindowColor,
				captionWindowOpacity = (SettingsModel.Opacity)SmartView2.Properties.Settings.Default.CaptionWindowOpacity
			};
			return settingsModel;
		}

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
			if (propertyChangedEventHandler != null)
			{
				propertyChangedEventHandler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public void Save(SettingsModel settings)
		{
			SmartView2.Properties.Settings.Default.ClosedCaption = settings.closedCaption;
			SmartView2.Properties.Settings.Default.CharacterColor = settings.characterColor;
			SmartView2.Properties.Settings.Default.CharacterOpacity = (int)settings.characterOpacity;
			SmartView2.Properties.Settings.Default.CharacterSize = settings.characterSize;
			SmartView2.Properties.Settings.Default.EdgeType = (int)settings.edgeType;
			SmartView2.Properties.Settings.Default.EdgeColor = settings.edgeColor;
			SmartView2.Properties.Settings.Default.FontStyle = (int)settings.fontStyle;
			SmartView2.Properties.Settings.Default.CaptionBackgroundColor = settings.captionBackgroundColor;
			SmartView2.Properties.Settings.Default.CaptionBackgroundOpacity = (int)settings.captionBackgroundOpacity;
			SmartView2.Properties.Settings.Default.CaptionWindowColor = settings.captionWindowColor;
			SmartView2.Properties.Settings.Default.CaptionWindowOpacity = (int)settings.captionWindowOpacity;
			SmartView2.Properties.Settings.Default.Save();
			if (SettingsModel.SettingsSaved != null)
			{
				SettingsModel.SettingsSaved(this, null);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public static event EventHandler SettingsSaved;

		public enum EdgeTypes
		{
			None,
			Raised,
			Depressed,
			Uniform,
			DropShadow
		}

		public enum FontStyles
		{
			Default,
			FontStyle1,
			FontStyle2,
			FontStyle3,
			FontStyle4
		}

		public enum Opacity
		{
			Default,
			Translucent,
			Transparency,
			Opacity
		}
	}
}