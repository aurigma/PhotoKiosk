// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Aurigma.PhotoKiosk
{
    public partial class ScreenKeyboardControl : UserControl
    {
        private enum KeyType
        {
            Backspace,
            Spacebar,
            Shift,
            HashKey,
            Symbol,
            EndRowMarker
        }

        private class KeyboardLayout
        {
            private Grid _keyboardLayout;
            private Style _defaultButtonStyle;
            private Style _backspaceButtonStyle;
            private Style _spaceButtonStyle;
            private Style _shiftButtonStyle;
            private RoutedEventHandler _clickEventHandler;

            public KeyboardLayout(string layoutMap, Style defaultButtonStyle, Style backspaceButtonStyle, Style spaceButtonStyle, Style shiftButtonStyle, RoutedEventHandler clickEventHandler)
            {
                _defaultButtonStyle = defaultButtonStyle;
                _backspaceButtonStyle = backspaceButtonStyle;
                _spaceButtonStyle = spaceButtonStyle;
                _shiftButtonStyle = shiftButtonStyle;

                _clickEventHandler = clickEventHandler;

                SetKeyboardLayout(layoutMap);
            }

            public Grid GetGrid()
            {
                return _keyboardLayout;
            }

            private void SetKeyboardLayout(string keyValues)
            {
                if (keyValues == null)
                {
                    return;
                }

                string[] splittedValues = keyValues.Trim().Split(Constants.SpecialKeyFramer);

                _keyboardLayout = new Grid();
                _keyboardLayout.RowDefinitions.Add(new RowDefinition());

                StackPanel keysRow = new StackPanel();
                keysRow.HorizontalAlignment = HorizontalAlignment.Center;
                keysRow.Orientation = Orientation.Horizontal;

                foreach (string values in splittedValues)
                {
                    KeyType keyType = GetKeyType(values);

                    if (keyType == KeyType.Symbol)
                    {
                        foreach (char ch in values)
                        {
                            Button inputButton = new Button();

                            inputButton.Content = ch;
                            inputButton.Style = _defaultButtonStyle;
                            inputButton.Click += _clickEventHandler;

                            keysRow.Children.Add(inputButton);
                        }
                    }
                    else if (keyType == KeyType.EndRowMarker)
                    {
                        _keyboardLayout.Children.Add(keysRow);
                        _keyboardLayout.RowDefinitions.Add(new RowDefinition());

                        Grid.SetColumn(keysRow, 0);
                        Grid.SetRow(keysRow, _keyboardLayout.Children.Count - 1);

                        keysRow = new StackPanel();
                        keysRow.Orientation = Orientation.Horizontal;
                        keysRow.HorizontalAlignment = HorizontalAlignment.Center;
                    }
                    else
                    {
                        Button specialButton = new Button();
                        specialButton.Click += _clickEventHandler;

                        if (keyType == KeyType.Backspace)
                        {
                            specialButton.Name = ButtonBackspaceName;
                            if (_backspaceButtonStyle != null)
                            {
                                char c = '\xAC';
                                specialButton.Style = _backspaceButtonStyle;
                                specialButton.FontFamily = new System.Windows.Media.FontFamily("Symbol");
                                specialButton.Content = c.ToString();
                            }
                        }
                        else if (keyType == KeyType.Spacebar)
                        {
                            specialButton.Name = ButtonSpaceName;
                            if (_spaceButtonStyle != null)
                            {
                                specialButton.Style = _spaceButtonStyle;
                            }
                        }
                        else if (keyType == KeyType.Shift)
                        {
                            specialButton.Name = ButtonShiftName;

                            string caption = "Shift";
                            int captionStartIndex = values.IndexOf(':');
                            if (captionStartIndex >= 0 && captionStartIndex < values.Length - 1)
                                caption = values.Substring(captionStartIndex + 1);

                            specialButton.Content = caption;
                            if (_shiftButtonStyle != null)
                            {
                                specialButton.Style = _shiftButtonStyle;
                            }
                        }
                        else if (keyType == KeyType.HashKey)
                        {
                            specialButton.Name = ButtonHashName;
                            specialButton.Content = Constants.SpecialKeyFramer;

                            if (_defaultButtonStyle != null)
                            {
                                specialButton.Style = _defaultButtonStyle;
                            }
                        }

                        keysRow.Children.Add(specialButton);
                    }
                }
                _keyboardLayout.Children.Add(keysRow);
                Grid.SetColumn(keysRow, 0);
                Grid.SetRow(keysRow, _keyboardLayout.Children.Count - 1);
            }

            private static KeyType GetKeyType(string keyName)
            {
                if (String.Compare(keyName, ButtonBackspaceName, true, System.Globalization.CultureInfo.InvariantCulture) == 0)
                {
                    return KeyType.Backspace;
                }
                else if (String.Compare(keyName, ButtonSpaceName, true, System.Globalization.CultureInfo.InvariantCulture) == 0)
                {
                    return KeyType.Spacebar;
                }
                else if (String.Compare(keyName, EndRowSplitter, true, System.Globalization.CultureInfo.InvariantCulture) == 0)
                {
                    return KeyType.EndRowMarker;
                }
                else if (String.Compare(keyName, ButtonHashName, true, System.Globalization.CultureInfo.InvariantCulture) == 0)
                {
                    return KeyType.HashKey;
                }
                else if (keyName.ToLower().StartsWith(ButtonShiftName))
                {
                    return KeyType.Shift;
                }
                else
                {
                    return KeyType.Symbol;
                }
            }
        }

        private const string ButtonBackspaceName = "backspace";
        private const string ButtonSpaceName = "space";
        private const string ButtonShiftName = "shift";
        private const string EndRowSplitter = "endrow";
        private const string ButtonHashName = "hash";

        private bool _isShifted;
        private KeyboardLayout _layout;
        private KeyboardLayout _shiftedLayout;
        private string _layoutMap;
        private string _shiftedLayoutMap;
        private string _typedString;
        private FocusableTextBlock _bindedControl;

        public ScreenKeyboardControl()
        {
            InitializeComponent();

            _isShifted = false;
        }

        public void ShowKeyboardLayout(bool shifted)
        {
            KeyboardControlFrame.Content = (shifted ? _shiftedLayout.GetGrid() : _layout.GetGrid());
        }

        public void ClearTypedString()
        {
            _typedString = "";
            UpdateBindedControl();
        }

        public string Layout
        {
            get
            {
                return _layoutMap;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _layoutMap = value;
                setLayout(value, false);
            }
        }

        public string ShiftedLayout
        {
            get
            {
                return _shiftedLayoutMap;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _shiftedLayoutMap = value;
                setLayout(value, true);
            }
        }

        public FocusableTextBlock BindedControl
        {
            get
            {
                return _bindedControl;
            }
            set
            {
                if (_bindedControl != null)
                    _bindedControl.Focused = false;

                _bindedControl = value;

                if (value != null)
                {
                    _typedString = _bindedControl.Text;
                    _bindedControl.Focused = true;
                }
            }
        }

        private void setLayout(string keyValues, bool shiftedLayout)
        {
            KeyboardLayout newLayout = new KeyboardLayout(keyValues,
                (Style)FindResource("KeyboardButtonStyle"),
                (Style)FindResource("BackspaceButtonStyle"),
                (Style)FindResource("SpaceButtonStyle"),
                (Style)FindResource("ShiftButtonStyle"),
                new RoutedEventHandler(ButtonClickEventHandler));

            if (shiftedLayout)
                _shiftedLayout = newLayout;
            else
                _layout = newLayout;
        }

        private void ButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;

            switch (senderButton.Name)
            {
                case ButtonBackspaceName:
                    {
                        if (_typedString.Length > 0)
                        {
                            _typedString = _typedString.Remove(_typedString.Length - 1);
                        }
                        break;
                    }
                case ButtonSpaceName:
                    {
                        _typedString += " ";
                        break;
                    }
                case ButtonShiftName:
                    {
                        _isShifted = !_isShifted;
                        ShowKeyboardLayout(_isShifted);
                        break;
                    }
                default:
                    {
                        _typedString += senderButton.Content.ToString();
                        break;
                    }
            }
            UpdateBindedControl();
        }

        private void Keyboard_Loaded(object sender, RoutedEventArgs e)
        {
            ShowKeyboardLayout(false);
        }

        private void UpdateBindedControl()
        {
            if (_bindedControl != null)
            {
                _bindedControl.Text = _typedString;
                _typedString = _bindedControl.Text;
            }
        }
    }
}