using System;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [DataContract]
    public partial class Healthcare_Questionnaire : BusinessBase<ulong>
    {
        #region Declarations

        //private ulong _HealthCare_Questionnaire_ID=0;
        private ulong _Encounter_ID=0;
        private ulong _Physician_ID=0;
        private ulong _Human_ID =0;
        private ulong _Questionnaire_Lookup_ID=0;
        private string _Questionnaire_Category = string.Empty;
        private string _Questionnaire_Type = string.Empty;
        private string _Question = string.Empty;
        private string _Selected_Option	 = string.Empty;
        private string _Notes = string.Empty;
        private string _createdby = string.Empty;
        private DateTime _createddateandtime=DateTime.MinValue;
        private string _modifiedby = string.Empty;
        private DateTime _modifieddateandtime=DateTime.MinValue;
        private string _Question_Loinc_Code = string.Empty;
        private string _Selected_Option_Loinc_Code = string.Empty;
        private int _version = 0;

        #endregion

        #region Constructors

        public Healthcare_Questionnaire() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            //sb.Append(_HealthCare_Questionnaire_ID);
            sb.Append(_Encounter_ID);
            sb.Append(_Physician_ID);
            sb.Append(_Human_ID);
            sb.Append(_Questionnaire_Lookup_ID);
            sb.Append(_Questionnaire_Category = string.Empty);
            sb.Append(_Questionnaire_Type = string.Empty);
            sb.Append(_Question = string.Empty);
            sb.Append(_Selected_Option = string.Empty);
            sb.Append(_Notes = string.Empty);
            sb.Append(_createdby = string.Empty);
            sb.Append(_createddateandtime);
            sb.Append(_modifieddateandtime);
            sb.Append(_version);
            sb.Append(_Question_Loinc_Code);
            sb.Append(_Selected_Option_Loinc_Code);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties


        //[DataMember]
        //public virtual ulong HealthCare_Questionnaire_ID
        //{
        //    get { return _HealthCare_Questionnaire_ID; }
        //    set
        //    {
        //        _HealthCare_Questionnaire_ID = value;
        //    }
        //}
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
        public virtual ulong Physician_ID
        {
            get { return _Physician_ID; }
            set
            {
                _Physician_ID = value;
            }
        }
        [DataMember]
        public virtual ulong Human_ID
        {
            get { return _Human_ID; }
            set
            {
                _Human_ID = value;
            }
        }
        [DataMember]
        public virtual ulong Questionnaire_Lookup_ID
        {
            get { return _Questionnaire_Lookup_ID; }
            set
            {
                _Questionnaire_Lookup_ID = value;
            }
        }
        [DataMember]
        public virtual string Questionnaire_Category
        {
            get { return _Questionnaire_Category; }
            set
            {
                _Questionnaire_Category = value;
            }
        }
        [DataMember]
        public virtual string Questionnaire_Type
        {
            get { return _Questionnaire_Type; }
            set
            {
                _Questionnaire_Type = value;
            }
        }
        [DataMember]
        public virtual string Question
        {
            get { return _Question; }
            set
            {
                _Question = value;
            }
        }
        [DataMember]
        public virtual string Selected_Option
        {
            get { return _Selected_Option; }
            set
            {
                _Selected_Option = value;
            }
        }
        [DataMember]
        public virtual string Notes
        {
            get { return _Notes; }
            set
            {
                _Notes = value;
            }
        }
        [DataMember]
        public virtual string Created_By
        {
            get { return _createdby; }
            set
            {
                _createdby = value;
            }
        }
        [DataMember]
        public virtual DateTime Created_Date_And_Time
        {
            get { return _createddateandtime; }
            set
            {
                _createddateandtime = value;
            }
        }
        [DataMember]
        public virtual string Modified_By
        {
            get { return _modifiedby; }
            set
            {
                _modifiedby = value;
            }
        }
        [DataMember]
        public virtual DateTime Modified_Date_And_Time
        {
            get { return _modifieddateandtime; }
            set
            {
                _modifieddateandtime = value;
            }
        }        
        [DataMember]
        public virtual int Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }
        [DataMember]
        public virtual string Question_Loinc_Code
        {
            get
            {
                return _Question_Loinc_Code;
            }
            set
            {
                _Question_Loinc_Code = value;
            }
        }
        [DataMember]
        public virtual string Selected_Option_Loinc_Code
        {
            get
            {
                return _Selected_Option_Loinc_Code;
            }
            set
            {
                _Selected_Option_Loinc_Code = value;
            }
        }
        #endregion
    }
}
