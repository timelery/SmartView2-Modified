using SmartView2.Models.Settings;
using SmartView2.Properties;
using SmartView2.Views.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UIFoundation;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels.Popups
{
	[RelatedView(typeof(CCSettingsPopup))]
	internal class CCSettingsViewModel : PopupViewModel
	{
		private SmartView2.Models.Settings.SettingsModel settingsModel;

		private ICommand previewCommand;

		private ICommand resetAllCommand;

		private ICommand okCommand;

		private PopupWrapper resetAllPopup;

		private PopupWrapper resetAllInfoPopup;

		private readonly List<Color> colors;

		private List<float> sizes;

		private List<SmartView2.Models.Settings.SettingsModel.FontStyles> fontStyles;

		private List<SmartView2.Models.Settings.SettingsModel.EdgeTypes> edgeTypes;

		private List<SmartView2.Models.Settings.SettingsModel.Opacity> opacities;

		private List<SmartView2.Models.Settings.SettingsModel.Opacity> allOpacities;

		public List<SmartView2.Models.Settings.SettingsModel.Opacity> AllOpacities
		{
			get
			{
				return this.allOpacities;
			}
		}

		public List<Color> Colors
		{
			get
			{
				return this.colors;
			}
		}

		public List<SmartView2.Models.Settings.SettingsModel.EdgeTypes> EdgeTypes
		{
			get
			{
				return this.edgeTypes;
			}
		}

		public List<SmartView2.Models.Settings.SettingsModel.FontStyles> FontStyles
		{
			get
			{
				return this.fontStyles;
			}
		}

		public ICommand OkCommand
		{
			get
			{
				return this.okCommand;
			}
		}

		public List<SmartView2.Models.Settings.SettingsModel.Opacity> Opacities
		{
			get
			{
				return this.opacities;
			}
		}

		public ICommand PreviewCommand
		{
			get
			{
				return this.previewCommand;
			}
		}

		public ICommand ResetAllCommand
		{
			get
			{
				return this.resetAllCommand;
			}
		}

		public SmartView2.Models.Settings.SettingsModel SettingsModel
		{
			get
			{
				return this.settingsModel;
			}
			set
			{
				base.SetProperty<SmartView2.Models.Settings.SettingsModel>(ref this.settingsModel, value, "SettingsModel");
			}
		}

		public List<float> Sizes
		{
			get
			{
				return this.sizes;
			}
		}

		public CCSettingsViewModel()
		{
			this.previewCommand = new UICommand(new Action<object>(this.OnPreview), null);
			this.resetAllCommand = new UICommand(new Action<object>(this.OnResetAll), null);
			this.okCommand = new UICommand(new Action<object>(this.OnOkCommand), null);
			List<Color> colors = new List<Color>()
			{
				(Color)ColorConverter.ConvertFromString("#FFAEAEAE"),
				(Color)ColorConverter.ConvertFromString("#FFFFFFFF"),
				(Color)ColorConverter.ConvertFromString("#FF1D1D1D"),
				(Color)ColorConverter.ConvertFromString("#FFE1433E"),
				(Color)ColorConverter.ConvertFromString("#FF39AC28"),
				(Color)ColorConverter.ConvertFromString("#FF238AC2"),
				(Color)ColorConverter.ConvertFromString("#FFF0BC00"),
				(Color)ColorConverter.ConvertFromString("#FFE97724"),
				(Color)ColorConverter.ConvertFromString("#FFFF69F3"),
				(Color)ColorConverter.ConvertFromString("#FF904FC1"),
				(Color)ColorConverter.ConvertFromString("#FF28CDBC"),
				(Color)ColorConverter.ConvertFromString("#FF008A65"),
				(Color)ColorConverter.ConvertFromString("#FFA3620A"),
				(Color)ColorConverter.ConvertFromString("#FF662D91"),
				(Color)ColorConverter.ConvertFromString("#FFFCC2A2"),
				(Color)ColorConverter.ConvertFromString("#FF790000"),
				(Color)ColorConverter.ConvertFromString("#FF603913"),
				(Color)ColorConverter.ConvertFromString("#FF0054A6"),
				(Color)ColorConverter.ConvertFromString("#FF6DCFF6"),
				(Color)ColorConverter.ConvertFromString("#FF8781BD")
			};
			this.colors = colors;
			this.opacities = new List<SmartView2.Models.Settings.SettingsModel.Opacity>()
			{
				SmartView2.Models.Settings.SettingsModel.Opacity.Default,
				SmartView2.Models.Settings.SettingsModel.Opacity.Translucent,
				SmartView2.Models.Settings.SettingsModel.Opacity.Opacity
			};
			this.allOpacities = new List<SmartView2.Models.Settings.SettingsModel.Opacity>(this.GetListFromEnum<SmartView2.Models.Settings.SettingsModel.Opacity>());
			List<float> singles = new List<float>()
			{
				0f,
				0.5f,
				1f,
				1.5f,
				2f
			};
			this.sizes = singles;
			this.fontStyles = new List<SmartView2.Models.Settings.SettingsModel.FontStyles>(this.GetListFromEnum<SmartView2.Models.Settings.SettingsModel.FontStyles>());
			this.edgeTypes = new List<SmartView2.Models.Settings.SettingsModel.EdgeTypes>(this.GetListFromEnum<SmartView2.Models.Settings.SettingsModel.EdgeTypes>());
		}

		private void ChangeOpacity(SmartView2.Models.Settings.SettingsModel.Opacity opacity, ref Color color)
		{
			switch (opacity)
			{
				case SmartView2.Models.Settings.SettingsModel.Opacity.Default:
				case SmartView2.Models.Settings.SettingsModel.Opacity.Translucent:
				{
					return;
				}
				case SmartView2.Models.Settings.SettingsModel.Opacity.Transparency:
				{
					color.A = 0;
					return;
				}
				case SmartView2.Models.Settings.SettingsModel.Opacity.Opacity:
				{
					color.A = 127;
					return;
				}
				default:
				{
					return;
				}
			}
		}

		private Style CreateStyle()
		{
			Style style = new Style(typeof(TextBlock), (Style)Application.Current.Resources["DefaultClosedCaptionTextBlockStyle"]);
			SolidColorBrush solidColorBrush = new SolidColorBrush();
			Color characterColor = this.settingsModel.CharacterColor;
			this.ChangeOpacity(this.settingsModel.CharacterOpacity, ref characterColor);
			solidColorBrush.Color = characterColor;
			style.Setters.Add(new Setter(TextBlock.ForegroundProperty, solidColorBrush));
			float characterSize = this.settingsModel.CharacterSize;
			if (characterSize == 0f || characterSize == 1f)
			{
				style.Setters.Add(new Setter(TextBlock.FontSizeProperty, (object)16));
			}
			if (characterSize == 0.5f)
			{
				style.Setters.Add(new Setter(TextBlock.FontSizeProperty, (object)9));
			}
			if (characterSize == 1.5f)
			{
				style.Setters.Add(new Setter(TextBlock.FontSizeProperty, (object)24));
			}
			if (characterSize == 2f)
			{
				style.Setters.Add(new Setter(TextBlock.FontSizeProperty, (object)32));
			}
			style.Setters.Add(new Setter(TextBlock.FontFamilyProperty, new FontFamily("Arial")));
			characterColor = this.settingsModel.CaptionBackgroundColor;
			this.ChangeOpacity(this.settingsModel.CaptionBackgroundOpacity, ref characterColor);
			SolidColorBrush solidColorBrush1 = new SolidColorBrush()
			{
				Color = characterColor
			};
			style.Setters.Add(new Setter(TextBlock.BackgroundProperty, solidColorBrush1));
			return style;
		}

		private IEnumerable<T> GetListFromEnum<T>()
		where T : struct, IConvertible
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}
			return Enum.GetValues(typeof(T)).Cast<T>();
		}

		public override void OnNavigateFrom()
		{
			base.OnNavigateFrom();
		}

		public override void OnNavigateTo(object p)
		{
			this.SettingsModel = SmartView2.Models.Settings.SettingsModel.Load();
			this.resetAllPopup = base.Controller.CreatePopup(new OkCancelPopupViewModel(ResourcesModel.Instanse.COM_IDS_TXT_RESET_ALL, ResourcesModel.Instanse.MAPP_SID_RESET_ALL_CLOSED_CAPTION_OPTIONS_DEFAULT), false);
			this.resetAllInfoPopup = base.Controller.CreatePopup(new MessagePopupViewModel(ResourcesModel.Instanse.MAPP_SID_CLOSED_CAPTION_OPTIONS_RESET, ResourcesModel.Instanse.COM_BDP_SID_SMARTHUB_RESET_COMPLETE_TEXT), false);
		}

		private void OnOkCommand(object obj)
		{
			this.SettingsModel.Save(this.settingsModel);
			if (this.settingsModel.ClosedCaption)
			{
				Application.Current.Resources["ClosedCaptionTextBlockStyle"] = this.CreateStyle();
			}
			else
			{
				Application.Current.Resources["ClosedCaptionTextBlockStyle"] = Application.Current.Resources["DefaultClosedCaptionTextBlockStyle"];
			}
			base.CloseCommand.Execute(null);
		}

		private void OnPreview(object obj)
		{
			PopupWrapper popupWrapper = base.Controller.CreatePopup(new PreviewPopupViewModel(ResourcesModel.Instanse.COM_STR_KEYPAD_FLS_CAPITAL_2, ResourcesModel.Instanse.COM_SID_PREVIEW, this.CreateStyle()), false);
			popupWrapper.Show();
		}

		private async void OnResetAll(object obj)
		{
			AlternativePopupEventArgs alternativePopupEventArg = await this.resetAllPopup.ShowDialogAsync() as AlternativePopupEventArgs;
			if (alternativePopupEventArg != null)
			{
				bool? decision = alternativePopupEventArg.Decision;
				if ((!decision.GetValueOrDefault() ? false : decision.HasValue))
				{
					this.resetAllInfoPopup.Show();
					SmartView2.Models.Settings.SettingsModel defaults = SmartView2.Models.Settings.SettingsModel.Defaults;
					defaults.ClosedCaption = this.settingsModel.ClosedCaption;
					this.SettingsModel = defaults;
				}
			}
		}
	}
}