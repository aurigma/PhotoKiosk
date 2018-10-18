// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Aurigma.PhotoKiosk
{
    public partial class ProgressScreen : Page
    {
        private FindingPhotosStage _stage;

        public ProgressScreen()
        {
            if (ExecutionEngine.Instance != null)
                Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);

            InitializeComponent();
        }

        public ProgressScreen(FindingPhotosStage stage)
            : this()
        {
            _stage = stage;

            _progressBar.Minimum = 0;
            _progressBar.Maximum = (double)ExecutionEngine.Config.SearchProcessDuration.Value / (double)Constants.FindingPhotosTimerInterval + 1;
        }

        public void AddPreviewPhoto(ImageSource previewPhoto)
        {
            if (previewPhoto == null)
                throw new ArgumentNullException("previewPhoto");

            var newImage = new Image();
            newImage.Source = previewPhoto;
            newImage.Width = previewPhoto.Width;
            newImage.Height = previewPhoto.Height;

            ApplyAnimationToImage(newImage);

            ExecutionEngine.Instance.PhotosCanvas.Children.Add(newImage);
        }

        private void ProcessScreenLoadedHandler(object sender, RoutedEventArgs e)
        {
            _stage.StartFindPhotos();
            ExecutionEngine.Instance.PhotosCanvas.Visibility = Visibility.Visible;
        }

        private void ProcessScreenUnloadedHandler(object sender, RoutedEventArgs e)
        {
            ExecutionEngine.Instance.PhotosCanvas.Visibility = Visibility.Hidden;
        }

        // It's difficult to animate from style image opacity and TranslateTransform both, so animation is implemented here.
        private void ApplyAnimationToImage(Image image)
        {
            if (image == null)
            {
                ExecutionEngine.EventLogger.Write("ProgressScreen:ApplyAnimationToImage. image == null");
                throw new ArgumentNullException("image is null");
            }

            if (ExecutionEngine.Config.SearchProcessDuration.Value < 3000)
                ExecutionEngine.EventLogger.Write("FindingPhotosDuration is recommended to be more than 3000 ms");

            var translateTransform = new TranslateTransform(0, ExecutionEngine.Instance.PhotosCanvas.ActualHeight / 2 - image.Height / 2);

            var moveImage = new DoubleAnimation();
            moveImage.Duration = new Duration(TimeSpan.FromMilliseconds(Constants.FindingPhotosMoveImageDuration));
            moveImage.From = 0;
            moveImage.To = ExecutionEngine.Instance.PhotosCanvas.ActualWidth - image.Width;

            var opacityAnimation = new DoubleAnimationUsingKeyFrames();

            opacityAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(0.0, TimeSpan.FromMilliseconds(0)));
            opacityAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(1.0, TimeSpan.FromMilliseconds(1000)));
            opacityAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(1.0, TimeSpan.FromMilliseconds(Constants.FindingPhotosMoveImageDuration - 1000)));
            opacityAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(0.0, TimeSpan.FromMilliseconds(Constants.FindingPhotosMoveImageDuration)));

            image.RenderTransform = translateTransform;
            translateTransform.BeginAnimation(TranslateTransform.XProperty, moveImage);
            image.BeginAnimation(Image.OpacityProperty, opacityAnimation);
        }
    }
}