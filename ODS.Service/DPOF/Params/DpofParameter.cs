// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
namespace ODS.Service.DPOF
{
    public abstract class DpofParameter
    {
        public abstract string Prefix { get; }
        public abstract string Suffix { get; }
        public abstract IndicationLevel IndicationLevel { get; }
        public abstract string Value { get; }

        public virtual string AuxiliaryValue1 { get { return string.Empty; } }
        public virtual string AuxiliaryValue2 { get { return string.Empty; } }

        public virtual string Render()
        {
            if (string.IsNullOrEmpty(AuxiliaryValue1) && string.IsNullOrEmpty(AuxiliaryValue2))
                return string.Format("{0} {1} = {2}", Prefix, Suffix, Value);
            return string.Format("{0} {1} = {2} -{3} {4}", Prefix, Suffix, Value, AuxiliaryValue1, AuxiliaryValue2);
        }
    }

    public abstract class DpofConditionalParameter : DpofParameter
    {
        public override sealed string Render()
        {
            if (string.IsNullOrEmpty(AuxiliaryValue1) && string.IsNullOrEmpty(AuxiliaryValue2))
                return string.Format("<{0} {1} = {2}>", Prefix, Suffix, Value);
            return string.Format("<{0} {1} = {2} -{3} {4}>", Prefix, Suffix, Value, AuxiliaryValue1, AuxiliaryValue2);
        }
    }
}