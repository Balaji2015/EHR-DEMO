using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Acurus.Capella.Core.DomainObjects;

namespace Acurus.Capella.Core.DTO
{

    [DataContract]
    public partial class FillAppointment
    {
        private IList<ulong> _EncounterId = new List<ulong>();
        private IList<int> _Appointment_Provider_ID = new List<int>();
        private IList<ulong> _Human_ID = new List<ulong>();
        private IList<DateTime> _Appointment_Date = new List<DateTime>();
        private IList<int> _Duration_Minutes = new List<int>();
        private IList<string> _FacilityName = new List<string>();
        private IList<string> _PatientName = new List<string>();
        private IList<string> _ApptStatus = new List<string>();
        private IList<string> _PhysicianName = new List<string>();
        IList<Blockdays> _blockList = new List<Blockdays>();
        private IList<string> _Payment_Paid = new List<string>();
        private IList<string> _E_Super_Bill = new List<string>();
        private IList<DateTime> _Birth_Date = new List<DateTime>();
        private IList<string> _Human_Type = new List<string>();
        private IList<string> _Is_Medicare_Plan = new List<string>();
        private IList<string> _Is_Batch_Created = new List<string>();
        private IList<string> _Outstanding_Orders = new List<string>();
        //private IList<string> _Perform_EV_Status = new List<string>();
        private IList<string> _TypeofVisit = new List<string>();
        //private  IList<string> _EVMode= new List<string>();
        private IList<string> _Is_ACO_Eligible = new List<string>();
        private IList<string> _Preferred_Language = new List<string>();
        private IList<string> _Is_General_Queue_Appoinment = new List<string>();
        private IList<string> _Is_Auth_Verified = new List<string>();
        #region Constructors

        public FillAppointment()
        {

        }

        #endregion

        #region Properties

        //[DataMember]
        //public virtual IList<string> EVMode
        //{
        //    get { return _EVMode; }
        //    set { _EVMode = value; }
        //}


        [DataMember]
        public virtual IList<ulong> EncounterId
        {
            get { return _EncounterId; }
            set { _EncounterId = value; }
        }
        [DataMember]
        public virtual IList<int> Appointment_Provider_ID
        {
            get { return _Appointment_Provider_ID; }
            set { _Appointment_Provider_ID = value; }
        }
        [DataMember]
        public virtual IList<ulong> Human_ID
        {
            get { return _Human_ID; }
            set { _Human_ID = value; }
        }
        [DataMember]
        public virtual IList<DateTime> Appointment_Date
        {
            get { return _Appointment_Date; }
            set { _Appointment_Date = value; }
        }
        [DataMember]
        public virtual IList<int> Duration_Minutes
        {
            get { return _Duration_Minutes; }
            set { _Duration_Minutes = value; }
        }
        [DataMember]
        public virtual IList<string> ApptStatus
        {
            get { return _ApptStatus; }
            set { _ApptStatus = value; }
        }
        [DataMember]
        public virtual IList<string> FacilityName
        {
            get { return _FacilityName; }
            set { _FacilityName = value; }
        }

        [DataMember]
        public virtual IList<string> PatientName
        {
            get { return _PatientName; }
            set { _PatientName = value; }
        }
        [DataMember]
        public virtual IList<string> PhysicianName
        {
            get { return _PhysicianName; }
            set { _PhysicianName = value; }
        }
        [DataMember]
        public virtual IList<Blockdays> blockList
        {
            get { return _blockList; }
            set { _blockList = value; }
        }

        [DataMember]
        public virtual IList<string> Payment_Paid
        {
            get { return _Payment_Paid; }
            set { _Payment_Paid = value; }
        }

        [DataMember]
        public virtual IList<string> E_Super_Bill
        {
            get { return _E_Super_Bill; }
            set { _E_Super_Bill = value; }
        }
        [DataMember]
        public virtual IList<string> Human_Type
        {
            get { return _Human_Type; }
            set { _Human_Type = value; }
        }
        [DataMember]
        public virtual IList<DateTime> Birth_Date
        {
            get { return _Birth_Date; }
            set { _Birth_Date = value; }
        }
        [DataMember]
        public virtual IList<string> Is_Medicare_Plan
        {
            get { return _Is_Medicare_Plan; }
            set { _Is_Medicare_Plan = value; }
        }
        [DataMember]
        public virtual IList<string> Is_Batch_Created
        {
            get { return _Is_Batch_Created; }
            set { _Is_Batch_Created = value; }
        }
        [DataMember]
        public virtual IList<string> Outstanding_Orders
        {
            get { return _Outstanding_Orders; }
            set
            {
                _Outstanding_Orders = value;
            }
        }
        //[DataMember]
        //public virtual IList<string> Perform_EV_Status
        //{
        //    get { return _Perform_EV_Status; }
        //    set
        //    {
        //        _Perform_EV_Status = value;
        //    }
        //}
        [DataMember]
        public virtual IList<string> TypeofVisit
        {
            get { return _TypeofVisit; }
            set
            {
                _TypeofVisit = value;
            }
        }
        [DataMember]
        public virtual IList<string> Is_ACO_Eligible
        {
            get { return _Is_ACO_Eligible; }
            set
            {
                _Is_ACO_Eligible = value;
            }
        }

        [DataMember]
        public virtual IList<string> Preferred_Language
        {
            get { return _Preferred_Language; }
            set
            {
                _Preferred_Language = value;
            }
        }
        [DataMember]
        public virtual IList<string> Is_General_Queue_Appoinment
        {
            get { return _Is_General_Queue_Appoinment; }
            set
            {
                _Is_General_Queue_Appoinment = value;
            }
        }
        //CAP-3272
        [DataMember]
        public virtual IList<string> Is_Auth_Verified
        {
            get { return _Is_Auth_Verified; }
            set
            {
                _Is_Auth_Verified = value;
            }
        }
        #endregion

    }
}
