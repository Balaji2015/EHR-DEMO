using System;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [DataContract]
    public partial class CQMDetail : BusinessBase<ulong>
    {
        #region Declarations


        //  private string _Examination_Details = string.Empty;
        private ulong _CQM_Summary_ID = 0;
        private string _Population_Set = string.Empty;
        private ulong _Human_ID = 0;
        private ulong _Encounter_ID = 0;
         private string _ICD = string.Empty;
        private string _Procedure_Code = string.Empty;
        private string _Snomed_Code = string.Empty;
        private string _Loinc_Code = string.Empty;
        private string _Loinc_Identifier = string.Empty;
        private DateTime _Documented_Date_Time = DateTime.MinValue;
        #endregion

        #region Constructors

        public CQMDetail() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_CQM_Summary_ID);
            sb.Append(_Population_Set);
            sb.Append(_Human_ID);
            sb.Append(_Encounter_ID);
            sb.Append(_ICD);
            sb.Append(_Procedure_Code);
            sb.Append(_Snomed_Code);
            sb.Append(_Loinc_Code);
            sb.Append(_Loinc_Identifier);
            sb.Append(_Documented_Date_Time);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        [DataMember]
        public virtual ulong CQM_Summary_ID
        {
            get { return _CQM_Summary_ID; }
            set
            {
                _CQM_Summary_ID = value;
            }
        }
        [DataMember]
        public virtual string Population_Set
        {
            get { return _Population_Set; }
            set
            {
                _Population_Set = value;
            }
        }
        [DataMember]
        //public virtual string Examination_Details
        //{
        //    get { return _Examination_Details; }
        //    set
        //    {
        //        _Examination_Details = value;
        //    }
        //}
        //[DataMember]
        public virtual ulong Human_ID
        {
            get { return _Human_ID; }
            set
            {
                _Human_ID = value;
            }
        }
        [DataMember]
        public virtual ulong Encounter_ID
        {
            get { return _Encounter_ID; }
            set
            {
                _Encounter_ID = value;
            }
        }
        
       

        [DataMember]
        public virtual string ICD
        {
            get { return _ICD; }
            set
            {
                _ICD = value;
            }
        }
        [DataMember]
        public virtual string Procedure_Code
        {
            get { return _Procedure_Code; }
            set
            {
                _Procedure_Code = value;
            }
        }
        [DataMember]
        public virtual string Snomed_Code
        {
            get { return _Snomed_Code; }
            set
            {
                _Snomed_Code = value;
            }
        }
        [DataMember]
        public virtual string Loinc_Code
        {
            get { return _Loinc_Code; }
            set
            {
                _Loinc_Code = value;
            }
        }
        [DataMember]
        public virtual string Loinc_Identifier
        {
            get { return _Loinc_Identifier; }
            set
            {
                _Loinc_Identifier = value;
            }
        }

        [DataMember]
        public virtual DateTime Documented_Date_Time
        {
            get { return _Documented_Date_Time; }
            set
            {
                _Documented_Date_Time = value;
            }
        }
        #endregion



    }
}