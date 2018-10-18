// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Aurigma.PhotoKiosk
{
    public class RectangleAdorner : Adorner
    {
        #region "[static] Move commands & attached properties"

        public static readonly DependencyProperty IsRectangleControlProperty =
            DependencyProperty.RegisterAttached(
                "IsRectangleControl",
                typeof(bool),
                typeof(RectangleAdorner),
                new PropertyMetadata(false, new PropertyChangedCallback(IsRectangleControlChangedHandler)));

        public static readonly DependencyProperty GripIndexProperty =
            DependencyProperty.RegisterAttached(
                "GripIndex",
                typeof(int),
                typeof(RectangleAdorner),
                new PropertyMetadata(-1, new PropertyChangedCallback(GripIndexChangedHandler)));

        private static RoutedCommand _moveLeftCommand;
        private static RoutedCommand _moveUpCommand;
        private static RoutedCommand _moveRightCommand;
        private static RoutedCommand _moveDownCommand;
        private static RoutedCommand _invertProportionCommand;

        public static RoutedCommand MoveLeft { get { return DeclareCommand(ref _moveLeftCommand, "MoveLeft"); } }
        public static RoutedCommand MoveUp { get { return DeclareCommand(ref _moveUpCommand, "MoveUp"); } }
        public static RoutedCommand MoveRight { get { return DeclareCommand(ref _moveRightCommand, "MoveRight"); } }
        public static RoutedCommand MoveDown { get { return DeclareCommand(ref _moveDownCommand, "MoveDown"); } }
        public static RoutedCommand InvertProportion { get { return DeclareCommand(ref _invertProportionCommand, "InvertProportion"); } }

        private static RoutedCommand DeclareCommand(ref RoutedCommand command, string commandName)
        {
            if (command == null)
            {
                command = new RoutedCommand(commandName, typeof(RectangleAdorner));
                CommandBinding commandBinding = new CommandBinding(command, RubberbandCommandHandler);
                CommandManager.RegisterClassCommandBinding(typeof(RectangleAdorner), commandBinding);
            }

            return command;
        }

        private static void RubberbandCommandHandler(object target, ExecutedRoutedEventArgs e)
        {
            ((RectangleAdorner)target).ExecuteCommand(e.Command);
        }

        public static bool GetIsRectangleControl(DependencyObject target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            return (bool)target.GetValue(IsRectangleControlProperty);
        }

        public static void SetIsRectangleControl(DependencyObject target, bool value)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            target.SetValue(IsRectangleControlProperty, value);
        }

        private static void IsRectangleControlChangedHandler(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement rectangleControl = target as FrameworkElement;
            if (rectangleControl != null && GetIsRectangleControl(rectangleControl))
            {
                ContentControl rubberbandControl = rectangleControl.TemplatedParent as ContentControl;
                if (rubberbandControl != null && rubberbandControl.Tag != null)
                {
                    RectangleAdorner rubberband = rubberbandControl.Tag as RectangleAdorner;
                    if (rubberband != null)
                        rubberband.SetRectangleControl(rectangleControl);
                }
            }
        }

        public static int GetGripIndex(DependencyObject target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            return (int)target.GetValue(GripIndexProperty);
        }

        public static void SetGripIndex(DependencyObject target, int value)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            target.SetValue(GripIndexProperty, value);
        }

        private static void GripIndexChangedHandler(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement gripElement = target as FrameworkElement;
            if (gripElement != null)
            {
                ContentControl rubberbandControl = gripElement.TemplatedParent as ContentControl;
                if (rubberbandControl != null && rubberbandControl.Tag != null)
                {
                    RectangleAdorner rubberband = rubberbandControl.Tag as RectangleAdorner;
                    if (rubberband != null)
                        rubberband.SetGripElement(gripElement, GetGripIndex(gripElement));
                }
            }
        }

        #endregion "[static] Move commands & attached properties"

        #region "[public] Construction / destruction"

        public RectangleAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            if (adornedElement == null)
                throw new ArgumentNullException("adornedElement");

            _rubberbandTransform = new TranslateTransform();
            _adornedImage = (Image)adornedElement;
            _adornedImage.SizeChanged += new SizeChangedEventHandler(AdornedImageSizeChangedHandler);

            _rubberbandControl = new ContentControl();
            _rubberbandControl.RenderTransform = _rubberbandTransform;
            _rubberbandControl.HorizontalAlignment = HorizontalAlignment.Left;
            _rubberbandControl.VerticalAlignment = VerticalAlignment.Top;
            _rubberbandControl.Width = _adornedImage.Width;
            _rubberbandControl.Height = _adornedImage.Height;
            _rubberbandControl.Style = (Style)this.FindResource("StyleForRubberband");
            _rubberbandControl.Tag = this;

            _containerVisual = new ContainerVisual();
            _containerVisual.Children.Add(_rubberbandControl);

            if (_adornedImage.Source.Height > _adornedImage.Source.Width)
                _orientation = Orientation.Vertical;
            else
                _orientation = Orientation.Horizontal;

            _isCentered = false;
            _rectangle = new Rect(0, 0, 400, 200);
            _widthToHeightProportion = 1.0f;
            _moveStep = 25;
        }

        #endregion "[public] Construction / destruction"

        #region "[public/protected] Properties"

        protected override void OnInitialized(EventArgs e)
        {
            AddVisualChild(_containerVisual);
            base.OnInitialized(e);
        }

        public double MoveStep
        {
            get { return _moveStep; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", "The value cannot be less than zero.");

                _moveStep = value;
            }
        }

        public bool IsProportional
        {
            get { return _isProportional; }
            set
            {
                if (value != _isProportional)
                {
                    _isProportional = value;
                    VerifyProportion(_widthToHeightProportion);
                    VerifyRectangleAndInvalidate();
                }
            }
        }

        public double WidthToHeightProportion
        {
            get { return _widthToHeightProportion; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "The value cannot be less than zero.");
                else if (value == 0)
                    _isProportional = false;
                else
                    _isProportional = true;

                if (value != _widthToHeightProportion)
                {
                    VerifyProportion(value);
                    _widthToHeightProportion = value;
                    VerifyRectangleAndInvalidate();
                }
            }
        }

        public bool IsCentered
        {
            get { return _isCentered; }
            set
            {
                if (value != _isCentered)
                {
                    _isCentered = value;
                }
            }
        }

        public Rect Rectangle
        {
            get { return _rectangle; }
            set
            {
                if (value.Width < MinRectangleWidth)
                    throw new ArgumentOutOfRangeException("value", "Rectangle width cannot be less than MinRectangleWidth value.");
                if (value.Height < MinRectangleHeight)
                    throw new ArgumentOutOfRangeException("value", "Rectangle height cannot be less than MinRectangleHeight value.");
                if (value.X < 0 || value.Y < 0 || value.Right > _adornedImage.Width || value.Bottom > _adornedImage.Height)
                    throw new ArgumentOutOfRangeException("value", "Rectangle should be inside image bounds.");

                _rectangle = value;
                VerifyRectangleAndInvalidate();
            }
        }

        protected override int VisualChildrenCount
        {
            get { return _containerVisual.Children.Count; }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            _rubberbandControl.Measure(new Size(double.MaxValue, double.MaxValue));
            return _rubberbandControl.DesiredSize;
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

        #endregion "[public/protected] Properties"

        #region "[protected] Overrided methods"

        protected override Visual GetVisualChild(int index)
        {
            return _containerVisual;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            UpdateRubberbandControl();
            base.OnRender(drawingContext);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            HandleCornerPointDragging(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (_draggedGripElement is Ellipse)
                _draggedGripElement.Style = (Style)FindResource("RubberbandEllipseStyle");
            _draggedGripElement = null;
            ReleaseMouseCapture();
        }

        #endregion "[protected] Overrided methods"

        #region "[private] Commands handling"

        private void ExecuteCommand(ICommand command)
        {
            if (command == _moveLeftCommand)
                Move(-_moveStep, 0);
            else if (command == _moveUpCommand)
                Move(0, -_moveStep);
            else if (command == _moveRightCommand)
                Move(_moveStep, 0);
            else if (command == _moveDownCommand)
                Move(0, _moveStep);
            else if (command == _invertProportionCommand)
                InvertAdornerProportion();
            else
                throw new ArgumentException("Unexpected command object.", "command");
        }

        private void Move(double offsetX, double offsetY)
        {
            _rectangle.Offset(offsetX, offsetY);
            VerifyRectangleAndInvalidate();
        }

        private void InvertAdornerProportion()
        {
            if (_widthToHeightProportion != 0)
                _widthToHeightProportion = 1.0 / _widthToHeightProportion;

            if (_isProportional)
            {
                double t = _rectangle.Width;
                _rectangle.Width = _rectangle.Height;
                _rectangle.Height = t;

                // Rotate around the center
                _rectangle.X += (_rectangle.Height - _rectangle.Width) / 2;
                _rectangle.Y += (_rectangle.Width - _rectangle.Height) / 2;
            }

            VerifyRectangleAndInvalidate();
        }

        #endregion "[private] Commands handling"

        #region "[private] Attached property handling"

        private void SetRectangleControl(FrameworkElement control)
        {
            _rectangleControl = control;

            _rectangleControl.PreviewMouseDown += new MouseButtonEventHandler(RectangleMouseDownHandler);
            _rectangleControl.PreviewMouseMove += new MouseEventHandler(RectanglePreviewMouseMoveHandler);
            _rectangleControl.PreviewMouseUp += new MouseButtonEventHandler(RectanglePreviewMouseUpHandler);
        }

        #endregion "[private] Attached property handling"

        #region "[private] Visual control maintaining functionality"

        private Rect GetRectangleControlRelativeBounds()
        {
            GeneralTransform transform = _rectangleControl.TransformToAncestor(_rubberbandControl);
            return transform.TransformBounds(new Rect(0, 0, _rubberbandControl.Width, _rubberbandControl.Height));
        }

        private void UpdateRubberbandControl()
        {
            if (_rectangleControl == null)
                return;

            _rectangleControl.Width = _rectangle.Width;
            _rectangleControl.Height = _rectangle.Height;

            Rect rectControlRelativeBounds = GetRectangleControlRelativeBounds();
            _rubberbandTransform.X = _rectangle.X - rectControlRelativeBounds.X;
            _rubberbandTransform.Y = _rectangle.Y - rectControlRelativeBounds.Y;
        }

        private void Invalidate()
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(_adornedImage);
            layer.InvalidateArrange();
        }

        #endregion "[private] Visual control maintaining functionality"

        #region "[private] Adorner resize handling"

        #region "[private] GripElements event handlers"

        private void LeftTopGripMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            _draggedGripElement = _leftTopGripElement;
            _leftTopGripElement.Style = (Style)FindResource("RubberbandEllipseSelectedStyle");
            _scaleRefPoint = _rectangle.BottomRight;
            _scaleSignX = -1;
            _scaleSignY = -1;

            CaptureMouse();
        }

        private void RightTopGripMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            _draggedGripElement = _rightTopGripElement;
            _rightTopGripElement.Style = (Style)FindResource("RubberbandEllipseSelectedStyle");
            _scaleRefPoint = _rectangle.BottomLeft;
            _scaleSignX = 1;
            _scaleSignY = -1;

            CaptureMouse();
        }

        private void RightBottomGripMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            _draggedGripElement = _rightBottomGripElement;
            _rightBottomGripElement.Style = (Style)FindResource("RubberbandEllipseSelectedStyle");
            _scaleRefPoint = _rectangle.TopLeft;
            _scaleSignX = 1;
            _scaleSignY = 1;

            CaptureMouse();
        }

        private void LeftBottomGripMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            _draggedGripElement = _leftBottomGripElement;
            _leftBottomGripElement.Style = (Style)FindResource("RubberbandEllipseSelectedStyle");
            _scaleRefPoint = _rectangle.TopRight;
            _scaleSignX = -1;
            _scaleSignY = 1;

            CaptureMouse();
        }

        #endregion "[private] GripElements event handlers"

        private void AdornedImageSizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            VerifyRectangleAndInvalidate();
        }

        private void SetGripElement(FrameworkElement element, int gripIndex)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            switch (gripIndex)
            {
                case 0:
                    _leftTopGripElement = element;
                    _leftTopGripElement.Style = (Style)FindResource("RubberbandEllipseStyle");
                    _leftTopGripElement.MouseLeftButtonDown += new MouseButtonEventHandler(LeftTopGripMouseDownHandler);
                    break;

                case 1:
                    _rightTopGripElement = element;
                    _rightTopGripElement.Style = (Style)FindResource("RubberbandEllipseStyle");
                    _rightTopGripElement.MouseLeftButtonDown += new MouseButtonEventHandler(RightTopGripMouseDownHandler);
                    break;

                case 2:
                    _rightBottomGripElement = element;
                    _rightBottomGripElement.Style = (Style)FindResource("RubberbandEllipseStyle");
                    _rightBottomGripElement.MouseLeftButtonDown += new MouseButtonEventHandler(RightBottomGripMouseDownHandler);
                    break;

                case 3:
                    _leftBottomGripElement = element;
                    _leftBottomGripElement.Style = (Style)FindResource("RubberbandEllipseStyle");
                    _leftBottomGripElement.MouseLeftButtonDown += new MouseButtonEventHandler(LeftBottomGripMouseDownHandler);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("gripIndex", "Grip index value should be in range from 0 to 3.");
            }
        }

        private void HandleCornerPointDragging(MouseEventArgs e)
        {
            if (_draggedGripElement == null)
                return;

            Point newPosition = e.GetPosition(_adornedImage);
            Point shift = GetGapFromGripToRectangle();
            newPosition.X += shift.X;
            newPosition.Y += shift.Y;

            if (newPosition.X < 0)
                newPosition.X = 0;
            if (newPosition.Y < 0)
                newPosition.Y = 0;
            if (newPosition.X >= _adornedImage.ActualWidth)
                newPosition.X = _adornedImage.ActualWidth - 1;
            if (newPosition.Y >= _adornedImage.ActualHeight)
                newPosition.Y = _adornedImage.ActualHeight - 1;

            double newWidth, newHeight;
            newWidth = (newPosition.X - _scaleRefPoint.X) * _scaleSignX;
            newHeight = (newPosition.Y - _scaleRefPoint.Y) * _scaleSignY;

            if (newWidth < MinRectangleWidth)
                newPosition.X = _scaleRefPoint.X + _scaleSignX;
            if (newHeight < MinRectangleHeight)
                newPosition.Y = _scaleRefPoint.Y + _scaleSignY;

            _rectangle = new Rect(newPosition, _scaleRefPoint);
            VerifyRectangleAndInvalidate();
        }

        private Point GetCurrentValueOfDraggedPoint()
        {
            if (_draggedGripElement == _leftTopGripElement)
                return _rectangle.TopLeft;
            if (_draggedGripElement == _rightTopGripElement)
                return _rectangle.TopRight;
            if (_draggedGripElement == _rightBottomGripElement)
                return _rectangle.BottomRight;
            if (_draggedGripElement == _leftBottomGripElement)
                return _rectangle.BottomLeft;

            throw new InvalidOperationException();
        }

        private Point GetGapFromGripToRectangle()
        {
            Point gripCenterPoint = _draggedGripElement.TranslatePoint(new Point(_draggedGripElement.Width / 2, _draggedGripElement.Height / 2), _adornedImage);
            Point rectanglePoint = GetCurrentValueOfDraggedPoint();
            return new Point(rectanglePoint.X - gripCenterPoint.X, rectanglePoint.Y - gripCenterPoint.Y);
        }

        private void VerifyRectangleConstraints()
        {
            if (!_adornedImage.IsArrangeValid)
                return;

            var bounds = new Size(_adornedImage.ActualWidth, _adornedImage.ActualHeight);
            if (_draggedGripElement != null)
            {
                bounds.Width = Math.Min(_adornedImage.ActualWidth, (_scaleSignX < 0 ? _scaleRefPoint.X : _adornedImage.ActualWidth - _scaleRefPoint.X));
                bounds.Height = Math.Min(_adornedImage.ActualHeight, (_scaleSignY < 0 ? _scaleRefPoint.Y : _adornedImage.ActualHeight - _scaleRefPoint.Y));
            }

            FitRectangleSizeIntoBounds(bounds);
            if (_draggedGripElement != null)
            {
                var newCorner = new Point(_scaleRefPoint.X + _scaleSignX * _rectangle.Width, _scaleRefPoint.Y + _scaleSignY * _rectangle.Height);
                _rectangle = new Rect(newCorner, _scaleRefPoint);
            }

            MoveRectanglePositionIntoImageBounds();
        }

        private void FitRectangleSizeIntoBounds(Size bounds)
        {
            double newWidth = _rectangle.Width;
            double newHeight = _rectangle.Height;

            // Check for minimal allowed size
            newWidth = Math.Max(newWidth, MinRectangleWidth);
            newHeight = Math.Max(newHeight, MinRectangleHeight);
            bounds.Width = Math.Max(bounds.Width, MinRectangleWidth);
            bounds.Height = Math.Max(bounds.Height, MinRectangleHeight);

            if (_isProportional)
            {
                double proportion = _widthToHeightProportion;
                if (_orientation == Orientation.Vertical)
                    proportion = 1 / _widthToHeightProportion;

                newHeight = Math.Round(newWidth / proportion);
                if (newHeight > bounds.Height)
                {
                    newHeight = bounds.Height;
                    newWidth = Math.Round(newHeight * proportion);
                }
                if (newWidth > bounds.Width)
                {
                    newWidth = bounds.Width;
                    newHeight = Math.Round(newWidth / proportion);
                }
            }

            if (_isCentered)
            {
                if (newWidth < bounds.Width)
                    _rectangle.X = (bounds.Width - newWidth) / 2;

                if (newHeight < bounds.Height)
                    _rectangle.Y = (bounds.Height - newHeight) / 2;
            }

            _rectangle.Width = newWidth;
            _rectangle.Height = newHeight;
        }

        private void MoveRectanglePositionIntoImageBounds()
        {
            var imageBounds = new Size(_adornedImage.ActualWidth, _adornedImage.ActualHeight);

            if (_rectangle.Width > imageBounds.Width || _rectangle.Height > imageBounds.Height)
                FitRectangleSizeIntoBounds(imageBounds);

            if (_rectangle.X < 0)
                _rectangle.X = 0;
            if (_rectangle.Y < 0)
                _rectangle.Y = 0;
            if (_rectangle.Right > imageBounds.Width)
            {
                _rectangle.X -= _rectangle.Right - imageBounds.Width;
                if (_rectangle.X < 0)
                {
                    _rectangle.Width -= _rectangle.X;
                    _rectangle.X = 0;
                }
            }
            if (_rectangle.Bottom > imageBounds.Height)
            {
                _rectangle.Y -= _rectangle.Bottom - imageBounds.Height;
                if (_rectangle.Y < 0)
                {
                    _rectangle.Height -= _rectangle.Y;
                    _rectangle.Y = 0;
                }
            }
        }

        private void VerifyRectangleAndInvalidate()
        {
            VerifyRectangleConstraints();
            UpdateRubberbandControl();
            Invalidate();
        }

        private void VerifyProportion(double value)
        {
            if (_adornedImage.IsArrangeValid && MinRectangleWidth / value > _adornedImage.ActualHeight && MinRectangleHeight * value > _adornedImage.ActualWidth)
                throw new ArgumentException("Specified proportion is not acceptable.", "value");
        }

        #endregion "[private] Adorner resize handling"

        #region "[private] Adorner drag'n'drop handling"

        private void RectanglePreviewMouseUpHandler(object sender, MouseButtonEventArgs e)
        {
            _rectangleDragging = false;
        }

        private void RectanglePreviewMouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (_rectangleDragging)
            {
                Point mousePosition = e.GetPosition(_adornedImage);
                _rectangle.X = mousePosition.X - _rectangleDragOrigin.X;
                _rectangle.Y = mousePosition.Y - _rectangleDragOrigin.Y;

                VerifyRectangleAndInvalidate();
            }
        }

        private void RectangleMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            _rectangleDragging = true;
            Point mousePosition = e.GetPosition(_adornedImage);
            _rectangleDragOrigin = new Point(mousePosition.X - _rectangle.X, mousePosition.Y - _rectangle.Y);
        }

        #endregion "[private] Adorner drag'n'drop handling"

        #region "[private] Member variables"

        private Image _adornedImage;
        private Rect _rectangle;
        private TranslateTransform _rubberbandTransform;

        private ContentControl _rubberbandControl;
        private FrameworkElement _rectangleControl;
        private ContainerVisual _containerVisual;

        private Point _scaleRefPoint;
        private int _scaleSignX;
        private int _scaleSignY;
        private FrameworkElement _draggedGripElement;
        private FrameworkElement _leftTopGripElement;
        private FrameworkElement _rightTopGripElement;
        private FrameworkElement _rightBottomGripElement;
        private FrameworkElement _leftBottomGripElement;

        private const int MinRectangleWidth = 1;
        private const int MinRectangleHeight = 1;

        private bool _isProportional;
        private double _widthToHeightProportion;
        private bool _isCentered;
        private Orientation _orientation;

        private double _moveStep;

        private bool _rectangleDragging;
        private Point _rectangleDragOrigin;

        #endregion "[private] Member variables"
    }
}