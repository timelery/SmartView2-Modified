using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SmartView2.Controls
{
	public class TemplatedAdorner : Adorner
	{
		private AdornerLayer layer;

		private ContentPresenter contentPresenter = new ContentPresenter();

		private Point position = new Point();

		protected override int VisualChildrenCount
		{
			get
			{
				return 1;
			}
		}

		public TemplatedAdorner(object content, DataTemplate contentTemplate, UIElement adornedElement, AdornerLayer layer) : base(adornedElement)
		{
			this.contentPresenter.Content = content;
			this.contentPresenter.ContentTemplate = contentTemplate;
			this.layer = layer;
			this.layer.Add(this);
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			this.contentPresenter.Arrange(new Rect(finalSize));
			return finalSize;
		}

		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			GeneralTransformGroup generalTransformGroup = new GeneralTransformGroup();
			generalTransformGroup.Children.Add(base.GetDesiredTransform(transform));
			generalTransformGroup.Children.Add(new TranslateTransform(this.position.X, this.position.Y));
			return generalTransformGroup;
		}

		protected override Visual GetVisualChild(int index)
		{
			return this.contentPresenter;
		}

		protected override Size MeasureOverride(Size constraint)
		{
			this.contentPresenter.Measure(constraint);
			return this.contentPresenter.DesiredSize;
		}

		public void Remove()
		{
			this.layer.Remove(this);
		}

		public void UpdatePosition(double x, double y)
		{
			this.position.X = x;
			this.position.Y = y;
			this.layer.Update(base.AdornedElement);
		}

		public void UpdatePosition(Point newPosition)
		{
			this.UpdatePosition(newPosition.X, newPosition.Y);
		}
	}
}