// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Aurigma.PhotoKiosk
{
    public partial class ImageEditorScreen : Page
    {
        #region [Constructor]

        public ImageEditorScreen(ImageEditorStage stage)
        {
            InitializeComponent();
            _imageEditorStage = stage;

            if (ExecutionEngine.Config.EnableRotation.Value)
            {
                _rotateLeftButton.Visibility = Visibility.Visible;
                _rotateRightButton.Visibility = Visibility.Visible;
            }
            else
            {
                _rotateLeftButton.Visibility = Visibility.Collapsed;
                _rotateRightButton.Visibility = Visibility.Collapsed;
            }

            if (ExecutionEngine.Config.EnableFlip.Value)
            {
                _flipVerticalButton.Visibility = Visibility.Visible;
                _flipHorizontalButton.Visibility = Visibility.Visible;
            }
            else
            {
                _flipVerticalButton.Visibility = Visibility.Collapsed;
                _flipHorizontalButton.Visibility = Visibility.Collapsed;
            }

            if (ExecutionEngine.Config.EnableCrop.Value)
                _cropButton.Visibility = Visibility.Visible;
            else
                _cropButton.Visibility = Visibility.Collapsed;

            if (ExecutionEngine.Config.EnableColorCorrection.Value)
                _colorButton.Visibility = Visibility.Visible;
            else
                _colorButton.Visibility = Visibility.Collapsed;

            if (ExecutionEngine.Config.EnableEffects.Value)
                _effectsButton.Visibility = Visibility.Visible;
            else
                _effectsButton.Visibility = Visibility.Collapsed;

            if (ExecutionEngine.Config.EnableRedEyeRemoval.Value)
                _redEyeButton.Visibility = Visibility.Visible;
            else
                _redEyeButton.Visibility = Visibility.Collapsed;
        }

        #endregion [Constructor]

        #region [Private common event handlers]

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            LoadImage();
            switch ((ImageEditorMode)ExecutionEngine.Context[Constants.ImageEditorModeKey])
            {
                case ImageEditorMode.View:
                    SwitchPanel(new Border());
                    _imageEditorStageText.TextContent = (string)ExecutionEngine.Instance.Resource[Constants.ImageEditorStageViewKey];
                    break;

                case ImageEditorMode.Edit:
                    _imageEditorStageText.TextContent = (string)ExecutionEngine.Instance.Resource[Constants.ImageEditorStageEditTextKey];
                    CreateCropButtonsPanel();
                    break;

                case ImageEditorMode.Crop:
                    SwitchPanel(new Border());
                    if ((ExecutionEngine.Context[Constants.OrderContextName] as Order).CropMode == Constants.CropToFillModeName)
                        _imageEditorStageText.TextContent = (string)ExecutionEngine.Instance.Resource[Constants.ImageEditorStageSetCropTextKey];
                    else
                        _imageEditorStageText.TextContent = (string)ExecutionEngine.Instance.Resource[Constants.ImageEditorStageViewKey];
                    break;
            }
        }

        private void FrameLoadedHandler(object sender, RoutedEventArgs e)
        {
            _rectangleAdorner = new RectangleAdorner(_frame.Photo);
            _pointAdorner = new PointAdorner(_frame.Photo);

            AdornerLayer layer = AdornerLayer.GetAdornerLayer(_frame.Photo);
            layer.Add(_rectangleAdorner);
            layer.Add(_pointAdorner);

            Reset();
        }

        private void ButtonReturnToPhotosClickHandler(object sender, RoutedEventArgs e)
        {
            if (_previewBitmap.UndoStepCount > 0 && !MessageDialog.ShowOkCancelMessage((string)TryFindResource(Constants.MessageWantToExitTextKey)))
                return;

            if (_tmpBitmap != null)
            {
                _tmpBitmap.Dispose();
                _tmpBitmap = null;
            }
            if (_autoLevelsBitmap != null)
            {
                _autoLevelsBitmap.Dispose();
                _autoLevelsBitmap = null;
            }
            if (_cropBitmap != null)
            {
                _cropBitmap.Dispose();
                _cropBitmap = null;
            }
            if (_redEyeBitmap != null)
            {
                _redEyeBitmap.Dispose();
                _redEyeBitmap = null;
            }

            if (_previewBitmap != null)
            {
                _previewBitmap.Dispose();
                _previewBitmap = null;
            }

            _listItem.GoToPage();

            _frame.Photo.MouseLeftButtonDown -= new MouseButtonEventHandler(ManualRedEyeClickHandler);
            ExecutionEngine.Instance.ExecuteCommand(new SwitchToStageCommand(Constants.SelectStageName));
        }

        private void PrevPhotoButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (_previewBitmap.UndoStepCount > 0 && !MessageDialog.ShowOkCancelMessage(ExecutionEngine.Instance.Resource[Constants.MessageEditPreviousTextKey] as string))
                return;

            SetPhoto(_listItem.GetPrevious());
            LoadImage();
            Reset();
        }

        private void NextPhotoButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (_previewBitmap.UndoStepCount > 0 && !MessageDialog.ShowOkCancelMessage(ExecutionEngine.Instance.Resource[Constants.MessageEditNextTextKey] as string))
                return;

            SetPhoto(_listItem.GetNext());
            LoadImage();
            Reset();
        }

        #endregion [Private common event handlers]

        #region [Private common methods]

        private void LoadImage()
        {
            _frame.Reset();

            _previewBitmap = CreatePreviewBitmap(_sourceBitmap, Aurigma.GraphicsMill.Transforms.ResizeMode.Shrink);
            _previewBitmap.UndoRedoEnabled = true;
            RefreshPreview();

            _transforms = new List<Transform>();
            _transformIndex = 0;
            _effect = EffectTransforms.None;
        }

        private void Reset()
        {
            _rectangleAdorner.Visibility = Visibility.Collapsed;
            _pointAdorner.Visibility = Visibility.Collapsed;

            if ((ImageEditorMode)ExecutionEngine.Context[Constants.ImageEditorModeKey] == ImageEditorMode.Edit)
                SwitchPanel(_editorButtonPanel);
        }

        private Aurigma.GraphicsMill.Bitmap CreatePreviewBitmap(Aurigma.GraphicsMill.Bitmap sourceBitmap, Aurigma.GraphicsMill.Transforms.ResizeMode resizeMode)
        {
            var bitmap = new Aurigma.GraphicsMill.Bitmap();

            using (var resize = new Aurigma.GraphicsMill.Transforms.Resize(new System.Drawing.Size((int)_frame.ImageSize.Width, (int)_frame.ImageSize.Height)))
            {
                resize.ResizeMode = resizeMode;
                resize.ApplyTransform(sourceBitmap, bitmap);
            }
            return bitmap;
        }

        private void CreateCropButtonsPanel()
        {
            int cropsCount = ExecutionEngine.Crops.CropFormats.Count;

            _cropButtonsGrid.Children.Clear();
            _cropButtonsGrid.ColumnDefinitions.Clear();
            _cropButtonsGrid.RowDefinitions.Clear();

            _cropButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _cropButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < (cropsCount + 1) / 2; i++)
            {
                _cropButtonsGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int j = 0; j < cropsCount; j++)
            {
                RadioButton cropButton = new CropButton(ExecutionEngine.Crops.CropFormats[j], (string)TryFindResource(Constants.ImageEditorCropOptionTextKey));
                cropButton.Style = (Style)FindResource("ImageEditorRadioButtonStyle");
                cropButton.Click += ButtonSetCropProportion;

                if (j % 2 == 0)
                    cropButton.Margin = new Thickness(9, 4, 4, 4);
                else
                    cropButton.Margin = new Thickness(4, 4, 9, 4);

                Grid.SetColumn(cropButton, j % 2);
                Grid.SetRow(cropButton, j / 2);

                _cropButtonsGrid.Children.Add(cropButton);
            }
        }

        private void RefreshPreview()
        {
            _undoButton.IsEnabled = _previewBitmap.CanUndo;
            _redoButton.IsEnabled = _previewBitmap.CanRedo;

            _frame.Photo.Source = PhotoItem.CreateBitmapSource(_previewBitmap);
            _frame.Update();
        }

        private void SwitchPanel(Border panel)
        {
            _editorButtonPanel.Visibility = Visibility.Collapsed;
            _cropToolsPanel.Visibility = Visibility.Collapsed;
            _colorToolsPanel.Visibility = Visibility.Collapsed;
            _effectToolsPanel.Visibility = Visibility.Collapsed;
            _redeyeToolsPanel.Visibility = Visibility.Collapsed;

            panel.Visibility = Visibility.Visible;
        }

        private void RegisterTransform(Transform transform)
        {
            if (_transformIndex < _transforms.Count)
            {
                _transforms[_transformIndex] = transform;
                _transformIndex++;
                _transforms.RemoveRange(_transformIndex, _transforms.Count - _transformIndex);
            }
            else
            {
                _transforms.Add(transform);
                _transformIndex++;
            }
        }

        #endregion [Private common methods]

        #region [Private main tool panel methods and event handlers]

        private void OnUndo(object sender, RoutedEventArgs e)
        {
            if (_transformIndex > 0)
            {
                _transformIndex--;

                if (_transforms[_transformIndex].TransformsCount <= _previewBitmap.UndoStepCount)
                    _previewBitmap.Undo(_transforms[_transformIndex].TransformsCount);

                RefreshPreview();
            }
        }

        private void OnRedo(object sender, RoutedEventArgs e)
        {
            if (_transformIndex < _transforms.Count)
            {
                if (_transforms[_transformIndex].TransformsCount <= _previewBitmap.RedoStepCount)
                    _previewBitmap.Redo(_transforms[_transformIndex].TransformsCount);

                _transformIndex++;

                RefreshPreview();
            }
        }

        private void OnFlipX(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Config.EnableFlip.Value)
            {
                var rotateAdnFlipTransform = new RotateAndFlipTransform(System.Drawing.RotateFlipType.RotateNoneFlipX);
                rotateAdnFlipTransform.Apply(_previewBitmap);
                RefreshPreview();
                RegisterTransform(rotateAdnFlipTransform);
            }
        }

        private void OnFlipY(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Config.EnableFlip.Value)
            {
                var rotateAndFlipTransform = new RotateAndFlipTransform(System.Drawing.RotateFlipType.RotateNoneFlipY);
                rotateAndFlipTransform.Apply(_previewBitmap);
                RefreshPreview();
                RegisterTransform(rotateAndFlipTransform);
            }
        }

        private void OnRotateLeft(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Config.EnableRotation.Value)
            {
                var rotateAndFlipTransform = new RotateAndFlipTransform(System.Drawing.RotateFlipType.Rotate270FlipNone);
                rotateAndFlipTransform.Apply(_previewBitmap);
                RefreshPreview();
                RegisterTransform(rotateAndFlipTransform);
            }
        }

        private void OnRotateRight(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Config.EnableRotation.Value)
            {
                var rotateAndFlipTransform = new RotateAndFlipTransform(System.Drawing.RotateFlipType.Rotate90FlipNone);
                rotateAndFlipTransform.Apply(_previewBitmap);
                RefreshPreview();
                RegisterTransform(rotateAndFlipTransform);
            }
        }

        private void CropButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Config.EnableCrop.Value)
            {
                _applyCropButton.IsEnabled = false;
                _cropSelectionRotateButton.Visibility = Visibility.Visible;

                SwitchPanel(_cropToolsPanel);
            }
        }

        private void ColorButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Config.EnableColorCorrection.Value)
            {
                _tmpBitmap = (Aurigma.GraphicsMill.Bitmap)_previewBitmap.Clone();
                _autoLevelsBitmap = (Aurigma.GraphicsMill.Bitmap)_previewBitmap.Clone();
                _autoLevelsBitmap.ColorAdjustment.AutoLevels();
                _brightnessAmount = 0;
                _contrastAmount = 0;
                _displayBrightness = 0;
                _displayContrast = 0;
                _brightnessText.Text = _displayBrightness.ToString();
                _contrastText.Text = _displayContrast.ToString();

                _applyColorButton.IsEnabled = false;
                _autoLevelsCheckBox.IsChecked = false;

                SwitchPanel(_colorToolsPanel);
            }
        }

        private void EffectsButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Config.EnableEffects.Value)
            {
                _blackAndWhiteButton.IsChecked = false;
                _sepiaButton.IsChecked = false;

                _applyEffectButton.IsEnabled = false;

                _effect = EffectTransforms.None;

                SwitchPanel(_effectToolsPanel);
            }
        }

        private void RedEyeRemovalButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Config.EnableRedEyeRemoval.Value)
            {
                _redEyeTransform = new RedEyeRemovalTransform();
                _applyRedEyeButton.IsEnabled = true;
                _redEyeRemovalStep = 0;

                _rectangleAdorner.Rectangle = new Rect(_frame.Photo.ActualWidth / 3, _frame.Photo.ActualHeight / 4, _frame.Photo.ActualWidth / 4, _frame.Photo.ActualHeight / 4);
                _rectangleAdorner.WidthToHeightProportion = 0;
                _rectangleAdorner.Visibility = Visibility.Visible;
                _pointAdorner.Visibility = Visibility.Collapsed;
                _nextRedEyeButton.Visibility = Visibility.Visible;
                _applyRedEyeButton.IsEnabled = false;
                _manualRedEyeButton.Visibility = Visibility.Collapsed;
                _removeRedEyeButton.Visibility = Visibility.Collapsed;

                _stepDescription.Text = (string)FindResource(Constants.ImageEditorRedEyeStep1TextKey);

                _undoRedEyeButton.IsEnabled = false;
                _redoRedEyeButton.IsEnabled = false;

                SwitchPanel(_redeyeToolsPanel);
            }
        }

        private void ButtonSaveClickHandler(object sender, RoutedEventArgs e)
        {
            if (!TransformBitmap(_sourceBitmap))
                return;

            Size sizeBefore = _sourceItem.ImageSize;

            if (_sourceItem != null)
                _sourceItem.UpdateSource(_result.Image);

            if (sizeBefore != _sourceItem.ImageSize)
            {
                foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                {
                    if (_listItem.Order.GetCount(format) == 0)
                        _listItem.Order.ClearCrop(format);
                    else
                        _listItem.Order.SetCrop(format);
                }
            }

            ExecutionEngine.Instance.ExecuteCommand(new SwitchToStageCommand(Constants.SelectStageName));
        }

        private void ButtonSaveAsClickHandler(object sender, RoutedEventArgs e)
        {
            if (!TransformBitmap(_sourceBitmap))
                return;

            _imageEditorStage.SaveEditedAsNew();
            ExecutionEngine.Instance.ExecuteCommand(new SwitchToStageCommand(Constants.SelectStageName));
        }

        private bool TransformBitmap(Aurigma.GraphicsMill.Bitmap bitmap)
        {
            _progressDialog = new ProgressDialog((string)ExecutionEngine.Instance.Resource[Constants.MessageTransformImageKey]);

            var workerThread = new Thread(new ParameterizedThreadStart(DoTransformBitmap));
            workerThread.Start((object)bitmap);

            DateTime showDialogTime = DateTime.Now.AddSeconds(Constants.ProgressDialogTimeout);
            while (DateTime.Now.Second < showDialogTime.Second)
            {
                if (_progressDialog.IsComplete)
                    return true;
            }

            _progressDialog.ShowDialog();

            return _progressDialog.DialogResult.Value;
        }

        private void DoTransformBitmap(object bitmapObject)
        {
            Aurigma.GraphicsMill.Bitmap bitmap = (Aurigma.GraphicsMill.Bitmap)bitmapObject;

            int transformsProgressStepCount = 0;

            for (int i = 0; i < _transformIndex; i++)
            {
                transformsProgressStepCount += _transforms[i].TransformProgressStepCount;
            }

            _progressDialog.SetRange(0, transformsProgressStepCount);

            for (int i = 0; i < _transformIndex; i++)
            {
                if (_progressDialog.IsAborted)
                    break;
                _transforms[i].Progress += new Aurigma.GraphicsMill.ProgressEventHandler(TransformProgressEventHandler);
                _transforms[i].Apply(bitmap);
                _transforms[i].Progress -= new Aurigma.GraphicsMill.ProgressEventHandler(TransformProgressEventHandler);
            }

            if (_progressDialog.IsAborted)
                _progressDialog.IsComplete = false;
            else
                _progressDialog.IsComplete = true;

            _progressDialog.End();
        }

        private void TransformProgressEventHandler(object sender, Aurigma.GraphicsMill.ProgressEventArgs e)
        {
            _progressDialog.Increment();
        }

        #endregion [Private main tool panel methods and event handlers]

        #region [Private crop panel method and event handlers]

        private void ButtonSetCropProportion(object sender, RoutedEventArgs e)
        {
            CropButton cropButton = sender as CropButton;
            ShowCropUI(cropButton.CropProportion);
        }

        private void CropRotateLeftButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (_rectangleAdorner.Visibility == Visibility.Visible)
                RectangleAdorner.InvertProportion.Execute(null, _rectangleAdorner);
        }

        private void ApplyCrop(object sender, RoutedEventArgs e)
        {
            if (_rectangleAdorner.Visibility == Visibility.Visible)
            {
                _cropBitmap = (Aurigma.GraphicsMill.Bitmap)_sourceBitmap.Clone();
                if (!TransformBitmap(_cropBitmap))
                    return;

                float x = (float)(_rectangleAdorner.Rectangle.X * (_cropBitmap.Width / _frame.Photo.ActualWidth));
                float y = (float)(_rectangleAdorner.Rectangle.Y * (_cropBitmap.Height / _frame.Photo.ActualHeight));
                float width = (float)(_rectangleAdorner.Rectangle.Width * (_cropBitmap.Width / _frame.Photo.ActualWidth));
                float height = (float)(_rectangleAdorner.Rectangle.Height * (_cropBitmap.Height / _frame.Photo.ActualHeight));

                if (width > Constants.MinCropWidth && height > Constants.MinCropHeight)
                {
                    _cropBitmap.Transforms.Crop(x, y, width, height);
                    _previewBitmap.Create(CreatePreviewBitmap(_cropBitmap, Aurigma.GraphicsMill.Transforms.ResizeMode.Shrink));
                    RefreshPreview();
                    RegisterTransform(new CropTransform(x, y, width, height));
                }
                else
                    MessageDialog.Show((string)ExecutionEngine.Instance.Resource[Constants.MessageMinCropSizeKey]);

                if (_cropBitmap != null)
                {
                    _cropBitmap.Dispose();
                    _cropBitmap = null;
                }

                _rectangleAdorner.Visibility = Visibility.Collapsed;
                SwitchPanel(_editorButtonPanel);
            }
        }

        private void CancelCrop(object sender, RoutedEventArgs e)
        {
            _rectangleAdorner.Visibility = Visibility.Collapsed;
            SwitchPanel(_editorButtonPanel);
        }

        private void ShowCropUI(double widthToHeightProportion)
        {
            _applyCropButton.IsEnabled = true;

            _rectangleAdorner.WidthToHeightProportion = widthToHeightProportion;
            _rectangleAdorner.IsCentered = true;
            _rectangleAdorner.Rectangle = new Rect(0, 0, _frame.Photo.ActualWidth, _frame.Photo.ActualHeight);

            _rectangleAdorner.Visibility = Visibility.Visible;
            _rectangleAdorner.IsCentered = false;

            if (_rectangleAdorner.IsProportional)
                _cropSelectionRotateButton.Visibility = Visibility.Visible;
            else
                _cropSelectionRotateButton.Visibility = Visibility.Collapsed;
        }

        private void CropToolsVisibleChangedEventHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "IsVisible" && (sender as Border).Visibility != Visibility.Visible)
            {
                foreach (CropButton button in _cropButtonsGrid.Children)
                {
                    button.IsChecked = false;
                }
            }
        }

        #endregion [Private crop panel method and event handlers]

        #region [Private color correction panel methods and event handlers]

        private void BrightnessDownButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (_brightnessAmount > -1)
            {
                _brightnessAmount -= 0.05f;
                _displayBrightness -= 1;
                if (_brightnessAmount < -1)
                {
                    _brightnessAmount = -1;
                    _displayBrightness = -20;
                }
                BrightnessContrast();
                _brightnessText.Text = _displayBrightness.ToString();
            }
        }

        private void BrightnessUpButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (_brightnessAmount < 1)
            {
                _brightnessAmount += 0.05f;
                _displayBrightness += 1;
                if (_brightnessAmount > 1)
                {
                    _brightnessAmount = 1;
                    _displayBrightness = 20;
                }
                BrightnessContrast();
                _brightnessText.Text = _displayBrightness.ToString();
            }
        }

        private void ContrastDownButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (_contrastAmount > -1)
            {
                _contrastAmount -= 0.05f;
                _displayContrast -= 1;
                if (_contrastAmount < -1)
                {
                    _contrastAmount = -1;
                    _displayContrast = -20;
                }
                BrightnessContrast();
                _contrastText.Text = _displayContrast.ToString();
            }
        }

        private void ContrastUpButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (_contrastAmount < 1)
            {
                _contrastAmount += 0.05f;
                _displayContrast += 1;
                if (_contrastAmount > 1)
                {
                    _contrastAmount = 1;
                    _displayContrast = 20;
                }
                BrightnessContrast();
                _contrastText.Text = _displayContrast.ToString();
            }
        }

        private void AutoLevelsButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (_brightnessAmount == 0 && _contrastAmount == 0)
            {
                if ((bool)_autoLevelsCheckBox.IsChecked)
                    _frame.Photo.Source = PhotoItem.CreateBitmapSource(_autoLevelsBitmap);
                else
                    _frame.Photo.Source = PhotoItem.CreateBitmapSource(_previewBitmap);

                _applyColorButton.IsEnabled = true;
            }
            else
                BrightnessContrast();
        }

        private void ApplyColorButtonClickHandler(object sender, RoutedEventArgs e)
        {
            var colorCorrectionTransform = new ColorCorrectionTransform((bool)_autoLevelsCheckBox.IsChecked, _brightnessAmount, _contrastAmount);

            colorCorrectionTransform.Apply(_previewBitmap);

            if (_tmpBitmap != null)
            {
                _tmpBitmap.Dispose();
                _tmpBitmap = null;
            }
            if (_autoLevelsBitmap != null)
            {
                _autoLevelsBitmap.Dispose();
                _autoLevelsBitmap = null;
            }

            RefreshPreview();
            RegisterTransform(colorCorrectionTransform);

            SwitchPanel(_editorButtonPanel);
        }

        private void CancelColorButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (_tmpBitmap != null)
            {
                _tmpBitmap.Dispose();
                _tmpBitmap = null;
            }
            if (_autoLevelsBitmap != null)
            {
                _autoLevelsBitmap.Dispose();
                _autoLevelsBitmap = null;
            }

            RefreshPreview();

            SwitchPanel(_editorButtonPanel);
        }

        private void BrightnessContrast()
        {
            if ((bool)_autoLevelsCheckBox.IsChecked)
                _tmpBitmap = (Aurigma.GraphicsMill.Bitmap)_autoLevelsBitmap.Clone();
            else
                _tmpBitmap = (Aurigma.GraphicsMill.Bitmap)_previewBitmap.Clone();

            _tmpBitmap.ColorAdjustment.BrightnessContrast(_brightnessAmount, _contrastAmount);
            _frame.Photo.Source = PhotoItem.CreateBitmapSource(_tmpBitmap);

            _applyColorButton.IsEnabled = true;
        }

        #endregion [Private color correction panel methods and event handlers]

        #region [Private effects panel methods and event handlers]

        private void BlackWhiteEffectButtonClickHandler(object sender, RoutedEventArgs e)
        {
            _tmpBitmap = (Aurigma.GraphicsMill.Bitmap)_previewBitmap.Clone();
            _tmpBitmap.ColorAdjustment.Desaturate();
            _frame.Photo.Source = PhotoItem.CreateBitmapSource(_tmpBitmap);

            _applyEffectButton.IsEnabled = true;
            _effect = EffectTransforms.BlackAndWhite;
        }

        private void SepiaButtonClickHandler(object sender, RoutedEventArgs e)
        {
            _tmpBitmap = (Aurigma.GraphicsMill.Bitmap)_previewBitmap.Clone();
            _tmpBitmap.ColorAdjustment.Desaturate();
            Single[] objColorBalance = { 0F, 0.05F, 0.1F, 0F };
            _tmpBitmap.ColorAdjustment.ChannelBalance(objColorBalance);
            _frame.Photo.Source = PhotoItem.CreateBitmapSource(_tmpBitmap);

            _applyEffectButton.IsEnabled = true;
            _effect = EffectTransforms.Sepia;
        }

        private void ApplyEffectButtonClickHandler(object sender, RoutedEventArgs e)
        {
            EffectTransform effectTransform = new EffectTransform(_effect);
            effectTransform.Apply(_previewBitmap);

            RefreshPreview();
            RegisterTransform(effectTransform);

            if (_tmpBitmap != null)
            {
                _tmpBitmap.Dispose();
                _tmpBitmap = null;
            }

            SwitchPanel(_editorButtonPanel);
        }

        private void CancelEffectButtonClickHandler(object sender, RoutedEventArgs e)
        {
            _effect = EffectTransforms.None;
            RefreshPreview();

            if (_tmpBitmap != null)
            {
                _tmpBitmap.Dispose();
                _tmpBitmap = null;
            }

            SwitchPanel(_editorButtonPanel);
        }

        #endregion [Private effects panel methods and event handlers]

        #region [Private red eye removal panel methods and event handlers]

        private void NextRedEyeButtonClickHandler(object sender, RoutedEventArgs e)
        {
            // The first step. Auto red eye removal
            if (_redEyeRemovalStep == 0)
            {
                _redEyeBitmap = (Aurigma.GraphicsMill.Bitmap)_sourceBitmap.Clone();
                if (!TransformBitmap(_redEyeBitmap))
                    return;

                _redEyeBitmap.UndoRedoEnabled = true;

                float x = (float)(_rectangleAdorner.Rectangle.X * (_redEyeBitmap.Width / _frame.Photo.ActualWidth));
                float y = (float)(_rectangleAdorner.Rectangle.Y * (_redEyeBitmap.Height / _frame.Photo.ActualHeight));
                float width = (float)(_rectangleAdorner.Rectangle.Width * (_redEyeBitmap.Width / _frame.Photo.ActualWidth));
                float height = (float)(_rectangleAdorner.Rectangle.Height * (_redEyeBitmap.Height / _frame.Photo.ActualHeight));
                _redEyeRect = new System.Drawing.RectangleF(x, y, width, height);

                _redEyeBitmap.Transforms.Crop(_redEyeRect);

                _progressDialog = new ProgressDialog((string)ExecutionEngine.Instance.Resource[Constants.MessageTransformImageKey]);

                var workerThread = new Thread(new ParameterizedThreadStart(DoAutoRedEye));
                workerThread.Start((object)_redEyeBitmap);

                DateTime showDialogTime = DateTime.Now.AddSeconds(Constants.ProgressDialogTimeout);
                while (DateTime.Now.Second < showDialogTime.Second)
                {
                    if (_progressDialog.IsComplete)
                        break;
                }

                if (!_progressDialog.IsComplete)
                {
                    _progressDialog.ShowDialog();
                    if (!_progressDialog.DialogResult.Value)
                        return;
                }

                _redEyeTransform.Transforms.Add(new AutoRedEyeRemoval(_redEyeRect));
                RegisterTransform(_redEyeTransform);

                _frame.Photo.Source = PhotoItem.CreateBitmapSource(CreatePreviewBitmap(_redEyeBitmap, Aurigma.GraphicsMill.Transforms.ResizeMode.Fit));
                _rectangleAdorner.Visibility = Visibility.Collapsed;
                _pointAdorner.Visibility = Visibility.Collapsed;
                _manualRedEyeButton.Visibility = Visibility.Visible;
                _applyRedEyeButton.IsEnabled = true;
                _nextRedEyeButton.Visibility = Visibility.Collapsed;
                _removeRedEyeButton.Visibility = Visibility.Collapsed;

                _stepDescription.Text = (string)FindResource(Constants.ImageEditorRedEyeStep2TextKey);

                _redEyeRemovalStep = 1;
            }
            else
                throw new System.Exception("Internal state error. NextRedEyeButtonClickHandler");
        }

        private void DoAutoRedEye(object objBitmap)
        {
            _progressDialog.SetRange(0, 100);
            Aurigma.GraphicsMill.Bitmap bitmap = (Aurigma.GraphicsMill.Bitmap)objBitmap;
            using (var autoModeTransform = new Aurigma.GraphicsMill.Transforms.RedEyeRemoval())
            {
                autoModeTransform.Mode = Aurigma.GraphicsMill.Transforms.RedEyeRemovalMode.Semiautomatic;
                autoModeTransform.Progress += new Aurigma.GraphicsMill.ProgressEventHandler(TransformProgressEventHandler);
                autoModeTransform.ApplyTransform(bitmap);
                autoModeTransform.Progress -= new Aurigma.GraphicsMill.ProgressEventHandler(TransformProgressEventHandler);
            }

            if (_progressDialog.IsAborted)
                _progressDialog.IsComplete = false;
            else
                _progressDialog.IsComplete = true;

            _progressDialog.End();
        }

        private void ApplyRedEyeButtonClickHandler(object sender, RoutedEventArgs e)
        {
            // The second step. Save auto red-eye removal result.
            if (_redEyeRemovalStep == 1)
            {
                _redEyeBitmap = (Aurigma.GraphicsMill.Bitmap)_sourceBitmap.Clone();
                if (!TransformBitmap(_redEyeBitmap))
                    return;

                _previewBitmap.Create(CreatePreviewBitmap(_redEyeBitmap, Aurigma.GraphicsMill.Transforms.ResizeMode.Shrink));
                RefreshPreview();

                SwitchPanel(_editorButtonPanel);
                if (_redEyeBitmap != null)
                {
                    _redEyeBitmap.Dispose();
                    _redEyeBitmap = null;
                }
                _stepDescription.Text = (string)FindResource(Constants.ImageEditorRedEyeStep1TextKey);
                _redEyeRemovalStep = 0;
            }

            // The third step. Save manual red-eye removal result.
            if (_redEyeRemovalStep == 2)
            {
                _redEyeTransform.Transforms.RemoveRange(_manualRedEyeIndex, _redEyeTransform.Transforms.Count - _manualRedEyeIndex);
                RegisterTransform(_redEyeTransform);

                _redEyeBitmap = (Aurigma.GraphicsMill.Bitmap)_sourceBitmap.Clone();
                if (!TransformBitmap(_redEyeBitmap))
                    return;

                _previewBitmap.Create(CreatePreviewBitmap(_redEyeBitmap, Aurigma.GraphicsMill.Transforms.ResizeMode.Shrink));
                RefreshPreview();

                SwitchPanel(_editorButtonPanel);
                if (_redEyeBitmap != null)
                {
                    _redEyeBitmap.Dispose();
                    _redEyeBitmap = null;
                }
                _pointAdorner.Visibility = Visibility.Collapsed;
                _frame.Photo.MouseLeftButtonDown -= new MouseButtonEventHandler(ManualRedEyeClickHandler);
                _stepDescription.Text = (string)FindResource(Constants.ImageEditorRedEyeStep1TextKey);
                _redEyeRemovalStep = 0;
            }
        }

        private void ManualRedEyeButtonClickHandler(object sende, RoutedEventArgs e)
        {
            _pointAdorner.Visibility = Visibility.Visible;
            _pointAdorner.Point = new Point(_frame.Photo.ActualWidth / 2, _frame.Photo.ActualHeight / 2);
            _removeRedEyeButton.Visibility = Visibility.Visible;
            _manualRedEyeButton.Visibility = Visibility.Collapsed;
            _nextRedEyeButton.Visibility = Visibility.Collapsed;

            _applyRedEyeButton.IsEnabled = false;

            _redEyeBitmap.Undo();
            _transformIndex--;
            _redEyeBitmap.ClearHistory();
            _redEyeTransform.Transforms.Clear();
            _manualRedEyeIndex = 0;

            _undoRedEyeButton.IsEnabled = _redEyeBitmap.CanUndo;
            _redoRedEyeButton.IsEnabled = _redEyeBitmap.CanRedo;

            _stepDescription.Text = (string)FindResource(Constants.ImageEditorRedEyeStep3TextKey);

            _frame.Photo.MouseLeftButtonDown += new MouseButtonEventHandler(ManualRedEyeClickHandler);

            _frame.Photo.Source = PhotoItem.CreateBitmapSource(CreatePreviewBitmap(_redEyeBitmap, Aurigma.GraphicsMill.Transforms.ResizeMode.Fit));
            _redEyeRemovalStep = 2;
        }

        private void ManualRedEyeClickHandler(object sender, MouseButtonEventArgs e)
        {
            if (_redEyeRemovalStep == 2)
            {
                _pointAdorner.Visibility = Visibility.Visible;
                _pointAdorner.Point = e.GetPosition(_frame.Photo);

                _stepDescription.Text = (string)FindResource(Constants.ImageEditorRedEyeStep3TextKey);
            }
            else
                throw new System.Exception("Internal state error. ManualRedEyeClickHandler");
        }

        private void RemoveRedEyeButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (_redEyeRemovalStep == 2)
            {
                float x = (float)(_pointAdorner.Point.X * (_redEyeBitmap.Width / _frame.Photo.ActualWidth));
                float y = (float)(_pointAdorner.Point.Y * (_redEyeBitmap.Height / _frame.Photo.ActualHeight));

                var redEyePoint = new System.Drawing.PointF(x, y);

                if (_manualRedEyeIndex < _redEyeTransform.Transforms.Count)
                    _redEyeTransform.Transforms[_manualRedEyeIndex] = new ManualRedEyeRemoval(_redEyeRect, redEyePoint);
                else
                    _redEyeTransform.Transforms.Add(new ManualRedEyeRemoval(_redEyeRect, redEyePoint));
                _manualRedEyeIndex++;

                using (var manualModeTransform = new Aurigma.GraphicsMill.Transforms.RedEyeRemoval())
                {
                    manualModeTransform.Mode = Aurigma.GraphicsMill.Transforms.RedEyeRemovalMode.Manual;
                    manualModeTransform.EyePoint = redEyePoint;
                    manualModeTransform.ApplyTransform(_redEyeBitmap);
                }

                _frame.Photo.Source = PhotoItem.CreateBitmapSource(CreatePreviewBitmap(_redEyeBitmap, Aurigma.GraphicsMill.Transforms.ResizeMode.Fit));

                _undoRedEyeButton.IsEnabled = _redEyeBitmap.CanUndo;
                _redoRedEyeButton.IsEnabled = _redEyeBitmap.CanRedo;

                _stepDescription.Text = (string)FindResource(Constants.ImageEditorRedEyeStep3TextKey);
                _applyRedEyeButton.IsEnabled = true;
            }
            else
                throw new System.Exception("Internal state error. RemoveRedEyeButtonClickHandler.");
        }

        private void UndoRedEyeButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (_redEyeRemovalStep == 2)
            {
                if (_manualRedEyeIndex > 0)
                {
                    _manualRedEyeIndex--;
                    _redEyeBitmap.Undo();
                    _frame.Photo.Source = PhotoItem.CreateBitmapSource(CreatePreviewBitmap(_redEyeBitmap, Aurigma.GraphicsMill.Transforms.ResizeMode.Fit));
                    _undoRedEyeButton.IsEnabled = _redEyeBitmap.CanUndo;
                    _redoRedEyeButton.IsEnabled = _redEyeBitmap.CanRedo;

                    _stepDescription.Text = (string)FindResource(Constants.ImageEditorRedEyeStep3TextKey);

                    if (_manualRedEyeIndex == 0)
                        _applyRedEyeButton.IsEnabled = false;
                }
            }
            else
                throw new System.Exception("Internal state error. UndoRedEyeButtonClickHandler.");
        }

        private void RedoRedEyeButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (_redEyeRemovalStep == 2)
            {
                if (_manualRedEyeIndex < _redEyeTransform.Transforms.Count)
                {
                    _manualRedEyeIndex++;
                    _redEyeBitmap.Redo();
                    _frame.Photo.Source = PhotoItem.CreateBitmapSource(CreatePreviewBitmap(_redEyeBitmap, Aurigma.GraphicsMill.Transforms.ResizeMode.Fit));
                    _undoRedEyeButton.IsEnabled = _redEyeBitmap.CanUndo;
                    _redoRedEyeButton.IsEnabled = _redEyeBitmap.CanRedo;

                    _stepDescription.Text = (string)FindResource(Constants.ImageEditorRedEyeStep3TextKey);

                    if (_manualRedEyeIndex > 0)
                        _applyRedEyeButton.IsEnabled = true;
                }
            }
            else
                throw new Exception("Internal state error. RedoRedEyeButtonClickHandler.");
        }

        private void CancelRedEyeButtonClickHandler(object sender, RoutedEventArgs e)
        {
            _rectangleAdorner.Visibility = Visibility.Collapsed;
            _pointAdorner.Visibility = Visibility.Collapsed;
            SwitchPanel(_editorButtonPanel);
            RefreshPreview();

            if (_redEyeBitmap != null)
            {
                _redEyeBitmap.Dispose();
                _redEyeBitmap = null;
            }

            if (_redEyeRemovalStep == 1)
            {
                _transformIndex--;
            }
            if (_redEyeRemovalStep == 2)
            {
                _frame.Photo.MouseLeftButtonDown -= new MouseButtonEventHandler(ManualRedEyeClickHandler);
            }

            _redEyeRemovalStep = 0;
            _stepDescription.Text = (string)FindResource(Constants.ImageEditorRedEyeStep1TextKey);
        }

        #endregion [Private red eye removal panel methods and event handlers]

        #region [Public methods and properties]

        public void SetPhoto(ThumbnailItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            _listItem = item;
            _result = (PhotoItem)item.Photo.Clone();
            _sourceItem = item.Photo;
            _sourceBitmap = _result.Image;
            _frame.OrderItem = item.Order;

            if (_sourceBitmap.PixelFormat != GraphicsMill.PixelFormat.Format32bppArgb &&
                _sourceBitmap.PixelFormat != GraphicsMill.PixelFormat.Format24bppRgb &&
                _sourceBitmap.PixelFormat != GraphicsMill.PixelFormat.Format32bppRgb)
                _sourceBitmap.ColorManagement.Convert(GraphicsMill.PixelFormat.Format32bppArgb);

            _sourceBitmap.UndoRedoEnabled = true;

            _filename.Text = _sourceItem.SourceFileNameWithoutPath;
            _prevPhotoButton.IsEnabled = _listItem.HasPrevious();
            _nextPhotoButton.IsEnabled = _listItem.HasNext();
        }

        public PhotoItem Result
        {
            get { return _result; }
        }

        #endregion [Public methods and properties]

        #region [Variables]

        private RectangleAdorner _rectangleAdorner;
        private PointAdorner _pointAdorner;

        private PhotoItem _result;
        private PhotoItem _sourceItem;
        private ThumbnailItem _listItem;

        private float _brightnessAmount;
        private float _contrastAmount;
        private int _displayBrightness;
        private int _displayContrast;

        private Aurigma.GraphicsMill.Bitmap _sourceBitmap;
        private Aurigma.GraphicsMill.Bitmap _previewBitmap;
        private Aurigma.GraphicsMill.Bitmap _cropBitmap;
        private Aurigma.GraphicsMill.Bitmap _tmpBitmap;
        private Aurigma.GraphicsMill.Bitmap _autoLevelsBitmap;
        private Aurigma.GraphicsMill.Bitmap _redEyeBitmap;

        private List<Transform> _transforms;
        private RedEyeRemovalTransform _redEyeTransform;

        private int _transformIndex;
        private int _manualRedEyeIndex;

        private EffectTransforms _effect;

        private int _redEyeRemovalStep;
        private System.Drawing.RectangleF _redEyeRect;

        private ImageEditorStage _imageEditorStage;
        private ProgressDialog _progressDialog;

        #endregion [Variables]
    }

    public enum ImageEditorMode
    {
        View,
        Edit,
        Crop
    }
}