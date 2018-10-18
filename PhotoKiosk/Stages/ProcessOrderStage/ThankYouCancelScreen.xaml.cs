// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Windows;

namespace Aurigma.PhotoKiosk
{
    public partial class ThankYouCancelScreen : System.Windows.Controls.Page
    {
        public ThankYouCancelScreen()
        {
            if (ExecutionEngine.Instance != null)
            {
                Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);
            }

            InitializeComponent();
        }

        public ThankYouCancelScreen(Aurigma.PhotoKiosk.ProcessOrderStage stage)
            : this()
        {
            _stage = stage;
        }

        private void ButtonToStartClick(object sender, RoutedEventArgs e)
        {
            _stage.SwitchToStartStage();
        }

        private Aurigma.PhotoKiosk.ProcessOrderStage _stage;
    }
}