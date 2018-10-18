namespace ODS.Service.DPOF
{
    /// <summary>
    /// Specifies the paper type and size of a print. A single parameter may be indicated for each Job Section
    /// ([JOB]). The paper type in Version 1.10 is normal paper only. The PCD (for Post Card) and the STK
    /// (for Sticker) are reserved for expansion use.
    /// For the normal paper, the size may be specified with the parameter auxiliary value 1 & 2. However, in
    /// Version 1.10 only DFL (i.e. DPOF Printer’s Device Dependent) has been defined.
    /// If the output DPOF Printer is not identified, there is a high possibility that the setting will not be
    /// reflected on the output prints and refraining from the usage is recommended.
    /// </summary>
    public class PrtPsl : DpofParameter
    {
        public override string Prefix
        {
            get { return "PRT"; }
        }

        public override string Suffix
        {
            get { return "PSL"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Optional; }
        }

        public override string Value
        {
            get { return "NML"; }
        }
    }
}