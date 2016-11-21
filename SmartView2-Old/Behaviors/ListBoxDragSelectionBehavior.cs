using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using UIFoundation.Interactivity;

namespace SmartView2.Behaviors
{
	public class ListBoxDragSelectionBehavior : Behavior<ListBox>
	{
		private readonly static Dictionary<ListBox, ListBoxDragSelectionBehavior> attachedControls;

		private ListBox listBox;

		private ScrollContentPresenter scrollContent;

		private ListBoxDragSelectionBehavior.SelectionAdorner selectionRect;

		private ListBoxDragSelectionBehavior.AutoScroller autoScroller;

		private ListBoxDragSelectionBehavior.ItemsControlSelector selector;

		private bool mouseCaptured;

		private Point start;

		private Point end;

		static ListBoxDragSelectionBehavior()
		{
			ListBoxDragSelectionBehavior.attachedControls = new Dictionary<ListBox, ListBoxDragSelectionBehavior>();
		}

		public ListBoxDragSelectionBehavior()
		{
		}

		private static T FindChild<T>(DependencyObject reference)
		where T : class
		{
			Queue<DependencyObject> dependencyObjects = new Queue<DependencyObject>();
			dependencyObjects.Enqueue(reference);
			while (dependencyObjects.Count > 0)
			{
				DependencyObject dependencyObject = dependencyObjects.Dequeue();
				T t = (T)(dependencyObject as T);
				if (t != null)
				{
					return t;
				}
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
				{
					dependencyObjects.Enqueue(VisualTreeHelper.GetChild(dependencyObject, i));
				}
			}
			return default(T);
		}

		private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
		{
			if (this.listBox.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated && this.Register())
			{
				this.listBox.Loaded -= new RoutedEventHandler(this.ItemContainerGenerator_StatusChanged);
			}
		}

		protected override void OnAttached()
		{
			ListBox associatedObject = base.AssociatedObject;
			this.listBox = associatedObject;
			if (!this.listBox.IsLoaded)
			{
				this.listBox.ItemContainerGenerator.StatusChanged += new EventHandler(this.ItemContainerGenerator_StatusChanged);
			}
			else
			{
				this.Register();
			}
			if (associatedObject.SelectionMode == SelectionMode.Single)
			{
				associatedObject.SelectionMode = SelectionMode.Extended;
			}
			ListBoxDragSelectionBehavior.attachedControls.Add(associatedObject, this);
		}

		protected override void OnDetaching()
		{
			ListBoxDragSelectionBehavior listBoxDragSelectionBehavior;
			ListBox associatedObject = base.AssociatedObject;
			if (ListBoxDragSelectionBehavior.attachedControls.TryGetValue(associatedObject, out listBoxDragSelectionBehavior))
			{
				ListBoxDragSelectionBehavior.attachedControls.Remove(associatedObject);
				listBoxDragSelectionBehavior.UnRegister();
			}
		}

		private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (this.mouseCaptured)
			{
				this.mouseCaptured = false;
				this.scrollContent.ReleaseMouseCapture();
				this.StopSelection();
			}
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			if (this.mouseCaptured)
			{
				this.end = e.GetPosition(this.scrollContent);
				this.autoScroller.Update(this.end);
				this.UpdateSelection();
			}
		}

		private void OnOffsetChanged(object sender, ListBoxDragSelectionBehavior.OffsetChangedEventArgs e)
		{
			this.selector.Scroll(e.HorizontalChange, e.VerticalChange);
			this.UpdateSelection();
		}

		private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Point position = e.GetPosition(this.scrollContent);
			if (position.X >= 0 && position.X < this.scrollContent.ActualWidth && position.Y >= 0 && position.Y < this.scrollContent.ActualHeight)
			{
				this.mouseCaptured = this.TryCaptureMouse(e);
				if (this.mouseCaptured)
				{
					this.StartSelection(position);
				}
			}
		}

		private bool Register()
		{
			this.scrollContent = ListBoxDragSelectionBehavior.FindChild<ScrollContentPresenter>(this.listBox);
			if (this.scrollContent != null)
			{
				this.autoScroller = new ListBoxDragSelectionBehavior.AutoScroller(this.listBox);
				this.autoScroller.OffsetChanged += new EventHandler<ListBoxDragSelectionBehavior.OffsetChangedEventArgs>(this.OnOffsetChanged);
				this.selectionRect = new ListBoxDragSelectionBehavior.SelectionAdorner(this.scrollContent);
				this.scrollContent.AdornerLayer.Add(this.selectionRect);
				this.selector = new ListBoxDragSelectionBehavior.ItemsControlSelector(this.listBox);
				this.listBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.OnPreviewMouseLeftButtonDown);
				this.listBox.MouseLeftButtonUp += new MouseButtonEventHandler(this.OnMouseLeftButtonUp);
				this.listBox.MouseMove += new MouseEventHandler(this.OnMouseMove);
			}
			return this.scrollContent != null;
		}

		private void StartSelection(Point location)
		{
			this.listBox.Focus();
			this.start = location;
			this.end = location;
			if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None)
			{
				this.listBox.SelectedItems.Clear();
			}
			this.selector.Reset();
			this.UpdateSelection();
			this.selectionRect.IsEnabled = true;
			this.autoScroller.IsEnabled = true;
		}

		private void StopSelection()
		{
			this.selectionRect.IsEnabled = false;
			this.autoScroller.IsEnabled = false;
		}

		private bool TryCaptureMouse(MouseButtonEventArgs e)
		{
			Point position = e.GetPosition(this.scrollContent);
			UIElement uIElement = this.scrollContent.InputHitTest(position) as UIElement;
			if (uIElement != null)
			{
				MouseButtonEventArgs mouseButtonEventArg = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left, e.StylusDevice)
				{
					RoutedEvent = Mouse.MouseDownEvent,
					Source = e.Source
				};
				uIElement.RaiseEvent(mouseButtonEventArg);
				if (Mouse.Captured != this.listBox)
				{
					return false;
				}
			}
			return this.scrollContent.CaptureMouse();
		}

		private void UnRegister()
		{
			this.StopSelection();
			this.listBox.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.OnPreviewMouseLeftButtonDown);
			this.listBox.MouseLeftButtonUp -= new MouseButtonEventHandler(this.OnMouseLeftButtonUp);
			this.listBox.MouseMove -= new MouseEventHandler(this.OnMouseMove);
			this.autoScroller.UnRegister();
		}

		private void UpdateSelection()
		{
			Point point = this.autoScroller.TranslatePoint(this.start);
			double num = Math.Min(point.X, this.end.X);
			double num1 = Math.Min(point.Y, this.end.Y);
			double num2 = Math.Abs(this.end.X - point.X);
			double num3 = Math.Abs(this.end.Y - point.Y);
			Rect rect = new Rect(num, num1, num2, num3);
			this.selectionRect.SelectionArea = rect;
			Point point1 = this.scrollContent.TranslatePoint(rect.TopLeft, this.listBox);
			Point point2 = this.scrollContent.TranslatePoint(rect.BottomRight, this.listBox);
			this.selector.UpdateSelection(new Rect(point1, point2));
		}

		private sealed class AutoScroller
		{
			private readonly DispatcherTimer autoScroll;

			private readonly ItemsControl itemsControl;

			private readonly ScrollViewer scrollViewer;

			private readonly ScrollContentPresenter scrollContent;

			private bool isEnabled;

			private Point offset;

			private Point mouse;

			public bool IsEnabled
			{
				get
				{
					return this.isEnabled;
				}
				set
				{
					if (this.isEnabled != value)
					{
						this.isEnabled = value;
						this.autoScroll.IsEnabled = false;
						this.offset = new Point();
					}
				}
			}

			public AutoScroller(ItemsControl itemsControl)
			{
				if (itemsControl == null)
				{
					throw new ArgumentNullException("itemsControl");
				}
				this.itemsControl = itemsControl;
				this.scrollViewer = ListBoxDragSelectionBehavior.FindChild<ScrollViewer>(itemsControl);
				this.scrollViewer.ScrollChanged += new ScrollChangedEventHandler(this.OnScrollChanged);
				this.scrollContent = ListBoxDragSelectionBehavior.FindChild<ScrollContentPresenter>(this.scrollViewer);
				this.autoScroll.Tick += new EventHandler((object param0, EventArgs param1) => this.PreformScroll());
				this.autoScroll.Interval = TimeSpan.FromMilliseconds((double)ListBoxDragSelectionBehavior.AutoScroller.GetRepeatRate());
			}

			private double CalculateOffset(int startIndex, int endIndex)
			{
				double actualHeight = 0;
				for (int i = startIndex; i != endIndex; i++)
				{
					FrameworkElement frameworkElement = this.itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
					if (frameworkElement != null)
					{
						actualHeight = actualHeight + frameworkElement.ActualHeight;
						actualHeight = actualHeight + (frameworkElement.Margin.Top + frameworkElement.Margin.Bottom);
					}
				}
				return actualHeight;
			}

			private static int GetRepeatRate()
			{
				return 400 - (int)((double)SystemParameters.KeyboardSpeed * 11.8387096774194);
			}

			private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
			{
				if (this.IsEnabled)
				{
					double horizontalChange = e.HorizontalChange;
					double verticalChange = e.VerticalChange;
					if (this.scrollViewer.CanContentScroll)
					{
						if (e.VerticalChange >= 0)
						{
							int verticalOffset = (int)(e.VerticalOffset - e.VerticalChange);
							verticalChange = this.CalculateOffset(verticalOffset, (int)e.VerticalOffset);
						}
						else
						{
							int num = (int)e.VerticalOffset;
							int verticalOffset1 = (int)(e.VerticalOffset - e.VerticalChange);
							verticalChange = -this.CalculateOffset(num, verticalOffset1);
						}
					}
					this.offset.X = this.offset.X + horizontalChange;
					this.offset.Y = this.offset.Y + verticalChange;
					EventHandler<ListBoxDragSelectionBehavior.OffsetChangedEventArgs> eventHandler = this.OffsetChanged;
					if (eventHandler != null)
					{
						eventHandler(this, new ListBoxDragSelectionBehavior.OffsetChangedEventArgs(horizontalChange, verticalChange));
					}
				}
			}

			private void PreformScroll()
			{
				bool flag = false;
				if (this.mouse.X > this.scrollContent.ActualWidth)
				{
					this.scrollViewer.LineRight();
					flag = true;
				}
				else if (this.mouse.X < 0)
				{
					this.scrollViewer.LineLeft();
					flag = true;
				}
				if (this.mouse.Y > this.scrollContent.ActualHeight)
				{
					this.scrollViewer.LineDown();
					flag = true;
				}
				else if (this.mouse.Y < 0)
				{
					this.scrollViewer.LineUp();
					flag = true;
				}
				this.autoScroll.IsEnabled = flag;
			}

			public Point TranslatePoint(Point point)
			{
				return new Point(point.X - this.offset.X, point.Y - this.offset.Y);
			}

			public void UnRegister()
			{
				this.scrollViewer.ScrollChanged -= new ScrollChangedEventHandler(this.OnScrollChanged);
			}

			public void Update(Point mouse)
			{
				this.mouse = mouse;
				if (!this.autoScroll.IsEnabled)
				{
					this.PreformScroll();
				}
			}

			public event EventHandler<ListBoxDragSelectionBehavior.OffsetChangedEventArgs> OffsetChanged;
		}

		private sealed class ItemsControlSelector
		{
			private readonly ItemsControl itemsControl;

			private Rect previousArea;

			public ItemsControlSelector(ItemsControl itemsControl)
			{
				if (itemsControl == null)
				{
					throw new ArgumentNullException("itemsControl");
				}
				this.itemsControl = itemsControl;
			}

			public void Reset()
			{
				this.previousArea = new Rect();
			}

			public void Scroll(double x, double y)
			{
				this.previousArea.Offset(-x, -y);
			}

			public void UpdateSelection(Rect area)
			{
				for (int i = 0; i < this.itemsControl.Items.Count; i++)
				{
					FrameworkElement frameworkElement = this.itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
					if (frameworkElement != null)
					{
						Point point = frameworkElement.TranslatePoint(new Point(0, 0), this.itemsControl);
						Rect rect = new Rect(point.X, point.Y, frameworkElement.ActualWidth, frameworkElement.ActualHeight);
						if (rect.IntersectsWith(area))
						{
							Selector.SetIsSelected(frameworkElement, true);
						}
						else if (rect.IntersectsWith(this.previousArea))
						{
							Selector.SetIsSelected(frameworkElement, false);
						}
					}
				}
				this.previousArea = area;
			}
		}

		private sealed class OffsetChangedEventArgs : EventArgs
		{
			private readonly double horizontal;

			private readonly double vertical;

			public double HorizontalChange
			{
				get
				{
					return this.horizontal;
				}
			}

			public double VerticalChange
			{
				get
				{
					return this.vertical;
				}
			}

			internal OffsetChangedEventArgs(double horizontal, double vertical)
			{
				this.horizontal = horizontal;
				this.vertical = vertical;
			}
		}

		private sealed class SelectionAdorner : Adorner
		{
			private Rect selectionRect;

			public Rect SelectionArea
			{
				get
				{
					return this.selectionRect;
				}
				set
				{
					this.selectionRect = value;
					base.InvalidateVisual();
				}
			}

			public SelectionAdorner(UIElement parent) : base(parent)
			{
				base.IsHitTestVisible = false;
				base.IsEnabledChanged += new DependencyPropertyChangedEventHandler((object param0, DependencyPropertyChangedEventArgs param1) => base.InvalidateVisual());
			}

			protected override void OnRender(DrawingContext drawingContext)
			{
				base.OnRender(drawingContext);
				if (base.IsEnabled)
				{
					double[] left = new double[] { this.SelectionArea.Left + 0.5, this.SelectionArea.Right + 0.5 };
					double[] numArray = left;
					double[] top = new double[] { this.SelectionArea.Top + 0.5, this.SelectionArea.Bottom + 0.5 };
					drawingContext.PushGuidelineSet(new GuidelineSet(numArray, top));
					Brush brush = SystemColors.HighlightBrush.Clone();
					brush.Opacity = 0.4;
					drawingContext.DrawRectangle(brush, new Pen(SystemColors.HighlightBrush, 1), this.SelectionArea);
				}
			}
		}
	}
}