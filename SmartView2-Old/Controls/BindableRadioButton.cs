using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SmartView2.Controls
{
	public class BindableRadioButton : RadioButton
	{
		public readonly static DependencyProperty RadioValueProperty;

		public readonly static DependencyProperty RadioBindingProperty;

		public object RadioBinding
		{
			get
			{
				return base.GetValue(BindableRadioButton.RadioBindingProperty);
			}
			set
			{
				base.SetValue(BindableRadioButton.RadioBindingProperty, value);
			}
		}

		public object RadioValue
		{
			get
			{
				return base.GetValue(BindableRadioButton.RadioValueProperty);
			}
			set
			{
				base.SetValue(BindableRadioButton.RadioValueProperty, value);
			}
		}

		static BindableRadioButton()
		{
			BindableRadioButton.RadioValueProperty = DependencyProperty.Register("RadioValue", typeof(object), typeof(BindableRadioButton), new UIPropertyMetadata(null));
			BindableRadioButton.RadioBindingProperty = DependencyProperty.Register("RadioBinding", typeof(object), typeof(BindableRadioButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(BindableRadioButton.OnRadioBindingChanged)));
		}

		public BindableRadioButton()
		{
		}

		protected override void OnChecked(RoutedEventArgs e)
		{
			base.OnChecked(e);
			base.SetCurrentValue(BindableRadioButton.RadioBindingProperty, this.RadioValue);
		}

		private static void OnRadioBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			BindableRadioButton bindableRadioButton = (BindableRadioButton)d;
			if (bindableRadioButton.RadioValue.Equals(e.NewValue))
			{
				bindableRadioButton.SetCurrentValue(ToggleButton.IsCheckedProperty, (object)true);
			}
			if (e.NewValue == null)
			{
				bindableRadioButton.SetCurrentValue(ToggleButton.IsCheckedProperty, (object)false);
			}
		}
	}
}