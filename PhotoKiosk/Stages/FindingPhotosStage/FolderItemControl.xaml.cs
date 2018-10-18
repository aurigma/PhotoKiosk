// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Aurigma.PhotoKiosk
{
    public partial class FolderItemControl : TreeViewItem
    {
        private const int Offset = 60;
        private const int MaximumWidth = 763;
        private const int MinimumWidth = 103;
        private const int MaxLevel = 11;

        private int _altIndex;

        private string _fullName;
        private string _name;
        private DateTime _date;
        private int _count;
        private int _level;

        public FolderItemControl()
        {
            InitializeComponent();
        }

        public bool IsChecked
        {
            get { return _folderBox.IsChecked.Value; }
            set { _folderBox.IsChecked = value; }
        }

        public int AltIndex
        {
            get { return _altIndex; }
            set { _altIndex = value; }
        }

        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        public string ShortName
        {
            get { return _name; }
            set { _name = value; }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public int PhotosCount
        {
            get { return _count; }
            set { _count = value; }
        }

        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }

        public event RoutedEventHandler Checked;

        public event RoutedEventHandler ExpandedCollapsed;

        public event DependencyPropertyChangedEventHandler VisibilityChanged;

        public void Update()
        {
            _folderName.Text = _name;
            _folderDate.Text = _date.ToString("d", CultureInfo.CurrentCulture);
            _folderFiles.Text = _count.ToString(CultureInfo.InvariantCulture);

            if (_level == 0)
                this.Margin = new Thickness(100, 0, 0, 0);

            if (_altIndex == 0)
            {
                _borderBig.Style = (Style)FindResource("AltBackgroundBig1");
                _borderSmall.Style = (Style)FindResource("AltBackgroundSmall1");
            }
            else if (_altIndex == 1)
            {
                _borderBig.Style = (Style)FindResource("AltBackgroundBig2");
                _borderSmall.Style = (Style)FindResource("AltBackgroundSmall2");
            }
        }

        public void SetWidth(int maxVisibleLevel)
        {
            int width, maxWidth;

            if (maxVisibleLevel <= MaxLevel)
                maxWidth = MaximumWidth;
            else
                maxWidth = MaximumWidth + (maxVisibleLevel - MaxLevel) * Offset;

            width = maxWidth - _level * Offset;

            if (width >= MinimumWidth)
                _folderName.Width = width;
            else
                _folderName.Width = MinimumWidth;
        }

        private void MouseDownHandler(object sender, RoutedEventArgs e)
        {
            this.IsExpanded = !this.IsExpanded;
        }

        private void CheckedHandler(object sender, RoutedEventArgs e)
        {
            if (Checked != null)
                Checked(this, e);
        }

        private void ExpandedCollapsedHandler(object sender, RoutedEventArgs e)
        {
            if (ExpandedCollapsed != null)
                ExpandedCollapsed(this, e);
        }

        private void VisibleChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (VisibilityChanged != null)
                VisibilityChanged(this, e);
        }
    }
}