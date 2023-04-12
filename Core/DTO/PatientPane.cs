using System;
using System.Runtime.Serialization;
using System.Collections.Generic;


namespace Acurus.Capella.Core.DTO
{
    
    [DataContract]

    public partial class PatientPane
    {
        #region Declarations

        private ulong humanid=0;
        private string lastname = string.Empty;
        private string firstname = string.Empty;
        private string _MI = string.Empty;
        private string _Suffix = string.Empty;
        private DateTime birthdate=DateTime.MinValue;
        private string sex = string.Empty;
        private string medicalrecordnumber = string.Empty;
        private string _ssn = string.Empty;
        private string _HomePhoneNo = string.Empty;
        private IList <string> insurancetype = null;
        private IList<ulong> insuranceplanid;
        private IList<string> insuranceplanname = null;
        private ulong encounterid=0;
        private DateTime dateofservice = DateTime.MinValue;
        private DateTime appointmentdatetime = DateTime.MinValue;
        private string _notes = string.Empty;
        private string assignedphysician = string.Empty;
        private string assignedphysicianusername = string.Empty;
        private string assignedmedicalassistant = string.Empty;
        private string _Physician_Suffix = string.Empty;
        private string _Physician_Middle_Name = string.Empty;
        private string _Physician_First_Name = string.Empty;
        private string _Physician_Last_Name = string.Empty;
        private IList<string> _CarrierName = null;
        //private string _Is_Sent_To_RCopia = string.Empty;
        private string _Is_Previous = string.Empty;
        //pravin-10-aug-2012
        private string _Patient_Status = string.Empty;
        private string _Facility_Name = string.Empty;
        private string _Patient_Type = string.Empty;
        //private string _AcoValidation = string.Empty;
        private string _CellPhoneNo = string.Empty;
        private string _PhotoPath = string.Empty;
        private string _PastDue = string.Empty;
        private string _ACO_Is_Eligible_Patient = string.Empty;
        private string _Preferred_Language = string.Empty;

        #endregion


        #region Constructor

        public PatientPane() { }

        #endregion


        #region Properties

        [DataMember]
        public virtual ulong Human_Id
        {
            get { return humanid; }
            set { humanid = value; }
        }
        [DataMember]
        public virtual string Last_Name
        {
            get { return lastname; }
            set { lastname = value; }
        }
        [DataMember]
        public virtual string First_Name
        {
            get { return firstname; }
            set { firstname = value; }
        }
        [DataMember]
        public virtual string MI
        {
            get { return _MI; }
            set { _MI = value; }
        }
        [DataMember]
        public virtual string HomePhoneNo
        {
            get { return _HomePhoneNo; }
            set { _HomePhoneNo = value; }
        }
        [DataMember]
        public virtual string Suffix
        {
            get { return _Suffix; }
            set { _Suffix = value; }
        }
        [DataMember]
        public virtual DateTime Birth_Date
        {
            get { return birthdate; }
            set { birthdate = value; }
        }
        [DataMember]
        public virtual string Sex
        {
            get { return sex; }
            set
            {
                sex = value;
            }
        }
        [DataMember]
        public virtual string Medical_Record_Number
        {
            get { return medicalrecordnumber; }
            set
            {
                medicalrecordnumber = value;
            }
        }
        [DataMember]
        public virtual string SSN
        {
            get { return _ssn; }
            set
            {
                _ssn = value;
            }
        }
        [DataMember]
        public virtual IList<string> Insurance_Type
        {
            get { return insurancetype; }
            set
            {
                insurancetype = value;
            }
        }
        [DataMember]
        public virtual IList<ulong>  Insurance_Plan_ID
        {
            get { return insuranceplanid; }
            set
            {
                insuranceplanid = value;
            }
        }
        [DataMember]
        public virtual IList< string> Ins_Plan_Name
        {
            get { return insuranceplanname; }
            set
            {
                insuranceplanname = value;
            }
        }
        [DataMember]
        public virtual ulong Encounter_ID
        {
            get { return encounterid; }
            set { encounterid = value; }
        }
        [DataMember]
        public virtual DateTime Date_of_Service
        {
            get { return dateofservice; }
            set { dateofservice = value; }
        }
        [DataMember]
        public virtual DateTime Appointment_Date_Time
        {
            get { return appointmentdatetime; }
            set { appointmentdatetime = value; }
        }
        //[DataMember]
        //public virtual string Notes
        //{
        //    get { return _notes; }
        //    set { _notes = value; }
        //}
        [DataMember]
        public virtual string Assigned_Physician
        {
            get { return assignedphysician; }
            set { assignedphysician = value; }
        }
        [DataMember]
        public virtual string Assigned_Physician_User_Name
        {
            get { return assignedphysicianusername; }
            set { assignedphysicianusername = value; }
        }
        [DataMember]
        public virtual string Assigned_Medical_Asst
        {
            get { return assignedmedicalassistant; }
            set
            {
                assignedmedicalassistant = value;
            }
        }

        //[DataMember]
        //public virtual string Physician_Last_Name
        //{
        //    get { return _Physician_Last_Name; }
        //    set { _Physician_Last_Name = value; }
        //}

        //[DataMember]
        //public virtual string Physician_First_Name
        //{
        //    get { return _Physician_First_Name; }
        //    set { _Physician_First_Name = value; }
        //}

        //[DataMember]
        //public virtual string Physician_Middle_Name
        //{
        //    get { return _Physician_Middle_Name; }
        //    set { _Physician_Middle_Name = value; }
        //}
        //[DataMember]
        //public virtual string Physician_Suffix
        //{
        //    get { return _Physician_Suffix; }
        //    set { _Physician_Suffix = value; }
        //}
        [DataMember]
        public virtual IList<string> CarrierName
        {
            get { return _CarrierName; }
            set { _CarrierName = value; }
        }
        //[DataMember]
        //public virtual string Is_Sent_To_RCopia
        //{
        //    get { return _Is_Sent_To_RCopia; }
        //    set { _Is_Sent_To_RCopia = value; }
        //}
        //[DataMember]
        //public virtual string Is_Previous
        //{
        //    get { return _Is_Previous; }
        //    set { _Is_Previous = value; }
        //}
        //pravin 10-aug-2012
        [DataMember]
        public virtual string Patient_Status
        {
            get { return _Patient_Status; }
            set { _Patient_Status = value; }
        }
        // Manimozhi 21st-Feb-2013
        //[DataMember]
        //public virtual string Facility_Name
        //{
        //    get { return _Facility_Name; }
        //    set { _Facility_Name = value; }
        //}
        [DataMember]
        public virtual string Patient_Type
        {
            get { return _Patient_Type; }
            set { _Patient_Type = value; }
        }

        //[DataMember]
        //public virtual string ACO_Validation
        //{
        //    get { return _AcoValidation; }
        //    set { _AcoValidation = value; }
        //}

        [DataMember]
        public virtual string PastDue
        {
            get { return _PastDue; }
            set { _PastDue = value; }
        }

        [DataMember]
        public virtual string CellPhoneNo
        {
            get { return _CellPhoneNo; }
            set { _CellPhoneNo = value; }
        }

        [DataMember]
        public virtual string PhotoPath
        {
            get { return _PhotoPath; }
            set { _PhotoPath = value; }
        }
        [DataMember]
        public virtual string ACO_Is_Eligible_Patient
        {
            get { return _ACO_Is_Eligible_Patient; }
            set { _ACO_Is_Eligible_Patient = value; }
        }

        
        [DataMember]

        public virtual string Preferred_Language
        {
            get { return _Preferred_Language; }
            set { _Preferred_Language = value; }
        }

        #endregion

    }
}
