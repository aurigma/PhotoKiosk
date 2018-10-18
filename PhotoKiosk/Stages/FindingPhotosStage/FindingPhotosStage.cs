// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.GraphicsMill;
using Aurigma.PhotoKiosk.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Threading;

namespace Aurigma.PhotoKiosk
{
    public class FindingPhotosStage : StageBase
    {
        private ProgressScreen _progressScreen;
        private SelectDeviceScreen _selectDeviceScreen;
        private SelectFoldersScreen _selectFoldersScreen;
        private BluetoothLoadScreen _bluetoothLoadScreen;

        private Collection<PhotoItem> _photoItems;
        private List<string> _files;
        private List<string> _photosToRemove;
        private List<Bitmap> _exifSources;
        private bool _photosFound;

        private List<string> _selectedFolders;
        private List<string> _deselectedFolders;
        private List<string> _lastSelectedFolders;

        private string[] _searchPaths;

        private DispatcherTimer _timer;
        private double _elapsedTime;
        private Thread _mainThread;
        private bool _isThreadAlive;

        public FindingPhotosStage()
            : base(Constants.FindingPhotosStageName, ExecutionEngine.Config.InactivityTimeout.Value)
        {
            _selectedFolders = new List<string>();
            _deselectedFolders = new List<string>();

            _selectDeviceScreen = new SelectDeviceScreen(this);
            _selectFoldersScreen = new SelectFoldersScreen(this);
            _bluetoothLoadScreen = new BluetoothLoadScreen(this);
            _progressScreen = new ProgressScreen(this);

            LastVisitedPage = null;

            _photoItems = new Collection<PhotoItem>();
            _photosToRemove = new List<string>();
            _files = new List<string>();
            _exifSources = new List<Bitmap>();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(Constants.FindingPhotosTimerInterval);
            _timer.Tick += TimerTickHandler;
            _mainThread = Thread.CurrentThread;

            _lastSelectedFolders = new List<string>();

            ExecutionEngine.EventLogger.Write("FindingPhotosStage created");
        }

        public override void Activate(ExecutionEngine engine)
        {
            base.Activate(engine);

            if (_timer.IsEnabled)
                _timer.Stop();

            _isThreadAlive = false;
            _elapsedTime = 0;

            ExecutionEngine.Instance.PhotosCanvas.Children.Clear();
            _progressScreen._progressBar.Value = 0;

            _exifSources.Clear();
            _photosFound = false;

            FillSearchPaths();

            // The very first activation
            if (LastVisitedPage == null)
            {
                if (ExecutionEngine.Config.IsBluetoothEnabled())
                    SwitchToSelectDeviceScreen();
                else
                    UseStorage();
            }
        }

        public override void Reset()
        {
            ExecutionEngine.EventLogger.Write("FindingPhotosStage:Reset");

            if (_timer.IsEnabled)
                _timer.Stop();

            _selectFoldersScreen._folderTree.Items.Clear();
            _selectDeviceScreen._storageDeviceButton.IsChecked = true;

            LastVisitedPage = null;

            _lastSelectedFolders.Clear();
            _selectedFolders.Clear();
            _deselectedFolders.Clear();

            _files.Clear();
            _photoItems.Clear();
        }

        public void SwitchToWelcomeScreen()
        {
            ExecutionEngine.EventLogger.Write("FindingPhotosStage:SwitchToWelcomeScreen");
            Engine.ExecuteCommand(new SwitchToStageCommand(Constants.WelcomeStageName));
        }

        public void SwitchToSelectDeviceScreen()
        {
            ExecutionEngine.EventLogger.Write("FindingPhotosStage:SwitchToSelectDeviceScreen");
            LastVisitedPage = _selectDeviceScreen;
            Engine.ExecuteCommand(new SwitchToScreenCommand(_selectDeviceScreen));
        }

        public void SwitchToSelectFoldersScreen()
        {
            ExecutionEngine.EventLogger.Write("FindingPhotosStage:SwitchToSelectFoldersScreen");
            LastVisitedPage = _selectFoldersScreen;
            Engine.ExecuteCommand(new SwitchToScreenCommand(_selectFoldersScreen));
        }

        public void SwitchToBluetoothLoadSceen()
        {
            ExecutionEngine.EventLogger.Write("FindingPhotosStage:SwitchToBluetoothLoadSceen");
            LastVisitedPage = _bluetoothLoadScreen;
            Engine.ExecuteCommand(new SwitchToScreenCommand(_bluetoothLoadScreen));
        }

        public void SwitchToProcessScreen()
        {
            ExecutionEngine.EventLogger.Write("FindingPhotosStage:SwitchToProcessScreen");
            Engine.ExecuteCommand(new SwitchToScreenCommand(_progressScreen));
        }

        public void SwitchToSelectPhotos()
        {
            ExecutionEngine.EventLogger.Write("FindingPhotosStage:SwitchToSelectPhotos");

            if (_timer.IsEnabled)
                _timer.Stop();

            ExecutionEngine.Context[Constants.FoundPhotos] = _photoItems;
            ExecutionEngine.Context[Constants.PhotosToRemove] = _photosToRemove;

            Engine.ExecuteCommand(new SwitchToStageCommand(Constants.SelectStageName));
        }

        public void StartFindPhotos()
        {
            var findPhotosThread = new Thread(new ThreadStart(FindPhotos));
            findPhotosThread.Priority = ThreadPriority.Lowest;
            findPhotosThread.Start();
            _timer.Start();
        }

        public void UseStorage()
        {
            if (SelectedFolders.Count == 0 && DeselectedFolders.Count == 0)
            {
                Engine.IsBluetooth = false;
                var paths = new List<string>(_searchPaths);
                var sourcePaths = new List<string>();
                while (paths.Count > 0)
                {
                    string path = paths[0];
                    paths.RemoveAt(0);

                    if (Directory.Exists(path))
                    {
                        SelectedFolders.Add(path);
                        var dirInfo = new DirectoryInfo(path);
                        try
                        {
                            foreach (DirectoryInfo subdirInfo in dirInfo.GetDirectories())
                            {
                                paths.Add(subdirInfo.FullName);
                            }
                        }
                        catch (System.Exception e)
                        {
                            ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                        }
                    }
                }

                if (!ExecutionEngine.Config.EnableFolderSelection.Value || SelectedFolders.Count == 1)
                {
                    Engine.IsShowFolders = false;
                    SwitchToProcessScreen();
                }
                else
                {
                    Engine.IsShowFolders = true;
                    SwitchToSelectFoldersScreen();
                }
            }
            else if (Engine.IsShowFolders)
            {
                SwitchToSelectFoldersScreen();
            }
            else
            {
                SwitchToProcessScreen();
            }
        }

        public void UseBluetooth()
        {
            Engine.IsBluetooth = true;
            FileOrderStorage.ClearBluetoothDirectory();
            SwitchToBluetoothLoadSceen();
        }

        public List<string> SelectedFolders
        {
            get { return _selectedFolders; }
            set { _selectedFolders = value; }
        }

        public List<string> DeselectedFolders
        {
            get { return _deselectedFolders; }
            set { _deselectedFolders = value; }
        }

        public List<string> LastSelectedFolders
        {
            get { return _lastSelectedFolders; }
            set { _lastSelectedFolders = value; }
        }

        public string[] SearchPaths
        {
            get { return _searchPaths; }
        }

        private void FillSearchPaths()
        {
            var pathsList = new List<string>();

            if (!string.IsNullOrEmpty(ExecutionEngine.Config.SourcePaths.Value))
            {
                foreach (string path in ExecutionEngine.Config.SourcePaths.Value.Split(';'))
                {
                    pathsList.Add(path);
                }
            }
            else
            {
                foreach (string drive in Directory.GetLogicalDrives())
                {
                    var driveInfo = new DriveInfo(drive);
                    if (driveInfo.DriveType == DriveType.Removable || driveInfo.DriveType == DriveType.CDRom)
                    {
                        pathsList.Add(drive);
                    }
                }
            }

            // Do not read photos from CD if primary action is CD burning
            if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.BurnCd)
            {
                foreach (string path in pathsList)
                {
                    if (path == ExecutionEngine.Config.DriveLetter.Value)
                    {
                        pathsList.Remove(path);
                        break;
                    }
                }
            }

            _searchPaths = pathsList.ToArray();
        }

        private void FindPhotos()
        {
            ExecutionEngine.EventLogger.Write("FindingPhotosStage:FindPhotos");

            _isThreadAlive = true;

            if (ExecutionEngine.Context.Contains(Constants.PhotosToRemove))
                _photosToRemove = (List<string>)ExecutionEngine.Context[Constants.PhotosToRemove];
            else
                _photosToRemove.Clear();

            if (_isThreadAlive)
            {
                if (DeselectedFolders != null)
                {
                    DeselectedFolders.ForEach(item =>
                    {
                        if (LastSelectedFolders.Contains(item) &&
                           (SelectedFolders == null || SelectedFolders.Contains(item))
                        )
                        {
                            LastSelectedFolders.Remove(item);
                        }
                    });

                    var folders = SelectedFolders == null ? DeselectedFolders : DeselectedFolders.Where(f => !SelectedFolders.Contains(f)).ToList();
                    foreach (string folder in folders)
                    {
                        var dirInfo = new DirectoryInfo(folder);
                        try
                        {
                            foreach (FileInfo fileInfo in dirInfo.GetFiles())
                            {
                                if (IsExtensionOk(fileInfo.Extension))
                                {
                                    if (_files.Contains(fileInfo.FullName))
                                    {
                                        _files.Remove(fileInfo.FullName);
                                    }
                                    _photosToRemove.Add(fileInfo.FullName);
                                }
                            }
                        }
                        catch (System.Exception e)
                        {
                            ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                        }
                    }
                }

                if (SelectedFolders != null)
                {
                    foreach (string path in SelectedFolders)
                    {
                        int index = _files.Count;
                        int count = 0;
                        var dirInfo = new DirectoryInfo(path);

                        try
                        {
                            foreach (FileInfo fileInfo in dirInfo.GetFiles())
                            {
                                if (IsExtensionOk(fileInfo.Extension) && !_files.Contains(fileInfo.FullName))
                                {
                                    _files.Add(fileInfo.FullName);
                                    count++;
                                }
                            }
                            _files.Sort(index, count, null);
                        }
                        catch (System.Exception e)
                        {
                            ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                        }
                    }
                    var newFiles = _files.Where(file => !_photoItems.Any(item => file.Equals(item.SourceFileName)));

                    _photoItems.Clear();
                    foreach (string fileName in newFiles)
                    {
                        _photoItems.Add(new PhotoItem(fileName));
                    }
                }
            }

            _photosFound = true;

            try
            {
                foreach (PhotoItem item in _photoItems)
                {
                    Bitmap exif = null;

                    try
                    {
                        exif = item.LoadExifThumbnail();
                    }
                    catch (Aurigma.GraphicsMill.Exception ex)
                    {
                        ExecutionEngine.EventLogger.WriteExceptionInfo(ex);
                    }
                    catch (IOException ex)
                    {
                        ExecutionEngine.EventLogger.WriteExceptionInfo(ex);
                    }

                    if (!_isThreadAlive || !IsMainThreadAlive())
                        break;

                    if (exif != null && !exif.IsEmpty)
                    {
                        lock (_exifSources)
                        {
                            _exifSources.Add(exif);
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                _photoItems.Clear();
            }

            SelectedFolders.ForEach(item =>
            {
                if (!LastSelectedFolders.Contains(item))
                {
                    LastSelectedFolders.Add(item);
                }
            });

            _isThreadAlive = false;
        }

        private static bool IsExtensionOk(string extension)
        {
            foreach (string supExt in Constants.SupportableExtensions)
            {
                if (supExt.CompareTo(extension.ToLower(CultureInfo.InvariantCulture)) == 0)
                    return true;
            }
            return false;
        }

        private bool IsMainThreadAlive()
        {
            return ((_mainThread.ThreadState & ThreadState.Stopped) == 0);
        }

        private void TimerTickHandler(object sender, EventArgs e)
        {
            _elapsedTime += _timer.Interval.TotalMilliseconds;

            if (!_timer.IsEnabled || !_photosFound)
                return;

            // No files at all
            if (_files.Count == 0)
            {
                _isThreadAlive = false;
                _timer.Stop();

                if (_photosToRemove.Count != 0)
                    ExecutionEngine.Context[Constants.PhotosToRemove] = _photosToRemove;

                MessageDialog.Show((string)ExecutionEngine.Instance.Resource[Constants.MessageNoFilesFoundKey]);
                if (Engine.IsShowFolders)
                    SwitchToSelectFoldersScreen();
                else if (ExecutionEngine.Config.IsBluetoothEnabled())
                    SwitchToSelectDeviceScreen();
                else
                    SwitchToWelcomeScreen();
                return;
            }

            // No new files
            if (_photoItems.Count == 0)
            {
                _timer.Stop();
                _isThreadAlive = false;
                SwitchToSelectPhotos();
                return;
            }

            Bitmap exifSource = null;

            lock (_exifSources)
            {
                if (_exifSources.Count > 0)
                {
                    exifSource = _exifSources[0];
                    _exifSources.RemoveAt(0);
                }
            }

            if (exifSource != null)
            {
                _progressScreen.AddPreviewPhoto(PhotoItem.CreateBitmapSource(exifSource));
            }

            _progressScreen._progressBar.Value++;

            if (_elapsedTime >= ExecutionEngine.Config.SearchProcessDuration.Value)
            {
                _timer.Stop();
                _isThreadAlive = false;
                SwitchToSelectPhotos();
            }
        }
    }
}