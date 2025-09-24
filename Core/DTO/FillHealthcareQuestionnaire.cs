using System;
using System.Runtime.Serialization;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;

namespace Acurus.Capella.Core.DTO
{

    [Serializable]

    public partial class FillHealthcareQuestionnaire
    {
        private ulong _HealthCare_Questionnaire_ID=0;
        private ulong _Encounter_ID;
        //private ulong _Physician_ID;
        //private ulong _Human_ID;
        private ulong _Questionnaire_Lookup_ID=0;
        private string _Questionnaire_Category = string.Empty;
        private string _Questionnaire_Type = string.Empty;
        private string _Question = string.Empty;
        private string _Selected_Option = string.Empty;
        private string _Possible_Options = string.Empty;
        private string _Notes = string.Empty;
        private string _createdby = string.Empty;
        private DateTime _createddateandtime=DateTime.MinValue;
        private string _modifiedby = string.Empty;
        private DateTime _modifieddateandtime=DateTime.MinValue;
        private int _version = 0;

        private string _Is_Calculation_Required = string.Empty;
        private string _Normal_Question_Status = string.Empty;
        private string _Is_Notes = string.Empty;
        private string _Controls = string.Empty;
        private string _Question_Loinc_Code = string.Empty;
        private string _Options_Loinc_Code = string.Empty;
        private string _Where_Criteria = string.Empty;
        // private IList<Healthcare_Questionnaire> _Question_List;





        public ulong HealthCare_Questionnaire_ID
        {
            get { return _HealthCare_Questionnaire_ID; }
            set
            {
                _HealthCare_Questionnaire_ID = value;
            }
        }
        [DataMember]
        public ulong Encounter_ID
        {
            get { return _Encounter_ID; }
            set
            {
                _Encounter_ID = value;
            }
        }
        //[DataMember]
        //public ulong Physician_ID
        //{
        //    get { return _Physician_ID; }
        //    set
        //    {
        //        _Physician_ID = value;
        //    }
        //}
        //[DataMember]
        //public  ulong Human_ID
        //{
        //    get { return _Human_ID; }
        //    set
        //    {
        //        _Human_ID = value;
        //    }
        //}
       
        public  ulong Questionnaire_Lookup_ID
        {
            get { return _Questionnaire_Lookup_ID; }
            set
            {
                _Questionnaire_Lookup_ID = value;
            }
        }
     
        public  string Questionnaire_Category
        {
            get { return _Questionnaire_Category; }
            set
            {
                _Questionnaire_Category = value;
            }
        }
      
        public string Questionnaire_Type
        {
            get { return _Questionnaire_Type; }
            set
            {
                _Questionnaire_Type = value;
            }
        }
   
        public  string Question
        {
            get { return _Question; }
            set
            {
                _Question = value;
            }
        }
      
        public  string Selected_Option
        {
            get { return _Selected_Option; }
            set
            {
                _Selected_Option = value;
            }
        }

    
        public string Possible_Options
        {
            get { return _Possible_Options; }
            set
            {
                _Possible_Options = value;
            }
        }
        
     
        public  string Notes
        {
            get { return _Notes; }
            set
            {
                _Notes = value;
            }
        }
       
        public  string Created_By
        {
            get { return _createdby; }
            set
            {
                _createdby = value;
            }
        }
     
        public  DateTime Created_Date_And_Time
        {
            get { return _createddateandtime; }
            set
            {
                _createddateandtime = value;
            }
        }
       
        public  string Modified_By
        {
            get { return _modifiedby; }
            set
            {
                _modifiedby = value;
            }
        }
     
        public  DateTime Modified_Date_And_Time
        {
            get { return _modifieddateandtime; }
            set
            {
                _modifieddateandtime = value;
            }
        }

        public  int Version
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


        public string Is_Calculation_Required 
        {
            get { return _Is_Calculation_Required; }
            set { _Is_Calculation_Required = value; }
        }
        
        public string Normal_Question_Status
        {
            get { return _Normal_Question_Status; }
            set { _Normal_Question_Status = value; }
        }
      


        public string Is_Notes
        {
            get { return _Is_Notes; }
            set { _Is_Notes = value; }
        }
        public string Controls
        {
            get { return _Controls; }
            set { _Controls = value; }
        }
        public string Question_Loinc_Code
        {
            get { return _Question_Loinc_Code; }
            set { _Question_Loinc_Code = value; }
        }
        public string Options_Loinc_Code
        {
            get { return _Options_Loinc_Code; }
            set { _Options_Loinc_Code = value; }
        }
        public string Where_Criteria
        {
            get { return _Where_Criteria; }
            set { _Where_Criteria = value; }
        }
    }
}
