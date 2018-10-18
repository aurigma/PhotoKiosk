// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.IO;

namespace Aurigma.PhotoKiosk
{
    public class WelcomeStage : StageBase
    {
        #region [Constructors]

        public WelcomeStage()
            : base(Constants.WelcomeStageName, Constants.WelcomeStageInactiveTime)
        {
            _welcomeScreen = new WelcomeScreen(this);
            LastVisitedPage = _welcomeScreen;

            ExecutionEngine.EventLogger.Write("WelcomeStage created");
        }

        #endregion [Constructors]

        #region [Public overriden methods]

        public override void Activate(ExecutionEngine engine)
        {
            base.Activate(engine);
        }

        public override void Reset()
        {
            LastVisitedPage = _welcomeScreen;
        }

        #endregion [Public overriden methods]

        #region [Public methods and props]

        public void SwitchToWelcomeScreen()
        {
            LastVisitedPage = _welcomeScreen;
            Engine.ExecuteCommand(new SwitchToScreenCommand(_welcomeScreen));
        }

        public void SwitchToFindingPhotosStage()
        {
            try
            {
                string tmpRoot = Path.GetPathRoot(Path.GetTempPath());
                string appRoot = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

                if (new DriveInfo(tmpRoot).AvailableFreeSpace < (long)ExecutionEngine.Config.MemoryCacheSize.Value * 1024 * 1024 * 2)
                {
                    MessageDialog.Show(string.Format(StringResources.GetString("MessageLowMemory"), tmpRoot));
                    return;
                }

                if (new DriveInfo(appRoot).AvailableFreeSpace < (long)1 /*GB*/ * 1024 * 1024 * 1024)
                {
                    MessageDialog.Show(string.Format(StringResources.GetString("MessageLowMemory"), appRoot));
                    return;
                }
            }
            catch (Exception e)
            {
                ExecutionEngine.EventLogger.Write("Unable to estimate available disk space");
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
            }

            Order currentOrder = (Order)ExecutionEngine.Context[Constants.OrderContextName];
            currentOrder.OrderPaperType = ExecutionEngine.Instance.PaperTypes.Count > 0 ? ExecutionEngine.Instance.PaperTypes[0] : null;

            LastVisitedPage = _welcomeScreen;
            ExecutionEngine.RunStartOrderProcess();
            Engine.ExecuteCommand(new SwitchToStageCommand(Constants.FindingPhotosStageName));
        }

        public void SwitchToProcessOrderStage()
        {
            LastVisitedPage = _welcomeScreen;
            Engine.ExecuteCommand(new SwitchToStageCommand(Constants.ProcessStageName));
        }

        #endregion [Public methods and props]

        #region [Variables]

        private WelcomeScreen _welcomeScreen;

        #endregion [Variables]
    }
}