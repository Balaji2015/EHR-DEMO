using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [DataContract]
    public partial class PQRI_Measure : BusinessBase<int>
    {
        #region Decleration

        private int _CQM_Measure_ID = 0;
        private string _Measurement_Name = string.Empty;
        private string _Description = string.Empty;
        private string _Numerator = string.Empty;
        private string _Denominator = string.Empty;
        private string _Exclusion = string.Empty;
        private string _Measurement_No = string.Empty;
        private int _Sort_Order = 0;
        //added by vince 2013-06-03//
        private string _Percentage = string.Empty;
        private string _RequiredPercentage = string.Empty;
        private string _Cleared = string.Empty;
        private string _InitialPatientPopulation = string.Empty;
        private string _DenominatorExclusion = string.Empty;
        private string _DenominatorException = string.Empty;
        private IList<string[]> _ICDCPTNumeratorList = null;
        private IList<string[]> _ICDCPTDenominatorList = null;
        private IList<string[]> _ICDCPTDenominatorExceptionList = null;
        private IList<string[]> _ICDCPTDenominatorExclusionList = null;
        private int _Measurement_Year = 0;
        private int _CQM_Summary_ID = 0;
        private string _NumeratorExclusion = string.Empty;

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_CQM_Measure_ID);
            sb.Append(_Measurement_Name);
            sb.Append(_Description);
            sb.Append(_Numerator);
            sb.Append(_Denominator);
            sb.Append(_Exclusion);
            sb.Append(_Measurement_No);
            sb.Append(_Sort_Order);
            sb.Append(_InitialPatientPopulation);
            sb.Append(_DenominatorExclusion);
            sb.Append(_DenominatorException);
            sb.Append(_ICDCPTNumeratorList);
            sb.Append(_ICDCPTDenominatorList);
            sb.Append(_ICDCPTDenominatorExceptionList);
            sb.Append(_ICDCPTDenominatorExclusionList);
            sb.Append(_CQM_Summary_ID);

            sb.Append(_Measurement_Year);
            sb.Append(_NumeratorExclusion);

            return sb.ToString().GetHashCode();
        }
        #endregion

        #region Implementation
        [DataMember]
        public virtual int CQM_Measure_ID
        {
            get { return _CQM_Measure_ID; }
            set { _CQM_Measure_ID = value; }
        }

        [DataMember]
        public virtual string Measurement_Name
        {
            get { return _Measurement_Name; }
            set { _Measurement_Name = value; }
        }
        [DataMember]
        public virtual string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        [DataMember]
        public virtual string Numerator
        {
            get { return _Numerator; }
            set { _Numerator = value; }
        }
        [DataMember]
        public virtual string Denominator
        {
            get { return _Denominator; }
            set { _Denominator = value; }
        }
        [DataMember]
        public virtual string Exclusion
        {
            get { return _Exclusion; }
            set { _Exclusion = value; }
        }
        [DataMember]
        public virtual string Measurement_No
        {
            get { return _Measurement_No; }
            set { _Measurement_No = value; }
        }
        [DataMember]
        public virtual int Sort_Order
        {
            get { return _Sort_Order; }
            set { _Sort_Order = value; }
        }
        [DataMember]
        public virtual string Percentage
        {
            get { return _Percentage; }
            set { _Percentage = value; }
        }
        [DataMember]
        public virtual string RequiredPercentage
        {
            get { return _RequiredPercentage; }
            set { _RequiredPercentage = value; }
        }
        [DataMember]
        public virtual string Cleared
        {
            get { return _Cleared; }
            set { _Cleared = value; }
        }
        [DataMember]
        public virtual string InitialPatientPopulation
        {
            get { return _InitialPatientPopulation; }
            set { _InitialPatientPopulation = value; }
        }
        [DataMember]
        public virtual string DenominatorExclusion
        {
            get { return _DenominatorExclusion; }
            set { _DenominatorExclusion = value; }
        }
        [DataMember]
        public virtual string DenominatorException
        {
            get { return _DenominatorException; }
            set { _DenominatorException = value; }
        }

        [DataMember]
        public virtual IList<string[]> ICDCPTNumeratorList
        {
            get { return _ICDCPTNumeratorList; }
            set { _ICDCPTNumeratorList = value; }
        }

        [DataMember]
        public virtual IList<string[]> ICDCPTDenominatorList
        {
            get { return _ICDCPTDenominatorList; }
            set { _ICDCPTDenominatorList = value; }
        }

        [DataMember]
        public virtual IList<string[]> ICDCPTDenominatorExceptionList
        {
            get { return _ICDCPTDenominatorExceptionList; }
            set { _ICDCPTDenominatorExceptionList = value; }
        }

        [DataMember]
        public virtual IList<string[]> ICDCPTDenominatorExclusionList
        {
            get { return _ICDCPTDenominatorExclusionList; }
            set { _ICDCPTDenominatorExclusionList = value; }
        }
        [DataMember]
        public virtual int Measurement_Year
        {
            get { return _Measurement_Year; }
            set { _Measurement_Year = value; }
        }

        [DataMember]
        public virtual int CQM_Summary_ID
        {
            get { return _CQM_Summary_ID; }
            set { _CQM_Summary_ID = value; }
        }

        [DataMember]
        public virtual string NumeratorExclusion
        {
            get { return _NumeratorExclusion; }
            set { _NumeratorExclusion = value; }
        }

        #endregion
    }
}
