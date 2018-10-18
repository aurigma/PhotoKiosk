// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Aurigma.PhotoKiosk
{
    internal class PointAdorner : Adorner
    {
        #region [static] Dependency properties

        public static readonly DependencyProperty IsEllipseControlProperty =
            DependencyProperty.RegisterAttached(
                "IsEllipseControl",
                typeof(bool),
                typeof(PointAdorner),
                new PropertyMetadata(false, new PropertyChangedCallback(IsEllipseControlChangedHandler)));

        public static readonly DependencyProperty IsArrowControlProperty =
            DependencyProperty.RegisterAttached(
                "IsArrowControl",
                typeof(bool),
                typeof(PointAdorner),
                new PropertyMetadata(false, new PropertyChangedCallback(IsArrowControlChangedHandler)));

        private static void IsEllipseControlChangedHandler(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement ellipseControl = target as FrameworkElement;
            if (ellipseControl != null && GetIsEllipseControl(ellipseControl))
            {
                ContentControl pointControl = ellipseControl.TemplatedParent as ContentControl;
                if (pointControl != null && pointControl.Tag != null)
                {
                    PointAdorner point = pointControl.Tag as PointAdorner;
                    if (point != null)
                        point.SetEllipseControl(ellipseControl);
                }
            }
        }

        private static void IsArrowControlChangedHandler(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement arrowControl = target as FrameworkElement;
            if (arrowControl != null && GetIsArrowControl(arrowControl))
            {
                ContentControl pointControl = arrowControl.TemplatedParent as ContentControl;
                if (pointControl != null && pointControl.Tag != null)
                {
                    PointAdorner point = pointControl.Tag as PointAdorner;
                    if (point != null)
                        point.SetArrowControl(arrowControl);
                }
            }
        }

        public static bool GetIsEllipseControl(DependencyObject target)
        {
            if (target == null)
                throw new System.ArgumentNullException("target");

            return (bool)target.GetValue(IsEllipseControlProperty);
        }

        public static void SetIsEllipseControl(DependencyObject target, bool value)
        {
            if (target == null)
                throw new System.ArgumentNullException("target");

            target.SetValue(IsEllipseControlProperty, value);
        }

        public static bool GetIsArrowControl(DependencyObject target)
        {
            if (target == null)
                throw new System.ArgumentNullException("target");

            return (bool)target.GetValue(IsArrowControlProperty);
        }

        public static void SetIsArrowControl(DependencyObject target, bool value)
        {
            if (target == null)
                throw new System.ArgumentNullException("target");

            target.SetValue(IsArrowControlProperty, value);
        }

        #endregion [static] Dependency properties

        #region [Public] Constructor

        public PointAdorner(UIElement adornedElement) : base(adornedElement)
        {
            if (adornedElement == null)
                throw new ArgumentNullException("adornedElement");

            _adornedImage = (Image)adornedElement;
            _containerVisual = new ContainerVisual();
            _controlTransform = new TranslateTransform();

            _pointControl = new ContentControl();
            _pointControl.RenderTransform = _controlTransform;
            _pointControl.HorizontalAlignment = HorizontalAlignment.Left;
            _pointControl.VerticalAlignment = VerticalAlignment.Top;
            _pointControl.Width = _adornedImage.Width;
            _pointControl.Height = _adornedImage.Height;
            _pointControl.Style = (Style)this.FindResource("StyleForPoint");
            _pointControl.Tag = this;

            _containerVisual.Children.Add(_pointControl);

            _point = new Point(400, 200);
            _isDragged = false;
        }

        #endregion [Public] Constructor

        #region [private] Methods

        private void SetEllipseControl(FrameworkElement control)
        {
            _ellipseControl = control;
            _ellipseControl.Style = (Style)FindResource("RubberbandEllipseStyle");

            _ellipseControl.PreviewMouseDown += new MouseButtonEventHandler(EllipseMouseDownEventHandler);
            _ellipseControl.PreviewMouseMove += new MouseEventHandler(EllipseMouseMoveEventHandler);
            _ellipseControl.PreviewMouseUp += new MouseButtonEventHandler(EllipseMouseUpEventHandler);
        }

        private void SetArrowControl(FrameworkElement control)
        {
            _arrowControl = control;
            _arrowControl.Style = (Style)FindResource("PointArrowStyle");
        }

        private void UpdateControl()
        {
            _controlTransform.X = _point.X;
            _controlTransform.Y = _point.Y;
        }

        private void Invalidate()
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(_adornedImage);
            layer.InvalidateArrange();
        }

        #endregion [private] Methods

        #region [private] Event handlers

        private void EllipseMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            _isDragged = true;
            Point mousePosition = e.GetPosition(_adornedImage);
            _pointDragOrigin = new Point(mousePosition.X - _point.X, mousePosition.Y - _point.Y);
            _ellipseControl.Style = (Style)FindResource("RubberbandEllipseSelectedStyle");
            _arrowControl.Style = (Style)FindResource("PointArrowSelectedStyle");
        }

        private void EllipseMouseMoveEventHandler(object sender, MouseEventArgs e)
        {
            if (_isDragged)
            {
                _point.X = e.GetPosition(_adornedImage).X - _pointDragOrigin.X;
                _point.Y = e.GetPosition(_adornedImage).Y - _pointDragOrigin.Y;

                if (_point.X < 0)
                    _point.X = 0;
                if (_point.Y < 0)
                    _point.Y = 0;
                if (_point.X >= _adornedImage.ActualWidth)
                    _point.X = _adornedImage.ActualWidth - 1;
                if (_point.Y >= _adornedImage.ActualHeight)
                    _point.Y = _adornedImage.ActualHeight - 1;

                UpdateControl();
                Invalidate();
            }
        }

        private void EllipseMouseUpEventHandler(object sender, MouseButtonEventArgs e)
        {
            _isDragged = false;
            _ellipseControl.Style = (Style)FindResource("RubberbandEllipseStyle");
            _arrowControl.Style = (Style)FindResource("PointArrowStyle");
        }

        #endregion [private] Event handlers

        #region [public] propertes

        public Point Point
        {
            get
            {
                return _point;
            }
            set
            {
                _point = value;
                if (_point.X < 0)
                    _point.X = 0;
                if (_point.Y < 0)
                    _point.Y = 0;
                if (_point.X >= _adornedImage.ActualWidth)
                    _point.X = _adornedImage.ActualWidth - 1;
                if (_point.Y >= _adornedImage.ActualHeight)
                    _point.Y = _adornedImage.ActualHeight - 1;
                UpdateControl();
                Invalidate();
            }
        }

        #endregion [public] propertes

        #region [protected] Overrided methods

        protected override void OnInitialized(EventArgs e)
        {
            AddVisualChild(_containerVisual);
            base.OnInitialized(e);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            UpdateControl();
            base.OnRender(drawingContext);
        }

        protected override Visual GetVisualChild(int index)
        {
            return _containerVisual;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Size result = base.ArrangeOverride(finalSize);

            ContainerVisual _container = GetVisualChild(0) as ContainerVisual;
            foreach (Visual child in _container.Children)
            {
                ((UIElement)child).Arrange(new Rect(new Point(), result));
            }

            return result;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            _pointControl.Measure(new Size(double.MaxValue, double.MaxValue));
            return _pointControl.DesiredSize;
        }

        #endregion [protected] Overrided methods

        #region [protected] Properties

        protected override int VisualChildrenCount
        {
            get
            {
                return _containerVisual.Children.Count;
            }
        }

        #endregion [protected] Properties

        #region [Private] Variables

        private Image _adornedImage;
        private ContainerVisual _containerVisual;
        private TranslateTransform _controlTransform;
        private ContentControl _pointControl;
        private FrameworkElement _ellipseControl;
        private FrameworkElement _arrowControl;

        private Point _point;
        private Point _pointDragOrigin;

        private bool _isDragged;

        #endregion [Private] Variables
    }
}