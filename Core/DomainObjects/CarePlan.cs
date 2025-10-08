using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
    public partial  class CarePlan:BusinessBase<ulong>
    {
       
         #region Declarations

        private ulong _Encounter_ID=0;
        private ulong _Human_ID=0;
        private ulong _Physician_ID=0;
        private string _Care_Name = string.Empty;
        private string _Care_Name_Value = string.Empty;
        private string _Status = string.Empty;
        private string _Care_Plan_Notes = string.Empty;
        private string _Created_By = string.Empty;
        private DateTime _Created_Date_And_Time=DateTime.MinValue;
        private string _Modified_By = string.Empty;
        private DateTime _Modified_Date_And_Time=DateTime.MinValue;
        private ulong _Care_Plan_Lookup_ID = 0;
        private int _Version = 0;
        private string _Plan_Date = string.Empty;
        private string _Status_Value =string.Empty;
        private bool _Internal_Property_bInsert = false;
        //private string _Reason_Not_Performed = string.Empty;
        //private string _Followup_Plan = string.Empty;
        private string _Snomed_Code = string.Empty;
        private string _Care_Plan_Loinc_Code = string.Empty;
        private string _Selected_Option_Loinc_Code = string.Empty;
      //  private DateTime _Captured_Date_Time = DateTime.MinValue;
        #endregion

        #region Constructors

        public CarePlan() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Encounter_ID);
            sb.Append(_Human_ID);
            sb.Append(_Physician_ID);
            sb.Append(_Care_Name);
            sb.Append(_Care_Name_Value);
            sb.Append(_Status);
            sb.Append(_Created_By);
            sb.Append(_Created_Date_And_Time);
            sb.Append(_Modified_By);
            sb.Append(_Modified_Date_And_Time);
            sb.Append(_Care_Plan_Lookup_ID);
            sb.Append(_Version);
            sb.Append(_Plan_Date);
            sb.Append(_Status_Value);
            sb.Append(_Care_Plan_Notes);
            sb.Append(_Internal_Property_bInsert);
            //sb.Append(_Reason_Not_Performed);
            //sb.Append(_Followup_Plan);
            sb.Append(_Snomed_Code);
            sb.Append(_Care_Plan_Loinc_Code);
            sb.Append(_Selected_Option_Loinc_Code);
          //  sb.Append(_Captured_Date_Time);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

     
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
        public virtual ulong Human_ID
        {
            get { return _Human_ID; }
            set
            {
                _Human_ID = value;
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
        public virtual string Care_Name
        {
            get { return _Care_Name; }
            set
            {
                _Care_Name = value;
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

        //[DataMember]
        //public virtual string Reason_Not_Performed
        //{
        //    get { return _Reason_Not_Performed; }
        //    set
        //    {
        //        _Reason_Not_Performed = value;
        //    }
        //}

        //[DataMember]
        //public virtual string Followup_Plan
        //{
        //    get { return _Followup_Plan; }
        //    set
        //    {
        //        _Followup_Plan = value;
        //    }
        //}
        [DataMember]
        public virtual string Care_Name_Value
        {
            get { return _Care_Name_Value; }
            set
            {
                _Care_Name_Value = value;
            }
        }
        [DataMember]
        public virtual string Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
            }
        }
        [DataMember]
        public virtual string Care_Plan_Notes
        {
            get { return _Care_Plan_Notes; }
            set
            {
                _Care_Plan_Notes = value;
            }
        }
        [DataMember]
        public virtual string Created_By
        {
            get { return _Created_By; }
            set
            {
                _Created_By = value;
            }
        }
        //[DataMember]
        //public virtual DateTime Captured_Date_Time
        //{
        //    get { return _Captured_Date_Time; }
        //    set
        //    {
        //        _Captured_Date_Time = value;
        //    }
        //}



        [DataMember]
        public virtual DateTime Created_Date_And_Time
        {
            get { return _Created_Date_And_Time; }
            set
            {
                _Created_Date_And_Time = value;
            }
        }
        [DataMember]
        public virtual string Modified_By
        {
            get { return _Modified_By; }
            set
            {
                _Modified_By = value;
            }
        }
        [DataMember]
        public virtual DateTime Modified_Date_And_Time
        {
            get { return _Modified_Date_And_Time; }
            set
            {
                _Modified_Date_And_Time = value;
            }
        }
        [DataMember]
        public virtual ulong Care_Plan_Lookup_ID
        {
            get { return _Care_Plan_Lookup_ID; }
            set
            {
                _Care_Plan_Lookup_ID = value;
            }
        }

        [DataMember]
        public virtual int Version
        {
            get
            {
                return _Version;
            }
            set
            {
                _Version = value;
            }
        }

        [DataMember]
        public virtual string Plan_Date
        {
            get
            {
                return _Plan_Date;
            }
            set
            {
                _Plan_Date = value;
            }
        }

        [DataMember]
        public virtual string Status_Value
        {
            get
            {
                return _Status_Value;
            }
            set
            {
                _Status_Value = value;
            }
        }
        [DataMember]
        public virtual bool Internal_Property_bInsert
        {
            get
            {
                return _Internal_Property_bInsert;
            }
            set
            {
                _Internal_Property_bInsert = value;
            }
        }
        [DataMember]
        public virtual string Care_Plan_Loinc_Code
        {
            get
            {
                return _Care_Plan_Loinc_Code;
            }
            set
            {
                _Care_Plan_Loinc_Code = value;
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
