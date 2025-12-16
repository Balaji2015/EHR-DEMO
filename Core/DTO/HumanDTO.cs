using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Acurus.Capella.Core.DomainObjects;

namespace Acurus.Capella.Core.DTO
{
    [Serializable]
    [DataContract]
    public partial class HumanDTO
    {
        #region Declarations

        private Human _HumanDetails;
        private bool _MedicalRecordNoList;
        private bool _Patient_Account_External;
        private InsurancePlan _InsuranceDetails;
        private ulong _ulMedicalRecordID;
        private ulong _ulPatientAccountExternalID;
        private string _InsuranceCopy = string.Empty;
        private string _PatientInfo = string.Empty;
        private string _EligiblityVerified = string.Empty;
        private InsurancePlan _SecInsuranceDetails;
        private string _Primary_EligiblityStatus = string.Empty;
        private string _Secondry_EligiblityStatus = string.Empty;
        private string _sPriInsuredName = string.Empty;
        private string _sPriInsuredID = string.Empty;
        private string _sPriRelation = string.Empty;
        private int _HumanListCount = 0;
        private IList<Human> _HumanList;
        private IList<Encounter> _EncList;
        private IList<WFObject> _WFObjectList;
        private string _sSecondaryInsuranceName = string.Empty;
        private string _sSecondaryInsuranceRelation = string.Empty;

        //private IList<Human> _MedicalRecordNoList;
        //private IList<Human> _Patient_Account_External;
        //private InsurancePlan _InsuranceDetails;
        //private Carrier _CarrierDetails;
        //private string _PhyFirstName=string.Empty;
        //private string _PhyLastName = string.Empty;
        //private string _PhyMiddleName = string.Empty;
        //private IList<State> _StateList;

        #endregion


        #region Constructor

        public HumanDTO() 
        {
            _HumanDetails = new Human();
            _InsuranceDetails = new InsurancePlan();
            _SecInsuranceDetails = new InsurancePlan();
            _HumanList = new List<Human>();
            _EncList = new List<Encounter>();
            _WFObjectList = new List<WFObject>();
            //_HumanDetails = new Human();
            //_MedicalRecordNoList = new List<Human>();
            //_Patient_Account_External = new List<Human>();
            //_CarrierDetails = new Carrier();
            //_InsuranceDetails = new InsurancePlan();
            //_StateList = new List<State>();
        }

        #endregion


        #region Properties

        [DataMember]
        public virtual string sSecondaryInsuranceName
        {
            get { return _sSecondaryInsuranceName; }
            set { _sSecondaryInsuranceName = value; }
        }
        [DataMember]
        public virtual string sSecondaryInsuranceRelation
        {
            get { return _sSecondaryInsuranceRelation; }
            set { _sSecondaryInsuranceRelation = value; }
        }


        [DataMember]
        public virtual Human HumanDetails
        {
            get { return _HumanDetails; }
            set { _HumanDetails = value; }
        }
        [DataMember]
        public virtual bool MedicalRecordNoList
        {
            get { return _MedicalRecordNoList; }
            set { _MedicalRecordNoList = value; }
        }

        [DataMember]
        public virtual bool Patient_Account_External
        {
            get { return _Patient_Account_External; }
            set { _Patient_Account_External = value; }
        }
        [DataMember]
        public virtual ulong ulMedicalRecordID
        {
            get { return _ulMedicalRecordID; }
            set { _ulMedicalRecordID = value; }
        }
        [DataMember]
        public virtual ulong ulPatientAccountExternalID
        {
            get { return _ulPatientAccountExternalID; }
            set { _ulPatientAccountExternalID = value; }
        }
        [DataMember]
        public virtual InsurancePlan InsuranceDetails
        {
            get { return _InsuranceDetails; }
            set { _InsuranceDetails = value; }
        }
        //[DataMember]
        //public virtual IList<Human> MedicalRecordNoList
        //{
        //    get { return _MedicalRecordNoList; }
        //    set { _MedicalRecordNoList = value; }
        //}

        //[DataMember]
        //public virtual IList<Human> Patient_Account_External
        //{
        //    get { return _Patient_Account_External; }
        //    set { _Patient_Account_External = value; }
        //}
        //[DataMember]
        //public virtual Carrier CarrierDetails
        //{
        //    get { return _CarrierDetails; }
        //    set { _CarrierDetails = value; }
        //}
        //[DataMember]
        //public virtual InsurancePlan InsuranceDetails
        //{
        //    get { return _InsuranceDetails; }
        //    set { _InsuranceDetails = value; }
        //}
        //[DataMember]
        //public virtual string PhyFirstName
        //{
        //    get { return _PhyFirstName; }
        //    set { _PhyFirstName = value; }
        //}

        //[DataMember]
        //public virtual string PhyLastName
        //{
        //    get { return _PhyLastName; }
        //    set { _PhyLastName = value; }
        //}
        //[DataMember]
        //public virtual string PhyMiddleName
        //{
        //    get { return _PhyMiddleName; }
        //    set { _PhyMiddleName = value; }
        //}
        //[DataMember]
        //public virtual IList<State> StateList
        //{
        //    get { return _StateList; }
        //    set { _StateList = value; }
        //}
        [DataMember]
        public virtual string EligiblityVerified
        {
            get { return _EligiblityVerified; }
            set { _EligiblityVerified = value; }
        }
        [DataMember]
        public virtual string InsuranceCopy
        {
            get { return _InsuranceCopy; }
            set { _InsuranceCopy = value; }
        }
        [DataMember]
        public virtual string PatientInfo
        {
            get { return _PatientInfo; }
            set { _PatientInfo = value; }
        }
        [DataMember]
        public virtual InsurancePlan SecInsuranceDetails
        {
            get { return _SecInsuranceDetails; }
            set { _SecInsuranceDetails = value; }
        }
        [DataMember]
        public virtual string Primary_EligiblityStatus
        {
            get { return _Primary_EligiblityStatus; }
            set { _Primary_EligiblityStatus = value; }
        }

        [DataMember]
        public virtual string Secondry_EligiblityStatus
        {
            get { return _Secondry_EligiblityStatus; }
            set { _Secondry_EligiblityStatus = value; }
        }
        [DataMember]
        public virtual string sPriInsuredName
        {
            get { return _sPriInsuredName; }
            set { _sPriInsuredName = value; }
        }
        [DataMember]
        public virtual string sPriInsuredID
        {
            get { return _sPriInsuredID; }
            set { _sPriInsuredID = value; }
        }
        [DataMember]
        public virtual string sPriRelation
        {
            get { return _sPriRelation; }
            set { _sPriRelation = value; }
        }
        [DataMember]
        public virtual int HumanListCount
        {
            get { return _HumanListCount; }
            set { _HumanListCount = value; }
        }
        [DataMember]
        public virtual IList<Human> HumanList
        {
            get { return _HumanList; }
            set { _HumanList = value; }
        }
        [DataMember]
        public virtual IList<Encounter> EncList
        {
            get { return _EncList; }
            set { _EncList = value; }
        }
        [DataMember]
        public virtual IList<WFObject> WFObjectList
        {
            get { return _WFObjectList; }
            set { _WFObjectList = value; }
        }
        #endregion

    }
}
