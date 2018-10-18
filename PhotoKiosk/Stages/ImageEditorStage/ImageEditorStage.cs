// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;

namespace Aurigma.PhotoKiosk
{
    public class ImageEditorStage : StageBase
    {
        #region [Constructors]

        public ImageEditorStage()
            : base(Constants.ImageEditorStageName, ExecutionEngine.Config.InactivityTimeout.Value)
        {
            _screen = new ImageEditorScreen(this);
            LastVisitedPage = _screen;
        }

        #endregion [Constructors]

        #region [Public overriden methods]

        public override void Activate(ExecutionEngine engine)
        {
            if (ExecutionEngine.Context.Contains(Constants.EditPhotoContextName))
                _screen.SetPhoto(ExecutionEngine.Context[Constants.EditPhotoContextName] as ThumbnailItem);

            base.Activate(engine);

            ExecutionEngine.EventLogger.Write("ImageEditorStage:Activate");
        }

        public override void Reset()
        {
            ExecutionEngine.EventLogger.Write("ImageEditorStage:Reset");
        }

        #endregion [Public overriden methods]

        #region [Public methods and props]

        public void SaveEditedAsNew()
        {
            ExecutionEngine.Context[Constants.ImageEditorResultKey] = _screen.Result;
            Engine.ExecuteCommand(new SwitchToStageCommand(Constants.SelectStageName));
        }

        #endregion [Public methods and props]

        #region Variables

        private ImageEditorScreen _screen;

        #endregion Variables
    }
}