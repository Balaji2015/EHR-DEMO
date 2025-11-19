using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
    [DataContract]
    public partial class Human : BusinessBase<ulong>
    {
        #region Declarations
        //Latha - Branch_52_production_for_Rcopia - Start - 4 Jul 2011
        //private ulong _Human_ID = 0;
        //Latha - Branch_52_production_for_Rcopia - End - 4 Jul 2011
        private string _sPrefix = string.Empty;
        private string _sLastName = string.Empty;
        private string _sFirstName = string.Empty;
        private string _sMI = string.Empty;
        private string _sSuffix = string.Empty;
        private DateTime _dtBirthDate = DateTime.MinValue;
        private string _sStreetAddress1 = string.Empty;
        private string _sCity = string.Empty;
        private string _sSex = string.Empty;
        private string _sState = string.Empty;
        private string _sZipCode = string.Empty;
        private string _sHomePhoneNo = string.Empty;
        private string _sSigOnFile = string.Empty;
        private int _iEncounter_Provider_ID = 0;
        private string _sFacility_Name = string.Empty;
        private string _sMaritalStatus = string.Empty;
        private string _sEmploymentStatus = string.Empty;
        private string _sAccountStatus = string.Empty;
        private DateTime _dtModifiedDateAndTime = DateTime.MinValue;
        private DateTime _dtCreatedDateAndTime = DateTime.MinValue;
        private string _sPatientNotes = string.Empty;
        private string _sStreetAddress2 = string.Empty;
        private string _sPatientStatementFormat = string.Empty;
        private string _sEMail = string.Empty;
        private string _sWorkPhoneNo = string.Empty;
        private string _sWorkPhoneExt = string.Empty;
        private string _sSSN = string.Empty;
        private DateTime _dtPatientDateLastBilled = DateTime.MinValue;
        private string _sEmployerName = string.Empty;
        private string _sPatientAccountExternal = string.Empty;
        private string _sDriverState = string.Empty;
        private string _sDriverLicenseNum = string.Empty;
        private string _sGuarantorLastName = string.Empty;
        private string _sGuarantorFirstName = string.Empty;
        private string _sGuarantorMI = string.Empty;
        private DateTime _dtGuarantorBirthDate = DateTime.MinValue;
        private string _sGuarantorStreetAddress1 = string.Empty;
        private string _sGuarantorStreetAddress2 = string.Empty;
        private string _sGuarantorCity = string.Empty;
        private string _sGuarantorSex = string.Empty;
        private string _sGuarantorState = string.Empty;
        private string _sGuarantorZipCode = string.Empty;
        private string _sGuarantorHomePhoneNumber = string.Empty;
        private double _dPatientUnAppliedPayments = 0;
        //thamizh
        //private string _sCollectionPriority = string.Empty;

        private string _sEmergencyCntLastName = string.Empty;
        private string _sEmergencyCntFirstName = string.Empty;
        private string _sEmergencyCntMI = string.Empty;
        private string _sEmergencyCntStreetAddr1 = string.Empty;
        private string _sEmergencyCntStreetAddr2 = string.Empty;
        private string _sEmergencyCntCity = string.Empty;
        private string _sEmergencyCntSex = string.Empty;
        private string _sEmergencyCntState = string.Empty;
        private string _sEmergencyCntZipCode = string.Empty;
        private string _sEmergencyCntHomePhoneNo = string.Empty;
        private string _sGuarantorIsPatient = string.Empty;
        private DateTime _dtEmergencyBirthDate = DateTime.MinValue;
        private string _sCellPhoneNo = string.Empty;
        private string _sGuarantorCellPhoneNumber = string.Empty;
        private string _sEmergencyCntCellPhoneNo = string.Empty;
        private string _sEmerRelation = string.Empty;
        private DateTime _dtDemoUpdateTimeStamp;
        private string _sMedicalRecordNumber = string.Empty;
        private int _iBatchID = 0;
        private decimal _iPastDue = 0;
        private string _sCreatedBy = string.Empty;
        private string _sModifiedBy = string.Empty;
        private string _sDemoStatus = string.Empty;
        private int _iVersion = 0;
        private string _sPeople_In_Collection = string.Empty;
        private string _sPreferred_Language = string.Empty;
        private string _sRace = string.Empty;
        private string _sEthnicity = string.Empty;
        private string _Photo_Path = string.Empty;
        private ulong _Pharmacy_Location_ID = 0;
        private ulong _Pharmacy_ID = 0;
        private string _Race_No = "0";
        private int _Ethnicity_No = 0;
        private string _Guarantor_Relationship = string.Empty;
        private IList<PatientInsuredPlan> _PatientInsuredBag;
        private string _Is_Sent_To_Rcopia = string.Empty;
        private int _Guarantor_Relationship_No = 0;
        //Latha - Rcopia Branch - Start
        private string _Is_Mail_Sent = string.Empty;
        private DateTime _Mail_Sent_Date = DateTime.MinValue;
        private string _Preferred_Confidential_Correspodence_Mode = string.Empty;
        private string _Password = string.Empty;
        //Latha - Rcopia Branch - End
        private string _Fax_Number = string.Empty;
        private DateTime _Reminder_Date = DateTime.MinValue;
        private string _Race_Alias = string.Empty;
        private string _sPatient_Status = "ALIVE";
        private DateTime _Date_Of_Death = DateTime.MinValue;
        private string _sReason_For_Death = string.Empty;
        private string _Human_Type = "REGULAR";// string.Empty;
        private string _Declared_Bankruptcy = string.Empty;

        private string _ACO_Is_Eligible_Patient = string.Empty;
        private string _Patient_Discussed = string.Empty;
        private string _Patient_Accepted = string.Empty;
        private ulong _Discussed_By = 0;
        private string _PCP_Name = string.Empty;
        private string _PCP_NPI = string.Empty;

        private string _Security_Question1 = string.Empty;
        private string _Answer1 = string.Empty;
        private string _Security_Question2 = string.Empty;

        private string _Answer2 = string.Empty;
        private string _Mothers_Maiden_Name = string.Empty;
        private string _Immunization_Registry_Status = string.Empty;
        private string _Publicity_Code = string.Empty;
        private string _Representative_Email = string.Empty;
        private string _Representative_Password = string.Empty;
        private string _Representative_Security_Question1 = string.Empty;
        private string _Representative_Answer1 = string.Empty;
        private string _Representative_Security_Question2 = string.Empty;
        private string _Representative_Answer2 = string.Empty;
        // Added by thamizh to add Guantor E-mail in the human table
        private string _Gaurantor_Email = string.Empty;


        private string _Sexual_Orientation = string.Empty;
        private string _Sexual_Orientation_Specify = string.Empty;
        private string _Gender_Identity = string.Empty;
        private string _Gender_Identity_Specify = string.Empty;
        private string _Data_Sharing_Preference = string.Empty;
        private string _Birth_Indicator = string.Empty;
        private string _Birth_Order = string.Empty;

        private string _Granularity = string.Empty;
        private string _Previous_Name = string.Empty;
        private string _Is_Medicaid = string.Empty;
        private string _Original_Eligibility_Due_To_Disability = string.Empty;
        private string _Is_Institutional = string.Empty;
        private string _Current_Eligibility_Due_To = string.Empty;
        private string _Enrollment_Status = string.Empty;
        private string _Appointment_Status = string.Empty;
        private string _Care_Giver_Last_Name = string.Empty;
        private string _Care_Giver_First_Name = string.Empty;
        private string _Care_Giver_Relation = string.Empty;
        private string _Care_Giver_Phone_Number = string.Empty;
        private string _sRAF_Status = string.Empty;
        private string _Is_Cerner_Registered = "N";
        private string _Cerner_Universal_Patient_ID = string.Empty;

        private DateTime _RCopia_Allergy_Last_Updated_Date_Time = DateTime.MinValue;
        private DateTime _RCopia_Medication_Last_Updated_Date_Time = DateTime.MinValue;
        private DateTime _RCopia_Prescription_Last_Updated_Date_Time = DateTime.MinValue;

        private string _Legal_Org = string.Empty;
        private ulong _Primary_Carrier_ID = 0;
        private string _sIsTranslatorRequired = "N";
        private string _Dynamics_Number = string.Empty;
        private string _Tribal_Affiliation = string.Empty;
        private string _Specific_Ethnicity = string.Empty;
        private string _Insurance_Status = string.Empty;
        #endregion

        #region HashCode Value

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Granularity);
            sb.Append(_Previous_Name);
            sb.Append(_Sexual_Orientation);
            sb.Append(_Sexual_Orientation_Specify);
            sb.Append(_Gender_Identity);
            sb.Append(_Gender_Identity_Specify);
            sb.Append(_sPrefix);
            sb.Append(_sLastName);
            sb.Append(_sFirstName);
            sb.Append(_sMI);
            sb.Append(_dtBirthDate);
            sb.Append(_sStreetAddress1);
            sb.Append(_sCity);
            sb.Append(_sSex);
            sb.Append(_sState);
            sb.Append(_sZipCode);
            sb.Append(_sHomePhoneNo);
            sb.Append(_sSigOnFile);
            sb.Append(_iEncounter_Provider_ID);
            sb.Append(_sFacility_Name);
            sb.Append(_sMaritalStatus);
            sb.Append(_sEmploymentStatus);
            sb.Append(_sAccountStatus);
            sb.Append(_dtModifiedDateAndTime);
            sb.Append(_dtCreatedDateAndTime);
            sb.Append(_sPatientNotes);
            sb.Append(_sStreetAddress2);
            sb.Append(_sPatientStatementFormat);
            sb.Append(_sEMail);
            sb.Append(_sWorkPhoneNo);
            sb.Append(_sWorkPhoneExt);
            sb.Append(_sSSN);
            sb.Append(_dtPatientDateLastBilled);
            sb.Append(_sEmployerName);
            sb.Append(_sPatientAccountExternal);
            sb.Append(_sDriverState);
            sb.Append(_sDriverLicenseNum);
            sb.Append(_sGuarantorLastName);
            sb.Append(_sGuarantorFirstName);
            sb.Append(_sGuarantorMI);
            sb.Append(_dtGuarantorBirthDate);
            sb.Append(_sGuarantorStreetAddress1);
            sb.Append(_sGuarantorStreetAddress2);
            sb.Append(_sGuarantorCity);
            sb.Append(_sGuarantorSex);
            sb.Append(_sGuarantorState);
            sb.Append(_sGuarantorZipCode);
            sb.Append(_sGuarantorHomePhoneNumber);
            sb.Append(_dPatientUnAppliedPayments);
            //thamizh
            //sb.Append(_sCollectionPriority);

            sb.Append(_sEmergencyCntLastName);
            sb.Append(_sEmergencyCntFirstName);
            sb.Append(_sEmergencyCntMI);
            sb.Append(_sEmergencyCntStreetAddr1);
            sb.Append(_sEmergencyCntStreetAddr2);
            sb.Append(_sEmergencyCntCity);
            sb.Append(_sEmergencyCntSex);
            sb.Append(_sEmergencyCntState);
            sb.Append(_sEmergencyCntZipCode);
            sb.Append(_sEmergencyCntHomePhoneNo);
            sb.Append(_sGuarantorIsPatient);
            sb.Append(_dtEmergencyBirthDate);
            sb.Append(_sCellPhoneNo);
            sb.Append(_sGuarantorCellPhoneNumber);
            sb.Append(_sEmergencyCntCellPhoneNo);
            sb.Append(_sEmerRelation);
            sb.Append(_dtDemoUpdateTimeStamp);
            sb.Append(_sMedicalRecordNumber);
            sb.Append(_iBatchID);
            sb.Append(_iPastDue);
            sb.Append(_sCreatedBy);
            sb.Append(_sModifiedBy);
            sb.Append(_sDemoStatus);
            sb.Append(_PatientInsuredBag);
            sb.Append(_iVersion);
            sb.Append(_sPreferred_Language);
            sb.Append(_sRace);
            sb.Append(_sEthnicity);
            sb.Append(_Pharmacy_Location_ID);
            sb.Append(_Pharmacy_ID);
            sb.Append(_sPeople_In_Collection);
            //Latha - Branch_52_production_for_Rcopia - Start - 4 Jul 2011
            //sb.Append(_Human_ID);
            //Latha - Branch_52_production_for_Rcopia - End - 4 Jul 2011
            sb.Append(_Photo_Path);
            sb.Append(_Race_No);
            sb.Append(_Ethnicity_No);
            sb.Append(_Guarantor_Relationship);
            sb.Append(_Is_Sent_To_Rcopia);
            sb.Append(_Guarantor_Relationship_No);
            //Latha - Rcopia Branch - Start
            sb.Append(_Is_Mail_Sent);
            sb.Append(_Mail_Sent_Date);
            sb.Append(_Preferred_Confidential_Correspodence_Mode);
            sb.Append(_Password);
            //Latha - Rcopia Branch - End
            sb.Append(_Fax_Number);
            sb.Append(_Reminder_Date);
            sb.Append(_Race_Alias);
            sb.Append(_sPatient_Status);
            sb.Append(_Date_Of_Death);
            sb.Append(_sReason_For_Death);
            sb.Append(_Human_Type);
            sb.Append(_Declared_Bankruptcy);

            sb.Append(_ACO_Is_Eligible_Patient);
            sb.Append(_Patient_Discussed);
            sb.Append(_Patient_Accepted);
            sb.Append(_Discussed_By);
            sb.Append(_PCP_Name);
            sb.Append(_PCP_NPI);

            sb.Append(_Security_Question1);
            sb.Append(_Answer1);
            sb.Append(_Security_Question2);
            sb.Append(_Answer2);


            sb.Append(_Mothers_Maiden_Name);
            sb.Append(_Immunization_Registry_Status);
            sb.Append(_Publicity_Code);
            sb.Append(_Representative_Email);
            sb.Append(_Representative_Password);
            sb.Append(_Representative_Security_Question1);
            sb.Append(_Representative_Answer1);
            sb.Append(_Representative_Security_Question2);
            sb.Append(_Representative_Answer2);
            sb.Append(_Gaurantor_Email);
            sb.Append(_Data_Sharing_Preference);
            sb.Append(_Birth_Indicator);
            sb.Append(_Birth_Order);
            sb.Append(_Is_Medicaid);
            sb.Append(_Original_Eligibility_Due_To_Disability);
            sb.Append(_Is_Institutional);
            sb.Append(_Current_Eligibility_Due_To);
            sb.Append(_Enrollment_Status);
            sb.Append(_Appointment_Status);
            sb.Append(_Care_Giver_First_Name);
            sb.Append(_Care_Giver_Last_Name);
            sb.Append(_Care_Giver_Relation);
            sb.Append(_Care_Giver_Phone_Number);
            sb.Append(_sRAF_Status);
            sb.Append(_Is_Cerner_Registered);
            sb.Append(_Cerner_Universal_Patient_ID);

            sb.Append(_RCopia_Allergy_Last_Updated_Date_Time);
            sb.Append(_RCopia_Medication_Last_Updated_Date_Time);
            sb.Append(_RCopia_Prescription_Last_Updated_Date_Time);

            sb.Append(_Legal_Org);
            sb.Append(_Primary_Carrier_ID);
            sb.Append(_sIsTranslatorRequired);
            sb.Append(_Tribal_Affiliation);
            sb.Append(_Specific_Ethnicity);
            sb.Append(_Insurance_Status);
            return sb.ToString().GetHashCode();
        }
        #endregion


        # region variable value implementation
        //Latha - Branch_52_production_for_Rcopia - Start - 4 Jul 2011
        //[DataMember]
        //public virtual ulong Human_ID
        //{
        //    get { return _Human_ID; }
        //    set { _Human_ID = value; }
        //}
        //Latha - Branch_52_production_for_Rcopia - End - 4 Jul 2011
        


             [DataMember]
        public virtual string Appointment_Status
        {
            get { return _Appointment_Status; }
            set { _Appointment_Status = value; }
        }
      
         [DataMember]
        public virtual string Granularity
        {
            get { return _Granularity; }
            set { _Granularity = value; }
        }

        [DataMember]
        public virtual string Previous_Name
        {
            get { return _Previous_Name; }
            set { _Previous_Name = value; }
        }


        [DataMember]
        public virtual string Sexual_Orientation
        {
            get { return _Sexual_Orientation; }
            set { _Sexual_Orientation = value; }
        }

        [DataMember]
        public virtual string Sexual_Orientation_Specify
        {
            get { return _Sexual_Orientation_Specify; }
            set { _Sexual_Orientation_Specify = value; }
        }


        [DataMember]
        public virtual string Gender_Identity
        {
            get { return _Gender_Identity; }
            set { _Gender_Identity = value; }
        }

        [DataMember]
        public virtual string Gender_Identity_Specify
        {
            get { return _Gender_Identity_Specify; }
            set { _Gender_Identity_Specify = value; }
        }
        // Added by thamizh to add guatantor email in the human table 

        [DataMember]
        public virtual string Gaurantor_Email
        {
            get { return _Gaurantor_Email; }
            set { _Gaurantor_Email = value; }
        }


        [DataMember]
        public virtual string Prefix
        {
            get { return _sPrefix; }
            set { _sPrefix = value; }
        }
        [DataMember]
        public virtual string Last_Name
        {
            get { return _sLastName; }
            set { _sLastName = value; }
        }
        [DataMember]
        public virtual string First_Name
        {
            get { return _sFirstName; }
            set { _sFirstName = value; }
        }
        [DataMember]
        public virtual string MI
        {
            get { return _sMI; }
            set { _sMI = value; }
        }
        [DataMember]
        public virtual string Suffix
        {
            get { return _sSuffix; }
            set { _sSuffix = value; }
        }
        [DataMember]
        public virtual DateTime Birth_Date
        {
            get { return _dtBirthDate; }
            set { _dtBirthDate = value; }
        }
        [DataMember]
        public virtual string Street_Address1
        {
            get { return _sStreetAddress1; }
            set { _sStreetAddress1 = value; }
        }
        [DataMember]
        public virtual string City
        {
            get { return _sCity; }
            set { _sCity = value; }
        }
        [DataMember]
        public virtual string Sex
        {
            get { return _sSex; }
            set { _sSex = value; }
        }
        [DataMember]
        public virtual string State
        {
            get { return _sState; }
            set { _sState = value; }
        }
        [DataMember]
        public virtual string ZipCode
        {
            get { return _sZipCode; }
            set { _sZipCode = value; }
        }
        [DataMember]
        public virtual string Home_Phone_No
        {
            get { return _sHomePhoneNo; }
            set { _sHomePhoneNo = value; }
        }
        [DataMember]
        public virtual string SigOn_File
        {
            get { return _sSigOnFile; }
            set { _sSigOnFile = value; }
        }
        [DataMember]
        public virtual int Encounter_Provider_ID
        {
            get { return _iEncounter_Provider_ID; }
            set { _iEncounter_Provider_ID = value; }
        }
        [DataMember]
        public virtual string Facility_Name
        {
            get { return _sFacility_Name; }
            set { _sFacility_Name = value; }
        }
        [DataMember]
        public virtual string Marital_Status
        {
            get { return _sMaritalStatus; }
            set { _sMaritalStatus = value; }
        }
        [DataMember]
        public virtual string Employment_Status
        {
            get { return _sEmploymentStatus; }
            set { _sEmploymentStatus = value; }
        }
        [DataMember]
        public virtual string Account_Status
        {
            get { return _sAccountStatus; }
            set { _sAccountStatus = value; }
        }
        [DataMember]
        public virtual DateTime Modified_Date_And_Time
        {
            get { return _dtModifiedDateAndTime; }
            set { _dtModifiedDateAndTime = value; }
        }
        [DataMember]
        public virtual DateTime Created_Date_And_Time
        {
            get { return _dtCreatedDateAndTime; }
            set { _dtCreatedDateAndTime = value; }
        }
        [DataMember]
        public virtual string Patient_Notes
        {
            get { return _sPatientNotes; }
            set { _sPatientNotes = value; }
        }
        [DataMember]
        public virtual string Street_Address2
        {
            get { return _sStreetAddress2; }
            set { _sStreetAddress2 = value; }
        }
        [DataMember]
        public virtual string Patient_Statement_Format
        {
            get { return _sPatientStatementFormat; }
            set { _sPatientStatementFormat = value; }
        }
        [DataMember]
        public virtual string EMail
        {
            get { return _sEMail; }
            set { _sEMail = value; }
        }
        [DataMember]
        public virtual string Work_Phone_No
        {
            get { return _sWorkPhoneNo; }
            set { _sWorkPhoneNo = value; }
        }
        [DataMember]
        public virtual string Work_Phone_Ext
        {
            get { return _sWorkPhoneExt; }
            set { _sWorkPhoneExt = value; }
        }
        [DataMember]
        public virtual string SSN
        {
            get { return _sSSN; }
            set { _sSSN = value; }
        }
        [DataMember]
        public virtual DateTime Patient_Date_Last_Billed
        {
            get { return _dtPatientDateLastBilled; }
            set { _dtPatientDateLastBilled = value; }
        }
        [DataMember]
        public virtual string Employer_Name
        {
            get { return _sEmployerName; }
            set { _sEmployerName = value; }
        }
        [DataMember]
        public virtual string Patient_Account_External
        {
            get { return _sPatientAccountExternal; }
            set { _sPatientAccountExternal = value; }
        }
        [DataMember]
        public virtual string Driver_State
        {
            get { return _sDriverState; }
            set { _sDriverState = value; }
        }
        [DataMember]
        public virtual string Driver_License_Num
        {
            get { return _sDriverLicenseNum; }
            set { _sDriverLicenseNum = value; }
        }
        [DataMember]
        public virtual string Guarantor_Last_Name
        {
            get { return _sGuarantorLastName; }
            set { _sGuarantorLastName = value; }
        }
        [DataMember]
        public virtual string Guarantor_First_Name
        {
            get { return _sGuarantorFirstName; }
            set { _sGuarantorFirstName = value; }
        }
        [DataMember]
        public virtual string Guarantor_MI
        {
            get { return _sGuarantorMI; }
            set { _sGuarantorMI = value; }
        }
        [DataMember]
        public virtual DateTime Guarantor_Birth_Date
        {
            get { return _dtGuarantorBirthDate; }
            set { _dtGuarantorBirthDate = value; }
        }
        [DataMember]
        public virtual string Guarantor_Street_Address1
        {
            get { return _sGuarantorStreetAddress1; }
            set { _sGuarantorStreetAddress1 = value; }
        }
        [DataMember]
        public virtual string Guarantor_Street_Address2
        {
            get { return _sGuarantorStreetAddress2; }
            set { _sGuarantorStreetAddress2 = value; }
        }
        [DataMember]
        public virtual string Guarantor_City
        {
            get { return _sGuarantorCity; }
            set { _sGuarantorCity = value; }
        }
        [DataMember]
        public virtual string Guarantor_Sex
        {
            get { return _sGuarantorSex; }
            set { _sGuarantorSex = value; }
        }
        [DataMember]
        public virtual string Guarantor_State
        {
            get { return _sGuarantorState; }
            set { _sGuarantorState = value; }
        }
        [DataMember]
        public virtual string Guarantor_Zip_Code
        {
            get { return _sGuarantorZipCode; }
            set { _sGuarantorZipCode = value; }
        }
        [DataMember]
        public virtual string Guarantor_Home_Phone_Number
        {
            get { return _sGuarantorHomePhoneNumber; }
            set { _sGuarantorHomePhoneNumber = value; }
        }
        [DataMember]
        public virtual double Patient_UnApplied_Payments
        {
            get { return _dPatientUnAppliedPayments; }
            set { _dPatientUnAppliedPayments = value; }
        }

        //thamizh
        //[DataMember]
        //public virtual string Collection_Priority
        //{
        //    get { return _sCollectionPriority; }
        //    set { _sCollectionPriority = value; }
        //}


        [DataMember]
        public virtual string Emergency_Cnt_Last_Name
        {
            get { return _sEmergencyCntLastName; }
            set { _sEmergencyCntLastName = value; }
        }
        [DataMember]
        public virtual string Emergency_Cnt_First_Name
        {
            get { return _sEmergencyCntFirstName; }
            set { _sEmergencyCntFirstName = value; }
        }
        [DataMember]
        public virtual string Emergency_Cnt_MI
        {
            get { return _sEmergencyCntMI; }
            set { _sEmergencyCntMI = value; }
        }
        [DataMember]
        public virtual string Emergency_Cnt_StreetAddr1
        {
            get { return _sEmergencyCntStreetAddr1; }
            set { _sEmergencyCntStreetAddr1 = value; }
        }
        [DataMember]
        public virtual string Emergency_Cnt_StreetAddr2
        {
            get { return _sEmergencyCntStreetAddr2; }
            set { _sEmergencyCntStreetAddr2 = value; }
        }
        [DataMember]
        public virtual string Emergency_Cnt_City
        {
            get { return _sEmergencyCntCity; }
            set { _sEmergencyCntCity = value; }
        }
        [DataMember]
        public virtual string Emergency_Cnt_Sex
        {
            get { return _sEmergencyCntSex; }
            set { _sEmergencyCntSex = value; }
        }
        [DataMember]
        public virtual string Emergency_Cnt_State
        {
            get { return _sEmergencyCntState; }
            set { _sEmergencyCntState = value; }
        }
        [DataMember]
        public virtual string Emergency_Cnt_ZipCode
        {
            get { return _sEmergencyCntZipCode; }
            set { _sEmergencyCntZipCode = value; }
        }
        [DataMember]
        public virtual string Emergency_Cnt_Home_Phone_Number
        {
            get { return _sEmergencyCntHomePhoneNo; }
            set { _sEmergencyCntHomePhoneNo = value; }
        }
        [DataMember]
        public virtual string Guarantor_Is_Patient
        {
            get { return _sGuarantorIsPatient; }
            set { _sGuarantorIsPatient = value; }
        }
        [DataMember]
        public virtual DateTime Emergency_BirthDate
        {
            get { return _dtEmergencyBirthDate; }
            set { _dtEmergencyBirthDate = value; }
        }
        [DataMember]
        public virtual string Cell_Phone_Number
        {
            get { return _sCellPhoneNo; }
            set { _sCellPhoneNo = value; }
        }
        [DataMember]
        public virtual string Guarantor_CellPhone_Number
        {
            get { return _sGuarantorCellPhoneNumber; }
            set { _sGuarantorCellPhoneNumber = value; }
        }
        [DataMember]
        public virtual string Emergency_Cnt_CellPhone_Number
        {
            get { return _sEmergencyCntCellPhoneNo; }
            set { _sEmergencyCntCellPhoneNo = value; }
        }
        [DataMember]
        public virtual string Emer_Relation
        {
            get { return _sEmerRelation; }
            set { _sEmerRelation = value; }
        }
        [DataMember]
        public virtual DateTime Demo_Update_Time_Stamp
        {
            get { return _dtDemoUpdateTimeStamp; }
            set { _dtDemoUpdateTimeStamp = value; }
        }
        [DataMember]
        public virtual string Medical_Record_Number
        {
            get { return _sMedicalRecordNumber; }
            set { _sMedicalRecordNumber = value; }
        }
        [DataMember]
        public virtual int Batch_ID
        {
            get { return _iBatchID; }
            set { _iBatchID = value; }
        }
        [DataMember]
        public virtual decimal Past_Due
        {
            get { return _iPastDue; }
            set { _iPastDue = value; }
        }
        [DataMember]
        public virtual string Created_By
        {
            get { return _sCreatedBy; }
            set { _sCreatedBy = value; }
        }
        [DataMember]
        public virtual string Modified_By
        {
            get { return _sModifiedBy; }
            set { _sModifiedBy = value; }
        }
        [DataMember]
        public virtual string Demo_Status
        {
            get { return _sDemoStatus; }
            set { _sDemoStatus = value; }
        }

        [DataMember]
        public virtual int Version
        {
            get { return _iVersion; }
            set { _iVersion = value; }
        }

        [DataMember]
        public virtual IList<PatientInsuredPlan> PatientInsuredBag
        {
            get { return _PatientInsuredBag; }
            set { _PatientInsuredBag = value; }
        }

        [DataMember]
        public virtual string People_In_Collection
        {
            get { return _sPeople_In_Collection; }
            set { _sPeople_In_Collection = value; }
        }

        [DataMember]
        public virtual string Preferred_Language
        {
            get { return _sPreferred_Language; }
            set { _sPreferred_Language = value; }
        }
        [DataMember]
        public virtual string Race
        {
            get { return _sRace; }
            set { _sRace = value; }
        }
        [DataMember]
        public virtual string Ethnicity
        {
            get { return _sEthnicity; }
            set { _sEthnicity = value; }
        }
        [DataMember]
        public virtual ulong Pharmacy_Location_ID
        {
            get { return _Pharmacy_Location_ID; }
            set { _Pharmacy_Location_ID = value; }
        }
        [DataMember]
        public virtual ulong Pharmacy_ID
        {
            get { return _Pharmacy_ID; }
            set { _Pharmacy_ID = value; }
        }
        [DataMember]
        public virtual string Photo_Path
        {
            get { return _Photo_Path; }
            set { _Photo_Path = value; }
        }
        [DataMember]
        public virtual string Race_No
        {
            get { return _Race_No; }
            set { _Race_No = value; }
        }
        [DataMember]
        public virtual int Ethnicity_No
        {
            get { return _Ethnicity_No; }
            set { _Ethnicity_No = value; }
        }
        [DataMember]
        public virtual string Guarantor_Relationship
        {
            get { return _Guarantor_Relationship; }
            set { _Guarantor_Relationship = value; }
        }
        [DataMember]
        public virtual string Is_Sent_To_Rcopia
        {
            get { return _Is_Sent_To_Rcopia; }
            set { _Is_Sent_To_Rcopia = value; }
        }
        [DataMember]
        public virtual int Guarantor_Relationship_No
        {
            get { return _Guarantor_Relationship_No; }
            set { _Guarantor_Relationship_No = value; }
        }
        //Latha - Rcopia Branch - Start
        [DataMember]
        public virtual string Is_Mail_Sent
        {
            get { return _Is_Mail_Sent; }
            set { _Is_Mail_Sent = value; }
        }
        [DataMember]
        public virtual DateTime Mail_Sent_Date
        {
            get { return _Mail_Sent_Date; }
            set { _Mail_Sent_Date = value; }
        }
        [DataMember]
        public virtual string Preferred_Confidential_Correspodence_Mode
        {
            get { return _Preferred_Confidential_Correspodence_Mode; }
            set { _Preferred_Confidential_Correspodence_Mode = value; }
        }
        [DataMember]
        public virtual string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        //Latha - Rcopia Branch - End

        [DataMember]
        public virtual string Fax_Number
        {
            get { return _Fax_Number; }
            set { _Fax_Number = value; }
        }

        [DataMember]
        public virtual DateTime Reminder_Date
        {
            get { return _Reminder_Date; }
            set { _Reminder_Date = value; }
        }

        [DataMember]
        public virtual string Race_Alias
        {
            get { return _Race_Alias; }
            set { _Race_Alias = value; }
        }
        [DataMember]
        public virtual string Patient_Status
        {
            get { return _sPatient_Status; }
            set { _sPatient_Status = value; }
        }
        [DataMember]
        public virtual DateTime Date_Of_Death
        {
            get { return _Date_Of_Death; }
            set { _Date_Of_Death = value; }
        }
        [DataMember]
        public virtual string Reason_For_Death
        {
            get { return _sReason_For_Death; }
            set { _sReason_For_Death = value; }
        }
        [DataMember]
        public virtual string Human_Type
        {
            get { return _Human_Type; }
            set { _Human_Type = value; }
        }
        [DataMember]
        public virtual string Declared_Bankruptcy
        {
            get { return _Declared_Bankruptcy; }
            set { _Declared_Bankruptcy = value; }
        }
        [DataMember]
        public virtual string ACO_Is_Eligible_Patient
        {
            get { return _ACO_Is_Eligible_Patient; }
            set { _ACO_Is_Eligible_Patient = value; }
        }
        [DataMember]
        public virtual string Patient_Discussed
        {
            get { return _Patient_Discussed; }
            set { _Patient_Discussed = value; }
        }
        [DataMember]
        public virtual string Patient_Accepted
        {
            get { return _Patient_Accepted; }
            set { _Patient_Accepted = value; }
        }
        [DataMember]
        public virtual ulong Discussed_By
        {
            get { return _Discussed_By; }
            set { _Discussed_By = value; }
        }
        [DataMember]
        public virtual string PCP_Name
        {
            get { return _PCP_Name; }
            set { _PCP_Name = value; }
        }
        [DataMember]
        public virtual string PCP_NPI
        {
            get { return _PCP_NPI; }
            set { _PCP_NPI = value; }
        }

        [DataMember]
        public virtual string Security_Question1
        {
            get { return _Security_Question1; }
            set
            {
                _Security_Question1 = value;
            }
        }

        [DataMember]
        public virtual string Answer1
        {
            get { return _Answer1; }
            set
            {
                _Answer1 = value;
            }
        }

        [DataMember]
        public virtual string Security_Question2
        {
            get { return _Security_Question2; }
            set
            {
                _Security_Question2 = value;
            }
        }

        [DataMember]
        public virtual string Answer2
        {
            get { return _Answer2; }
            set
            {
                _Answer2 = value;
            }
        }
        [DataMember]
        public virtual string Mothers_Maiden_Name
        {
            get { return _Mothers_Maiden_Name; }
            set
            {
                _Mothers_Maiden_Name = value;
            }
        }
        [DataMember]
        public virtual string Immunization_Registry_Status
        {
            get { return _Immunization_Registry_Status; }
            set
            {
                _Immunization_Registry_Status = value;
            }
        }
        [DataMember]
        public virtual string Publicity_Code
        {
            get { return _Publicity_Code; }
            set
            {
                _Publicity_Code = value;
            }
        }
        [DataMember]
        public virtual string Representative_Email
        {
            get { return _Representative_Email; }
            set
            {
                _Representative_Email = value;
            }
        }
        [DataMember]
        public virtual string Representative_Password
        {
            get { return _Representative_Password; }
            set
            {
                _Representative_Password = value;
            }
        }
        [DataMember]
        public virtual string Representative_Security_Question1
        {
            get { return _Representative_Security_Question1; }
            set
            {
                _Representative_Security_Question1 = value;
            }
        }
        [DataMember]
        public virtual string Representative_Answer1
        {
            get { return _Representative_Answer1; }
            set
            {
                _Representative_Answer1 = value;
            }
        }
        [DataMember]
        public virtual string Representative_Security_Question2
        {
            get { return _Representative_Security_Question2; }
            set
            {
                _Representative_Security_Question2 = value;
            }
        }
        [DataMember]
        public virtual string Representative_Answer2
        {
            get { return _Representative_Answer2; }
            set
            {
                _Representative_Answer2 = value;
            }
        }
        [DataMember]
        public virtual string Data_Sharing_Preference
        {
            get { return _Data_Sharing_Preference; }
            set
            {
                _Data_Sharing_Preference = value;
            }
        }
        [DataMember]
        public virtual string Birth_Indicator
        {
            get { return _Birth_Indicator; }
            set
            {
                _Birth_Indicator = value;
            }
        }
        [DataMember]
        public virtual string Birth_Order
        {
            get { return _Birth_Order; }
            set
            {
                _Birth_Order = value;
            }
        }
        [DataMember]
        public virtual string Is_Medicaid
        {
            get { return _Is_Medicaid; }
            set
            {
                _Is_Medicaid = value;
            }
        }
        [DataMember]
        public virtual string Original_Eligibility_Due_To_Disability
        {
            get { return _Original_Eligibility_Due_To_Disability; }
            set
            {
                _Original_Eligibility_Due_To_Disability = value;
            }
        }

        [DataMember]
        public virtual string Is_Institutional
        {
            get { return _Is_Institutional; }
            set
            {
                _Is_Institutional = value;
            }
        }

        [DataMember]
        public virtual string Current_Eligibility_Due_To
        {
            get { return _Current_Eligibility_Due_To; }
            set
            {
                _Current_Eligibility_Due_To = value;
            }
        }
        [DataMember]
        public virtual string Enrollment_Status
        {
            get { return _Enrollment_Status; }
            set { _Enrollment_Status = value; }
        }
        [DataMember]
        public virtual string Care_Giver_Relation
        {
            get { return _Care_Giver_Relation; }
            set
            {
                _Care_Giver_Relation = value;
            }
        }

        [DataMember]
        public virtual string Care_Giver_First_Name
        {
            get { return _Care_Giver_First_Name; }
            set
            {
                _Care_Giver_First_Name = value;
            }
        }

        [DataMember]
        public virtual string Care_Giver_Last_Name
        {
            get { return _Care_Giver_Last_Name; }
            set
            {
                _Care_Giver_Last_Name = value;
            }
        }
        [DataMember]
        public virtual string Care_Giver_Phone_Number
        {
            get { return _Care_Giver_Phone_Number; }
            set { _Care_Giver_Phone_Number = value; }
        }
        [DataMember]
        public virtual string RAF_Status
        {
            get { return _sRAF_Status; }
            set { _sRAF_Status = value; }
        }
        [DataMember]
        public virtual string Is_Cerner_Registered
        {
            get { return _Is_Cerner_Registered; }
            set { _Is_Cerner_Registered = value; }
        }
        [DataMember]
        public virtual string Cerner_Universal_Patient_ID
        {
            get { return _Cerner_Universal_Patient_ID; }
            set { _Cerner_Universal_Patient_ID = value; }
        }

        [DataMember]
        public virtual DateTime RCopia_Allergy_Last_Updated_Date_Time
        {
            get { return _RCopia_Allergy_Last_Updated_Date_Time; }
            set { _RCopia_Allergy_Last_Updated_Date_Time = value; }
        }

        [DataMember]
        public virtual DateTime RCopia_Medication_Last_Updated_Date_Time
        {
            get { return _RCopia_Medication_Last_Updated_Date_Time; }
            set { _RCopia_Medication_Last_Updated_Date_Time = value; }
        }

        [DataMember]
        public virtual DateTime RCopia_Prescription_Last_Updated_Date_Time
        {
            get { return _RCopia_Prescription_Last_Updated_Date_Time; }
            set { _RCopia_Prescription_Last_Updated_Date_Time = value; }
        }

        [DataMember]
        public virtual string Legal_Org
        {
            get { return _Legal_Org; }
            set { _Legal_Org = value; }
        }

        [DataMember]
        public virtual ulong Primary_Carrier_ID
        {
            get { return _Primary_Carrier_ID; }
            set { _Primary_Carrier_ID = value; }
        }
        [DataMember]
        public virtual string Is_Translator_Required
        {
            get { return _sIsTranslatorRequired; }
            set { _sIsTranslatorRequired = value; }
        }
        //CAP-602 - Add new field in Human table
        [DataMember]
        public virtual string Dynamics_Number
        {
            get { return _Dynamics_Number; }
            set { _Dynamics_Number = value; }
        }
        //CAP-3554
        [DataMember]
        public virtual string Tribal_Affiliation
        {
            get { return _Tribal_Affiliation; }
            set { _Tribal_Affiliation = value; }
        }
        [DataMember]
        public virtual string Specific_Ethnicity
        {
            get { return _Specific_Ethnicity; }
            set { _Specific_Ethnicity = value; }
        }
        [DataMember]
        public virtual string Insurance_Status
        {
            get { return _Insurance_Status; }
            set { _Insurance_Status = value; }
        }
        #endregion

    }

}
