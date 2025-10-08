using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Acurus.Capella.Core.DomainObjects;

namespace Acurus.Capella.Core.DTO
{

    [Serializable]
    public partial class CarePlanDTO
    {

        private ulong _Plan_Care_ID = 0;
        private string _Care_Name = string.Empty;
        private string _Care_Name_Value = string.Empty;
        private string _Status = string.Empty;
        private string _Options = string.Empty;
        private string _Care_Plan_Notes = string.Empty;
        private string _Created_By = string.Empty;
        private DateTime _Created_Date_And_Time = DateTime.MinValue;
        private string _Modified_By = string.Empty;
        private DateTime _Modified_Date_And_Time = DateTime.MinValue;
        private ulong _Care_Plan_Lookup_ID = 0;
        private int _Version = 0;
        private string _Controls = string.Empty;
        private string _Plan_Date = string.Empty;
        private string _Vitals_BMI = string.Empty;
        private string _Vitals_BMI_Status_Value = string.Empty;
        private string _Vitals_BP = string.Empty;
        private string _Vitals_BP_Status_Value = string.Empty;
        private string _Status_Value = string.Empty;
        //private IList<Vitals> _VitalLst = null;   Comment by bala for Performance tuning
        //private SocialHistory _Social = null;
        //private string  _MaritalStatus =string.Empty;
        //private string _MaritalDescription = string.Empty;
        //private IList<string> _immunStatus = new List<string>();
        //private IList<string> _VitalList = new List<string>();
        //private string _rosStatus = string.Empty;
        //private string _pfshStatus = string.Empty;
        //private string _adStatus = string.Empty;
        //private string _adComment = string.Empty;
        //added by pravin for copy previous plan
        private ulong _PreviousEncounterId = 0;
        private bool _physician_process;
        private ulong _Master_ID = 0;
        private string _Control_Name = string.Empty;
        private string _Gender = string.Empty;

        private string _From_Age = string.Empty;

        private string _To_Age = string.Empty;
        private string _Additional_Rule = string.Empty;
        private int _Parent_Rule_ID = 0;

        //private string _Followup_Plan = string.Empty;
        //private string _Reason_Not_Performed = string.Empty;
        private string _Snomed_Code = string.Empty;
        private string _Care_Plan_Loinc_Code = string.Empty;
        private string _Options_Loinc_Code = string.Empty;

        public CarePlanDTO() 
        {
            _physician_process = false;
        }
        [DataMember]
        public virtual string Control_Name
        {
            get { return _Control_Name; }
            set { _Control_Name = value; }
        }
        [DataMember]
        public virtual ulong Plan_Care_ID
        {
            get { return _Plan_Care_ID; }
            set
            {
                _Plan_Care_ID = value;
            }
        }
        public virtual ulong Master_ID
        {
            get { return _Master_ID; }
            set
            {
                _Master_ID = value;
            }
        }

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
        public virtual string Options
        {
            get { return _Options; }
            set
            {
                _Options = value;
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
        public virtual string Controls
        {
            get { return _Controls; }
            set { _Controls = value; }
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

        //[DataMember]
        //public virtual IList<Vitals> VitalLst
        //{
        //    get
        //    {
        //        return _VitalLst;
        //    }
        //    set
        //    {
        //        _VitalLst = value;
        //    }
        //}

        //[DataMember]
        //public virtual string MaritalStatus
        //{
        //    get
        //    {
        //        return _MaritalStatus ;
        //    }
        //    set
        //    {
        //        _MaritalStatus = value;
        //    }
        //}

        [DataMember]
        public virtual string Vitals_BP
        {
            get
            {
                return _Vitals_BP;
            }
            set
            {
                _Vitals_BP = value;
            }
        }

        [DataMember]
        public virtual string Vitals_BP_Status_Value
        {
            get
            {
                return _Vitals_BP_Status_Value;
            }
            set
            {
                _Vitals_BP_Status_Value = value;
            }
        }
        [DataMember]
        public virtual string Vitals_BMI
        {
            get
            {
                return _Vitals_BMI;
            }
            set
            {
                _Vitals_BMI = value;
            }
        }

        [DataMember]
        public virtual string Vitals_BMI_Status_Value
        {
            get
            {
                return _Vitals_BMI_Status_Value;
            }
            set
            {
                _Vitals_BMI_Status_Value = value;
            }
        }
        //[DataMember]
        //public virtual IList<string> immunStatus
        //{
        //    get
        //    {
        //        return _immunStatus;
        //    }
        //    set
        //    {
        //        _immunStatus = value;
        //    }
        //}


        //[DataMember]
        //public virtual IList<string> VitalList
        //{
        //    get
        //    {
        //        return _VitalList;
        //    }
        //    set
        //    {
        //        _VitalList = value;
        //    }
        //}

        //[DataMember]
        //public virtual string rosStatus
        //{
        //    get
        //    {
        //        return _rosStatus;
        //    }
        //    set
        //    {
        //        _rosStatus = value;
        //    }
        //}

        //[DataMember]
        //public virtual string pfshStatus
        //{
        //    get
        //    {
        //        return _pfshStatus;
        //    }
        //    set
        //    {
        //        _pfshStatus = value;
        //    }
        //}

        //[DataMember]
        //public virtual string adStatus
        //{
        //    get
        //    {
        //        return _adStatus;
        //    }
        //    set
        //    {
        //        _adStatus = value;
        //    }
        //}

        //[DataMember]
        //public virtual string adComment
        //{
        //    get
        //    {
        //        return _adComment;
        //    }
        //    set
        //    {
        //        _adComment = value;
        //    }
        //}
        //[DataMember]
        //public virtual string MaritalDescription
        //{
        //    get
        //    {
        //        return _MaritalDescription;
        //    }
        //    set
        //    {
        //        _MaritalDescription = value;
        //    }
        //}
        [DataMember]
        public ulong PreviousEncounterId
        {
            get { return _PreviousEncounterId; }
            set { _PreviousEncounterId = value; }
        }
        [DataMember]
        public virtual bool Physician_Process
        {
            get { return _physician_process; }
            set { _physician_process = value; }
        }


        [DataMember]
        public virtual string Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }

        [DataMember]
        public virtual string From_Age
        {
            get { return _From_Age; }
            set { _From_Age = value; }
        }

        [DataMember]
        public virtual string To_Age
        {
            get { return _To_Age; }
            set { _To_Age = value; }
        }

        [DataMember]
        public virtual string Additional_Rule
        {
            get { return _Additional_Rule; }
            set
            {
                _Additional_Rule = value;
            }
        }

        [DataMember]
        public virtual int Parent_Rule_ID
        {
            get { return _Parent_Rule_ID; }
            set
            {
                _Parent_Rule_ID = value;
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
        public virtual string Care_Plan_Loinc_Code
        {
            get { return _Care_Plan_Loinc_Code; }
            set
            {
                _Care_Plan_Loinc_Code = value;
            }
        }
        [DataMember]
        public virtual string Options_Loinc_Code
        {
            get { return _Options_Loinc_Code; }
            set
            {
                _Options_Loinc_Code = value;
            }
        }
    }

     
}
