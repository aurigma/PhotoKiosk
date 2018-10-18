// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Aurigma.PhotoKiosk
{
    public sealed class OutlinedText : FrameworkElement
    {
        private FormattedText _formattedText;
        private FontStretch _fontStretch;
        private bool _highlight;
        private Geometry _textGeometry;
        private Geometry _textHighLightGeometry;
        private bool _geometryObjectChanged;

        #region [DependencyProperties]

        public static readonly System.Windows.DependencyProperty TextContentProperty =
            System.Windows.DependencyProperty.Register(
                "TextContent",
                typeof(string),
                typeof(OutlinedText),
                new System.Windows.FrameworkPropertyMetadata("")
                );

        public static readonly System.Windows.DependencyProperty FontFamilyProperty =
            System.Windows.DependencyProperty.Register(
                "FontFamily",
                typeof(string),
                typeof(OutlinedText),
                new System.Windows.FrameworkPropertyMetadata("Calibry")
            );

        public static readonly System.Windows.DependencyProperty FontSizeProperty =
            System.Windows.DependencyProperty.Register(
                "FontSize",
                typeof(double),
                typeof(OutlinedText),
                new System.Windows.FrameworkPropertyMetadata(14.0)
            );

        public static readonly System.Windows.DependencyProperty FontWeightProperty =
            System.Windows.DependencyProperty.Register(
                "FontWeight",
                typeof(System.Windows.FontWeight),
                typeof(OutlinedText),
                new System.Windows.FrameworkPropertyMetadata(System.Windows.FontWeights.Normal)
            );

        public static readonly System.Windows.DependencyProperty FontStyleProperty =
            System.Windows.DependencyProperty.Register(
                "FontStyle",
                typeof(System.Windows.FontStyle),
                typeof(OutlinedText),
                new System.Windows.FrameworkPropertyMetadata(System.Windows.FontStyles.Normal)
            );

        public static readonly System.Windows.DependencyProperty FillProperty =
            System.Windows.DependencyProperty.Register(
                "Fill",
                typeof(System.Windows.Media.Brush),
                typeof(OutlinedText),
                new System.Windows.FrameworkPropertyMetadata(System.Windows.Media.Brushes.Transparent)
            );

        public static readonly System.Windows.DependencyProperty StrokeProperty =
            System.Windows.DependencyProperty.Register(
                "Stroke",
                typeof(System.Windows.Media.Brush),
                typeof(OutlinedText),
                new System.Windows.FrameworkPropertyMetadata(System.Windows.Media.Brushes.Black)
            );

        public static readonly System.Windows.DependencyProperty StrokeThicknessProperty =
            System.Windows.DependencyProperty.Register(
                "StrokeThickness",
                typeof(double),
                typeof(OutlinedText),
                new System.Windows.FrameworkPropertyMetadata(1.0)
            );

        public static readonly System.Windows.DependencyProperty OffsetProperty =
            System.Windows.DependencyProperty.Register(
                "Offset",
                typeof(double),
                typeof(OutlinedText),
                new System.Windows.FrameworkPropertyMetadata(0.0)
             );

        #endregion [DependencyProperties]

        public OutlinedText()
        {
            _geometryObjectChanged = true;
            _fontStretch = FontStretches.Normal;
        }

        public OutlinedText(string textContent, Style style)
        {
            _geometryObjectChanged = true;

            this.TextContent = textContent;
            this.Style = style;
        }

        public void CreateText()
        {
            if (this.TextContent == null)
                throw new NullReferenceException("TextContent value cannot be null.");

            if (_geometryObjectChanged)
            {
                _formattedText = new FormattedText(
                    this.TextContent,
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface(new FontFamily(this.FontFamily), this.FontStyle, this.FontWeight, _fontStretch),
                    this.FontSize,
                    Brushes.Black);

                _textGeometry = _formattedText.BuildGeometry(new Point(0, this.Offset));

                if (_highlight)
                    _textHighLightGeometry = _formattedText.BuildHighlightGeometry(new Point(0, 0));

                _geometryObjectChanged = false;
            }
        }

        public string TextContent
        {
            get
            {
                return (string)GetValue(TextContentProperty);
            }
            set
            {
                if (this.TextContent != value)
                {
                    SetValue(TextContentProperty, value);
                    _geometryObjectChanged = true;
                    this.InvalidateVisual();
                }
            }
        }

        public string FontFamily
        {
            get
            {
                return (string)GetValue(FontFamilyProperty);
            }
            set
            {
                if (this.FontFamily != value)
                {
                    SetValue(FontFamilyProperty, value);
                    _geometryObjectChanged = true;
                    this.InvalidateVisual();
                }
            }
        }

        public double FontSize
        {
            get
            {
                return (double)GetValue(FontSizeProperty);
            }
            set
            {
                if (this.FontSize != value)
                {
                    SetValue(FontSizeProperty, value);
                    _geometryObjectChanged = true;
                    this.InvalidateVisual();
                }
            }
        }

        public FontWeight FontWeight
        {
            get
            {
                return (FontWeight)GetValue(FontWeightProperty);
            }
            set
            {
                if (!this.FontWeight.Equals(value))
                {
                    SetValue(FontWeightProperty, value);
                    _geometryObjectChanged = true;
                }
            }
        }

        public FontStyle FontStyle
        {
            get
            {
                return (FontStyle)GetValue(FontStyleProperty);
            }
            set
            {
                if (!this.FontStyle.Equals(value))
                {
                    SetValue(FontStyleProperty, value);
                    _geometryObjectChanged = true;
                }
            }
        }

        public Brush Fill
        {
            get
            {
                return (Brush)GetValue(FillProperty);
            }
            set
            {
                if (!this.Fill.Equals(value))
                {
                    SetValue(FillProperty, value);
                    _geometryObjectChanged = true;
                }
            }
        }

        public Brush Stroke
        {
            get
            {
                return (Brush)GetValue(StrokeProperty);
            }
            set
            {
                if (!this.Stroke.Equals(value))
                {
                    SetValue(StrokeProperty, value);
                    _geometryObjectChanged = true;
                }
            }
        }

        public double StrokeThickness
        {
            get
            {
                return (double)GetValue(StrokeThicknessProperty);
            }
            set
            {
                if (this.StrokeThickness != value)
                {
                    SetValue(StrokeThicknessProperty, value);
                    _geometryObjectChanged = true;
                }
            }
        }

        public double Offset
        {
            get
            {
                return (double)GetValue(OffsetProperty);
            }
            set
            {
                if (this.Offset != value)
                {
                    SetValue(OffsetProperty, value);
                    _geometryObjectChanged = true;
                }
            }
        }

        public FontStretch FontStretch
        {
            get
            {
                return _fontStretch;
            }
            set
            {
                if (!_fontStretch.Equals(value))
                {
                    _fontStretch = value;
                    _geometryObjectChanged = true;
                }
            }
        }

        public bool Highlight
        {
            get
            {
                return _highlight;
            }
            set
            {
                if (_highlight != value)
                {
                    _highlight = value;
                    _geometryObjectChanged = true;
                }
            }
        }

        public Geometry TextGeometry
        {
            get
            {
                CreateText();
                return _textGeometry;
            }
            set
            {
                _textGeometry = value;
                _geometryObjectChanged = false;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            CreateText();

            drawingContext.DrawGeometry(this.Fill, new Pen(this.Stroke, this.StrokeThickness), _textGeometry);
            if (_highlight)
                drawingContext.DrawGeometry(this.Fill, new Pen(this.Stroke, this.StrokeThickness), _textHighLightGeometry);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            CreateText();
            return new Size(_formattedText.Width, _formattedText.Height);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            CreateText();
            return new Size(_formattedText.Width, _formattedText.Height);
        }
    }
}