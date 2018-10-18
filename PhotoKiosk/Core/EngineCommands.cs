// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Windows.Controls;

namespace Aurigma.PhotoKiosk
{
    public abstract class EngineCommandBase
    {
        protected EngineCommandBase(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            _name = name;
        }

        private string _name;

        public string Name
        {
            get { return _name; }
        }
    }

    public class SwitchToStageCommand : EngineCommandBase
    {
        public SwitchToStageCommand(string stage)
            : base(Constants.SwitchToStage)
        {
            _stageName = stage;
        }

        private string _stageName;

        public string StageName
        {
            get { return _stageName; }
        }
    }

    public class SwitchToScreenCommand : EngineCommandBase
    {
        public SwitchToScreenCommand(Page page)
            : base(Constants.SwitchToScreen)
        {
            _page = page;
        }

        private Page _page;

        public Page Page
        {
            get { return _page; }
        }
    }

    public class ResetOrderDataCommand : EngineCommandBase
    {
        public ResetOrderDataCommand()
            : base(Constants.ResetOrderData)
        {
            FileOrderStorage.ClearBluetoothDirectory();
        }
    }
}