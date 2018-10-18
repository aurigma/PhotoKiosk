// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System.Windows;
using System.Windows.Controls;

namespace Aurigma.PhotoKiosk
{
    public class CropButton : RadioButton
    {
        public CropButton(CropFormat cropStruct, string cropTitle)
        {
            if (cropStruct.Width < 0 || cropStruct.Height <= 0)
                _cropProportion = 0;
            else
                _cropProportion = (double)cropStruct.Width / (double)cropStruct.Height;

            Style = (Style)FindResource("ImageEditorRadioButtonStyle");
            Width = 100;
            Height = 70;
            Margin = new Thickness(9, 11, 9, 11);

            StackPanel panel = new StackPanel();
            panel.VerticalAlignment = VerticalAlignment.Center;
            panel.HorizontalAlignment = HorizontalAlignment.Center;

            Aurigma.PhotoKiosk.OutlinedText cropOutliledText = new OutlinedText(cropStruct.Name, (Style)FindResource("CropParamsTextStyle"));
            cropOutliledText.HorizontalAlignment = HorizontalAlignment.Center;
            cropOutliledText.Margin = new Thickness(8, 0, 8, 0);
            panel.Children.Add(cropOutliledText);

            TextBlock cropTextBlock = new TextBlock();
            cropTextBlock.Style = (Style)FindResource("ImageEditorButtonTextStyle");
            cropTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            cropTextBlock.Text = cropTitle;
            panel.Children.Add(cropTextBlock);

            this.Content = panel;
        }

        private double _cropProportion;

        public double CropProportion
        {
            get { return _cropProportion; }
        }
    }
}