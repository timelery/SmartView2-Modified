using System;
using System.Windows;
using System.Windows.Controls;

namespace SmartView2.Controls
{
	public class ScrollPageBar : Control
	{
		private Panel templateRoot;

		public readonly static DependencyProperty ItemTemplateProperty;

		public readonly static DependencyProperty ViewportSizeProperty;

		public readonly static DependencyProperty ValueProperty;

		public readonly static DependencyProperty MaximumProperty;

		public DataTemplate ItemTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(ScrollPageBar.ItemTemplateProperty);
			}
			set
			{
				base.SetValue(ScrollPageBar.ItemTemplateProperty, value);
			}
		}

		public double Maximum
		{
			get
			{
				return (double)base.GetValue(ScrollPageBar.MaximumProperty);
			}
			set
			{
				base.SetValue(ScrollPageBar.MaximumProperty, value);
			}
		}

		public double Value
		{
			get
			{
				return (double)base.GetValue(ScrollPageBar.ValueProperty);
			}
			set
			{
				base.SetValue(ScrollPageBar.ValueProperty, value);
			}
		}

		public double ViewportSize
		{
			get
			{
				return (double)base.GetValue(ScrollPageBar.ViewportSizeProperty);
			}
			set
			{
				base.SetValue(ScrollPageBar.ViewportSizeProperty, value);
			}
		}

		static ScrollPageBar()
		{
			ScrollPageBar.ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ScrollPageBar), new PropertyMetadata(null));
			ScrollPageBar.ViewportSizeProperty = DependencyProperty.Register("ViewportSize", typeof(double), typeof(ScrollPageBar), new PropertyMetadata((double)0, new PropertyChangedCallback(ScrollPageBar.OnViewportSizeChanged)));
			ScrollPageBar.ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(ScrollPageBar), new PropertyMetadata((double)0, new PropertyChangedCallback(ScrollPageBar.OnValueChanged)));
			ScrollPageBar.MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(ScrollPageBar), new PropertyMetadata((double)0, new PropertyChangedCallback(ScrollPageBar.OnMaximumChanged)));
		}

		public ScrollPageBar()
		{
		}

		private void InvalidateChildren()
		{
			int num = (int)Math.Ceiling((this.Maximum + this.ViewportSize) / this.ViewportSize);
			int num1 = (int)Math.Ceiling(this.Value / this.ViewportSize);
			this.templateRoot.Children.Clear();
			for (int i = 0; i < num; i++)
			{
				UIElementCollection children = this.templateRoot.Children;
				ContentControl contentControl = new ContentControl()
				{
					ContentTemplate = this.ItemTemplate,
					Content = i == num1,
					Focusable = false
				};
				children.Add(contentControl);
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.templateRoot = (Panel)base.GetTemplateChild("PART_TemplateRoot");
		}

		private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			ScrollPageBar.UpdateScrollBar(d);
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			ScrollPageBar.UpdateScrollBar(d);
		}

		private static void OnViewportSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			ScrollPageBar.UpdateScrollBar(d);
		}

		private static void UpdateScrollBar(DependencyObject d)
		{
			ScrollPageBar scrollPageBar = d as ScrollPageBar;
			if (scrollPageBar != null)
			{
				scrollPageBar.InvalidateChildren();
			}
		}
	}
}