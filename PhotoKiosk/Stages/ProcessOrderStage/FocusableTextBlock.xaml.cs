// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Windows;
using System.Windows.Controls;

namespace Aurigma.PhotoKiosk
{
    public partial class FocusableTextBlock : UserControl
    {
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(FocusableTextBlock), new FrameworkPropertyMetadata("", CaptionChangedHandler));

        private static void CaptionChangedHandler(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((FocusableTextBlock)o).UpdateDisplayedText();
        }

        private bool _focused;
        private string _text;
        private int _maxTextLength;

        public FocusableTextBlock()
        {
            InitializeComponent();

            _focused = false;
            _text = "";
            _maxTextLength = 0;

            UpdateDisplayedText();
        }

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                UpdateDisplayedText();
            }
        }

        public bool Focused
        {
            get
            {
                return _focused;
            }
            set
            {
                _focused = value;
                UpdateBorder();
            }
        }

        public int MaxTextLength
        {
            get
            {
                return _maxTextLength;
            }
            set
            {
                _maxTextLength = value;
                UpdateDisplayedText();
            }
        }

        private void UpdateBorder()
        {
            _border.Style = (Style)(_focused ? FindResource("FocusedTextBoxBorderStyle") : FindResource("TextBoxBorderStyle"));
        }

        private void UpdateDisplayedText()
        {
            if (_maxTextLength > 0)
                _text = _text.Substring(0, Math.Min(_text.Length, _maxTextLength));

            if (_text.Length > 0)
            {
                _textBlock.Text = _text;
                _textBlock.Style = (Style)FindResource("KeyboardTextBoxStyle");
            }
            else
            {
                _textBlock.Text = this.Caption;
                _textBlock.Style = (Style)FindResource("KeyboardTextBoxCaptionStyle");
            }
        }
    }
}