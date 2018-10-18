// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Aurigma.PhotoKiosk
{
    public partial class SelectFoldersScreen : Page
    {
        private FindingPhotosStage _findingPhotosStage;
        private bool _isDisposed;

        public SelectFoldersScreen()
        {
            if (ExecutionEngine.Instance != null)
                Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);

            InitializeComponent();
        }

        public SelectFoldersScreen(FindingPhotosStage stage)
            : this()
        {
            _findingPhotosStage = stage;
            if (ExecutionEngine.Config.IsBluetoothEnabled())
                _prevButton.Visibility = Visibility.Visible;
            else
                _prevButton.Visibility = Visibility.Collapsed;

            _folderTree.Items.Clear();
        }

        ~SelectFoldersScreen()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            try
            {
                Dispose(true);
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
                _isDisposed = true;
        }

        private void CheckDisposedState()
        {
            if (_isDisposed)
                throw new ObjectDisposedException("SelectFoldersScreen");
        }

        private void PageLoadedHandler(object sender, RoutedEventArgs e)
        {
            CheckDisposedState();

            if (_folderTree.Items.Count == 0)
            {
                _folderTree.Items.Clear();
                _findingPhotosStage.SelectedFolders.Clear();
                _findingPhotosStage.DeselectedFolders.Clear();

                // Fill folder treeView
                for (int i = 0; i < _findingPhotosStage.SearchPaths.Length; i++)
                {
                    AddFolder(new DirectoryInfo(_findingPhotosStage.SearchPaths[i]), _folderTree, 0);
                }

                // No files found
                if (_folderTree.Items.Count == 0)
                {
                    MessageDialog.Show((string)FindResource(Constants.MessageNoFilesFoundKey));
                    if (ExecutionEngine.Config.IsBluetoothEnabled())
                        _findingPhotosStage.SwitchToSelectDeviceScreen();
                    else
                        _findingPhotosStage.SwitchToWelcomeScreen();
                    return;
                }

                // Check all folders
                foreach (FolderItemControl folder in _folderTree.Items)
                {
                    folder.IsExpanded = true;
                }

                _foldersCount.Text = _findingPhotosStage.SelectedFolders.Count.ToString();

                SetAltIndexes();
            }
            _nextButton.IsEnabled = _findingPhotosStage.SelectedFolders.Count > 0;
        }

        private void ButtonPrevStageClickHandler(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Config.IsBluetoothEnabled())
            {
                CheckDisposedState();
                _findingPhotosStage.SwitchToSelectDeviceScreen();
            }
        }

        private void ButtonNextClickHandler(object sender, RoutedEventArgs e)
        {
            CheckDisposedState();
            _findingPhotosStage.SwitchToProcessScreen();
        }

        private void CheckedHandler(object sender, RoutedEventArgs e)
        {
            var folder = (FolderItemControl)sender;
            if (folder != null)
            {
                if (folder.IsChecked)
                {
                    if (!_findingPhotosStage.SelectedFolders.Contains(folder.FullName))
                        _findingPhotosStage.SelectedFolders.Add(folder.FullName);

                    if (_findingPhotosStage.DeselectedFolders.Contains(folder.FullName))
                        _findingPhotosStage.DeselectedFolders.Remove(folder.FullName);

                    CheckItem(folder, true);
                }
                else
                {
                    if (_findingPhotosStage.SelectedFolders.Contains(folder.FullName))
                        _findingPhotosStage.SelectedFolders.Remove(folder.FullName);

                    if (!_findingPhotosStage.DeselectedFolders.Contains(folder.FullName))
                        _findingPhotosStage.DeselectedFolders.Add(folder.FullName);

                    CheckItem(folder, false);
                }

                _foldersCount.Text = _findingPhotosStage.SelectedFolders.Count.ToString();

                _nextButton.IsEnabled = _findingPhotosStage.SelectedFolders.Count > 0;
            }
        }

        private void ExpandedCollapsedHandler(object sender, RoutedEventArgs e)
        {
            SetAltIndexes();
        }

        private void VisibilityChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateTreeWidth(GetMaxVisibleLevel());
        }

        private void AddFolder(DirectoryInfo dir, ItemsControl item, int level)
        {
            try
            {
                if (item != null && dir.Exists)
                {
                    string name;
                    int photosCount;

                    // Root folder. Display volume label and drive type
                    if (dir.Parent == null)
                    {
                        var drive = new DriveInfo(dir.Name);
                        if (drive.VolumeLabel != "")
                            name = drive.VolumeLabel;
                        else
                            name = DriveTypeToString(drive.DriveType);
                    }
                    // Display folder name
                    else if ((dir.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        name = dir.Name;
                    }
                    // Skip hidden folders
                    else
                    {
                        return;
                    }

                    var newItem = new FolderItemControl();
                    newItem.FullName = dir.FullName;
                    item.Items.Add(newItem);

                    foreach (DirectoryInfo subDir in dir.GetDirectories())
                    {
                        AddFolder(subDir, newItem, level + 1);
                    }

                    photosCount = GetPhotosCount(newItem);
                    if (photosCount > 0)
                    {
                        newItem.ShortName = name;
                        newItem.Date = dir.LastWriteTime;
                        newItem.PhotosCount = photosCount;
                        newItem.Level = level;
                        newItem.Update();
                        newItem.Checked += new RoutedEventHandler(CheckedHandler);
                        newItem.ExpandedCollapsed += new RoutedEventHandler(ExpandedCollapsedHandler);
                        newItem.VisibilityChanged += new DependencyPropertyChangedEventHandler(VisibilityChangedHandler);
                    }
                    else
                    {
                        item.Items.Remove(newItem);
                        newItem = null;
                    }
                }
            }
            catch (Exception e)
            {
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
            }
        }

        private string DriveTypeToString(DriveType type)
        {
            string result;
            switch (type)
            {
                case DriveType.CDRom:
                    result = (string)FindResource(Constants.CDRomDriveTypeTextKey);
                    break;

                case DriveType.Removable:
                    result = (string)FindResource(Constants.RemovableDriveTypeTextKey);
                    break;

                default:
                    result = type.ToString();
                    break;
            }
            return result;
        }

        private int GetPhotosCount(FolderItemControl item)
        {
            int count = 0;
            foreach (FolderItemControl subItem in item.Items)
            {
                count += subItem.PhotosCount;
            }

            var dir = new DirectoryInfo(item.FullName);
            foreach (FileInfo file in dir.GetFiles())
            {
                if (IsImageExtension(file))
                    count++;
            }

            return count;
        }

        private bool IsImageExtension(FileInfo file)
        {
            foreach (string ext in Constants.SupportableExtensions)
            {
                if (ext.CompareTo(file.Extension.ToLower(CultureInfo.InvariantCulture)) == 0)
                    return true;
            }
            return false;
        }

        private void CheckItem(FolderItemControl item, bool check)
        {
            item.IsChecked = check;
            foreach (FolderItemControl subItem in item.Items)
            {
                CheckItem(subItem, check);
            }
        }

        private void SetAltIndexes()
        {
            int index = 0;
            for (int i = 0; i < _folderTree.Items.Count; i++)
            {
                index = SetAltIndex((FolderItemControl)_folderTree.Items[i], index);
            }
        }

        private int SetAltIndex(FolderItemControl item, int index)
        {
            const int altCount = 2;
            if (index >= altCount)
                index = 0;

            item.AltIndex = index++;
            item.Update();
            if (item.IsExpanded && item.Items.Count > 0)
            {
                foreach (FolderItemControl subItem in item.Items)
                {
                    index = SetAltIndex(subItem, index);
                }
            }

            return index;
        }

        private void UpdateTreeWidth(int maxVisibleLevel)
        {
            for (int i = 0; i < _folderTree.Items.Count; i++)
            {
                UpdateWidth((FolderItemControl)_folderTree.Items[i], maxVisibleLevel);
            }
        }

        private void UpdateWidth(FolderItemControl item, int maxVisibleLevel)
        {
            item.SetWidth(maxVisibleLevel);
            if (item.Items.Count > 0)
            {
                foreach (FolderItemControl subItem in item.Items)
                {
                    UpdateWidth(subItem, maxVisibleLevel);
                }
            }
        }

        private int GetMaxVisibleLevel()
        {
            int maxLevel = 0;
            for (int i = 0; i < _folderTree.Items.Count; i++)
            {
                maxLevel = GetMaxVisibleLevel((FolderItemControl)_folderTree.Items[i], maxLevel);
            }

            return maxLevel;
        }

        private int GetMaxVisibleLevel(FolderItemControl item, int maxLevel)
        {
            if (item.IsVisible)
            {
                int level = Math.Max(maxLevel, item.Level);
                if (item.IsExpanded && item.Items.Count > 0)
                {
                    foreach (FolderItemControl subItem in item.Items)
                    {
                        level = GetMaxVisibleLevel(subItem, level);
                    }
                }
                return level;
            }
            return maxLevel;
        }
    }
}